using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Enums.Base.DataBase
{
    public enum DbReadConcernType
    {
        Availble = 0,

        Default = 1,

        Linearizable = 2,

        Local = 3,

        Majority = 4,

        Snapshot = 5
    }
}
