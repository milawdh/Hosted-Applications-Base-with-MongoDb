using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Shared.ViewModels.Token
{
    public class RefreshTokenResultDto
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
