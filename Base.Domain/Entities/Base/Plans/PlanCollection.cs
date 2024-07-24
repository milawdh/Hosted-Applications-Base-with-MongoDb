using Base.Domain.Audities;
using Base.Domain.Enums.Base.Plans;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Entities.Base.Plans
{

    public class PlanCollection : FullAuditedEntity
    {
        public PlanCollection() : base()
        {

        }
        /// <summary>
        /// Plan's Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Plan's Desciption <Html>
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Plan's Slogan
        /// </summary>
        public string Slogan { get; set; }

        /// <summary>
        /// Plan's Period Id
        /// </summary>
        public ObjectId PeriodId { get; set; }
        public PlanPeriodCollection Period { get; set; }
        /// <summary>
        /// Plan's Type
        /// </summary>
        public PlanType Type { get; set; }

        /// <summary>
        /// Plan's Allowed Agency Count
        /// </summary>
        public int AgencyCount { get; set; }

        /// <summary>
        /// Plan's Allowed Product Count
        /// </summary>
        public int ProductCount { get; set; }

        /// <summary>
        /// Plan's Base Price
        /// </summary>
        public double BasePrice { get; set; }

        /// <summary>
        /// Plan's Each More User Price
        /// </summary>
        public double UserPrice { get; set; }

        /// <summary>
        /// Plan's Discount Percent
        /// </summary>
        public int DiscountPercent { get; set; } = 0;

        /// <summary>
        /// Is this plan popular? -> it will be configured manually by admins
        /// </summary>
        public bool IsPopular { get; set; }

        /// <summary>
        /// Is this Plan unlimited User
        /// </summary>
        public bool HasUnlimitedUser { get; set; }

        /// <summary>
        /// Is this plan free
        /// </summary>
        public bool IsFreePlan { get; set; }

        /// <summary>
        /// Allowed Maximum DataBase Size for this plan
        /// </summary>
        public double MaximumDbSize { get; set; }

        /// <summary>
        /// Module Ids that this plan has
        /// </summary>
        public List<PlanModule> ModuleIds { get; set; }

    }
}
