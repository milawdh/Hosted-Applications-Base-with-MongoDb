using Base.Domain.RepositoriesApi.Context;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Audities
{
    /// <summary>
    /// Basic Entity Abstraction
    /// </summary>
    /// <typeparam name="TKey">The Key Type of Entity</typeparam>
    public interface IBaseEntity<TKey> : IBaseDomainValidation, IDomainDataSeeds
    {
        public TKey Id { get; set; }
    }

    /// <summary>
    /// Basic Entity Abstraction
    /// </summary>
    /// <typeparam name="TKey">The Key Type of Entity</typeparam>
    public abstract class BaseEntity<TKey> : IBaseEntity<TKey>
    {
        [BsonId]
        public TKey Id { get; set; }

        public virtual void SeedData(DomainSeedContext seedContext)
        {
        }

        public virtual void ValidateAdd(DomainValidationContext validationContext)
        {
        }

        public virtual void ValidateDelete(DomainValidationContext validationContext)
        {
        }

        public virtual void ValidateUpdate(DomainValidationContext validationContext)
        {
        }


    }

    /// <summary>
    /// Default Implemention of <see cref="BaseEntity{TKey}"/> with  Type of Key : <see cref="ObjectId"/>
    /// </summary>
    public abstract class BaseEntity : BaseEntity<ObjectId>
    {
    }
}
