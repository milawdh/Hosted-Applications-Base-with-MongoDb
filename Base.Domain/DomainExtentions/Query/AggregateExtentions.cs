using Amazon.Auth.AccessControlPolicy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using Mapster;
using Mapster.Models;
using Microsoft.IdentityModel.Tokens;
using Base.Domain.DomainExceptions;
using Base.Domain.Entities.Base.Access;
using Base.Domain.Models.Query;
using Base.Domain.Enums.Base.Query;
using Base.Domain.Audities;

namespace Base.Domain.DomainExtentions.Query
{
    public static class AggregateExtentions
    {

        /// <summary>
        /// Gets Expression of Mapping From Mapster MapContext
        /// </summary>
        /// <typeparam name="TSource">The Source Type You want To Append Map On It</typeparam>
        /// <typeparam name="TDestionantion">The Destination Type You Want To Have From Map operation</typeparam>
        /// <returns></returns>
        public static Expression<Func<TSource, TDestionantion>> CreateMapExpression<TSource, TDestionantion>()
        {
            TypeTuple tuple = new TypeTuple(typeof(TSource), typeof(TDestionantion));
            var expression = (Expression<Func<TSource, TDestionantion>>)TypeAdapterConfig.GlobalSettings.CreateMapExpression(tuple, MapType.Map);

            return expression;
        }

        /// <summary>
        /// Creates a Filter Defination for <see cref="AggregateDynamicFilterItem"/> Model 
        /// </summary>
        /// <typeparam name="TSource">The Source Aggregate Type</typeparam>
        /// <param name="filter">Dynamic Filter Model</param>
        /// <param name="mapkeys">Mapkey for the fields in dto that changed name from database</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException">If the filter type was not valid this exception will occur!</exception>
        public static FilterDefinition<TSource> GetFilterDefination<TSource>(AggregateDynamicFilterItem filter, Dictionary<string, string> mapkeys = null)
        {
            FilterDefinition<TSource> filterDefinition = null;
            string fieldName;

            mapkeys.TryGetValue(filter.FieldName, out fieldName);
            fieldName = fieldName ?? filter.FieldName;

            switch (filter.FilterType)
            {
                case FilterType.Eq:
                    filterDefinition = Builders<TSource>.Filter.And(Builders<TSource>.Filter.Exists(fieldName), Builders<TSource>.Filter.Eq(fieldName, filter.FilterCompareValue));
                    break;
                case FilterType.Gt:
                    filterDefinition = Builders<TSource>.Filter.And(Builders<TSource>.Filter.Exists(fieldName), Builders<TSource>.Filter.Gt(fieldName, filter.FilterCompareValue));
                    break;
                case FilterType.Regex:
                    filterDefinition = Builders<TSource>.Filter.And(Builders<TSource>.Filter.Exists(fieldName), Builders<TSource>.Filter.Regex(fieldName, filter.FilterStrValue));
                    break;
                case FilterType.Gte:
                    filterDefinition = Builders<TSource>.Filter.And(Builders<TSource>.Filter.Exists(fieldName), Builders<TSource>.Filter.Gte(fieldName, filter.FilterCompareValue));
                    break;
                case FilterType.Lt:
                    filterDefinition = Builders<TSource>.Filter.And(Builders<TSource>.Filter.Exists(fieldName), Builders<TSource>.Filter.Lt(fieldName, filter.FilterCompareValue));
                    break;
                case FilterType.Mod:
                    filterDefinition = Builders<TSource>.Filter.And(Builders<TSource>.Filter.Exists(fieldName), Builders<TSource>.Filter.Mod(fieldName, filter.FilterIntValue, 0));
                    break;
                case FilterType.Lte:
                    filterDefinition = Builders<TSource>.Filter.And(Builders<TSource>.Filter.Exists(fieldName), Builders<TSource>.Filter.Lte(fieldName, filter.FilterCompareValue));
                    break;
                case FilterType.In:
                    filterDefinition = Builders<TSource>.Filter.And(Builders<TSource>.Filter.Exists(fieldName), Builders<TSource>.Filter.In(fieldName, filter.FilterArrayValue));
                    break;
                case FilterType.StrIn:

                    var regexes = filter.FilterStrArrayValue.Select(x => Builders<TSource>.Filter.Regex(fieldName, new BsonRegularExpression(x))).ToArray();
                    filterDefinition = Builders<TSource>.Filter.Or(regexes);

                    break;
                case FilterType.Range:
                    FilterDefinition<TSource> rangeFilterFrom;
                    FilterDefinition<TSource> rangeFilterUntil;

                    //From Filter
                    if (filter.RangeValue.IsFromEqual)
                        rangeFilterFrom = Builders<TSource>.Filter.And(Builders<TSource>.Filter.Exists(fieldName), Builders<TSource>.Filter.Gte(fieldName, filter.RangeValue.FromValue));
                    else
                        rangeFilterFrom = Builders<TSource>.Filter.And(Builders<TSource>.Filter.Exists(fieldName), Builders<TSource>.Filter.Gt(fieldName, filter.RangeValue.FromValue));

                    //Until Filter
                    if (filter.RangeValue.IsUntileEqual)
                        rangeFilterUntil = Builders<TSource>.Filter.And(Builders<TSource>.Filter.Exists(fieldName), Builders<TSource>.Filter.Lte(fieldName, filter.RangeValue.UntilValue));
                    else
                        rangeFilterUntil = Builders<TSource>.Filter.And(Builders<TSource>.Filter.Exists(fieldName), Builders<TSource>.Filter.Lt(fieldName, filter.RangeValue.UntilValue));

                    //Defination
                    filterDefinition = Builders<TSource>.Filter.And(rangeFilterFrom, rangeFilterUntil);

                    break;
                case FilterType.ComplexWordSearch:

                    filterDefinition = Builders<TSource>.Filter.Text(filter.FilterStrValue, new TextSearchOptions
                    {
                        CaseSensitive = false,
                        DiacriticSensitive = false,
                    });

                    break;
                default:
                    throw new NotImplementedException();
            }

            return filterDefinition;
        }

