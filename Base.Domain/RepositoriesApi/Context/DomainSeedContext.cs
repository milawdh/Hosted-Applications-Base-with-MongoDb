using Base.Domain.RepositoriesApi.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.RepositoriesApi.Context
{
    public class DomainSeedContext
    {
        public DomainSeedContext(IBaseCore mainCore)
        {
            MainCore = mainCore;
        }

        public IBaseCore MainCore { get; set; }
    }
}
