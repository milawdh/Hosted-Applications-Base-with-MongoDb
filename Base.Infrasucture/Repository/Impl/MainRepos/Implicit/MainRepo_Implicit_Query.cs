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
using Base.Domain.Audities;

namespace Base.Infrasucture.Repository.Impl.MainRepos
{
    public partial class MainRepo<TEntity, TKey> : IMainRepo<TEntity, TKey> where TEntity : IBaseEntity<TKey>
    {
        public IMongoQueryable<TEntity> AsQuerable(AggregateOptions options = null) => _mongoCollection.AsQueryable(ClientSession);

        #region Get

        public bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return _mongoCollection.Find(predicate).Any();
        }

        public IFindFluent<TEntity, TEntity> Get(FilterDefinition<TEntity> predicate, FindOptions findOptions = null)
        {
            return _mongoCollection.Find(ClientSession, predicate, findOptions);
        }

        public IFindFluent<TEntity, TEntity> Get(Expression<Func<TEntity, bool>> predicate = null, FindOptions findOptions = null)
        {
            if (predicate == null)
                predicate = x => true;
            return _mongoCollection.Find(ClientSession, predicate, findOptions);
        }

        public TEntity? GetById(TKey Id)
        {
            return Get(x => x.Id.Equals(Id)).FirstOrDefault();
        }

        public IFilteredMongoCollection<TProjection> GetOfType<TProjection>() where TProjection : TEntity
            => _mongoCollection.OfType<TProjection>();
        #endregion

        #region AsyncCursors

        public Task<IAsyncCursor<TEntity>> CursorAsync(Expression<Func<TEntity, bool>> predicate = null, FindOptions<TEntity, TEntity> findOptions = null)
        {
            if (predicate is null)
                predicate = x => true;
            return _mongoCollection.FindAsync(ClientSession, predicate, findOptions);
        }

        public Task<IAsyncCursor<TEntity>> CursorAsync(FilterDefinition<TEntity> predicate, FindOptions<TEntity, TEntity> findOptions = null)
        {
            return _mongoCollection.FindAsync(ClientSession, predicate, findOptions);
        }

        public IAsyncCursor<TEntity> CursorLockAsync(Expression<Func<TEntity, bool>> predicate = null, FindOptions<TEntity, TEntity> findOptions = null)
        {
            if (predicate is null)
                predicate = x => true;
            return _mongoCollection.FindSync(ClientSession, predicate, findOptions);
        }

        public IAsyncCursor<TEntity> CursorLockAsync(FilterDefinition<TEntity> predicate, FindOptions<TEntity, TEntity> findOptions = null)
        {
            return _mongoCollection.FindSync(ClientSession, predicate, findOptions);
        }

        public IChangeStreamCursor<ChangeStreamDocument<TEntity>> Watch(ChangeStreamOptions options = null) => _mongoCollection.Watch(ClientSession);
        public Task<IChangeStreamCursor<ChangeStreamDocument<TEntity>>> WatchAsync(ChangeStreamOptions options = null) => _mongoCollection.WatchAsync(ClientSession);


        #endregion

        #region Aggregate

        public IAggregateFluent<TEntity> Aggregate(AggregateOptions aggregateOptions = null)
        {
            return _mongoCollection.Aggregate(ClientSession, aggregateOptions);
        }

        public IAsyncCursor<TResult> Aggregate<TResult>(PipelineDefinition<TEntity, TResult> pipelineDefinition, AggregateOptions aggregateOptions = null)
        {
            return _mongoCollection.Aggregate(ClientSession, pipelineDefinition, aggregateOptions);
        }

        public Task<IAsyncCursor<TResult>> AggregateAsync<TResult>(PipelineDefinition<TEntity, TResult> pipelineDefinition,
            AggregateOptions aggregateOptions = null)
        {
            return _mongoCollection.AggregateAsync(ClientSession, pipelineDefinition, aggregateOptions);
        }

        #endregion

        #region Distinc
        public IAsyncCursor<TField> GetDistincValues<TField>(FieldDefinition<TEntity, TField> fieldDefinition, Expression<Func<TEntity, bool>> predicate = null, DistinctOptions distinctOptions = null)
        {
            return _mongoCollection.Distinct(ClientSession, fieldDefinition, predicate, distinctOptions);
        }

        public Task<IAsyncCursor<TField>> GetDistincValuesAsync<TField>(FieldDefinition<TEntity, TField> fieldDefinition, Expression<Func<TEntity, bool>> predicate = null, DistinctOptions distinctOptions = null)
        {
            return _mongoCollection.DistinctAsync(ClientSession, fieldDefinition, predicate, distinctOptions);
        }

        public IAsyncCursor<TField> GetDistincValues<TField>(FieldDefinition<TEntity, TField> fieldDefinition, FilterDefinition<TEntity> predicate, DistinctOptions distinctOptions = null)
        {
            return _mongoCollection.Distinct(ClientSession, fieldDefinition, predicate, distinctOptions);
        }

        public Task<IAsyncCursor<TField>> GetDistincValuesAsync<TField>(FieldDefinition<TEntity, TField> fieldDefinition, FilterDefinition<TEntity> predicate, DistinctOptions distinctOptions = null)
        {
            return _mongoCollection.DistinctAsync(ClientSession, fieldDefinition, predicate, distinctOptions);
        }
        #endregion

        #region Count

        public long Count(Expression<Func<TEntity, bool>> predicate, CountOptions countOptions = null)
        {
            var result = _mongoCollection.Count(ClientSession, predicate, countOptions);

            return result;
        }

        public long Count(FilterDefinition<TEntity> predicate, CountOptions countOptions = null)
        {
            var result = _mongoCollection.Count(ClientSession, predicate, countOptions);

            return result;
        }

        public Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate, CountOptions countOptions = null)
        {
            var result = _mongoCollection.CountAsync(ClientSession, predicate, countOptions);

            return result;
        }

        public Task<long> CountAsync(FilterDefinition<TEntity> predicate, CountOptions countOptions = null)
        {
            var result = _mongoCollection.CountAsync(ClientSession, predicate, countOptions);
            return result;
        }


        public long EstimateAllDocsCount(EstimatedDocumentCountOptions options = null) => _mongoCollection.EstimatedDocumentCount();
        public Task<long> EstimateAllDocsCountAsync(EstimatedDocumentCountOptions options = null) => _mongoCollection.EstimatedDocumentCountAsync();

        #endregion



    }
}