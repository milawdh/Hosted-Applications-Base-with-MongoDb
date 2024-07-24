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
    /// Has Creator of Entity Details
    /// </summary>
    /// <typeparam name="TKey">The Key Type of Entity</typeparam>
    public interface ICreationAuditedEntity<TKey>
    {
        public UserType CreatorUserType { get; set; }
        public ObjectId CreatedById { get; set; }
        public DateTime CreatedOn { get; set; }


        public UserCollection CreatorUser { get; set; }
        public CustomerCollection CreatorCustomer { get; set; }
    }
    /// <summary>
    /// Has Creator of Entity Details
    /// </summary>
    /// <typeparam name="TKey">The Key Type of Entity</typeparam>
    public abstract class CreationAuditedEntity<TKey> : BaseEntity<TKey>, ICreationAuditedEntity<TKey>
    {
        public UserType CreatorUserType { get; set; }
        public ObjectId CreatedById { get; set; }
        public DateTime CreatedOn { get; set; }
        public UserCollection CreatorUser { get; set; }
        public CustomerCollection CreatorCustomer { get; set; }
    }

    /// <summary>
    ///  The Defualt Implemention of <see cref="CreationAuditedEntity{TKey}"/>
    /// </summary>
    public abstract class CreationAuditedEntity : CreationAuditedEntity<ObjectId>
    {
    }

}
