using Amazon.Runtime.Internal;

using MongoDB.Driver;
using MongoDB.Driver.Linq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SharpCompress.Common;
using Base.Domain.RepositoriesApi.Repo;
using Base.Domain.RepositoriesApi.Core;
using Base.Domain.Audities;

namespace Base.Infrasucture.Repository.Impl.MainRepos
{
    public partial class MainRepo<TEntity, TKey> : IMainRepo<TEntity, TKey> where TEntity : IBaseEntity<TKey>
    {
        #region Force

        /// <summary>
        /// Marks Deleting Explicitly Entity Operation On BulkWirte! Need To Call <see cref="ICore.SaveChange()"/>
        /// </summary>
        /// <param name="entity">Entity To Remove Explicitly From DataBase</param>
        public void DeleteForce(TEntity entity, bool igonreValidation = false)
        {
            if (!igonreValidation)
                entity.ValidateDelete(_validationContext);

            var filter = Builders<TEntity>.Filter.Eq(x => x.Id, entity.Id);
            WritesQue.Append(new DeleteOneModel<TEntity>(filter));

            PublishOnWriteAddedEvent();
        }

        /// <summary>
        /// Marks Deleting Explicitly Entity Operation On BulkWirte! Need To Call <see cref="ICore.SaveChange()"/>
        /// </summary>
        /// <param name="Id">Entity Id To Remove Explicitly From DataBase</param>
        public void DeleteForce(TKey Id)
        {
            var filter = Builders<TEntity>.Filter.Eq(x => x.Id, Id);
            WritesQue.Append(new DeleteOneModel<TEntity>(filter));

            PublishOnWriteAddedEvent();
        }

        /// <summary>
        /// Marks Deleting Explicitly Entities Operation On BulkWirte! Need To Call <see cref="ICore.SaveChange()"/>
        /// </summary>
        /// <param name="entities">Entities To Remove Explicitly From DataBase</param>
        public void DeleteForceMany(IEnumerable<TEntity> entities, bool igonreValidation = false)
        {

            if (!igonreValidation)
                foreach (var entity in entities)
                    entity.ValidateDelete(_validationContext);

            var filter = Builders<TEntity>.Filter.In(x => x.Id, entities.Select(a => a.Id));

            WritesQue.Append(new DeleteManyModel<TEntity>(filter));

            PublishOnWriteAddedEvent();
        }


        /// <summary>
        /// Marks Deleting Explicitly Entities Operation On BulkWirte! Need To Call <see cref="ICore.SaveChange()"/>
        /// </summary>
        /// <param name="Ids">Entity Ids To Remove Explicitly From DataBase</param>
        public void DeleteForceMany(IEnumerable<TKey> Ids)
        {
            var filter = Builders<TEntity>.Filter.In(x => x.Id, Ids);

            WritesQue.Append(new DeleteManyModel<TEntity>(filter));

            PublishOnWriteAddedEvent();
        }

        #endregion

        #region Soft Deletes

        /// <summary>
        /// Marks Soft Deleting Entity Operation On BulkWrite! Need To Call <see cref="ICore.SaveChange()"/>
        ///  (If Entity wa Not Assinable from <see cref="IDeletetationAuditedEntity{TKey}"/> Will Be Deleted Explicitly!)
        /// </summary>
        /// <param name="entity">Entity To Remove As Soft</param>
        public void Delete(TEntity entity, bool igonreValidation = false)
        {

            if (entity is IDeletetationAuditedEntity<TKey>)
            {
                AppendSoftDeleteAudited(ref entity);

                if (!igonreValidation)
                    entity.ValidateDelete(_validationContext);

                var request = new ReplaceOneModel<TEntity>(Builders<TEntity>.Filter.Eq(x => x.Id, entity.Id), entity);
                WritesQue.Add(request);
            }
            else
                DeleteForce(entity);

            PublishOnWriteAddedEvent();
        }

        /// <summary>
        /// Marks Soft Deleting Entity By It's Id Operation On BulkWrite! Need To Call <see cref="ICore.SaveChange()"/>
        ///  (If Entity wa Not Assinable from <see cref="IDeletetationAuditedEntity{TKey}"/> Will Be Deleted Explicitly!)
        /// </summary>
        /// <param name="Id">Entity Id To Remove As Soft</param>
        public void Delete(TKey Id)
        {
            if (typeof(TEntity).IsAssignableFrom(typeof(IDeletetationAuditedEntity<TKey>)))
            {
                var filter = Builders<TEntity>.Filter.Eq(x => x.Id, Id);
                var defination = AppendSoftDeleteAudited();

                var request = new UpdateOneModel<TEntity>(filter, defination);
                WritesQue.Add(request);
            }
            else
                DeleteForce(Id);

            PublishOnWriteAddedEvent();
        }

        /// <summary>
        /// Marks Soft Deleting Many Entities Operation On BulkWrite! Need To Call <see cref="ICore.SaveChange()"/>
        ///  (If Entity wa Not Assinable from <see cref="IDeletetationAuditedEntity{TKey}"/> Will Be Deleted Explicitly!)
        /// </summary>
        /// <param name="entities">Entities To Remove As Soft</param>
        public void DeleteMany(IEnumerable<TEntity> entities, bool igonreValidation = false)
        {
            if (typeof(TEntity).IsAssignableFrom(typeof(IDeletetationAuditedEntity<TKey>)))
            {
                var requests = AppendSoftDeleteAudited(ref entities);

                if (!igonreValidation)
                    foreach (var item in entities)
                        item.ValidateDelete(_validationContext);

                foreach (var item in requests)
                {
                    WritesQue.Add(item);
                }
            }
            else
                DeleteForceMany(entities);

            PublishOnWriteAddedEvent();
        }


        /// <summary>
        /// Marks Soft Deleting Many Entities By Their Ids Operation On BulkWrite! Need To Call <see cref="ICore.SaveChange()"/>
        ///  (If Entity wa Not Assinable from <see cref="IDeletetationAuditedEntity{TKey}"/> Will Be Deleted Explicitly!)
        /// </summary>
        /// <param name="Ids">Entity Ids To Remove As Soft</param>
        public void DeleteMany(IEnumerable<TKey> Ids)
        {
            if (typeof(TEntity).IsAssignableFrom(typeof(IDeletetationAuditedEntity<TKey>)))
            {
                var filter = Builders<TEntity>.Filter.In(x => x.Id, Ids);
                var defination = AppendSoftDeleteAudited();

                var request = new UpdateManyModel<TEntity>(filter, defination);

                WritesQue.Add(request);
            }
            else
                DeleteForceMany(Ids);

            PublishOnWriteAddedEvent();
        }

        #endregion

    }
}