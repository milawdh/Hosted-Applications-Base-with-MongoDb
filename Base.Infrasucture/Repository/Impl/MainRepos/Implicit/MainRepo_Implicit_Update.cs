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
using Base.Domain.Models.OutPutModels;
using Base.Domain.Audities;

namespace Base.Infrasucture.Repository.Impl.MainRepos
{
    public partial class MainRepo<TEntity, TKey> : IMainRepo<TEntity, TKey> where TEntity : IBaseEntity<TKey>
    {

        #region Update

        public ServiceResult<UpdateResult> UpdateImplicit(TKey Id,
            UpdateDefinition<TEntity> updateDefinition, UpdateOptions updateOptions = null)
        {
            Expression<Func<TEntity, bool>> predicate = x => x.Id.Equals(Id);

            return UpdateImplicit(predicate, updateDefinition, updateOptions);
        }

        public ServiceResult<UpdateResult> UpdateImplicit(FilterDefinition<TEntity> predicate,
            UpdateDefinition<TEntity> updateDefinition, UpdateOptions updateOptions = null)
        {
            AppendUpdateAudited(ref updateDefinition);

            var result = _mongoCollection.UpdateOne(ClientSession, predicate, updateDefinition, updateOptions);

            return new ServiceResult<UpdateResult>(result);
        }

        public async Task<ServiceResult<UpdateResult>> UpdateImplicitAsync(FilterDefinition<TEntity> predicate,
            UpdateDefinition<TEntity> updateDefinition, UpdateOptions updateOptions = null)
        {
            AppendUpdateAudited(ref updateDefinition);

            var result = await _mongoCollection.UpdateOneAsync(ClientSession, predicate, updateDefinition, updateOptions);

            return new ServiceResult<UpdateResult>(result);
        }
        public ServiceResult<UpdateResult> UpdateImplicit(Expression<Func<TEntity, bool>> predicate,
            UpdateDefinition<TEntity> updateDefinition, UpdateOptions updateOptions = null)
        {
            AppendUpdateAudited(ref updateDefinition);

            var result = _mongoCollection.UpdateOne(ClientSession, predicate, updateDefinition, updateOptions);

            return new ServiceResult<UpdateResult>(result);
        }

        public async Task<ServiceResult<UpdateResult>> UpdateImplicitAsync(Expression<Func<TEntity, bool>> predicate,
            UpdateDefinition<TEntity> updateDefinition, UpdateOptions updateOptions = null)
        {
            AppendUpdateAudited(ref updateDefinition);

            var result = await _mongoCollection.UpdateOneAsync(ClientSession, predicate, updateDefinition, updateOptions);

            return new ServiceResult<UpdateResult>(result);
        }

        public ServiceResult<UpdateResult> UpdateManyImplicit(FilterDefinition<TEntity> predicate,
            UpdateDefinition<TEntity> updateDefinition, UpdateOptions updateOptions = null)
        {
            AppendUpdateAudited(ref updateDefinition);

            var result = _mongoCollection.UpdateMany(ClientSession, predicate, updateDefinition, updateOptions);

            return new ServiceResult<UpdateResult>(result);
        }

        public async Task<ServiceResult<UpdateResult>> UpdateManyImplicitAsync(FilterDefinition<TEntity> predicate,
            UpdateDefinition<TEntity> updateDefinition, UpdateOptions updateOptions = null)
        {
            AppendUpdateAudited(ref updateDefinition);

            var result = await _mongoCollection.UpdateManyAsync(ClientSession, predicate, updateDefinition, updateOptions);

            return new ServiceResult<UpdateResult>(result);
        }
        public ServiceResult<UpdateResult> UpdateManyImplicit(Expression<Func<TEntity, bool>> predicate,
            UpdateDefinition<TEntity> updateDefinition, UpdateOptions updateOptions = null)
        {
            AppendUpdateAudited(ref updateDefinition);

            var result = _mongoCollection.UpdateMany(ClientSession, predicate, updateDefinition, updateOptions);

            return new ServiceResult<UpdateResult>(result);
        }

