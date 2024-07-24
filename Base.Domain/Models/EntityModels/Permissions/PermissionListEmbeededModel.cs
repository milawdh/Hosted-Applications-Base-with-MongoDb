using Base.Domain.Enums.Base.Permissions;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Models.EntityModels.Permissions
{
    public class PermissionListEmbeededModel
    {
        public int Id { get; set; }
        public PermissionType PermissionType { get; set; }
    }
}
