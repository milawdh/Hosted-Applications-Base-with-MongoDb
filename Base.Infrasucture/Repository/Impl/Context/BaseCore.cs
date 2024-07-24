

using Base.Infrasucture.Repository.Impl.MainRepos;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Base.Domain.RepositoriesApi.Core.ICore;
using System.ComponentModel.DataAnnotations;
using Base.Domain.Entities.Base.Access;
using Base.Domain.RepositoriesApi.Context;
using Base.Domain.RepositoriesApi.Repo;
using Base.Domain.RepositoriesApi.Core;
using Base.Domain.RepositoriesApi.UserInfo;
using Base.Domain.Entities.Base.Cities;
using Base.Domain.Entities.Base.Sms;
using Base.Domain.Entities.Base.Users;
using Base.Infrasucture.Connections.Api;

namespace Base.Infrasucture.Repository.Impl.Context
{
    public class BaseCore : Core, IBaseCore
    {
        #region Ctor & Fields

        public BaseCore(IBaseDbConnectionContext mainDbConnectionContext,
            IBaseUserInfoContext baseUserInfoContext, DomainValidationContext validationContext) : base(mainDbConnectionContext, baseUserInfoContext, validationContext)
        {
        }

        #endregion

        #region Private Repos

        private MainRepo<CityCollection, ObjectId> _cityRepoMain;
        private MainRepo<ProvinceCollection, ObjectId> _provinceMain;
        private MainRepo<UserCollection, ObjectId> _userMain;
        private MainRepo<SmsArchiveCollention, ObjectId> _smsVerificationCodeStoreMain;
        private IMainRepo<TokenStoreCollection, ObjectId> _userTokenStoreMain;
        private IMainRepo<UserRoleCollection, int> _userRolesMain;
        private IMainRepo<UserPermissionCollection, int> _permissions;

        #endregion


        #region Public Repos

        public IMainRepo<CityCollection, ObjectId> CityMain
        {
            get
            {
                if (_cityRepoMain == null)
                    _cityRepoMain = RegisterRepo<CityCollection, ObjectId>();

                return _cityRepoMain;
            }
        }
        public IMainRepo<ProvinceCollection, ObjectId> ProvinceMain
        {
            get
            {
                if (_provinceMain == null)
                    _provinceMain = RegisterRepo<ProvinceCollection, ObjectId>();

                return _provinceMain;
            }
        }


        public IMainRepo<UserCollection, ObjectId> Users
        {
            get
            {
                if (_userMain == null)
                    _userMain = RegisterRepo<UserCollection, ObjectId>();

                return _userMain;
            }
        }

        public IMainRepo<SmsArchiveCollention, ObjectId> PhoneVerificationMain
        {
            get
            {
                if (_smsVerificationCodeStoreMain == null)
                    _smsVerificationCodeStoreMain = RegisterRepo<SmsArchiveCollention, ObjectId>();

                return _smsVerificationCodeStoreMain;
            }
        }

        public IMainRepo<TokenStoreCollection, ObjectId> UserTokenStoreMain
        {
            get
            {
                if (_userTokenStoreMain == null)
                    _userTokenStoreMain = RegisterRepo<TokenStoreCollection, ObjectId>();

                return _userTokenStoreMain;
            }
        }

        public IMainRepo<UserRoleCollection, int> UserRole
        {
            get
            {
                if (_userRolesMain == null)
                    _userRolesMain = RegisterRepo<UserRoleCollection, int>();

                return _userRolesMain;
            }
        }
        public IMainRepo<UserPermissionCollection, int> UserPermissions
        {
            get
            {
                if (_permissions == null)
                    _permissions = RegisterRepo<UserPermissionCollection, int>();

                return _permissions;
            }
        }

        #endregion


        //public void StartTransactionMain()
        //{
        //    if (!_mainDBConnectionsContext.DbSession.IsInTransaction)
        //        _mainDBConnectionsContext.DbSession.StartTransaction();
        //}

        //public void CommitTransactionMain()
        //{
        //    if (_mainDBConnectionsContext.DbSession.IsInTransaction)
        //        _mainDBConnectionsContext.DbSession.CommitTransaction();
        //}
        //public void AbortTransactionMain()
        //{
        //    if (_mainDBConnectionsContext.DbSession.IsInTransaction)
        //        _mainDBConnectionsContext.DbSession.AbortTransaction();
        //}

        //public virtual ServiceResult<List<ServiceResult>> SaveChange()
        //{
        //    if (SaveChangedEvent is not null)
        //    {
        //        SaveChangedAsyncEvent = null;
        //        var args = new SaveChangeEventArgs();
        //        List<ServiceResult> results = new();

        //        if (!_mainDBConnectionsContext.DbSession.IsInTransaction)
        //            _mainDBConnectionsContext.DbSession.StartTransaction();

        //        if (_cutomerDbConnectionContext.DbSession is not null)
        //        {
        //            if (!_cutomerDbConnectionContext.DbSession.IsInTransaction)
        //                _cutomerDbConnectionContext.DbSession.StartTransaction();
        //        }
        //        foreach (SaveChangedEventHandler @event in SaveChangedEvent.GetInvocationList())
        //        {
        //            var result = @event.Invoke(this, args);
        //            results.Add(result);
        //            if (result.IsFailed)
        //            {
        //                _mainDBConnectionsContext.DbSession.AbortTransaction();

