using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Shared.ViewModels.Permisison
{
    public class CustomAction
    {
        public CustomAction(string action, string controller, string area)
        {
            Action = action;
            Controller = controller;
            Area = area;
        }

        public string Action { get; set; }
        public string Controller { get; set; }
        public string Area { get; set; }

    }
}
