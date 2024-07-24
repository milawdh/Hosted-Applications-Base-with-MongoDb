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
using Base.Domain.Audities;

namespace Base.Infrasucture.Repository.Impl.MainRepos
{
    public partial class MainRepo<TEntity, TKey> : IMainRepo<TEntity, TKey> where TEntity : IBaseEntity<TKey>

    {
        /// <summary>
        /// Marks An Update Operation For Entity With Given Id! Need To Call <seealso cref="ICore.SaveChange()"/>!
        /// </summary>
        /// <param name="id">Entity Id</param>
        /// <param name="updateDefinition">Defination For Update Operation!</param>
        public void Update(TKey id, UpdateDefinition<TEntity> updateDefinition)
        {
            AppendUpdateAudited(ref updateDefinition);

            var filter = Builders<TEntity>.Filter.Eq(x => x.Id, id);
            WritesQue.Add(new UpdateOneModel<TEntity>(filter, updateDefinition));

            PublishOnWriteAddedEvent();
        }

        /// <summary>
        /// Marks An Update Operation For Entity With Given Filter! Need To Call <seealso cref="ICore.SaveChange()"/>!
        /// </summary>
        /// <param name="filter">Filter Of One Entitiy Yo Want To Update</param>
        /// <param name="updateDefinition">Defination For Update Operation!</param>
        public void Update(FilterDefinition<TEntity> filter, UpdateDefinition<TEntity> updateDefinition)
        {
            AppendUpdateAudited(ref updateDefinition);

            WritesQue.Add(new UpdateOneModel<TEntity>(filter, updateDefinition));

            PublishOnWriteAddedEvent();
        }


        /// <summary>
        /// Marks An Update Operation For Entity With Given Expression! Need To Call <seealso cref="ICore.SaveChange()"/>!
        /// </summary>
        /// <param name="predicate">Expression Predicate Of One Entity Yo Want To Update</param>
        /// <param name="updateDefinition">Defination For Update Operation!</param>
        public void Update(Expression<Func<TEntity, bool>> predicate, UpdateDefinition<TEntity> updateDefinition)
        {
            AppendUpdateAudited(ref updateDefinition);

            WritesQue.Add(new UpdateOneModel<TEntity>(predicate, updateDefinition));

            PublishOnWriteAddedEvent();
        }

        /// <summary>
        /// Marks An UpdateMany Operation For Entity With Given Ids! Need To Call <seealso cref="ICore.SaveChange()"/>!
        /// </summary>
        /// <param name="ids">Ids Of Entities Yo Want To Update</param>
        /// <param name="updateDefinition">Defination For Update Operation!</param>
        public void UpdateMany(IEnumerable<TKey> ids, UpdateDefinition<TEntity> updateDefinition)
        {
            AppendUpdateAudited(ref updateDefinition);

            var filter = Builders<TEntity>.Filter.In(x => x.Id, ids);

            WritesQue.Add(new UpdateManyModel<TEntity>(filter, updateDefinition));

            PublishOnWriteAddedEvent();
        }

        /// <summary>
        /// Marks An UpdateMany Operation For Entity With Given Ids! Need To Call <seealso cref="ICore.SaveChange()"/>!
        /// </summary>
        /// <param name="filter">Filter Of Entities Yo Want To Update</param>
        /// <param name="updateDefinition">Defination For Update Operation!</param>
        public void UpdateMany(FilterDefinition<TEntity> filter, UpdateDefinition<TEntity> updateDefinition)
        {
            AppendUpdateAudited(ref updateDefinition);

            WritesQue.Add(new UpdateManyModel<TEntity>(filter, updateDefinition));

            PublishOnWriteAddedEvent();
        }

        /// <summary>
        /// Marks An UpdateMany Operation For Entity With Given Expression! Need To Call <seealso cref="ICore.SaveChange()"/>!
        /// </summary>
        /// <param name="predicate">Expression Predicate Of Entities Yo Want To Update</param>
        /// <param name="updateDefinition">Defination For Update Operation!</param>
        public void UpdateMany(Expression<Func<TEntity, bool>> predicate, UpdateDefinition<TEntity> updateDefinition)
        {
            AppendUpdateAudited(ref updateDefinition);

            WritesQue.Add(new UpdateManyModel<TEntity>(predicate, updateDefinition));

            PublishOnWriteAddedEvent();
        }

        /// <summary>
        /// Marks An ReplaceOne Operation For Entity With Given Id! Need To Call <seealso cref="ICore.SaveChange()"/>!
        /// </summary>
        /// <param name="Id">Entity Id</param>
        /// <param name="replacement">Replacement Of Entity That Matches The Given Id</param>
        public void ReplaceOne(TKey Id, TEntity replacement, bool igonreValidation = false)
        {
            if (!igonreValidation)
                replacement.ValidateUpdate(_validationContext);

            AppendUpdateAudited(ref replacement);
            var predicate = Builders<TEntity>.Filter.Eq(x => x.Id, Id);
            var request = new ReplaceOneModel<TEntity>(predicate, replacement);

            WritesQue.Add(request);

            PublishOnWriteAddedEvent();
        }

        /// <summary>
        /// Marks An ReplaceOne Operation For Entity With Given Filter! Need To Call <seealso cref="ICore.SaveChange()"/>!
        /// </summary>
        /// <param name="filter">Filter Of One Entitiy Yo Want To Replace</param>
        /// <param name="replacement">Replacement Of Entity That Matches The Given Id</param>
        public void ReplaceOne(FilterDefinition<TEntity> filter, TEntity replacement, bool igonreValidation = false)
        {
            if (!igonreValidation)
                replacement.ValidateUpdate(_validationContext);

            AppendUpdateAudited(ref replacement);

            var request = new ReplaceOneModel<TEntity>(filter, replacement);

            WritesQue.Add(request);

            PublishOnWriteAddedEvent();
        }

        /// <summary>
        /// Marks An ReplaceOne Operation For Entity With Given Expression! Need To Call <seealso cref="ICore.SaveChange()"/>!
        /// </summary>
        /// <param name="predicate">Expression Predicate Of One Entity Yo Want To Replace</param>
        /// <param name="replacement">Replacement Of Entity That Matches The Given Id</param>
        public void ReplaceOne(Expression<Func<TEntity, bool>> predicate, TEntity replacement, bool igonreValidation = false)
        {
            if (!igonreValidation)
                replacement.ValidateUpdate(_validationContext);

            AppendUpdateAudited(ref replacement);

            var request = new ReplaceOneModel<TEntity>(predicate, replacement);

            WritesQue.Add(request);

            PublishOnWriteAddedEvent();
        }


    }
}