        //                if (_cutomerDbConnectionContext.DbSession is not null)
        //                {
        //                    if (_cutomerDbConnectionContext.DbSession.IsInTransaction)
        //                        _cutomerDbConnectionContext.DbSession.AbortTransaction();
        //                }

        //                return new ServiceResult<List<ServiceResult>>(result.Messages);
        //            }



        //        }

        //        _mainDBConnectionsContext.DbSession.CommitTransaction();

        //        if (_cutomerDbConnectionContext.DbSession is not null)
        //        {
        //            if (_cutomerDbConnectionContext.DbSession.IsInTransaction)
        //                _cutomerDbConnectionContext.DbSession.CommitTransaction();
        //        }

        //        return new ServiceResult<List<ServiceResult>>(results);
        //    }

        //    return new ServiceResult<List<ServiceResult>>(new List<ServiceResult>());
        //}

        //public virtual async Task<ServiceResult<List<ServiceResult>>> SaveChangeAsync()
        //{
        //    if (SaveChangedAsyncEvent is not null)
        //    {
        //        SaveChangedEvent = null;

        //        var args = new SaveChangeEventArgs();
        //        List<ServiceResult> results = new();

        //        if (!_mainDBConnectionsContext.DbSession.IsInTransaction)
        //            _mainDBConnectionsContext.DbSession.StartTransaction();

        //        if (_cutomerDbConnectionContext.DbSession is not null)
        //        {
        //            if (!_cutomerDbConnectionContext.DbSession.IsInTransaction)
        //                _cutomerDbConnectionContext.DbSession.StartTransaction();
        //        }
        //        else
        //        {
        //            foreach (SaveChangedEventHandler @event in SaveChangedEvent.GetInvocationList())
        //            {
        //                var result = @event.Invoke(this, args);
        //                results.Add(result);
        //                if (result.IsFailed)
        //                {
        //                    _mainDBConnectionsContext.DbSession.AbortTransaction();

        //                    if (_cutomerDbConnectionContext.DbSession is not null)
        //                    {
        //                        if (_cutomerDbConnectionContext.DbSession.IsInTransaction)
        //                            _cutomerDbConnectionContext.DbSession.AbortTransaction();
        //                    }

        //                    return new ServiceResult<List<ServiceResult>>(result.Messages);
        //                }



        //            }

        //            _mainDBConnectionsContext.DbSession.CommitTransaction();

        //            if (_cutomerDbConnectionContext.DbSession is not null)
        //            {
        //                if (_cutomerDbConnectionContext.DbSession.IsInTransaction)
        //                    _cutomerDbConnectionContext.DbSession.CommitTransaction();
        //            }

        //        }
        //        return new ServiceResult<List<ServiceResult>>(results);
        //    }

        //    return new ServiceResult<List<ServiceResult>>(new List<ServiceResult>());
        //}


        //public void SetValidationContext(DomainValidationContext validationContext)
        //{
        //    _validationContext = validationContext;
        //}

        //#region Events

        //protected virtual MainRepo<TEntity, TKey> RegisterRepo<TEntity, TKey>(IClientSessionHandle clientSessionHandle, IMongoDatabase mongoDatabase)
        //     where TEntity : IBaseEntity<TKey>
        //{
        //    var collection = mongoDatabase.GetCollection<TEntity>(typeof(TEntity).Name);
        //    var repo = new MainRepo<TEntity, TKey>(clientSessionHandle, collection, _webAppContext, _validationContext);

        //    repo.SubscribeWriteQueAppendedEvent((x) =>
        //    {
        //        x.Register(this);
        //    });

        //    return repo;
        //}

        //public void SubscribeSaveChangedEvent(SaveChangedEventHandler handler)
        //{
        //    SaveChangedEvent += handler;
        //}
        //public void SubscribeSaveChangedAsyncEvent(OnSaveChangedAsyncEventHandler handler)
        //{
        //    SaveChangedAsyncEvent += handler;
        //}

        //public void UnSubscribeSaveChangedEvent(SaveChangedEventHandler handler)
        //{
        //    if (SaveChangedEvent != null)
        //        if (SaveChangedEvent.GetInvocationList().Any(x => x == (MulticastDelegate)handler))
        //            SaveChangedEvent -= handler;
        //}

        //public void UnSubscribeSaveChangedAsyncEvent(OnSaveChangedAsyncEventHandler handler)
        //{
        //    if (SaveChangedAsyncEvent is not null)
        //        if (SaveChangedAsyncEvent.GetInvocationList().Any(x => x == (MulticastDelegate)handler))
        //            SaveChangedAsyncEvent -= handler;
        //}

        //public void Dispose()
        //{
        //}

        //private event SaveChangedEventHandler SaveChangedEvent;

        //private event OnSaveChangedAsyncEventHandler SaveChangedAsyncEvent;


        //#endregion

    }
}
