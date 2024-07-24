using Amazon.Runtime.Internal;


using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Base.Domain.RepositoriesApi.Repo;
using Base.Domain.RepositoriesApi.Core;
using Base.Domain.Models.OutPutModels;
using Base.Domain.Audities;

namespace Base.Infrasucture.Repository.Impl.MainRepos
{
    public partial class MainRepo<TEntity, TKey> : IMainRepo<TEntity, TKey> where TEntity : IBaseEntity<TKey>

    {
        #region Delete By Entity

        #region Delete One
        /// <summary>
        /// Deletes Entity Explicitly From DataBase At The Calling Time Implicitly!
        /// No Need To Call <see cref="ICore.SaveChange()"/>
        /// </summary>
        /// <param name="entity">Entity To Remove Explicitly</param>
        /// <param name="deleteOptions">Deleting Options</param>
        public ServiceResult<DeleteResult> DeleteForceImplicit(TEntity entity, DeleteOptions deleteOptions = null, bool igonreValidation = false)
        {
            if (!igonreValidation)
                entity.ValidateDelete(_validationContext);

            var predicate = Builders<TEntity>.Filter.Eq(nameof(BaseEntity.Id), entity.Id);

            var result = _mongoCollection.DeleteOne(ClientSession, predicate, deleteOptions);
            if (result.DeletedCount <= 0)
                return new ServiceResult<DeleteResult>("خطا در حذف موجودیت!");

            return new ServiceResult<DeleteResult>(result);
        }

        /// <summary>
        /// Replaces Entity With Softly Deleted Version At The Calling Time Implicitly!
        /// No Need To Call <see cref="ICore.SaveChange()"/>
        ///  (If Entity was Not Assinable from <see cref="IDeletetationAuditedEntity{TKey}"/> Will Be Deleted Explicitly!)
        /// </summary>
        /// <param name="entity">Entity To Remove As Soft</param>
        /// <param name="softDeleteOptions">ReplaceOne Options</param>
        public ServiceResult DeleteImplicit(TEntity entity, ReplaceOptions softDeleteOptions = null, bool igonreValidation = false)
        {
            if (entity is IDeletetationAuditedEntity<TKey>)
            {
                if (!igonreValidation)
                    entity.ValidateDelete(_validationContext);

                AppendSoftDeleteAudited(ref entity);
                var predicate = Builders<TEntity>.Filter.Eq(x => x.Id, entity.Id);

                var result = _mongoCollection.ReplaceOne(ClientSession, predicate, entity, softDeleteOptions);
                return new ServiceResult<ReplaceOneResult>(result);
            }

            DeleteForce(entity);
            return new ServiceResult();
        }
        /// <summary>
        /// Deletes Entity Explicitly From DataBase At The Calling Time Asynchoronously & Implicitly!
        /// No Need To Call <see cref="ICore.SaveChange()"/>
        /// </summary>
        /// <param name="entity">Entity To Remove Explicitly</param>
        /// <param name="deleteOptions">Deleting Options</param>
        public async Task<ServiceResult<DeleteResult>> DeleteForceImplicitAsync(TEntity entity, DeleteOptions deleteOptions = null, bool igonreValidation = false)
        {
            if (!igonreValidation)
                entity.ValidateDelete(_validationContext);

            var predicate = Builders<TEntity>.Filter.Eq(x => x.Id, entity.Id);

            var result = await _mongoCollection.DeleteOneAsync(ClientSession, predicate, deleteOptions);

            return new ServiceResult<DeleteResult>(result);
        }

