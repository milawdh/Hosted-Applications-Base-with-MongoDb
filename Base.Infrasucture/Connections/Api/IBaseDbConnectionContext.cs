using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Infrasucture.Connections.Api
{
    public interface IBaseDbConnectionContext : IConnectionContext
    {


    }

    public interface IConnectionContext : IDisposable
    {
        IMongoDatabase DataBase { get; }
        IMongoClient DbClient { get; }
        IClientSessionHandle DbSession { get; }
    }
}
