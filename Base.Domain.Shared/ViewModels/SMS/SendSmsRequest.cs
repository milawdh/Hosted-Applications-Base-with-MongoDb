using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Shared.ViewModels.SMS
{
    public class SendSmsRequest
    {
        public string PhoneNumber { get; set; }
        public string Body { get; set; }
    }
}
