using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Enums.Base.Sms
{
    public enum SmsCodeStatus
    {
        AckNowledged = 1,

        Sent = 2,

        IsUsed = 3,

        IsExpired = 4,
    }
}
