using Base.Contract.Base.AppBaseServices;
using Base.Domain.Audities;
using Base.Domain.DomainExceptions;
using Base.Domain.DomainExtentions.Query;
using Base.Domain.Enums.Base.Query;
using Base.Domain.Models.OutPutModels;
using Base.Domain.Models.Query;
using Base.Domain.RepositoriesApi.Core;
using Base.Domain.RepositoriesApi.Repo;
using Base.Domain.Shared.ViewModels.General;

using Mapster;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace Base.ServiceLayer.BaseServices
{
    /// <summary>
    /// Application Base Service For Entities does not have Soft Delete!
    /// </summary>
    /// <typeparam name="TEntity">Entity Type</typeparam>
    /// <typeparam name="TKey">Entity Primary Key Type</typeparam>
    /// <typeparam name="TAutoCompleteResult">The Result Type for Auto Complete</typeparam>
    /// <typeparam name="TEntityDto">Entity Result Dto</typeparam>
    /// <typeparam name="TPagedRequestDto">Entity Paged List Request Type For List Filterings</typeparam>
    /// <typeparam name="TPagedResultDto">Entity Paged List Request Type For List Result<typeparam>
    /// <typeparam name="TCreateDto">Entity Create Dto</typeparam>
    /// <typeparam name="TUpdateDto">Entity Update Dto</typeparam>
    public class AppBaseService<TEntity, TKey, TEntityDto, TAutoCompleteResult, TPagedRequestDto, TPagedResultDto, TCreateDto, TUpdateDto> : IAppBaseService<TEntity, TKey, TAutoCompleteResult, TPagedRequestDto, TPagedResultDto, TCreateDto, TUpdateDto> where TEntity : class, IBaseEntity<TKey>
        where TAutoCompleteResult : GeneralIdAndTitle
        where TPagedRequestDto : SearchRequestModel
    {
        protected readonly IBaseCore _mainCore;
        protected readonly ITenantCore _customerCore;

        public AppBaseService(IBaseCore core, ITenantCore customerCore)
        {
            _mainCore = core;
            _customerCore = customerCore;
        }

        /// <summary>
        /// The Main Repository For Given Entity! You Should Override it and give the related repository from <see cref="IBaseCore"/>!
        /// </summary>
        protected virtual IMainRepo<TEntity, TKey> Repository
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #region Query & CheckAccess

        /// <summary>
        /// Query for Managing Related Cartables!
        /// </summary>
        protected IAggregateFluent<TEntity> AggregateManagement
        {
            get
            {
                return Repository.Aggregate().Match(ExpressionManagement);
            }
        }

        protected IAggregateFluent<TEntity> AggregateUsing
        {
            get
            {
                return Repository.Aggregate().Match(ExpressionUsing);
            }
        }

        /// <summary>
        /// The Filter Expression of the Related Cartable!
        /// </summary>
        protected virtual Expression<Func<TEntity, bool>> ExpressionManagement
        {
            get
            {
                return x => true;
            }
        }
        protected virtual Expression<Func<TEntity, bool>> ExpressionUsing
        {
            get
            {
                return ExpressionManagement;
            }
        }


        /// <summary>
        /// Get Entity By Checking Access of current user for the given Id!
        /// </summary>
        /// <param name="Id">Id of Entity</param>
        /// <param name="forView">Is Checking access for Showing Entity</param>
        /// <param name="custom">Custom Aggreagation for filtering or other customizings!</param>
        /// <returns></returns>
        public virtual async Task<ServiceResult<TEntity>> CheckAccessAsync(TKey Id, bool forView, Func<IAggregateFluent<TEntity>, IAggregateFluent<TEntity>> custom = null)
        {
            var query = AggregateManagement.Match(Builders<TEntity>.Filter.Eq(x => x.Id, Id));

            if (!query.Any())
                return new ServiceResult<TEntity>("موجودیت مورد نظر یافت نشد!");

            if (custom != null)
                query = custom(query);


            TEntity entity = query.FirstOrDefault();

            if (entity == null)
                return new ServiceResult<TEntity>("شما به این موجودیت دسترسی ندارید!");

            return new ServiceResult<TEntity>(entity);
        }


        /// <summary>
        /// Get Entity By Checking Access of current user for the given Id and Project It to the <typeparamref name="TResult"/>
        /// </summary>
        /// <param name="Id">Id of Entity</param>
        /// <param name="forView">Is Checking access for Showing Entity</param>
        /// <param name="custom">Custom Aggreagation for Projection and filtering or other customizings! If You give it null The Mapster Projection Will Be Used!</param>
        /// <typeparam name="TResult">Type of Projection You need!</typeparam>
        /// <returns></returns>
        public virtual async Task<ServiceResult<TResult>> GetByIdAsync<TResult>(TKey Id, bool forView, Func<IAggregateFluent<TEntity>, IAggregateFluent<TResult>> custom = null)
        {
            if (custom != null)
            {
                var aggreagate = AggregateManagement.Match(Builders<TEntity>.Filter.Eq(x => x.Id, Id));

                if (!aggreagate.Any())
                    return new ServiceResult<TResult>("موجودیت مورد نظر یافت نشد!");


                var customAggreagate = custom(aggreagate);

                TResult aggreagateResult = customAggreagate.FirstOrDefault();

                if (aggreagateResult is null)
                    return new ServiceResult<TResult>("شما به این موجودیت دسترسی ندارید!");

                return new ServiceResult<TResult>(aggreagateResult);
            }

            var query = AggregateManagement.Match(a => a.Id.Equals(Id));

            if (!query.Any())
                return new ServiceResult<TResult>("موجودیت مورد نظر یافت نشد!");

            TResult queryResult = query.Project(AggregateExtentions.CreateMapExpression<TEntity, TResult>()).FirstOrDefault();

            if (queryResult is null)
                return new ServiceResult<TResult>("شما به این موجودیت دسترسی ندارید!");

            return new ServiceResult<TResult>(queryResult);

        }

        #endregion

        #region GetList

        /// <summary>
        /// Auto Complete With UsingExpression with the given q! it search all entire documents with created Indexes
        /// </summary>
        /// <param name="q">The Search Phrase</param>
        /// <returns></returns>
        public virtual List<TAutoCompleteResult> GetAutoComplete(string q)
        {
            var filter = AggregateExtentions.GetFilterDefination<TEntity>(new AggregateDynamicFilterItem
            {
                FilterCompareValue = q,
                FilterType = FilterType.ComplexWordSearch
            });

            return AggregateUsing.Match(filter).Limit(10).Project(AggregateExtentions.CreateMapExpression<TEntity, TAutoCompleteResult>()).ToList();
        }

        /// <summary>
        /// Use it For Custom Filtering Query of PagedResult before paging and Quering!
        /// </summary>
        /// <param name="model">The Request Model of Paging!</param>
        /// <param name="source">The Source Query that you want to filter</param>
        /// <returns></returns>
        protected virtual ServiceResult<IAggregateFluent<TEntity>> CustomizeGetListFilter(TPagedRequestDto model, IAggregateFluent<TEntity> source)
        {
            return new ServiceResult<IAggregateFluent<TEntity>>(source);
        }

        /// <summary>
        /// Gets Fields Map Keys For <see cref="GetList(TPagedRequestDto)"/> Method Filtering and Sortings!
        /// </summary>
        /// <returns></returns>
        protected virtual Dictionary<string, string> GetListMapKeys()
        {
            return new Dictionary<string, string>();
        }

        /// <summary>
        /// Get Paged List Of Entities for Grid!
        /// </summary>
        /// <param name="model">The Request Model of Paging!</param>
        /// <returns></returns>
        public virtual PagedAggregate<TEntity, TPagedResultDto> GetList(TPagedRequestDto model)
        {
            var filterResult = CustomizeGetListFilter(model, Repository.Aggregate().Match(ExpressionManagement));
            if (filterResult.IsFailed)
                throw new InvalidArgumentException(filterResult.Messages);


            IAggregateFluent<TEntity> filteredAggregate = filterResult.Result;
            var mapKeys = GetListMapKeys();


            if (model.GetFilterModel() is not null)
                filteredAggregate = AggregateExtentions.FilterAggregate(filteredAggregate, model.GetFilterModel(), mapKeys);

            if (model.GetSortModel() is not null)
                filteredAggregate = AggregateExtentions.SortAggregate(filteredAggregate, model.GetSortModel(), mapKeys);

            var result = new PagedAggregate<TEntity, TPagedResultDto>(filterResult.Result, model.StartCount, model.EndCount, Repository.EstimateAllDocsCount(), GetListCustomizeAggregate);

            var afterResult = AfterGetList(model, result);
            if (afterResult.IsFailed)
                throw new InvalidArgumentException(afterResult.Messages);

            return result;
        }

        /// <summary>
        /// The Customization of Aggregate after paging and befor projection would be here!  You can perform any pipeline that would be needed before projection!
        /// </summary>
        /// <param name="aggregate">The Aggregate That Paged in the <see cref="PagedAggregate{TSource, TResult}"/> Model!</param>
        /// <returns></returns>
        protected virtual IAggregateFluent<TEntity> GetListCustomizeAggregate(IAggregateFluent<TEntity> aggregate)
        {
            return aggregate;
        }

        /// <summary>
        /// Will be invoked after GetList and you can do some operations on the result!
        /// </summary>
        /// <param name="model">The Request Model of Paging!</param>
        /// <param name="source">The Result of GetList!</param>
        /// <returns></returns>
        protected virtual ServiceResult AfterGetList(TPagedRequestDto model, PagedAggregate<TEntity, TPagedResultDto> source)
        {
            return new ServiceResult();
        }

        #endregion

        #region Add

        /// <summary>
        /// Will Be Invoked Before Adding Entity To DataBase
        /// </summary>
        /// <param name="model">Create Dto of Entity</param>
        /// <param name="entity">Adapted Entity Before Saving It on DataBase!</param>
        /// <returns></returns>
        protected virtual async Task<ServiceResult> BeforeAddAsync(TCreateDto model, TEntity entity)
        {
            return new ServiceResult();
        }

        /// <summary>
        /// Will Be Invoked After Adding Entity To DataBase
        /// </summary>
        /// <param name="model">Create Dto of Entity</param>
        /// <param name="entity">Adapted Entity After Saving It on DataBase!</param>
        /// <returns></returns>
        protected virtual async Task<ServiceResult> AfterAddAsync(TCreateDto model, TEntity entity)
        {
            return new ServiceResult();
        }

        /// <summary>
        /// Adds Entity On DataBase
        /// </summary>
        /// <param name="model">Create Dto of Entity</param>
        /// <returns></returns>
        public virtual async Task<ServiceResult<TKey>> AddAsync(TCreateDto model)
        {
            var entity = model.Adapt<TEntity>();

            var beforeResult = await BeforeAddAsync(model, entity);
            if (beforeResult.IsFailed)
                return new ServiceResult<TKey>(beforeResult);

            Repository.Add(entity);

            var saveResult = _mainCore.SaveChange();
            if (saveResult.IsFailed)
                return new ServiceResult<TKey>(saveResult);

            var afterResult = await AfterAddAsync(model, entity);
            if (afterResult.IsFailed)
                return new ServiceResult<TKey>(afterResult);

            return new ServiceResult<TKey>(entity.Id);
        }

        #endregion

        #region Update

        /// <summary>
        /// Will Be Invoked Before Updating Entity!
        /// </summary>
        /// <param name="model">Update Dto of Entity!</param>
        /// <param name="entity">CheckAccessed Entity!</param>
        /// <returns></returns>
        protected virtual async Task<ServiceResult> BeforeUpdate(TUpdateDto model, TEntity entity)
        {
            return new ServiceResult();
        }

        /// <summary>
        /// Will Be Invoked After Updating Entity!
        /// </summary>
        /// <param name="model">Update Dto of Entity!</param>
        /// <param name="entity">CheckAccessed Entity!</param>
        /// <returns></returns>
        protected virtual async Task<ServiceResult> AfterUpdate(TUpdateDto model, TEntity entity)
        {
            return new ServiceResult();
        }

        /// <summary>
        /// Maps UpdateDto To Entity!
        /// </summary>
        /// <param name="model">Update Dto of Entity!</param>
        /// <param name="entity">CheckAccessed Entity!</param>
        /// <returns></returns>
        protected virtual void MapEntityForUpdate(TUpdateDto model, ref TEntity entity)
        {
            var newEntity = model.Adapt<TEntity>();
            newEntity.Id = entity.Id;

            entity = newEntity;
        }

        /// <summary>
        /// Updates Entity On DataBase!
        /// </summary>
        /// <param name="model">Update Dto of Entity!</param>
        /// <param name="entity">CheckAccessed Entity!</param>
        /// <returns></returns>
        public virtual async Task<ServiceResult> UpdateAsync(TUpdateDto model, TEntity entity)
        {
            var beforeResult = await BeforeUpdate(model, entity);
            if (beforeResult.IsFailed)
                return new ServiceResult(beforeResult);

            MapEntityForUpdate(model, ref entity);

            Repository.ReplaceOne(x => x.Id.Equals(entity.Id), entity);

            var saveResult = _mainCore.SaveChange();
            if (saveResult.IsFailed)
                return new ServiceResult(saveResult);

            var afterResult = await AfterUpdate(model, entity);
            if (afterResult.IsFailed)
                return new ServiceResult<TKey>(afterResult);

            return new ServiceResult();
        }


        #endregion

        #region Delete

        /// <summary>
        /// Will Be Invoked Before Deleting Entity!
        /// </summary>
        /// <param name="entity">CheckAccessed Entity!</param>
        /// <returns></returns>
        public virtual async Task<ServiceResult> BeforeDeleteAsync(TEntity entity)
        {
            return new ServiceResult();
        }

        /// <summary>
        /// Will Be Invoked After Entity Deleted From DataBase!
        /// </summary>
        /// <param name="entity">CheckAccessed Entity!</param>
        /// <returns></returns>
        public virtual async Task<ServiceResult> AfterDeleteAsync(TEntity entity)
        {
            return new ServiceResult();
        }

        /// <summary>
        /// Deletes Entity
        /// </summary>
        /// <param name="entity">CheckAccessed Entity!</param>
        /// <returns></returns>
        public virtual async Task<ServiceResult> DeleteAsync(TEntity entity)
        {
            var beforeResult = await BeforeDeleteAsync(entity);
            if (beforeResult.IsFailed)
                return beforeResult;

            Repository.DeleteForce(entity);

            var saveResult = _mainCore.SaveChange();
            if (saveResult.IsFailed)
                return new ServiceResult(saveResult);

            var afterResult = await AfterDeleteAsync(entity);
            if (afterResult.IsFailed)
                return new ServiceResult<TKey>(afterResult);

            return new ServiceResult();
        }

        #endregion

        #region Validations

        /// <summary>
        /// Validate Ids by the using expression!
        /// </summary>
        /// <param name="Ids">The Ids That Shoud be Validated</param>
        /// <returns></returns>
        public bool ValidateUsing(params TKey[] Ids)
        {
            return AggregateUsing.Match(x => Ids.Contains(x.Id)).Count().FirstOrDefault().Count == Ids.Length;
        }

        /// <summary>
        /// Validate Ids by the mangement expression!
        /// </summary>
        /// <param name="Ids">The Ids That Shoud be Validated</param>
        /// <returns></returns>
        public bool ValidateManagement(params TKey[] Ids)
        {
            return AggregateManagement.Match(x => Ids.Contains(x.Id)).Count().FirstOrDefault().Count == Ids.Length;
        }

        #endregion
    }

    /// <summary>
    /// Application Base Service For Entities have Soft Delete!
    /// </summary>
    /// <typeparam name="TEntity">Entity Type</typeparam>
    /// <typeparam name="TKey">Entity Primary Key Type</typeparam>
    /// <typeparam name="TEntityDto">Entity Result Dto</typeparam>
    /// <typeparam name="TPagedRequestDto">Entity Paged List Request Type For List Filterings</typeparam>
    /// <typeparam name="TPagedResultDto">Entity Paged List Request Type For List Result<typeparam>
    /// <typeparam name="TAutoCompleteResult">The Result Type for Auto Complete</typeparam>
    /// <typeparam name="TCreateDto">Entity Create Dto</typeparam>
    /// <typeparam name="TUpdateDto">Entity Update Dto</typeparam>
    public class AppBaseFullService<TEntity, TKey, TEntityDto, TAutoCompleteResult, TPagedRequestDto, TPagedResultDto, TCreateDto, TUpdateDto> :
        AppBaseService<TEntity, TKey, TEntityDto, TAutoCompleteResult, TPagedRequestDto, TPagedResultDto, TCreateDto, TUpdateDto>,
        IAppBaseFullService<TEntity, TKey, TEntityDto, TAutoCompleteResult, TPagedRequestDto, TPagedResultDto, TCreateDto, TUpdateDto>

        where TEntity : class, IFullAuditedEntity<TKey>, IBaseEntity<TKey>
        where TAutoCompleteResult : GeneralIdAndTitle
        where TPagedRequestDto : SearchRequestModel
    {
        public AppBaseFullService(IBaseCore core, ITenantCore customerCore) : base(core, customerCore)
        {
        }

        protected override Expression<Func<TEntity, bool>> ExpressionManagement
        {
            get
            {
                return x => !x.IsDeleted;
            }
        }

        public override async Task<ServiceResult> DeleteAsync(TEntity entity)
        {
            var beforeResult = await BeforeDeleteAsync(entity);
            if (beforeResult.IsFailed)
                return beforeResult;

            Repository.Delete(entity);

            if (_mainCore.SaveChange().IsFailed)
                return new ServiceResult<TKey>("خطایی در انجام عملیات رخ داد!");

            var afterResult = await AfterDeleteAsync(entity);
            if (afterResult.IsFailed)
                return new ServiceResult<TKey>(afterResult);

            return new ServiceResult();
        }
    }
}
