using Base.Domain.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Utils.Configuration
{
    public class AppSettings : ISingleton
    {

        public void Add(Type type, ISetting config)
        {
            if (!typeof(ISetting).IsAssignableFrom(type))
                throw new ArgumentException($"The Type: {type} Must Inherit from `IConfig`!");

            settings.Add(type, config);
        }


        public TSetting Get<TSetting>()
            where TSetting : ISetting
        {
            if (settings.Any(x => x.Key == typeof(TSetting)))
                return (TSetting)settings[typeof(TSetting)];

            return (TSetting)Activator.CreateInstance(typeof(TSetting));
        }

        public Dictionary<Type, ISetting> settings = new Dictionary<Type, ISetting>();
    }
}
