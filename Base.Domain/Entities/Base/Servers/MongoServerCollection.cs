using Base.Domain.Audities;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Entities.Base.Servers
{

    public class MongoServerCollection : BaseEntity
    {
        /// <summary>
        /// MongoServer's Tilte
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Mongo Server's Hashed Connection string
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// If it is active it will be set for the that moment Purchasing Tenant's Server
        /// </summary>
        public bool IsActive { get; set; }
    }
}
