using Base.Domain.ApplicationSettings.MongoOperations;
using Base.Domain.Utils.Configuration;
using Base.Infrasucture.Connections.Api;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Infrasucture.Connections.Impl
{
    public class BaseDbConnectionContext : IBaseDbConnectionContext
    {

        private readonly MainDbConnectionSettings _mainDbSettings;
        public BaseDbConnectionContext()
        {
            _mainDbSettings = SingleTon<AppSettings>.Instance.Get<MainDbConnectionSettings>();
        }

        private IClientSessionHandle _mainDbSession;
        private IMongoClient _mainDbClient;
        private IMongoDatabase _mainDataBase;

        public IClientSessionHandle DbSession
        {
            get
            {
                if (_mainDbSession is null)
                {
                    _mainDbSession = DbClient.StartSession();
                }
                return _mainDbSession;
            }

        }


        public IMongoClient DbClient
        {
            get
            {
                if (_mainDbClient is null)
                    _mainDbClient = new MongoClient(_mainDbSettings.MainDataBaseClientSettings);

                return _mainDbClient;
            }
        }


        public IMongoDatabase DataBase
        {
            get
            {
                if (_mainDataBase is null)
                    _mainDataBase = DbClient.GetDatabase(_mainDbSettings.MainDataBaseName, _mainDbSettings.MainDatabaseSettings);

                return _mainDataBase;
            }
        }



        public void Dispose()
        {
            DbSession?.Dispose();
        }
    }
}
