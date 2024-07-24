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
        #region Add

        /// <summary>
        /// Adds One Entity Implicitly To DataBase At that Moment! No need To  Need To Call <see cref="ICore.SaveChange()"/>
        /// </summary>
        /// <param name="entity">Entity You Want to add</param>
        /// <param name="insertOneOptions">Inserting Options</param>
        /// <returns></returns>
        public ServiceResult AddImplicit(TEntity entity, InsertOneOptions insertOneOptions = null, bool igonreValidation = false)
        {
            AppendAddAudited(ref entity);

            if (!igonreValidation)
                entity.ValidateAdd(_validationContext);

            _mongoCollection.InsertOne(ClientSession, entity, insertOneOptions);
            return new ServiceResult();
        }

        /// <summary>
        /// Adds One Entity Implicitly To DataBase At that Moment Asynchronounsly! No need To Call <see cref="ICore.SaveChange()"/>
        /// </summary>
        /// <param name="entity">Entity You Want to add</param>
        /// <param name="insertOneOptions">Inserting Options</param>
        /// <returns></returns>
        public async Task<ServiceResult> AddImplicitAsync(TEntity entity, InsertOneOptions insertOneOptions, bool igonreValidation = false)
        {
            AppendAddAudited(ref entity);

            if (!igonreValidation)
                entity.ValidateAdd(_validationContext);

            await _mongoCollection.InsertOneAsync(ClientSession, entity, insertOneOptions);
            return new ServiceResult();
        }

        /// <summary>
        /// Adds Many Entity Implicitly To DataBase At that Moment! No need To Call  <see cref="ICore.SaveChange()"/>
        /// </summary>
        /// <param name="entities">Entity You Want to add</param>
        /// <param name="insertManyOptions">Inserting Options</param>
        /// <returns></returns>
        public ServiceResult AddImplicitMany(IEnumerable<TEntity> entities, InsertManyOptions insertManyOptions = null, bool igonreValidation = false)
        {
            AppendAddAudited(ref entities);
            
            if (!igonreValidation)
                foreach (TEntity entity in entities)
                entity.ValidateAdd(_validationContext);

            _mongoCollection.InsertMany(ClientSession, entities, insertManyOptions);
            return new ServiceResult();
        }

        /// <summary>
        /// Adds Many Entity Implicitly To DataBase At that Moment Asynchronounsly! No need To Call <see cref="ICore.SaveChange()"/>
        /// </summary>
        /// <param name="entities">Entity You Want to add</param>
        /// <param name="insertManyOptions">Inserting Options</param>
        /// <returns></returns>
        public async Task<ServiceResult> AddManyImplicitAsync(IEnumerable<TEntity> entities, InsertManyOptions insertManyOptions = null, bool igonreValidation = false)
        {
            AppendAddAudited(ref entities);

            if (!igonreValidation)
            foreach (TEntity entity in entities)
                entity.ValidateAdd(_validationContext);

            await _mongoCollection.InsertManyAsync(ClientSession, entities, insertManyOptions);
            return new ServiceResult();
        }

        #endregion
    }
}