using Base.Infrasucture.Connections.Api;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Infrasucture.Connections.Impl
{
    //TODO : Implement All
    public class TenantDbConnectionContext : ITenantDbConnectionContext
    {
        private readonly IBaseDbConnectionContext _mainDbConnectionContext;
        public TenantDbConnectionContext(IBaseDbConnectionContext mainDbConnectionContext)
        {
            _mainDbConnectionContext = mainDbConnectionContext;
        }


        private IClientSessionHandle _dbSession;
        private IMongoClient _dbClient;
        private IMongoDatabase _dataBase;

        //TODO : Implement All
        public IMongoDatabase DataBase
        {
            get
            {
                if (_dataBase is null)
                    _dataBase = DbClient.GetDatabase("CoVisonCustomer");

                return _dataBase;
            }
        }

        public IMongoClient DbClient
        {
            get
            {
                var res = new MongoClientSettings()
                {
                    Server = new MongoServerAddress("localhost", 27017),
                    RetryWrites = true,
                    UseTls = false,
                    ServerSelectionTimeout = TimeSpan.FromSeconds(30),
                    MaxConnectionPoolSize = 100,
                    RetryReads = true,
                    MaxConnectionLifeTime = TimeSpan.FromMinutes(30),
                };

                _dbClient = new MongoClient(res);

                return _dbClient;
            }
        }

        public IClientSessionHandle DbSession
        {
            get
            {
                if (_dbSession is null)
                {
                    _dbSession = DbClient.StartSession();
                }
                return _dbSession;
            }
        }


        public void Dispose()
        {
        }
    }
}
