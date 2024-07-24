using Base.Domain.Audities;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Entities.Base.Cities
{

    public class ProvinceCollection : BaseEntity
    {
        public ProvinceCollection() : base() { }
        /// <summary>
        /// Province Name
        /// </summary>
        public string Name { get; set; }

        #region Navigations

        public List<CityCollection> Cities { get; set; }

        #endregion
    }
}
