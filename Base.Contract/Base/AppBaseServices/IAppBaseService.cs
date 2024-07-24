using Base.Domain.Audities;
using Base.Domain.Models.OutPutModels;
using Base.Domain.Models.Query;
using Base.Domain.Shared.ViewModels.General;
using MongoDB.Driver;

namespace Base.Contract.Base.AppBaseServices
{
    public interface IAppBaseService<TEntity, TKey, TAutoCompleteResult, TPagedRequestDto, TPagedResultDto, TCreateDto, TUpdateDto>
        where TEntity : class, IBaseEntity<TKey>
        where TAutoCompleteResult : GeneralIdAndTitle
        where TPagedRequestDto : SearchRequestModel
    {
        Task<ServiceResult<TKey>> AddAsync(TCreateDto model);
        Task<ServiceResult> AfterDeleteAsync(TEntity entity);
        Task<ServiceResult> BeforeDeleteAsync(TEntity entity);
        Task<ServiceResult<TEntity>> CheckAccessAsync(TKey Id, bool forView, Func<IAggregateFluent<TEntity>, IAggregateFluent<TEntity>> custom = null);
        Task<ServiceResult<TResult>> GetByIdAsync<TResult>(TKey Id, bool forView, Func<IAggregateFluent<TEntity>, IAggregateFluent<TResult>> custom = null);
        Task<ServiceResult> DeleteAsync(TEntity entity);
        List<TAutoCompleteResult> GetAutoComplete(string q);
        PagedAggregate<TEntity, TPagedResultDto> GetList(TPagedRequestDto model);
        Task<ServiceResult> UpdateAsync(TUpdateDto model, TEntity entity);
        bool ValidateManagement(params TKey[] Ids);
        bool ValidateUsing(params TKey[] Ids);
    }

    public interface IAppBaseFullService<TEntity, TKey, TEntityDto, TAutoCompleteResult, TPagedRequestDto, TPagedResultDto, TCreateDto, TUpdateDto> :
        IAppBaseService<TEntity, TKey, TAutoCompleteResult, TPagedRequestDto, TPagedResultDto, TCreateDto, TUpdateDto>
        where TEntity : class, IBaseEntity<TKey>
        where TAutoCompleteResult : GeneralIdAndTitle
        where TPagedRequestDto : SearchRequestModel
    {

    }
}