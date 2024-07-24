using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Models.Permissions
{
    public class UserPermissionJsonModel
    {
        public List<UserPermissionJsonItem> Items { get; set; }

    }

    public class UserPermissionJsonItem
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Route { get; set; }
        public bool ShowInMenu { get; set; }
        public bool HasPermissonType { get; set; }
        public int Order { get; set; }
        public string Icon { get; set; }
        public List<UserPermissionJsonItem> Items { get; set; }
    }
}