        /// <summary>
        /// Filters Aggregate By the model! Mostly will used in grids
        /// </summary>
        /// <param name="source">The source aggregate that you want to append filter</param>
        /// <typeparam name="TSource">The Source Aggregate Type</typeparam>
        /// <param name="model">Dynamic Filter Model</param>
        /// <param name="mapKeys">Mapkey for the fields in dto that changed name from database</param>
        /// <returns></returns>
        public static IAggregateFluent<TSource> FilterAggregate<TSource>(IAggregateFluent<TSource> source, AggregateDynamicFilterModel model, Dictionary<string, string> mapKeys = null)
            where TSource : class
        {

            model.ValidateFilter();

            mapKeys = mapKeys ?? new Dictionary<string, string>();

            List<FilterDefinition<TSource>> filterDefinitions = new List<FilterDefinition<TSource>>();

            foreach (var filter in model.FilterItems)
            {
                string fieldName;
                mapKeys.TryGetValue(filter.FieldName, out fieldName);
                fieldName = fieldName ?? filter.FieldName;

                FilterDefinition<TSource> filterDefinition = null;
                filterDefinition = GetFilterDefination<TSource>(filter, mapKeys);

                if (!filter.FilterIsContain)
                    filterDefinition = Builders<TSource>.Filter.Not(filterDefinition);

                filterDefinitions.Add(filterDefinition);
            }

            var finalFilter = Builders<TSource>.Filter.And(filterDefinitions.ToArray());

            return source.Match(finalFilter);
        }

        /// <summary>
        /// Appends Sorting By the dynamic sort model on the aggregate!
        /// </summary>
        /// <param name="source">The source aggregate that you want to append filter</param>
        /// <typeparam name="TSource">The Source Aggregate Type</typeparam>
        /// <param name="model">Dynamic Sort Model</param>
        /// <param name="mapKeys">Mapkey for the fields in dto that changed name from database</param>
        /// <returns></returns>
        public static IAggregateFluent<TSource> SortAggregate<TSource>(IAggregateFluent<TSource> source, AggregateSortModel model, Dictionary<string, string> mapKeys = null)
        {

            mapKeys = mapKeys ?? new Dictionary<string, string>();

            List<FilterDefinition<TSource>> filterDefinitions = new List<FilterDefinition<TSource>>();

            string fieldName;
            mapKeys.TryGetValue(model.FieldName, out fieldName);
            fieldName = fieldName ?? model.FieldName;


            SortDefinition<TSource> sortDefinition;

            if (model.Ascending)
                sortDefinition = Builders<TSource>.Sort.Ascending(fieldName);
            else
                sortDefinition = Builders<TSource>.Sort.Descending(fieldName);

            return source.Sort(sortDefinition);
        }


