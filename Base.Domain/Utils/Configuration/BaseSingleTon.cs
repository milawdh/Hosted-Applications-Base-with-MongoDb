using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Utils.Configuration
{
    public class BaseSingleTon
    {
        public static void Add(Type type, object value)
        {
            if (Values.ContainsKey(type))
            {
                Values.Remove(type);
            }
            Values.Add(type, value);
        }



        public static Dictionary<Type, object> Values = new Dictionary<Type, object>();
    }
}