        public async Task<ServiceResult<UpdateResult>> UpdateManyImplicitAsync(Expression<Func<TEntity, bool>> predicate,
            UpdateDefinition<TEntity> updateDefinition, UpdateOptions updateOptions = null)
        {
            AppendUpdateAudited(ref updateDefinition);

            var result = await _mongoCollection.UpdateManyAsync(ClientSession, predicate, updateDefinition, updateOptions);

            return new ServiceResult<UpdateResult>(result);
        }

        public ServiceResult<ReplaceOneResult> ReplaceOneImplicit(TKey Id,
            TEntity replacement, UpdateOptions updateOptions = null, bool igonreValidation = false)
        {
            if (!igonreValidation)
                replacement.ValidateUpdate(_validationContext);

            AppendUpdateAudited(ref replacement);

            Expression<Func<TEntity, bool>> predicate = x => x.Id.Equals(Id);
            var result = _mongoCollection.ReplaceOne(ClientSession, predicate, replacement, updateOptions);

            if (result.ModifiedCount <= 0)
                return new ServiceResult<ReplaceOneResult>("خطایی در انجام عملیات رخ داد");
            return new ServiceResult<ReplaceOneResult>(result);
        }
        public ServiceResult<ReplaceOneResult> ReplaceOneImplicit(FilterDefinition<TEntity> predicate,
            TEntity replacement, UpdateOptions updateOptions = null, bool igonreValidation = false)
        {
            if (!igonreValidation)
                replacement.ValidateUpdate(_validationContext);

            AppendUpdateAudited(ref replacement);


            var result = _mongoCollection.ReplaceOne(ClientSession, predicate, replacement, updateOptions);

            if (result.ModifiedCount <= 0)
                return new ServiceResult<ReplaceOneResult>("خطایی در انجام عملیات رخ داد");

            return new ServiceResult<ReplaceOneResult>(result);
        }

        public async Task<ServiceResult<ReplaceOneResult>> ReplaceOneImplicitAsync(FilterDefinition<TEntity> predicate,
            TEntity replacement, UpdateOptions updateOptions = null, bool igonreValidation = false)
        {
            if (!igonreValidation)
                replacement.ValidateUpdate(_validationContext);

            AppendUpdateAudited(ref replacement);


            var result = await _mongoCollection.ReplaceOneAsync(ClientSession, predicate, replacement, updateOptions);

            if (result.ModifiedCount <= 0)
                return new ServiceResult<ReplaceOneResult>("خطایی در انجام عملیات رخ داد");

            return new ServiceResult<ReplaceOneResult>(result);
        }
        public ServiceResult<ReplaceOneResult> ReplaceOneImplicit(Expression<Func<TEntity, bool>> predicate,
            TEntity replacement, UpdateOptions updateOptions = null, bool igonreValidation = false)
        {
            if (!igonreValidation)
                replacement.ValidateUpdate(_validationContext);

            AppendUpdateAudited(ref replacement);


            var result = _mongoCollection.ReplaceOne(ClientSession, predicate, replacement, updateOptions);

            if (result.ModifiedCount <= 0)
                return new ServiceResult<ReplaceOneResult>("خطایی در انجام عملیات رخ داد");

            return new ServiceResult<ReplaceOneResult>(result);
        }

        public async Task<ServiceResult<ReplaceOneResult>> ReplaceOneImplicitAsync(Expression<Func<TEntity, bool>> predicate,
            TEntity replacement, UpdateOptions updateOptions = null, bool igonreValidation = false)
        {
            if (!igonreValidation)
                replacement.ValidateUpdate(_validationContext);

            AppendUpdateAudited(ref replacement);


            var result = await _mongoCollection.ReplaceOneAsync(ClientSession, predicate, replacement, updateOptions);

            if (result.ModifiedCount <= 0)
                return new ServiceResult<ReplaceOneResult>("خطایی در انجام عملیات رخ داد");

            return new ServiceResult<ReplaceOneResult>(result);
        }


        #endregion
    }
}