        /// <summary>
        /// Replaces Entity With Softly Deleted Version At The Calling Time Asynchoronously & Implicitly!
        ///  (If Entity was Not Assinable from <see cref="IDeletetationAuditedEntity{TKey}"/> Will Be Deleted Explicitly!)
        /// No Need To Call <see cref="ICore.SaveChange()"/>
        /// </summary>
        /// <param name="entity">Entity To Remove As Soft</param>
        /// <param name="softDeleteOptions">ReplaceOne Options</param>
        public async Task<ServiceResult> DeleteImplicitAsync(TEntity entity, ReplaceOptions softDeleteOptions = null, bool igonreValidation = false)
        {
            if (entity is IDeletetationAuditedEntity<TKey>)
            {
                if (!igonreValidation)
                    entity.ValidateDelete(_validationContext);

                AppendSoftDeleteAudited(ref entity);
                var predicate = Builders<TEntity>.Filter.Eq(x => x.Id, entity.Id);

                var result = await _mongoCollection.ReplaceOneAsync(ClientSession, predicate, entity, softDeleteOptions);
                return new ServiceResult();
            }

            var deleteResult = await DeleteForceImplicitAsync(entity);
            return new ServiceResult(deleteResult);
        }

        #endregion

        #region DeleteMany
        /// <summary>
        /// Deletes Many Entity Explicitly From DataBase At The Calling Time Implicitly!
        /// No Need To Call <see cref="ICore.SaveChange()"/>
        /// </summary>
        /// <param name="entities">Entities You Want To Delete!</param>
        /// <param name="deleteOptions">Deleting Operation Options</param>
        /// <returns></returns>
        public ServiceResult<DeleteResult> DeleteManyForceImplicit(IEnumerable<TEntity> entities, DeleteOptions deleteOptions = null, bool igonreValidation = false)
        {
            if (!igonreValidation)
                foreach (TEntity entity in entities)
                    entity.ValidateDelete(_validationContext);

            List<TKey> Ids = entities.Select(x => x.Id).ToList();
            var predicate = Builders<TEntity>.Filter.In(nameof(BaseEntity.Id), Ids);

            var result = _mongoCollection.DeleteMany(ClientSession, predicate, deleteOptions);

            return new ServiceResult<DeleteResult>(result);
        }

        /// <summary>
        /// Deletes Many Entity Explicitly From DataBase At The Calling Time Implicitly!
        ///  (If Entity was Not Assinable from <see cref="IDeletetationAuditedEntity{TKey}"/> Will Be Deleted Explicitly!)
        /// No Need To Call <see cref="ICore.SaveChange()"/>
        /// </summary>
        /// <param name="entities">Entities You Want To Delete!</param>
        /// <param name="softDeleteOptions">Update Operation Options</param>
        /// <returns></returns>
        public ServiceResult DeleteManyImplicit(IEnumerable<TEntity> entities, UpdateOptions softDeleteOptions = null, bool igonreValidation = false)
        {



            if (typeof(TEntity).IsAssignableFrom(typeof(IDeletetationAuditedEntity<TKey>)))
            {
                if (!igonreValidation)
                    foreach (TEntity entity in entities)
                        entity.ValidateDelete(_validationContext);

                var filter = Builders<TEntity>.Filter.In(x => x.Id, entities.Select(x => x.Id).ToList());
                var defination = AppendSoftDeleteAudited();

                var result = _mongoCollection.UpdateMany(ClientSession, filter, defination, softDeleteOptions);

                return new ServiceResult();
            }
            var deleteResult = DeleteManyForceImplicit(entities);

            return new ServiceResult(deleteResult);
        }
        public async Task<ServiceResult<DeleteResult>> DeleteManyForceImplicitAsync(IEnumerable<TEntity> entities, DeleteOptions deleteOptions = null, bool igonreValidation = false)
        {
            if (!igonreValidation)
                foreach (TEntity entity in entities)
                    entity.ValidateDelete(_validationContext);

            List<TKey> Ids = entities.Select(x => x.Id).ToList();
            var predicate = Builders<TEntity>.Filter.In(nameof(BaseEntity.Id), Ids);

            var result = await _mongoCollection.DeleteManyAsync(ClientSession, predicate, deleteOptions);

            return new ServiceResult<DeleteResult>(result);
        }

