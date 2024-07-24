using Base.Domain.Entities.Base.Users;
using Base.Domain.Enums.Base.User;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Audities
{
    public interface IModificationAuditedEntity<TKey> : IBaseEntity<TKey>
    {
        public UserType? ModifierUserType { get; set; }
        public ObjectId? ModifiedById { get; set; }
        public DateTime? ModifiedOn { get; set; }


        public UserCollection ModiferUser { get; set; }
        public CustomerCollection ModiferCustomer { get; set; }
    }

    /// <summary>
    /// Has Last Modifier of Entity Details
    /// </summary>
    /// <typeparam name="TKey">The Key Type of Entity</typeparam>
    public abstract class ModificationAuditedEntity<TKey> : BaseEntity<TKey>, IModificationAuditedEntity<TKey>
    {
        public UserType? ModifierUserType { get; set; }
        public ObjectId? ModifiedById { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public UserCollection ModiferUser { get; set; }
        public CustomerCollection ModiferCustomer { get; set; }
    }

    /// <summary>
    /// The Defualt Implemention of <see cref="ModificationAuditedEntity{TKey}"/>
    /// </summary>
    public abstract class ModificationAuditedEntity : ModificationAuditedEntity<ObjectId>
    {
    }


}
