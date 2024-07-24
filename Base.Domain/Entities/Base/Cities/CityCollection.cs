using Base.Domain.Audities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Entities.Base.Cities
{

    public class CityCollection : BaseEntity
    {
        public CityCollection() : base() { }

        /// <summary>
        /// City Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// City's Province Id
        /// </summary>
        public ObjectId ProvinceId { get; set; }

        public ProvinceCollection Province { get; set; }
    }
}
