using Base.Domain.Audities;
using Base.Domain.Entities.Base.Servers;
using Base.Domain.Models.EntityModels.Plans;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Entities.Base.Tenants
{

    public class TenantCollection : FullAuditedEntity
    {
        /// <summary>
        /// Tenant's Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Tenat Identity String for db!
        /// </summary>
        public string Identity { get; set; }

        /// <summary>
        /// Tenant Domain
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// Tenant's moment of purchase Details
        /// </summary>
        public TenantPlanModel PlanDetails { get; set; }

        /// <summary>
        /// Tenant's Allowed users to have
        /// </summary>
        public int AllowedUserCount { get; set; }

        /// <summary>
        /// Tenant Plan's Renewal date
        /// </summary>
        public DateTime RenewalDate { get; set; }

        /// <summary>
        /// Tenant Current DataBase UsingSize
        /// </summary>
        public int UsedSize { get; set; }

        /// <summary>
        /// Tenant's Mongo Server to Connect
        /// </summary>
        public ObjectId MongoServerID { get; set; }

        public MongoServerCollection Server { get; set; }
    }
}
