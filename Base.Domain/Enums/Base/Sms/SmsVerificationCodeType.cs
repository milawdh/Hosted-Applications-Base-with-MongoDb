using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Enums.Base.Sms
{
    public enum SmsVerificationCodeType
    {
        TwoFactorLogin = 1,

        Login = 2,

        ForgotPassword = 3,

        RegisterphoneNumber = 4,
    }
}
