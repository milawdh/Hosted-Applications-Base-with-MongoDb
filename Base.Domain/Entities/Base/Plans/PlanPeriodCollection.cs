using Base.Domain.Audities;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Entities.Base.Plans
{

    public class PlanPeriodCollection : FullAuditedEntity
    {
        public PlanPeriodCollection() : base()
        {

        }
        /// <summary>
        /// Plan's Period Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Plan Period Days Count
        /// </summary>
        public string DayCount { get; set; }

        /// <summary>
        /// Plan's Period Discount Percent
        /// </summary>
        public int DiscountPercent { get; set; }

        /// <summary>
        /// Plan Period's Order Number to sort
        /// </summary>
        public int Order { get; set; }

    }
}
