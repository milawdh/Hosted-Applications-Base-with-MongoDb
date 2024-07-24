using Base.Domain.Audities;
using Base.Domain.DomainExtentions.Permission;
using Base.Domain.RepositoriesApi.Context;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Entities.Base.Access
{


    public class UserPermissionCollection : BaseEntity<int>
    {
        public string Title { get; set; }
        public int? ParentId { get; set; }
        public string Route { get; set; }
        public bool ShowInMenu { get; set; }
        public bool HasPermissionType { get; set; }
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Icon { get; set; }
        public int Order { get; set; }

        public override void SeedData(DomainSeedContext seedContext)
        {
            seedContext.MainCore.UserPermissions.DropCollection();
            seedContext.MainCore.UserPermissions.AddMany(PermissionReloadExtentions.GetUserPemissionCollection());

            base.SeedData(seedContext);
        }
    }
}
