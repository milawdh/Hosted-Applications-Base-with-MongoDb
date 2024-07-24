using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Enums.Base.DataBase
{
    public enum DbWriteConcernType
    {
        Unacknowledged = 0,

        Acknowledged = 1,

        WMajority = 2,

        W1 = 3,

        W2 = 4,

        W3 = 5,

        CustomCount = 6,
    }
}
