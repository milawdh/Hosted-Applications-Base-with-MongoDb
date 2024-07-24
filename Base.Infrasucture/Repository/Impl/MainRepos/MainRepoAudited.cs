using Amazon.Runtime.Internal;


using MongoDB.Driver;
using MongoDB.Driver.Linq;

using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using Base.Domain.RepositoriesApi.Repo;
using Base.Domain.Audities;

namespace Base.Infrasucture.Repository.Impl.MainRepos
{
    public partial class MainRepo<TEntity, TKey> : IMainRepo<TEntity, TKey> where TEntity : IBaseEntity<TKey>
    {

        public void AppendUpdateAudited(ref UpdateDefinition<TEntity> updateDefinition)
        {
            if (typeof(TEntity).IsAssignableFrom(typeof(IDeletetationAuditedEntity<TKey>)))
            {

                var ModifiedById = nameof(ModificationAuditedEntity.ModifiedById);
                var ModifiedDateName = nameof(ModificationAuditedEntity.ModifiedOn);
                var ModifierUserTypeName = nameof(ModificationAuditedEntity.ModifierUserType);

                updateDefinition = Builders<TEntity>.Update.Combine(
                        updateDefinition,
                        Builders<TEntity>.Update.Set(ModifiedById, _baseUserInfoContext.UserId),
                        Builders<TEntity>.Update.Set(ModifiedDateName, DateTime.Now),
                        Builders<TEntity>.Update.Set(ModifierUserTypeName, _baseUserInfoContext.UserType)
                    );
            }
        }
        public void AppendUpdateAudited(ref TEntity entity)
        {
            if (entity is IModificationAuditedEntity<TKey>)
            {
                var audited = entity as IModificationAuditedEntity<TKey>;
                {
                    audited.ModifiedById = _baseUserInfoContext.UserId;
                    audited.ModifierUserType = _baseUserInfoContext.UserType;
                    audited.ModifiedOn = DateTime.Now;
                }
            }
        }

        public void AppendUpdateAudited(ref IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                if (entity is IModificationAuditedEntity<TKey>)
                {
                    var audited = entity as IModificationAuditedEntity<TKey>;
                    {
                        audited.ModifiedById = _baseUserInfoContext.UserId;
                        audited.ModifierUserType = _baseUserInfoContext.UserType;
                        audited.ModifiedOn = DateTime.Now;
                    }
                }
            }

        }


        /// <summary>
        /// Gets An Update Defination For Entity That Inherits <see cref="IDeletetationAuditedEntity{TKey}"/>!
        /// </summary>
        /// <returns></returns>
        private UpdateDefinition<TEntity> AppendSoftDeleteAudited()
        {
            if (typeof(TEntity).IsAssignableFrom(typeof(IDeletetationAuditedEntity<TKey>)))
            {
                var IsDeletedName = nameof(DeletetationAuditedEntity.IsDeleted);
                var DeletedByIdName = nameof(DeletetationAuditedEntity.DeletedById);
                var DeletedDateName = nameof(DeletetationAuditedEntity.DeletedDate);
                var DeleterUserTypeName = nameof(DeletetationAuditedEntity.DeleterUserType);

                var condtion = Builders<TEntity>.Update.Combine(
                         Builders<TEntity>.Update.Set(IsDeletedName, true),
                         Builders<TEntity>.Update.Set(DeletedByIdName, _baseUserInfoContext.UserId),
                         Builders<TEntity>.Update.Set(DeletedDateName, DateTime.Now),
                         Builders<TEntity>.Update.Set(DeleterUserTypeName, _baseUserInfoContext.UserType)
                     );
                return condtion;
            }
            return Builders<TEntity>.Update.Combine();
        }

        /// <summary>
        /// Gets An Update Defination For Entity That Inherits <see cref="IDeletetationAuditedEntity{TKey}"/>!
        /// </summary>
        /// <param name="entity">Entity You Want To Append Soft Delete</param>
        /// <returns></returns>
        public void AppendSoftDeleteAudited(ref TEntity entity)
        {
            if (entity is IDeletetationAuditedEntity<TKey>)
            {
                var auidited = entity as IDeletetationAuditedEntity<TKey>;

                auidited.DeletedById = _baseUserInfoContext.UserId;
                auidited.DeleterUserType = _baseUserInfoContext.UserType;
                auidited.DeletedDate = DateTime.Now;
                auidited.IsDeleted = true;
            }
        }

        /// <summary>
        /// Gets A List Of ReplaceOne Defination for Soft Delete! Entities That Inherits <see cref="IDeletetationAuditedEntity{TKey}"/>!
        /// </summary>
        /// <param name="entities">Entities You Want To Append Soft Delete</param>
        /// <returns></returns>
        public List<ReplaceOneModel<TEntity>> AppendSoftDeleteAudited(ref IEnumerable<TEntity> entities)
        {
            var requests = new List<ReplaceOneModel<TEntity>>();
            foreach (var entity in entities)
            {
                if (entity is IDeletetationAuditedEntity<TKey>)
                {
                    var audited = entity as IDeletetationAuditedEntity<TKey>;
                    audited.DeletedById = _baseUserInfoContext.UserId;
                    audited.DeleterUserType = _baseUserInfoContext.UserType;
                    audited.DeletedDate = DateTime.Now;
                    audited.IsDeleted = true;

                    var filter = Builders<TEntity>.Filter.Eq(x => x.Id, entity.Id);

                    requests.Add(new ReplaceOneModel<TEntity>(filter, entity));
                }
            }
            return requests;
        }

        /// <summary>
        /// Append Creation Shadow Properties to Entity That Inherits <see cref="ICreationAuditedEntity{TKey}"/>
        /// </summary>
        /// <param name="entity">Entity You Want to append Creation Shaow Properties</param>
        public void AppendAddAudited(ref TEntity entity)
        {
            if (entity is ICreationAuditedEntity<TKey>)
            {
                var audited = entity as ICreationAuditedEntity<TKey>;
                if (audited.CreatedById != ObjectId.Empty)
                    audited.CreatedById = _baseUserInfoContext.UserId;

                audited.CreatorUserType = _baseUserInfoContext.UserType;
                audited.CreatedOn = DateTime.Now;
            }
        }

        /// <summary>
        /// Append Creation Shadow Properties to Entities That Inherits <see cref="ICreationAuditedEntity{TKey}"/>
        /// </summary>
        /// <param name="entity">Entity You Want to append Creation Shaow Properties</param>
        public void AppendAddAudited(ref IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                if (entity is ICreationAuditedEntity<TKey>)
                {
                    var audited = entity as ICreationAuditedEntity<TKey>;
                    {
                        audited.CreatedById = _baseUserInfoContext.UserId;
                        audited.CreatorUserType = _baseUserInfoContext.UserType;
                        audited.CreatedOn = DateTime.Now;
                    }
                }
            }

        }



    }




}
