using Base.Domain.Enums.Base.Permissions;
using Base.Domain.Models.OutPutModels;
using Base.Domain.Shared.ViewModels.Permisison;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Contract.Permission
{
    public interface IBasePermissionService
    {
        ServiceResult<PermissionType> CheckPermission(CustomAction customAction = null, ObjectId? userId = null);
    }
}
