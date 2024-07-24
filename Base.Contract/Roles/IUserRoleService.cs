using Base.Contract.Base.AppBaseServices;
using Base.Domain.Entities.Base.Access;
using Base.Domain.Models.Query;
using Base.Domain.Shared.ViewModels.General;
using Base.Domain.Shared.ViewModels.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Contract.Roles
{
    public interface IUserRoleService : IAppBaseFullService<UserRoleCollection, int, UserRoleDto, GeneralIdAndTitle<int>, SearchRequestModel, UserRoleListDto, CreateUpdateUserRoleDto, CreateUpdateUserRoleDto>
    {
    }
}
