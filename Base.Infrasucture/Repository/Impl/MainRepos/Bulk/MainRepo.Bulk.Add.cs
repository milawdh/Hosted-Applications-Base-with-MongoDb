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
        /// Marks Adding Operation As A Part of BulkWrite! Need To Call <see cref="ICore.SaveChange()"/>
        /// </summary>
        /// <param name="entity">Entity To Add</param>
        public void Add(TEntity entity, bool igonreValidation = false)
        {
            if (!igonreValidation)
                entity.ValidateAdd(_validationContext);

            if (entity is ICreationAuditedEntity<TKey>)
                AppendAddAudited(ref entity);


            WritesQue.Add(new InsertOneModel<TEntity>(entity));
            PublishOnWriteAddedEvent();
        }

        /// <summary>
        /// Marks AddingMany Operation As A Part of BulkWrite! Need To Call <see cref="ICore.SaveChange()"/>
        /// </summary>
        /// <param name="entities">Entities To Add</param>
        public void AddMany(IEnumerable<TEntity> entities, bool igonreValidation = false)
        {
            if (!igonreValidation)
                foreach (var entity in entities)
                    entity.ValidateAdd(_validationContext);

            AppendAddAudited(ref entities);

            foreach (var entity in entities)
                WritesQue.Add(new InsertOneModel<TEntity>(entity));

            PublishOnWriteAddedEvent();
        }
    }
}