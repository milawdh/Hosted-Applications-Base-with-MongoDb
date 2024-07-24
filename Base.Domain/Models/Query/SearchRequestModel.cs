using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Models.Query
{
    public class SearchRequestModel
    {
        public int StartCount { get; set; } = 0;
        public int EndCount { get; set; } = 19;
        public string FilterJson { get; set; } = "{}";

        public AggregateDynamicFilterModel GetFilterModel()
        {
            if (FilterJson == "{}")
                return null;

            return JsonConvert.DeserializeObject<AggregateDynamicFilterModel>(FilterJson);
        }
        public string SortJson { get; set; } = "{}";

        public AggregateSortModel GetSortModel()
        {
            if (SortJson is "{}")
                return null;

            return JsonConvert.DeserializeObject<AggregateSortModel>(SortJson);

        }
    }
}
