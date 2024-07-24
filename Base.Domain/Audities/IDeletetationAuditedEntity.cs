using Base.Domain.Entities.Base.Users;
using Base.Domain.Enums.Base.User;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Audities
{

    /// <summary>
    /// Has Deletor of Entity Details
    /// </summary>
    /// <typeparam name="TKey">The Key Type of Entity</typeparam>
    public interface IDeletetationAuditedEntity<TKey> : IBaseEntity<TKey>
    {
        public bool IsDeleted { get; set; }
        public UserType? DeleterUserType { get; set; }
        public ObjectId? DeletedById { get; set; }
        public DateTime? DeletedDate { get; set; }

        public UserCollection DeleterUser { get; set; }
        public CustomerCollection DeleterCustomer { get; set; }
    }


    /// <summary>
    /// Has Deletor of Entity Details
    /// </summary>
    /// <typeparam name="TKey">The Key Type of Entity</typeparam>
    public abstract class DeletetationAuditedEntity<TKey> : BaseEntity<TKey>, IDeletetationAuditedEntity<TKey>
    {
        public bool IsDeleted { get; set; } = false;
        public UserType? DeleterUserType { get; set; }
        public ObjectId? DeletedById { get; set; }
        public DateTime? DeletedDate { get; set; }
        public UserCollection DeleterUser { get; set; }
        public CustomerCollection DeleterCustomer { get; set; }
    }


    /// <summary>
    /// The Defualt Implemention of <see cref="DeletetationAuditedEntity{TKey}"/>
    /// </summary>
    public abstract class DeletetationAuditedEntity : DeletetationAuditedEntity<ObjectId>
    {
    }



}
