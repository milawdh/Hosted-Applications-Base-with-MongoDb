using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Enums.Base.DataBase
{
    public enum DbReadPrefrenceType
    {
        Nearest = 1,

        Primary = 2,

        PrimaryPreferred = 3,

        Secondary = 4,

        SecondaryPreferred = 5,
    }
}
