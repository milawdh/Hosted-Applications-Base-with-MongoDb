using Base.Domain.Utils.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.ApplicationSettings.DataSeed
{
    public class DataSeedSettings : ISetting
    {
        public bool Enabeld { get; set; } = false;

        public bool DropBeforeInsert { get; set; } = false;
    }
}
