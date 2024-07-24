using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Utils.Configuration
{
    public class SingleTon<T> : BaseSingleTon

        where T : class
    {
        public static T Instance
        {
            get
            {
                return (T)Values[typeof(T)];
            }
        }
    }

}
