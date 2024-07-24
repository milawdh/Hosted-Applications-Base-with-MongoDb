using Base.Domain.Audities;
using Base.Domain.Entities.Base.Tenants;
using Base.Domain.Enums.Base.User;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Entities.Base.Users
{
    /// <summary>
    /// Application Customer Users
    /// </summary>

    public class CustomerCollection : FullAuditedEntity
    {
        /// <summary>
        /// Customer's FullName 
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Customers Gender : True if is Man , False if is Woman
        /// </summary>
        public GenderEnum Gender { get; set; }

        /// <summary>
        /// Customer's Mobile Phone number : +989000000000
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// Customer's Birth Date
        /// </summary>
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Customer's Main Address
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Customer's NationalCode
        /// </summary>
        public string NationalCode { get; set; }


        /// <summary>
        /// Customer's Hashed Password
        /// </summary>
        public string Password { get; set; }


        /// <summary>
        /// Customer's Temp Code for Password Forgotting
        /// </summary>
        public string TempCode { get; set; }

        /// <summary>
        /// Customerr's Request for Temp Code Count
        /// </summary>
        public int TempCodeCount { get; set; }

        /// <summary>
        /// DateTime For TempCode Expiration 
        /// </summary>
        public DateTime? TempCodeExpireDate { get; set; }

        /// <summary>
        /// Customer's LogginAttempted and faild Count!
        /// </summary>
        public int AccessFailedCount { get; set; }

        /// <summary>
        /// If Customer's Account Locks it's Locked until this date!
        /// </summary>
        public DateTime? AccountLockExpireDate { get; set; }

        /// <summary>
        /// Customer's CityId
        /// </summary>
        public int CityId { get; set; }

        /// <summary>
        /// The Tenant Ids that customer is in!
        /// </summary>
        public List<ObjectId> TenantIds { get; set; }

        public List<TenantCollection> Tenants { get; set; }

    }
}
