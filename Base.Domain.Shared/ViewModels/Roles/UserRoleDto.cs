using Base.Domain.Models.EntityModels.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Shared.ViewModels.Roles
{
    public class UserRoleDto
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public DateTime CreatedOn { get; set; }
        public string CreatedById { get; set; }
        public string CreatedByName { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedById { get; set; }
        public string ModifiedByName { get; set; }

        public List<PermissionListEmbeededModel> Permissions { get; set; }
    }
}
