using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Models.Query
{
    public class AggregateSortModel
    {
        public string FieldName { get; set; }
        public bool Ascending { get; set; }
    }
}
