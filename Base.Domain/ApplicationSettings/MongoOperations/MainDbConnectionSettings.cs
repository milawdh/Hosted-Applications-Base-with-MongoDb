using Base.Domain.Enums.Base.DataBase;
using Base.Domain.Utils.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.ApplicationSettings.MongoOperations
{
    public class MainDbConnectionSettings : ISetting
    {
        public string MainDataBaseName { get; set; } = "BASEDB";
        public string Host { get; set; } = "localhost";
        public int Port { get; set; } = 27017;
        public bool UseTls { get; set; } = false;
        public int ServerSelectionTimeoutMinutes { get; set; } = 30;
        public int MaxConnectionLifeTimeMinutes { get; set; } = 30;
        public int MaxConnectionPoolSize { get; set; } = 500;
        public bool RetryReads { get; set; } = true;
        public bool RetryWrites { get; set; } = true;
        public DbReadConcernType ReadConcern { get; set; } = DbReadConcernType.Default;
        public DbWriteConcernType WriteConcern { get; set; } = DbWriteConcernType.Acknowledged;
        public DbReadPrefrenceType ReadPreference { get; set; } = DbReadPrefrenceType.Nearest;
        public int CustomWriteAcknowledgeCount { get; set; } = 1;
        public MongoClientSettings MainDataBaseClientSettings
        {
            get
            {
                var res = new MongoClientSettings()
                {
                    Server = new MongoServerAddress(Host, Port),
                    RetryWrites = RetryWrites,
                    UseTls = UseTls,
                    ServerSelectionTimeout = TimeSpan.FromSeconds(ServerSelectionTimeoutMinutes),
                    MaxConnectionPoolSize = MaxConnectionPoolSize,
                    RetryReads = RetryReads,
                    MaxConnectionLifeTime = TimeSpan.FromMinutes(MaxConnectionLifeTimeMinutes),
                };
                switch (ReadConcern)
                {
                    case DbReadConcernType.Availble:
                        res.ReadConcern = MongoDB.Driver.ReadConcern.Available;
                        break;
                    case DbReadConcernType.Default:
                        res.ReadConcern = MongoDB.Driver.ReadConcern.Default;

                        break;
                    case DbReadConcernType.Linearizable:
                        res.ReadConcern = MongoDB.Driver.ReadConcern.Linearizable;

                        break;
                    case DbReadConcernType.Local:
                        res.ReadConcern = MongoDB.Driver.ReadConcern.Local;

                        break;
                    case DbReadConcernType.Majority:
                        res.ReadConcern = MongoDB.Driver.ReadConcern.Majority;

                        break;
                    case DbReadConcernType.Snapshot:
                        res.ReadConcern = MongoDB.Driver.ReadConcern.Snapshot;

                        break;
                    default:
                        break;
                }

                switch (WriteConcern)
                {
                    case DbWriteConcernType.Unacknowledged:
                        res.WriteConcern = MongoDB.Driver.WriteConcern.Unacknowledged;
                        break;
                    case DbWriteConcernType.Acknowledged:
                        res.WriteConcern = MongoDB.Driver.WriteConcern.Acknowledged;

                        break;
                    case DbWriteConcernType.WMajority:
                        res.WriteConcern = MongoDB.Driver.WriteConcern.WMajority;

                        break;
                    case DbWriteConcernType.W1:
                        res.WriteConcern = MongoDB.Driver.WriteConcern.W1;

                        break;
                    case DbWriteConcernType.W2:
                        res.WriteConcern = MongoDB.Driver.WriteConcern.W2;

                        break;
                    case DbWriteConcernType.W3:
                        res.WriteConcern = MongoDB.Driver.WriteConcern.W3;

                        break;
                    case DbWriteConcernType.CustomCount:
                        res.WriteConcern = new WriteConcern(CustomWriteAcknowledgeCount);

                        break;
                    default:
                        break;
                }

                switch (ReadPreference)
                {
                    case DbReadPrefrenceType.Nearest:
                        res.ReadPreference = MongoDB.Driver.ReadPreference.Nearest;
                        break;
                    case DbReadPrefrenceType.Primary:
                        res.ReadPreference = MongoDB.Driver.ReadPreference.Primary;

                        break;
                    case DbReadPrefrenceType.PrimaryPreferred:
                        res.ReadPreference = MongoDB.Driver.ReadPreference.PrimaryPreferred;

                        break;
                    case DbReadPrefrenceType.Secondary:
                        res.ReadPreference = MongoDB.Driver.ReadPreference.Secondary;

                        break;
                    case DbReadPrefrenceType.SecondaryPreferred:
                        res.ReadPreference = MongoDB.Driver.ReadPreference.SecondaryPreferred;

                        break;
                    default:
                        break;
                }

                return res;
            }
        }

        public MongoDatabaseSettings MainDatabaseSettings
        {
            get
            {
                var res = new MongoDatabaseSettings();

                switch (ReadConcern)
                {
                    case DbReadConcernType.Availble:
                        res.ReadConcern = MongoDB.Driver.ReadConcern.Available;
                        break;
                    case DbReadConcernType.Default:
                        res.ReadConcern = MongoDB.Driver.ReadConcern.Default;

                        break;
                    case DbReadConcernType.Linearizable:
                        res.ReadConcern = MongoDB.Driver.ReadConcern.Linearizable;

                        break;
                    case DbReadConcernType.Local:
                        res.ReadConcern = MongoDB.Driver.ReadConcern.Local;

                        break;
                    case DbReadConcernType.Majority:
                        res.ReadConcern = MongoDB.Driver.ReadConcern.Majority;

                        break;
                    case DbReadConcernType.Snapshot:
                        res.ReadConcern = MongoDB.Driver.ReadConcern.Snapshot;

                        break;
                    default:
                        break;
                }

                switch (WriteConcern)
                {
                    case DbWriteConcernType.Unacknowledged:
                        res.WriteConcern = MongoDB.Driver.WriteConcern.Unacknowledged;
                        break;
                    case DbWriteConcernType.Acknowledged:
                        res.WriteConcern = MongoDB.Driver.WriteConcern.Acknowledged;

                        break;
                    case DbWriteConcernType.WMajority:
                        res.WriteConcern = MongoDB.Driver.WriteConcern.WMajority;

                        break;
                    case DbWriteConcernType.W1:
                        res.WriteConcern = MongoDB.Driver.WriteConcern.W1;

                        break;
                    case DbWriteConcernType.W2:
                        res.WriteConcern = MongoDB.Driver.WriteConcern.W2;

                        break;
                    case DbWriteConcernType.W3:
                        res.WriteConcern = MongoDB.Driver.WriteConcern.W3;

                        break;
                    case DbWriteConcernType.CustomCount:
                        res.WriteConcern = new WriteConcern(CustomWriteAcknowledgeCount);

                        break;
                    default:
                        break;
                }

                switch (ReadPreference)
                {
                    case DbReadPrefrenceType.Nearest:
                        res.ReadPreference = MongoDB.Driver.ReadPreference.Nearest;
                        break;
                    case DbReadPrefrenceType.Primary:
                        res.ReadPreference = MongoDB.Driver.ReadPreference.Primary;

                        break;
                    case DbReadPrefrenceType.PrimaryPreferred:
                        res.ReadPreference = MongoDB.Driver.ReadPreference.PrimaryPreferred;

                        break;
                    case DbReadPrefrenceType.Secondary:
                        res.ReadPreference = MongoDB.Driver.ReadPreference.Secondary;

                        break;
                    case DbReadPrefrenceType.SecondaryPreferred:
                        res.ReadPreference = MongoDB.Driver.ReadPreference.SecondaryPreferred;

                        break;
                    default:
                        break;
                }

                return res;
            }

        }
    }
}