        public async Task<ServiceResult> DeleteManyImplicitAsync(IEnumerable<TEntity> entities, UpdateOptions softDeleteOptions = null, bool igonreValidation = false)
        {
            if (typeof(TEntity).IsAssignableFrom(typeof(IDeletetationAuditedEntity<TKey>)))
            {
                if (!igonreValidation)
                    foreach (TEntity entity in entities)
                        entity.ValidateDelete(_validationContext);

                var filter = Builders<TEntity>.Filter.In(x => x.Id, entities.Select(x => x.Id).ToList());
                var defination = AppendSoftDeleteAudited();

                var result = await _mongoCollection.UpdateManyAsync(ClientSession, filter, defination, softDeleteOptions);

                return new ServiceResult();
            }
            var deleteResult = await DeleteManyForceImplicitAsync(entities);

            return new ServiceResult(deleteResult);
        }

        #endregion

        #endregion


        #region Delete By Predicates

        #region Delete One

        public ServiceResult<DeleteResult> DeleteForceImplicit(FilterDefinition<TEntity> predicate, DeleteOptions deleteOptions = null)
        {
            var result = _mongoCollection.DeleteOne(ClientSession, predicate, deleteOptions);

            return new ServiceResult<DeleteResult>(result);
        }

        public ServiceResult<DeleteResult> DeleteForceImplicit(Expression<Func<TEntity, bool>> predicate, DeleteOptions deleteOptions = null)
        {
            var result = _mongoCollection.DeleteOne(ClientSession, predicate, deleteOptions);

            return new ServiceResult<DeleteResult>(result);
        }

        public ServiceResult DeleteImplicit(FilterDefinition<TEntity> predicate)
        {
            if (typeof(TEntity).IsAssignableFrom(typeof(IDeletetationAuditedEntity<TKey>)))
            {
                var defination = AppendSoftDeleteAudited();
                _mongoCollection.UpdateOne(predicate, defination);
                return new ServiceResult();
            }

            var deleteResult = DeleteForceImplicit(predicate);
            return new ServiceResult(deleteResult);
        }

        public ServiceResult DeleteImplicit(Expression<Func<TEntity, bool>> predicate, DeleteOptions deleteOptions = null)
        {
            if (typeof(TEntity).IsAssignableFrom(typeof(IDeletetationAuditedEntity<TKey>)))
            {
                var defination = AppendSoftDeleteAudited();
                _mongoCollection.UpdateOne(predicate, defination);
                return new ServiceResult();
            }

            var deleteResult = DeleteForceImplicit(predicate);
            return new ServiceResult(deleteResult);
        }

        public async Task<ServiceResult<DeleteResult>> DeleteForceImplicitAsync(FilterDefinition<TEntity> predicate, DeleteOptions deleteOptions = null)
        {
            var result = await _mongoCollection.DeleteOneAsync(ClientSession, predicate, deleteOptions);

            return new ServiceResult<DeleteResult>(result);
        }

        public async Task<ServiceResult<DeleteResult>> DeleteForceImplicitAsync(Expression<Func<TEntity, bool>> predicate, DeleteOptions deleteOptions = null)
        {
            var result = await _mongoCollection.DeleteOneAsync(ClientSession, predicate, deleteOptions);

            return new ServiceResult<DeleteResult>(result);
        }

        public async Task<ServiceResult> DeleteImplicitAsync(FilterDefinition<TEntity> predicate)
        {
            if (typeof(TEntity).IsAssignableFrom(typeof(IDeletetationAuditedEntity<TKey>)))
            {
                var defination = AppendSoftDeleteAudited();
                await _mongoCollection.UpdateOneAsync(predicate, defination);
                return new ServiceResult();
            }
            var deleteResult = await DeleteForceImplicitAsync(predicate);
            return new ServiceResult(deleteResult);

        }

