using Base.Domain.Models.OutPutModels;
using Base.Domain.Shared.ViewModels.SMS;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Contract.Sms
{
    public interface ISmsService
    {
        ServiceResult SendSms(SendSmsRequest model);
    }
}
