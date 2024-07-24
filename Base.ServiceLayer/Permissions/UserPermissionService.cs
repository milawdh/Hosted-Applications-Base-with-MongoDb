using Base.Contract.Permission;
using Base.Domain.DomainExceptions;
using Base.Domain.Entities.Base.Access;
using Base.Domain.Entities.Base.Users;
using Base.Domain.Enums.Base.Permissions;
using Base.Domain.Models.OutPutModels;
using Base.Domain.RepositoriesApi.Core;
using Base.Domain.RepositoriesApi.UserInfo;
using Base.Domain.Shared.ViewModels.Permisison;

using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using ZstdSharp.Unsafe;

namespace Base.ServiceLayer.Permissions
{
    public class UserPermissionService : IUserPermissionService
    {
        private readonly IBaseCore _mainCore;
        private readonly IUserInfoContext _userInfoContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserPermissionService(IBaseCore mainCore, IUserInfoContext baseUserInfoContext, IHttpContextAccessor httpContextAccessor)
        {
            _mainCore = mainCore;
            _userInfoContext = baseUserInfoContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public ServiceResult<PermissionType> CheckPermission(CustomAction customAction = null, ObjectId? userId = null)
        {
            string area = customAction?.Area ?? _httpContextAccessor.HttpContext.Request.RouteValues["area"].ToString();
            string controller = customAction?.Controller ?? _httpContextAccessor.HttpContext.Request.RouteValues["controller"].ToString();
            string action = customAction?.Action ?? _httpContextAccessor.HttpContext.Request.RouteValues["action"].ToString();

            userId = userId ?? _userInfoContext.UserId;

            var permissionDoc = _mainCore.UserPermissions.Get(x => x.Area == area && x.Controller == controller && x.Action == action).FirstOrDefault();
            if (permissionDoc is null)
                throw new InvalidArgumentException("Permission Not Found!");


            var userPermissions = _mainCore.Users.Aggregate()
                .Match(x => x.Id == userId)
                .Lookup<UserCollection, UserRoleCollection, UserCollection>(_mainCore.UserRole.GetCollection(), x => x.RoleId, x => x.Id, x => x.Role)
                .Project(x => x.Role.SelectMany(c => c.Permissions).Union(x.CustomPermissions ?? new()))
                .ToList()
                .SelectMany(a => a)
                .ToList();

            var permission = userPermissions.FirstOrDefault(x => x.Id == permissionDoc.Id);

            if (permission == null)
                return new ServiceResult<PermissionType>("شما به این عملیات دسترسی ندارید!");

            return new ServiceResult<PermissionType>(permission.PermissionType);
        }
    }
}
