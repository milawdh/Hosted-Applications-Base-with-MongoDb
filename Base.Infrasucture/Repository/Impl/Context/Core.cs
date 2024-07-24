using Base.Domain.Audities;
using Base.Domain.DomainExceptions;
using Base.Domain.EventArgsModels.Repo;
using Base.Domain.Models.OutPutModels;
using Base.Domain.RepositoriesApi.Context;
using Base.Domain.RepositoriesApi.Core;
using Base.Domain.RepositoriesApi.UserInfo;
using Base.Infrasucture.Connections.Api;

using Base.Infrasucture.Repository.Impl.MainRepos;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Base.Domain.RepositoriesApi.Core.ICore;

namespace Base.Infrasucture.Repository.Impl.Context
{
    public abstract class Core : ICore
    {
        protected DomainValidationContext _validationContext;
        protected readonly IBaseUserInfoContext _baseUserInfoContext;
        protected readonly IConnectionContext _dbConnectionsContext;
        public Core(IConnectionContext mainDbConnectionContext,
            IBaseUserInfoContext baseUserInfoContext, DomainValidationContext validationContext)
        {
            _dbConnectionsContext = mainDbConnectionContext;
            _baseUserInfoContext = baseUserInfoContext;
            _validationContext = validationContext;
        }

        public void StartTransactionMain()
        {
            if (!_dbConnectionsContext.DbSession.IsInTransaction)
                _dbConnectionsContext.DbSession.StartTransaction();
        }

        public void CommitTransactionMain()
        {
            if (_dbConnectionsContext.DbSession.IsInTransaction)
                _dbConnectionsContext.DbSession.CommitTransaction();
        }
        public void AbortTransactionMain()
        {
            if (_dbConnectionsContext.DbSession.IsInTransaction)
                _dbConnectionsContext.DbSession.AbortTransaction();
        }

        public virtual ServiceResult<List<ServiceResult>> SaveChange()
        {
            if (SaveChangedEvent is not null)
            {
                SaveChangedAsyncEvent = null;
                var args = new SaveChangeEventArgs();
                List<ServiceResult> results = new();

                if (!_dbConnectionsContext.DbSession.IsInTransaction)
                    _dbConnectionsContext.DbSession.StartTransaction();

                foreach (SaveChangedEventHandler @event in SaveChangedEvent.GetInvocationList())
                {
                    try
                    {

                        var result = @event.Invoke(this, args);
                        results.Add(result);
                        if (result.IsFailed)
                        {
                            _dbConnectionsContext.DbSession.AbortTransaction();

                            return new ServiceResult<List<ServiceResult>>(result.Messages);
                        }
                    }
                    catch (Exception ex)
                    {
                        ElmahCore.ElmahExtensions.RaiseError(ex);
                        _dbConnectionsContext.DbSession.AbortTransaction();
                        throw new InvalidArgumentException("خطایی در انجام عملیات رخ داد لطفا به پشتیبانی اطلاع دهید!");
                    }
                }

                _dbConnectionsContext.DbSession.CommitTransaction();

                return new ServiceResult<List<ServiceResult>>(results);
            }

            return new ServiceResult<List<ServiceResult>>(new List<ServiceResult>());
        }

        public virtual async Task<ServiceResult<List<ServiceResult>>> SaveChangeAsync()
        {
            if (SaveChangedAsyncEvent is not null)
            {
                SaveChangedEvent = null;

                var args = new SaveChangeEventArgs();
                List<ServiceResult> results = new();

                if (!_dbConnectionsContext.DbSession.IsInTransaction)
                    _dbConnectionsContext.DbSession.StartTransaction();

                else
                {
                    foreach (SaveChangedEventHandler @event in SaveChangedEvent.GetInvocationList())
                    {
                        try
                        {
                            var result = @event.Invoke(this, args);
                            results.Add(result);
                            if (result.IsFailed)
                            {
                                _dbConnectionsContext.DbSession.AbortTransaction();

                                return new ServiceResult<List<ServiceResult>>(result.Messages);
                            }
                        }
                        catch (Exception ex)
                        {
                            ElmahCore.ElmahExtensions.RaiseError(ex);
                            _dbConnectionsContext.DbSession.AbortTransaction();

                            throw new InvalidArgumentException("خطایی در انجام عملیات رخ داد لطفا به پشتیبانی اطلاع دهید!");
                        }

                    }
                    _dbConnectionsContext.DbSession.CommitTransaction();

                }
                return new ServiceResult<List<ServiceResult>>(results);
            }

            return new ServiceResult<List<ServiceResult>>(new List<ServiceResult>());
        }


        public void SetValidationContext(DomainValidationContext validationContext)
        {
            _validationContext = validationContext;
        }

        #region Events

        protected virtual MainRepo<TEntity, TKey> RegisterRepo<TEntity, TKey>()
             where TEntity : IBaseEntity<TKey>
        {
            var collection = _dbConnectionsContext.DataBase.GetCollection<TEntity>(typeof(TEntity).Name);
            var repo = new MainRepo<TEntity, TKey>(_dbConnectionsContext.DbSession, collection, _baseUserInfoContext, _validationContext);

            repo.SubscribeWriteQueAppendedEvent((x) =>
            {
                x.Register(this);
            });

            return repo;
        }

        public void SubscribeSaveChangedEvent(SaveChangedEventHandler handler)
        {
            SaveChangedEvent += handler;
        }
        public void SubscribeSaveChangedAsyncEvent(OnSaveChangedAsyncEventHandler handler)
        {
            SaveChangedAsyncEvent += handler;
        }

        public void UnSubscribeSaveChangedEvent(SaveChangedEventHandler handler)
        {
            if (SaveChangedEvent != null)
                if (SaveChangedEvent.GetInvocationList().Any(x => x == (MulticastDelegate)handler))
                    SaveChangedEvent -= handler;
        }

        public void UnSubscribeSaveChangedAsyncEvent(OnSaveChangedAsyncEventHandler handler)
        {
            if (SaveChangedAsyncEvent is not null)
                if (SaveChangedAsyncEvent.GetInvocationList().Any(x => x == (MulticastDelegate)handler))
                    SaveChangedAsyncEvent -= handler;
        }

        public void Dispose()
        {
        }

        private event SaveChangedEventHandler SaveChangedEvent;

        private event OnSaveChangedAsyncEventHandler SaveChangedAsyncEvent;


        #endregion
    }
}
