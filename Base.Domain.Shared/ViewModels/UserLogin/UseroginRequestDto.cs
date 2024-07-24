using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Shared.ViewModels.UserLogin
{
    public class UseroginRequestDto : BaseUserLoginRequestDto
    {
        [Required(AllowEmptyStrings = false)]
        //[RegularExpression(@"")]
        public string UserName { get; set; }
    }

    public class BaseUserLoginRequestDto
    {
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
