using Base.Domain.Audities;
using Base.Domain.Enums.Base.Sms;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Entities.Base.Sms
{

    public class SmsArchiveCollention : BaseEntity
    {
        public string PhoneNumber { get; set; } = null!;

        public PhoneNumberStatus Status { get; set; } = PhoneNumberStatus.Undefined;

        public SmsCodeStoreEmbeeded Code { get; set; }
    }

    public class SmsCodeStoreEmbeeded
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Code { get; set; }
        public DateTime? SentDate { get; set; }

        public DateTime ExpireDate { get; set; }
        public DateTime RequestedDate { get; set; }

        public SmsCodeStatus Status { get; set; }

        public SmsVerificationCodeType CodeType { get; set; }
    }
}
