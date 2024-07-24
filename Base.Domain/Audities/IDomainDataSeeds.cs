using Base.Domain.RepositoriesApi.Context;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Audities
{
    public interface IDomainDataSeeds
    {
        void SeedData(DomainSeedContext seedContext);

    }
}
