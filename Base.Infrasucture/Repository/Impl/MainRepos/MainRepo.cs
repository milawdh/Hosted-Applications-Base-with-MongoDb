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
using static Base.Domain.RepositoriesApi.Core.ICore;
using MongoDB.Bson;
using Base.Domain.RepositoriesApi.Context;
using Base.Domain.RepositoriesApi.Repo;
using Base.Domain.RepositoriesApi.Core;
using Base.Domain.Models.OutPutModels;
using Base.Domain.RepositoriesApi.UserInfo;
using Base.Domain.EventArgsModels.Repo;
using Base.Domain.Audities;
namespace Base.Infrasucture.Repository.Impl.MainRepos
{

    public partial class MainRepo<TEntity, TKey> : IMainRepo<TEntity, TKey> where TEntity : IBaseEntity<TKey>
    {
        public List<WriteModel<TEntity>> WritesQue { get; private set; }

        private bool isRegisteredForSaveChange = false;
        private readonly IClientSessionHandle ClientSession;
        protected readonly IMongoCollection<TEntity> _mongoCollection;
        protected readonly IBaseUserInfoContext _baseUserInfoContext;

        private DomainValidationContext _validationContext;
        public MainRepo(IClientSessionHandle clientSession, IMongoCollection<TEntity> mongoCollection, IBaseUserInfoContext baseUserInfoContext, DomainValidationContext validationContext)
        {
            _mongoCollection = mongoCollection;
            ClientSession = clientSession;
            _baseUserInfoContext = baseUserInfoContext;

            WritesQue = new();
            this._validationContext = validationContext;
        }

        public IMongoCollection<TEntity> GetCollection() => _mongoCollection;
        public IMongoCollection<BsonDocument> GetCollectionAsBson()
        {
            return _mongoCollection.Database.GetCollection<BsonDocument>(typeof(TEntity).Name);
        }
        public void DropCollection()
        {
            _mongoCollection.Database.DropCollection(ClientSession, typeof(TEntity).Name);
        }

        public ServiceResult OnSaveChanged(ICore sender, SaveChangeEventArgs saveChangeEventArgs)
        {
            if (WritesQue.Any())
            {
                var result = BulkWrite(WritesQue);
                WritesQue.Clear();
                return result;
            }

            return new ServiceResult();
        }

        public async Task<ServiceResult> OnSaveChangedAsync(ICore sender, SaveChangeEventArgs saveChangeEventArgs)
        {
            if (WritesQue.Any())
            {
                var result = await BulkWriteAsync(WritesQue);
                WritesQue.Clear();
                return result;
            }

            return new ServiceResult();
        }

        public void Register(ICore core)
        {
            core.SubscribeSaveChangedEvent(OnSaveChanged);

            core.SubscribeSaveChangedAsyncEvent(OnSaveChangedAsync);
        }



        public void SubscribeWriteQueAppendedEvent(IMainRepo<TEntity, TKey>.WriteQueAppendedHandler handler)
        {
            WriteQueAppendedEvent += handler;
        }
        public void UnSubscribeWriteQueAppendedEvent(IMainRepo<TEntity, TKey>.WriteQueAppendedHandler handler)
        {

            if (WriteQueAppendedEvent != null)
                if (WriteQueAppendedEvent.GetInvocationList().Any(x => x == (MulticastDelegate)handler))
                    WriteQueAppendedEvent -= handler;
        }

        private void PublishOnWriteAddedEvent()
        {
            if (!isRegisteredForSaveChange)
            {
                WriteQueAppendedEvent?.Invoke(this);
                isRegisteredForSaveChange = true;
            }
        }

        public ServiceResult BulkWrite(IEnumerable<WriteModel<TEntity>> requests, BulkWriteOptions bulkWriteOptions = null)
        {
            var result = _mongoCollection.BulkWrite(ClientSession, requests, bulkWriteOptions);

            return new ServiceResult();
        }

        public async Task<ServiceResult> BulkWriteAsync(IEnumerable<WriteModel<TEntity>> requests, BulkWriteOptions bulkWriteOptions = null)
        {
            await _mongoCollection.BulkWriteAsync(ClientSession, requests, bulkWriteOptions);
            return new ServiceResult();
        }

        //TODO : Handle Error Such as log
        protected virtual async Task HandleErrorAsync(Exception exception)
        {

        }


        //TODO : Session & Transactions disposing handeling
        public void Dispose()
        {
        }



        private event IMainRepo<TEntity, TKey>.WriteQueAppendedHandler WriteQueAppendedEvent;
    }

}
