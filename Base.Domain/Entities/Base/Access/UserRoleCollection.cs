using Base.Domain.Api;
using Base.Domain.Audities;
using Base.Domain.DomainExtentions.Permission;
using Base.Domain.Enums.Base.Permissions;
using Base.Domain.Models.EntityModels.Permissions;
using Base.Domain.RepositoriesApi.Context;
using Mapster;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Entities.Base.Access
{

    public class UserRoleCollection : FullAuditedEntity<int>, IHasCustomMap
    {
        public string Title { get; set; }

        public List<PermissionListEmbeededModel> Permissions { get; set; }

        public void ConfigMap()
        {

        }


        public override void SeedData(DomainSeedContext seedContext)
        {
            seedContext.MainCore.UserRole.DropCollection();
            seedContext.MainCore.UserRole.Add(
            new UserRoleCollection
            {
                Id = 1,
                Title = "Host",
                CreatedById = ObjectId.Parse("66966858f505629c7995a3e6"),
                Permissions = PermissionReloadExtentions.GetUserPemissionCollection().Select(x => new PermissionListEmbeededModel
                {
                    Id = x.Id,
                    PermissionType = PermissionType.All
                }).ToList(),
            });
        }
    }
}
