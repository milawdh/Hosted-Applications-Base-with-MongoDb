using Base.Domain.Entities.Base.Users;
using Base.Domain.Enums.Base.User;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Audities
{
    /// <summary>
    /// Has Full Details of Creator & Last Modifier & Deletor of Entity's Details
    /// </summary>
    /// <typeparam name="TKey">The Key Type of Entity</typeparam>
    public interface IFullAuditedEntity<TKey> : ICreationAuditedEntity<TKey>, IModificationAuditedEntity<TKey>, IDeletetationAuditedEntity<TKey>, IBaseEntity<TKey>
    {
    }

    /// <summary>
    /// The Defualt Implemention of <see cref="FullAuditedEntity{TKey}"/>
    /// </summary>
    public abstract class FullAuditedEntity : FullAuditedEntity<ObjectId>
    {
    }

    /// <summary>
    /// Has Full Details of Creator & Last Modifier & Deletor of Entity's Details
    /// </summary>
    /// <typeparam name="TKey">The Key Type of Entity</typeparam>
    public abstract class FullAuditedEntity<TKey> : BaseEntity<TKey>, IFullAuditedEntity<TKey>
    {
        public UserType CreatorUserType { get; set; }
        public ObjectId CreatedById { get; set; }
        public DateTime CreatedOn { get; set; }

        public ObjectId? ModifiedById { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public UserType? ModifierUserType { get; set; }


        public bool IsDeleted { get; set; }
        public UserType? DeleterUserType { get; set; }
        public ObjectId? DeletedById { get; set; }
        public DateTime? DeletedDate { get; set; }
        public UserCollection CreatorUser { get; set; }
        public CustomerCollection CreatorCustomer { get; set; }
        public UserCollection ModiferUser { get; set; }
        public CustomerCollection ModiferCustomer { get; set; }
        public UserCollection DeleterUser { get; set; }
        public CustomerCollection DeleterCustomer { get; set; }
    }
}