        public async Task<ServiceResult> DeleteImplicitAsync(Expression<Func<TEntity, bool>> predicate)
        {
            if (typeof(TEntity).IsAssignableFrom(typeof(IDeletetationAuditedEntity<TKey>)))
            {
                var defination = AppendSoftDeleteAudited();
                await _mongoCollection.UpdateOneAsync(predicate, defination);
                return new ServiceResult();
            }
            var deleteResult = await DeleteForceImplicitAsync(predicate);
            return new ServiceResult(deleteResult);
        }

        #endregion

        #region DeleteMany

        public ServiceResult<DeleteResult> DeleteManyForceImplicit(FilterDefinition<TEntity> predicate, DeleteOptions deleteOptions = null)
        {
            var result = _mongoCollection.DeleteMany(ClientSession, predicate, deleteOptions);
            return new ServiceResult<DeleteResult>(result);
        }

        public ServiceResult<DeleteResult> DeleteManyForceImplicit(Expression<Func<TEntity, bool>> predicate, DeleteOptions deleteOptions = null)
        {
            var result = _mongoCollection.DeleteMany(ClientSession, predicate, deleteOptions);
            return new ServiceResult<DeleteResult>(result);
        }

        public ServiceResult DeleteManyImplicit(FilterDefinition<TEntity> predicate)
        {
            if (typeof(TEntity).IsAssignableFrom(typeof(IDeletetationAuditedEntity<TKey>)))
            {
                var defination = AppendSoftDeleteAudited();
                _mongoCollection.UpdateMany(predicate, defination);
                return new ServiceResult();
            }
            var deleteResult = DeleteManyForceImplicit(predicate);
            return new ServiceResult(deleteResult);
        }

        public ServiceResult DeleteManyImplicit(Expression<Func<TEntity, bool>> predicate)
        {
            if (typeof(TEntity).IsAssignableFrom(typeof(IDeletetationAuditedEntity<TKey>)))
            {
                var defination = AppendSoftDeleteAudited();
                _mongoCollection.UpdateMany(predicate, defination);
                return new ServiceResult();
            }
            var deleteResult = DeleteManyForceImplicit(predicate);
            return new ServiceResult(deleteResult);
        }

        public async Task<ServiceResult<DeleteResult>> DeleteManyForceImplicitAsync(FilterDefinition<TEntity> predicate, DeleteOptions deleteOptions = null)
        {
            var result = await _mongoCollection.DeleteManyAsync(ClientSession, predicate, deleteOptions);

            return new ServiceResult<DeleteResult>(result);
        }

        public async Task<ServiceResult<DeleteResult>> DeleteManyForceImplicitAsync(Expression<Func<TEntity, bool>> predicate, DeleteOptions deleteOptions = null)
        {
            var result = await _mongoCollection.DeleteManyAsync(ClientSession, predicate, deleteOptions);

            return new ServiceResult<DeleteResult>(result);
        }

        public async Task<ServiceResult> DeleteManyImplicitAsync(FilterDefinition<TEntity> predicate)
        {
            if (typeof(TEntity).IsAssignableFrom(typeof(IDeletetationAuditedEntity<TKey>)))
            {
                var defination = AppendSoftDeleteAudited();
                await _mongoCollection.UpdateManyAsync(predicate, defination);
                return new ServiceResult();
            }
            var deleteResult = await DeleteManyForceImplicitAsync(predicate);
            return new ServiceResult(deleteResult);
        }

        public async Task<ServiceResult> DeleteManyImplicitAsync(Expression<Func<TEntity, bool>> predicate)
        {
            if (typeof(TEntity).IsAssignableFrom(typeof(IDeletetationAuditedEntity<TKey>)))
            {
                var defination = AppendSoftDeleteAudited();
                await _mongoCollection.UpdateManyAsync(predicate, defination);
                return new ServiceResult();
            }

            var deleteResult = await DeleteManyForceImplicitAsync(predicate);
            return new ServiceResult(deleteResult);
        }

        #endregion

        #endregion
    }
}