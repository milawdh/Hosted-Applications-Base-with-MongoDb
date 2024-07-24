using Base.Domain.Models.EntityModels.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Shared.ViewModels.Roles
{
    public class CreateUpdateUserRoleDto
    {
        public string Title { get; set; }

        public List<PermissionListEmbeededModel> Permissions { get; set; } = new List<PermissionListEmbeededModel>();
    }
}
