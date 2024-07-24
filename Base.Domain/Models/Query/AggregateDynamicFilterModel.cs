using Base.Domain.Enums.Base.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Models.Query
{
    public class AggregateDynamicFilterModel
    {
        public List<AggregateDynamicFilterItem> FilterItems { get; set; }
    }
    public class AggregateDynamicFilterItem
    {
        public FilterType FilterType { get; set; }
        public bool FilterIsContain { get; set; }
        public string FieldName { get; set; }
        public string FilterStrValue { get; set; }
        public object FilterCompareValue { get; set; }
        public long FilterIntValue { get; set; }
        public object[] FilterArrayValue { get; set; }
        public string[] FilterStrArrayValue { get; set; }

        public AggregateFilterRangeValue RangeValue { get; set; }
    }

    public class AggregateFilterRangeValue
    {
        public object FromValue { get; set; }
        public bool IsFromEqual { get; set; }

        public object UntilValue { get; set; }
        public bool IsUntileEqual { get; set; }
    }
}
