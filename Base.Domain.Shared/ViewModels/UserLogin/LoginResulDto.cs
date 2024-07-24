using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Base.Domain.Shared.ViewModels.UserLogin
{
    public class LoginResulDto
    {
        public UserIdentitiedDetailsDto UserDetails { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }

        [JsonIgnore]
        public bool HasTwoFactor { get; set; }
    }
}
