using Base.Domain.Consts;
using Base.Domain.Entities.Base.Access;
using Base.Domain.Models.Permissions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.DomainExtentions.Permission
{
    public static class PermissionReloadExtentions
    {
        public static List<UserPermissionCollection> GetUserPemissionCollection()
        {
            var usersPermissionJson = File.ReadAllText(AppPathsConsts.UsersPermissionJsonPath);
            var model = JsonConvert.DeserializeObject<UserPermissionJsonModel>(usersPermissionJson);

            var collection = new List<UserPermissionCollection>();
            var list = GetCollectionChilds(model.Items, null);

            collection.AddRange(list);

            return collection;
        }

        private static List<UserPermissionCollection> GetCollectionChilds(List<UserPermissionJsonItem> items, UserPermissionCollection parent)
        {
            List<UserPermissionCollection> result = new();

            foreach (var item in items)
            {
                var doc = new UserPermissionCollection()
                {
                    Action = item.Action,
                    Area = item.Area,
                    Controller = item.Controller,
                    HasPermissionType = item.HasPermissonType,
                    Order = item.Order,
                    Icon = item.Icon,
                    Route = item.Route,
                    ShowInMenu = item.ShowInMenu,
                    Title = item.Title,
                    Id = parent != null ? Convert.ToInt32(parent.Id + item.Id) : Convert.ToInt32(item.Id),
                    ParentId = parent != null ? Convert.ToInt32(parent.Id) : null,
                };
                result.Add(doc);

                if (item.Items != null)
                    result.AddRange(GetCollectionChilds(item.Items, doc));
            }

            return result;
        }
    }
}
