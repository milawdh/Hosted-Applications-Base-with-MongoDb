using Base.Domain.Enums.Base.User;
using Mapster;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Shared.ViewModels.UserLogin
{
    public class RegisterIdentitiedUserDto
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        [Compare(nameof(Password))]
        public string ConfirmedPassword { get; set; }

        public string ImageProfileBase64 { get; set; }

        public GenderEnum Gender { get; set; }

        [Required(AllowEmptyStrings = false)]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@gmail\.com$")]
        public string Email { get; set; }

        [Compare(nameof(Email))]
        public string ConfirmedEmail { get; set; }
    }
}
