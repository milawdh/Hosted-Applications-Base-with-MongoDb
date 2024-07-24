using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Models.Query
{
    public class PagedResultModel<T>
    {
        public long TotalDataCount { get; set; }
        public int PageCount { get; set; }

        public List<T> Data { get; set; }
    }
}
