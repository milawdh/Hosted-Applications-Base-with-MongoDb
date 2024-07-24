using Base.Contract.Sms;
using Base.Domain.Models.OutPutModels;
using Base.Domain.Shared.ViewModels.SMS;

namespace Base.ServiceLayer.Sms
{
    public class SmsService : ISmsService
    {
        public ServiceResult SendSms(SendSmsRequest model)
        {
            //TODO : Implement It with a panel
            return new ServiceResult();
        }
    }
}
