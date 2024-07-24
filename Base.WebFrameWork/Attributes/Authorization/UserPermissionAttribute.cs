using Base.Contract.Permission;
using Base.Domain.DomainExceptions;
using Base.Domain.RepositoriesApi.Core;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.WebFrameWork.Attributes.Authorization
{
    /// <summary>
    /// Custom Authorization
    /// </summary>
    public class UserPermissionAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string customAction;

        public UserPermissionAttribute(string customAction = null)
        {
            this.customAction = customAction;
        }
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            bool hasAllowAnonymous = filterContext.ActionDescriptor.EndpointMetadata
                          .Any(em => em.GetType() == typeof(AllowAnonymousAttribute));

            if (hasAllowAnonymous)
                return;

            var _core = filterContext.HttpContext.RequestServices.GetRequiredService<IBaseCore>();

            var userPermissionService = filterContext.HttpContext.RequestServices.GetRequiredService<IUserPermissionService>();
            var checkResult = userPermissionService.CheckPermission(new Domain.Shared.ViewModels.Permisison.CustomAction(customAction, null, null));

            if (checkResult.IsFailed)
                throw new AuthorizationException(checkResult.Messages.FirstOrDefault() ?? "شما به این عملیات دسترسی ندارید!");
        }
    }
}
