using Base.Domain.DomainExceptions;
using ElmahCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.WebFrameWork.ExceptionHandeling
{
    public class CustomElmahErrorFilter : IErrorFilter
    {
        public void OnErrorModuleFiltering(object sender, ExceptionFilterEventArgs args)
        {
            if (args.Exception is DomainException)
                args.Dismiss();
        }
    }
}
