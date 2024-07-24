using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Shared.ViewModels.UserLogin
{
    public class ForgotPasswordSubmitCode
    {
        public string UserName { get; set; }
        public string Code { get; set; }

        public string Password { get; set; }

        [Compare(nameof(Password))]
        public string PasswordConfirm { get; set; }

    }
}
