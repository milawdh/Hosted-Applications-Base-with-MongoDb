using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Enums.Base.Query
{
    public enum FilterType
    {
        Eq = 1,

        Gt = 2,

        Gte = 3,

        Lt = 4,

        Lte = 5,

        In = 6,

        StrIn = 7,

        Mod = 8,

        Regex = 9,

        ComplexWordSearch = 10,

        Range = 11,
    }
}
