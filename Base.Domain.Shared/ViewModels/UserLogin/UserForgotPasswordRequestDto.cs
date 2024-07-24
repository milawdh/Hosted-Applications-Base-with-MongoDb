using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Shared.ViewModels.UserLogin
{
    public class UserForgotPasswordRequestDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "نام کاربری خالی میباشد")]
        public string UserName { get; set; }

    }
}
