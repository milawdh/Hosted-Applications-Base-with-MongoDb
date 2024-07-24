using Base.Domain.EventArgsModels.Repo;
using Base.Domain.Models.OutPutModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.RepositoriesApi.Core
{
    public interface ICore : IDisposable
    {
        ServiceResult<List<ServiceResult>> SaveChange();
        Task<ServiceResult<List<ServiceResult>>> SaveChangeAsync();



        public void StartTransactionMain();

        public void CommitTransactionMain();
        public void AbortTransactionMain();



        public void SubscribeSaveChangedEvent(SaveChangedEventHandler handler);
        public void SubscribeSaveChangedAsyncEvent(OnSaveChangedAsyncEventHandler handler);
        public void UnSubscribeSaveChangedEvent(SaveChangedEventHandler handler);
        public void UnSubscribeSaveChangedAsyncEvent(OnSaveChangedAsyncEventHandler handler);

        delegate ServiceResult SaveChangedEventHandler(ICore sender, SaveChangeEventArgs saveChangeEventArgs);
        delegate Task<ServiceResult> OnSaveChangedAsyncEventHandler(ICore sender, SaveChangeEventArgs saveChangeEventArgs);

    }
}
