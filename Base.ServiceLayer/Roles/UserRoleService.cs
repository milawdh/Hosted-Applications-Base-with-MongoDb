using Base.Contract.Roles;
using Base.Domain.DomainExtentions.Query;
using Base.Domain.Entities.Base.Access;
using Base.Domain.Models.OutPutModels;
using Base.Domain.Models.Query;
using Base.Domain.RepositoriesApi.Core;
using Base.Domain.RepositoriesApi.Repo;
using Base.Domain.Shared.ViewModels.General;
using Base.Domain.Shared.ViewModels.Roles;
using Base.ServiceLayer.BaseServices;
using Mapster;
using Mapster.Utils;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.ServiceLayer.Roles
{
    public class UserRoleService :
        AppBaseFullService<UserRoleCollection,
            int,
            UserRoleDto,
            GeneralIdAndTitle<int>,
            SearchRequestModel,
            UserRoleListDto,
            CreateUpdateUserRoleDto,
            CreateUpdateUserRoleDto>, IUserRoleService
    {
        public UserRoleService(IBaseCore core, ITenantCore customerCore) : base(core, customerCore)
        {
        }

        protected override IMainRepo<UserRoleCollection, int> Repository => _mainCore.UserRole;

        protected override async Task<ServiceResult> BeforeAddAsync(CreateUpdateUserRoleDto model, UserRoleCollection entity)
        {
            if (_mainCore.UserPermissions.Get(x => model.Permissions.Select(a => a.Id).Contains(x.Id)).Count() != model.Permissions.Count)
                return new ServiceResult("دسترسی های نامعتبر");


            return await base.BeforeAddAsync(model, entity);
        }

        protected override async Task<ServiceResult> BeforeUpdate(CreateUpdateUserRoleDto model, UserRoleCollection entity)
        {
            if (_mainCore.UserPermissions.Get(x => model.Permissions.Select(a => a.Id).Contains(x.Id)).Count() != model.Permissions.Count)
                return new ServiceResult("دسترسی های نامعتبر");


            return await base.BeforeUpdate(model, entity);
        }

        protected override IAggregateFluent<UserRoleCollection> GetListCustomizeAggregate(IAggregateFluent<UserRoleCollection> aggregate)
        {
            return aggregate.JoinCreatorUser<UserRoleCollection, int>(_mainCore.Users.GetCollectionAsBson());

        }
    }
}