        public static IAggregateFluent<TSource> JoinCreatorUser<TSource, TSourceKey>(this IAggregateFluent<TSource> aggregate, IMongoCollection<BsonDocument> foreignCollection) where TSource : ICreationAuditedEntity<TSourceKey>
        {

            aggregate = aggregate.Lookup(foreignCollection, new BsonDocument("key", $"${nameof(ICreationAuditedEntity<TSourceKey>.CreatedById)}"),
                  new BsonDocument[]
                      {
                        new BsonDocument("$match",new BsonDocument()
                        {
                            { "$expr" , new BsonDocument("$and" ,  new BsonArray
                            {
                                new BsonDocument("$eq",new BsonArray{$"$_id","$$key"})
                            }) }
                        })
                      },
                  "joins"
                  )
                  .AppendStage<BsonDocument>(new BsonDocument("$set", new BsonDocument(nameof(UserRoleCollection.CreatorUser), new BsonDocument("$arrayElemAt", new BsonArray { "$joins", 0 }))))
                  .Project<BsonDocument>(Builders<BsonDocument>.Projection.Exclude("joins"))
                  .As<TSource>();

            return aggregate;
        }

        public static IAggregateFluent<TSource> JoinCreatorCustomer<TSource, TSourceKey>(this IAggregateFluent<TSource> aggregate, IMongoCollection<BsonDocument> foreignCollection)
            where TSource : ICreationAuditedEntity<TSourceKey>
        {

            aggregate = aggregate.Lookup(foreignCollection, new BsonDocument("key", $"${nameof(ICreationAuditedEntity<TSourceKey>.CreatedById)}"),
                  new BsonDocument[]
                      {
                        new BsonDocument("$match",new BsonDocument()
                        {
                            { "$expr" , new BsonDocument("$and" ,  new BsonArray
                            {
                                new BsonDocument("$eq",new BsonArray{$"$_id","$$key"}),
                            }) }
                        })
                      },
                  "joins"
                  )
                  .AppendStage<BsonDocument>(new BsonDocument("$set", new BsonDocument(nameof(UserRoleCollection.CreatorCustomer), new BsonDocument("$arrayElemAt", new BsonArray { "$joins", 0 }))))
                  .Project<BsonDocument>(Builders<BsonDocument>.Projection.Exclude("joins"))
                  .As<TSource>();

            return aggregate;
        }

        /// <summary>
        /// Validates Dynamic Filter Model!
        /// </summary>
        /// <param name="model">The model you want to validate</param>
        /// <exception cref="InvalidArgumentException">It will occur if the model was not valid!</exception>
        public static void ValidateFilter(this AggregateDynamicFilterModel model)
        {
            foreach (var item in model.FilterItems)
            {
                switch (item.FilterType)
                {
                    case FilterType.Eq:
                        ValidateComparValue(item.FilterCompareValue);
                        break;
                    case FilterType.Gt:
                        ValidateComparValue(item.FilterCompareValue);

                        break;
                    case FilterType.Gte:
                        ValidateComparValue(item.FilterCompareValue);

                        break;
                    case FilterType.Lt:
                        ValidateComparValue(item.FilterCompareValue);

                        break;
                    case FilterType.Lte:
                        ValidateComparValue(item.FilterCompareValue);

                        break;
                    case FilterType.In:
                        if (item.FilterCompareValue is null)
                            throw new InvalidArgumentException("Invalid Filter");

                        foreach (var arrayItem in item.FilterArrayValue)
                        {
                            ValidateComparValue(arrayItem);
                        }
                        break;
                    case FilterType.StrIn:
                        if (item.FilterStrArrayValue is null)
                            throw new InvalidArgumentException("Invalid Filter");

                        break;
                    case FilterType.Mod:
                        if (item.FilterIntValue <= 0)
                            throw new InvalidArgumentException("Invalid Filter");

                        break;
                    case FilterType.Regex:
                        if (item.FilterStrValue.IsNullOrEmpty())
                            throw new InvalidArgumentException("Invalid Filter");

                        break;
                    case FilterType.ComplexWordSearch:
                        if (item.FilterStrValue.IsNullOrEmpty())
                            throw new InvalidArgumentException("Invalid Filter");

                        break;
                    case FilterType.Range:
                        ValidateComparValue(item.RangeValue?.FromValue);
                        ValidateComparValue(item.RangeValue?.UntilValue);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Validates Objects values in filter model
        /// </summary>
        /// <param name="value">The object you want to validate in filter model!</param>
        /// <exception cref="InvalidArgumentException">It will occur if the model was not valid!</exception>
        private static void ValidateComparValue(object value)
        {
            if (value is string ||
                value is long ||
                value is TimeSpan ||
                value is bool ||
                value is DateTime)
                return;

            throw new InvalidArgumentException("Invalid Filter");
        }
    }
}
