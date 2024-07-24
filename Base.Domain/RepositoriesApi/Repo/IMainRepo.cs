using Base.Domain.Audities;
using Base.Domain.EventArgsModels.Repo;
using Base.Domain.Models.OutPutModels;
using Base.Domain.RepositoriesApi.Core;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq.Expressions;

namespace Base.Domain.RepositoriesApi.Repo
{
    public interface IMainRepo<TEntity, TKey> : IDisposable
        where TEntity : IBaseEntity<TKey>
    {
        List<WriteModel<TEntity>> WritesQue { get; }

        void DropCollection();
        void Add(TEntity entity, bool igonreValidation = false);
        ServiceResult AddImplicit(TEntity entity, InsertOneOptions insertOneOptions = null, bool igonreValidation = false);
        Task<ServiceResult> AddImplicitAsync(TEntity entity, InsertOneOptions insertOneOptions, bool igonreValidation = false);
        ServiceResult AddImplicitMany(IEnumerable<TEntity> entities, InsertManyOptions insertManyOptions = null, bool igonreValidation = false);
        void AddMany(IEnumerable<TEntity> entities, bool igonreValidation = false);
        Task<ServiceResult> AddManyImplicitAsync(IEnumerable<TEntity> entities, InsertManyOptions insertManyOptions = null, bool igonreValidation = false);
        IAggregateFluent<TEntity> Aggregate(AggregateOptions aggregateOptions = null);
        IAsyncCursor<TResult> Aggregate<TResult>(PipelineDefinition<TEntity, TResult> pipelineDefinition, AggregateOptions aggregateOptions = null);
        Task<IAsyncCursor<TResult>> AggregateAsync<TResult>(PipelineDefinition<TEntity, TResult> pipelineDefinition, AggregateOptions aggregateOptions = null);
        bool Any(Expression<Func<TEntity, bool>> predicate);
        void AppendAddAudited(ref IEnumerable<TEntity> entities);
        void AppendAddAudited(ref TEntity entity);
        List<ReplaceOneModel<TEntity>> AppendSoftDeleteAudited(ref IEnumerable<TEntity> entities);
        void AppendSoftDeleteAudited(ref TEntity entity);
        void AppendUpdateAudited(ref IEnumerable<TEntity> entities);
        void AppendUpdateAudited(ref TEntity entity);
        void AppendUpdateAudited(ref UpdateDefinition<TEntity> updateDefinition);
        IMongoQueryable<TEntity> AsQuerable(AggregateOptions options = null);
        ServiceResult BulkWrite(IEnumerable<WriteModel<TEntity>> requests, BulkWriteOptions bulkWriteOptions = null);
        Task<ServiceResult> BulkWriteAsync(IEnumerable<WriteModel<TEntity>> requests, BulkWriteOptions bulkWriteOptions = null);
        long Count(Expression<Func<TEntity, bool>> predicate, CountOptions countOptions = null);
        long Count(FilterDefinition<TEntity> predicate, CountOptions countOptions = null);
        Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate, CountOptions countOptions = null);
        Task<long> CountAsync(FilterDefinition<TEntity> predicate, CountOptions countOptions = null);
        Task<IAsyncCursor<TEntity>> CursorAsync(Expression<Func<TEntity, bool>> predicate = null, FindOptions<TEntity, TEntity> findOptions = null);
        Task<IAsyncCursor<TEntity>> CursorAsync(FilterDefinition<TEntity> predicate, FindOptions<TEntity, TEntity> findOptions = null);
        IAsyncCursor<TEntity> CursorLockAsync(Expression<Func<TEntity, bool>> predicate = null, FindOptions<TEntity, TEntity> findOptions = null);
        IAsyncCursor<TEntity> CursorLockAsync(FilterDefinition<TEntity> predicate, FindOptions<TEntity, TEntity> findOptions = null);
        void Delete(TEntity entity, bool igonreValidation = false);
        void Delete(TKey Id);
        void DeleteForce(TEntity entity, bool igonreValidation = false);
        void DeleteForce(TKey Id);
        ServiceResult<DeleteResult> DeleteForceImplicit(Expression<Func<TEntity, bool>> predicate, DeleteOptions deleteOptions = null);
        ServiceResult<DeleteResult> DeleteForceImplicit(FilterDefinition<TEntity> predicate, DeleteOptions deleteOptions = null);
        ServiceResult<DeleteResult> DeleteForceImplicit(TEntity entity, DeleteOptions deleteOptions = null, bool igonreValidation = false);
        Task<ServiceResult<DeleteResult>> DeleteForceImplicitAsync(Expression<Func<TEntity, bool>> predicate, DeleteOptions deleteOptions = null);
        Task<ServiceResult<DeleteResult>> DeleteForceImplicitAsync(FilterDefinition<TEntity> predicate, DeleteOptions deleteOptions = null);
        Task<ServiceResult<DeleteResult>> DeleteForceImplicitAsync(TEntity entity, DeleteOptions deleteOptions = null, bool igonreValidation = false);
        void DeleteForceMany(IEnumerable<TEntity> entities, bool igonreValidation = false);
        void DeleteForceMany(IEnumerable<TKey> Ids);
        ServiceResult DeleteImplicit(Expression<Func<TEntity, bool>> predicate, DeleteOptions deleteOptions = null);
        ServiceResult DeleteImplicit(FilterDefinition<TEntity> predicate);
        ServiceResult DeleteImplicit(TEntity entity, ReplaceOptions softDeleteOptions = null, bool igonreValidation = false);
        Task<ServiceResult> DeleteImplicitAsync(Expression<Func<TEntity, bool>> predicate);
        Task<ServiceResult> DeleteImplicitAsync(FilterDefinition<TEntity> predicate);
        Task<ServiceResult> DeleteImplicitAsync(TEntity entity, ReplaceOptions softDeleteOptions = null, bool igonreValidation = false);
        void DeleteMany(IEnumerable<TEntity> entities, bool igonreValidation = false);
        void DeleteMany(IEnumerable<TKey> Ids);
        ServiceResult<DeleteResult> DeleteManyForceImplicit(Expression<Func<TEntity, bool>> predicate, DeleteOptions deleteOptions = null);
        ServiceResult<DeleteResult> DeleteManyForceImplicit(FilterDefinition<TEntity> predicate, DeleteOptions deleteOptions = null);
        ServiceResult<DeleteResult> DeleteManyForceImplicit(IEnumerable<TEntity> entities, DeleteOptions deleteOptions = null, bool igonreValidation = false);
        Task<ServiceResult<DeleteResult>> DeleteManyForceImplicitAsync(Expression<Func<TEntity, bool>> predicate, DeleteOptions deleteOptions = null);
        Task<ServiceResult<DeleteResult>> DeleteManyForceImplicitAsync(FilterDefinition<TEntity> predicate, DeleteOptions deleteOptions = null);
        Task<ServiceResult<DeleteResult>> DeleteManyForceImplicitAsync(IEnumerable<TEntity> entities, DeleteOptions deleteOptions = null, bool igonreValidation = false);
        ServiceResult DeleteManyImplicit(Expression<Func<TEntity, bool>> predicate);
        ServiceResult DeleteManyImplicit(FilterDefinition<TEntity> predicate);
        ServiceResult DeleteManyImplicit(IEnumerable<TEntity> entities, UpdateOptions softDeleteOptions = null, bool igonreValidation = false);
        Task<ServiceResult> DeleteManyImplicitAsync(Expression<Func<TEntity, bool>> predicate);
        Task<ServiceResult> DeleteManyImplicitAsync(FilterDefinition<TEntity> predicate);
        Task<ServiceResult> DeleteManyImplicitAsync(IEnumerable<TEntity> entities, UpdateOptions softDeleteOptions = null, bool igonreValidation = false);
        void Dispose();
        long EstimateAllDocsCount(EstimatedDocumentCountOptions options = null);
        Task<long> EstimateAllDocsCountAsync(EstimatedDocumentCountOptions options = null);
        IFindFluent<TEntity, TEntity> Get(Expression<Func<TEntity, bool>> predicate = null, FindOptions findOptions = null);
        IFindFluent<TEntity, TEntity> Get(FilterDefinition<TEntity> predicate, FindOptions findOptions = null);
        TEntity? GetById(TKey Id);
        IMongoCollection<TEntity> GetCollection();
        IMongoCollection<BsonDocument> GetCollectionAsBson();
        IAsyncCursor<TField> GetDistincValues<TField>(FieldDefinition<TEntity, TField> fieldDefinition, Expression<Func<TEntity, bool>> predicate = null, DistinctOptions distinctOptions = null);
        IAsyncCursor<TField> GetDistincValues<TField>(FieldDefinition<TEntity, TField> fieldDefinition, FilterDefinition<TEntity> predicate, DistinctOptions distinctOptions = null);
        Task<IAsyncCursor<TField>> GetDistincValuesAsync<TField>(FieldDefinition<TEntity, TField> fieldDefinition, Expression<Func<TEntity, bool>> predicate = null, DistinctOptions distinctOptions = null);
        Task<IAsyncCursor<TField>> GetDistincValuesAsync<TField>(FieldDefinition<TEntity, TField> fieldDefinition, FilterDefinition<TEntity> predicate, DistinctOptions distinctOptions = null);
        IFilteredMongoCollection<TProjection> GetOfType<TProjection>() where TProjection : TEntity;
        ServiceResult OnSaveChanged(ICore sender, SaveChangeEventArgs saveChangeEventArgs);
        Task<ServiceResult> OnSaveChangedAsync(ICore sender, SaveChangeEventArgs saveChangeEventArgs);
        void Register(ICore core);
        void ReplaceOne(Expression<Func<TEntity, bool>> predicate, TEntity replacement, bool igonreValidation = false);
        void ReplaceOne(FilterDefinition<TEntity> filter, TEntity replacement, bool igonreValidation = false);
        void ReplaceOne(TKey Id, TEntity replacement, bool igonreValidation = false);
        ServiceResult<ReplaceOneResult> ReplaceOneImplicit(Expression<Func<TEntity, bool>> predicate, TEntity replacement, UpdateOptions updateOptions = null, bool igonreValidation = false);
        ServiceResult<ReplaceOneResult> ReplaceOneImplicit(FilterDefinition<TEntity> predicate, TEntity replacement, UpdateOptions updateOptions = null, bool igonreValidation = false);
        ServiceResult<ReplaceOneResult> ReplaceOneImplicit(TKey Id, TEntity replacement, UpdateOptions updateOptions = null, bool igonreValidation = false);
        Task<ServiceResult<ReplaceOneResult>> ReplaceOneImplicitAsync(Expression<Func<TEntity, bool>> predicate, TEntity replacement, UpdateOptions updateOptions = null, bool igonreValidation = false);
        Task<ServiceResult<ReplaceOneResult>> ReplaceOneImplicitAsync(FilterDefinition<TEntity> predicate, TEntity replacement, UpdateOptions updateOptions = null, bool igonreValidation = false);
        void SubscribeWriteQueAppendedEvent(IMainRepo<TEntity, TKey>.WriteQueAppendedHandler handler);
        void UnSubscribeWriteQueAppendedEvent(IMainRepo<TEntity, TKey>.WriteQueAppendedHandler handler);
        void Update(Expression<Func<TEntity, bool>> predicate, UpdateDefinition<TEntity> updateDefinition);
        void Update(FilterDefinition<TEntity> filter, UpdateDefinition<TEntity> updateDefinition);
        void Update(TKey id, UpdateDefinition<TEntity> updateDefinition);
        ServiceResult<UpdateResult> UpdateImplicit(Expression<Func<TEntity, bool>> predicate, UpdateDefinition<TEntity> updateDefinition, UpdateOptions updateOptions = null);
        ServiceResult<UpdateResult> UpdateImplicit(FilterDefinition<TEntity> predicate, UpdateDefinition<TEntity> updateDefinition, UpdateOptions updateOptions = null);
        ServiceResult<UpdateResult> UpdateImplicit(TKey Id, UpdateDefinition<TEntity> updateDefinition, UpdateOptions updateOptions = null);
        Task<ServiceResult<UpdateResult>> UpdateImplicitAsync(Expression<Func<TEntity, bool>> predicate, UpdateDefinition<TEntity> updateDefinition, UpdateOptions updateOptions = null);
        Task<ServiceResult<UpdateResult>> UpdateImplicitAsync(FilterDefinition<TEntity> predicate, UpdateDefinition<TEntity> updateDefinition, UpdateOptions updateOptions = null);
        void UpdateMany(Expression<Func<TEntity, bool>> predicate, UpdateDefinition<TEntity> updateDefinition);
        void UpdateMany(FilterDefinition<TEntity> filter, UpdateDefinition<TEntity> updateDefinition);
        void UpdateMany(IEnumerable<TKey> ids, UpdateDefinition<TEntity> updateDefinition);
        ServiceResult<UpdateResult> UpdateManyImplicit(Expression<Func<TEntity, bool>> predicate, UpdateDefinition<TEntity> updateDefinition, UpdateOptions updateOptions = null);
        ServiceResult<UpdateResult> UpdateManyImplicit(FilterDefinition<TEntity> predicate, UpdateDefinition<TEntity> updateDefinition, UpdateOptions updateOptions = null);
        Task<ServiceResult<UpdateResult>> UpdateManyImplicitAsync(Expression<Func<TEntity, bool>> predicate, UpdateDefinition<TEntity> updateDefinition, UpdateOptions updateOptions = null);
        Task<ServiceResult<UpdateResult>> UpdateManyImplicitAsync(FilterDefinition<TEntity> predicate, UpdateDefinition<TEntity> updateDefinition, UpdateOptions updateOptions = null);
        IChangeStreamCursor<ChangeStreamDocument<TEntity>> Watch(ChangeStreamOptions options = null);
        Task<IChangeStreamCursor<ChangeStreamDocument<TEntity>>> WatchAsync(ChangeStreamOptions options = null);



        delegate void WriteQueAppendedHandler(IMainRepo<TEntity, TKey> repo);
    }
}