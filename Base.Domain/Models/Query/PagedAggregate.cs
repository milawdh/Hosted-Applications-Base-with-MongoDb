using Amazon.Auth.AccessControlPolicy;
using Base.Domain.DomainExtentions.Query;
using Mapster;
using Mapster.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Core.Misc;
using MongoDB.Driver.Search;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Models.Query
{
    /// <summary>
    /// A List That Represents Paging and Projection From <typeparamref name="TSource"/> To <typeparamref name="TResult"/>!
    /// </summary>
    /// <typeparam name="TSource">The Type of Source Query</typeparam>
    /// <typeparam name="TResult">The type of Projection Result!</typeparam>
    public class PagedAggregate<TSource, TResult> : IAggregateFluent<TResult>, IEnumerable<TResult>, IList<TResult>
    {

        protected List<TResult> _list;

        protected int _startCount;

        protected int _endCount;
        protected Func<IAggregateFluent<TSource>, IAggregateFluent<TSource>> _customizeBeforeProject;

        /// <summary>
        /// Append Paging On Aggregate
        /// </summary>
        /// <param name="source">Source Aggrefate</param>
        /// <param name="startCount">Paging Start From Count!(Offset count)</param>
        /// <param name="endCount">Paging End Count!</param>
        /// <param name="estimateTotalCount">Etimated Count of All the dcouments in source</param>
        /// <param name="customizeBeforeProject">The customization method after paging and before porjection! it can be some joins or another things!</param>
        public PagedAggregate(IAggregateFluent<TSource> source, int startCount, int endCount, long? estimateTotalCount = null, Func<IAggregateFluent<TSource>, IAggregateFluent<TSource>> customizeBeforeProject = null)
        {
            Source = source;
            _startCount = startCount;
            _endCount = endCount;
            if (estimateTotalCount != null)
            {
                TotalDataCount = estimateTotalCount.Value;
            }
            _customizeBeforeProject = customizeBeforeProject;
        }

        /// <summary>
        /// The Given Source Query
        /// </summary>
        public IAggregateFluent<TSource> Source { get; private set; }

        public IAggregateFluent<TResult> FinalSource
        {
            get
            {
                return GetFinalSource();
            }
        }

        /// <summary>
        /// Total Count of Source
        /// </summary>
        public long? TotalDataCount { get; set; }

        /// <summary>
        /// Enumerable Result
        /// </summary>
        public List<TResult> Result
        {
            get
            {
                PrepareResult();
                return _list;
            }
        }

        /// <summary>
        /// Converts the Enumerable Result to Model
        /// </summary>
        /// <returns></returns>
        public PagedResultModel<TResult> AsPagedModel()
        {
            return new PagedResultModel<TResult>()
            {
                Data = Result,
                PageCount = (int)Math.Ceiling(Convert.ToDouble((double)TotalDataCount / (_endCount - _startCount))),
                TotalDataCount = TotalDataCount.Value
            };
        }

        public TResult this[int index]
        {
            get
            {
                return _list[index];
            }
            set
            {
                _list[index] = value;
            }
        }

        public int Count
        {
            get
            {
                PrepareResult();
                return _list.Count;
            }
        }

        public bool IsReadOnly => true;

        public IMongoDatabase Database => FinalSource.Database;

        public AggregateOptions Options => FinalSource.Options;

        public IList<IPipelineStageDefinition> Stages => FinalSource.Stages;

        public void Add(TResult item)
        {
            _list.Add(item);
        }

        public void Clear()
        {
            _list.Clear();
        }

        public bool Contains(TResult item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(TResult[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public int IndexOf(TResult item)
        {
            return _list.IndexOf(item);
        }

        public void Insert(int index, TResult item)
        {
            _list.Insert(index, item);
        }

        public bool Remove(TResult item)
        {
            return _list.Remove(item);
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            PrepareResult();
            return _list.GetEnumerator();
        }
        public IEnumerator<TResult> GetEnumerator()
        {
            PrepareResult();
            return _list.GetEnumerator();
        }


        /// <summary>
        /// Prepares Enumurating Before getting Result from list!
        /// </summary>
        private void PrepareResult()
        {
            if (_list is null)
            {
                TotalDataCount = TotalDataCount ?? Source.Count().FirstOrDefault().Count;

                var aggregate = GetFinalSource();

                _list = aggregate.ToList();
            }
        }

        /// <summary>
        /// Applies Paging to Source
        /// </summary>
        /// <returns></returns>
        protected virtual IAggregateFluent<TResult> GetFinalSource()
        {
            if (_customizeBeforeProject is null)
                return Source.Skip(_startCount).Limit(_endCount - _startCount).Project(AggregateExtentions.CreateMapExpression<TSource, TResult>());
            var x = _customizeBeforeProject(Source.Skip(_startCount).Limit(_endCount - _startCount)).ToList();
            return _customizeBeforeProject(Source.Skip(_startCount).Limit(_endCount - _startCount)).Project(AggregateExtentions.CreateMapExpression<TSource, TResult>());
        }


        public IAggregateFluent<TNewResult> AppendStage<TNewResult>(PipelineStageDefinition<TResult, TNewResult> stage)
        {
            return FinalSource.AppendStage(stage);
        }


        public IAggregateFluent<TNewResult> As<TNewResult>(IBsonSerializer<TNewResult> newResultSerializer = null)
        {
            return FinalSource.As(newResultSerializer);

        }

        public IAggregateFluent<AggregateBucketResult<TValue>> Bucket<TValue>(AggregateExpressionDefinition<TResult, TValue> groupBy, IEnumerable<TValue> boundaries, AggregateBucketOptions<TValue> options = null)
        {
            return FinalSource.Bucket(groupBy, boundaries, options);
        }

        public IAggregateFluent<TNewResult> Bucket<TValue, TNewResult>(AggregateExpressionDefinition<TResult, TValue> groupBy, IEnumerable<TValue> boundaries, ProjectionDefinition<TResult, TNewResult> output, AggregateBucketOptions<TValue> options = null)
        {
            return FinalSource.Bucket(groupBy, boundaries, output, options);
        }

        public IAggregateFluent<AggregateBucketAutoResult<TValue>> BucketAuto<TValue>(AggregateExpressionDefinition<TResult, TValue> groupBy, int buckets, AggregateBucketAutoOptions options = null)
        {
            return FinalSource.BucketAuto(groupBy, buckets, options);
        }

        public IAggregateFluent<TNewResult> BucketAuto<TValue, TNewResult>(AggregateExpressionDefinition<TResult, TValue> groupBy, int buckets, ProjectionDefinition<TResult, TNewResult> output, AggregateBucketAutoOptions options = null)
        {
            return FinalSource.BucketAuto(groupBy, buckets, output, options);
        }

        public IAggregateFluent<ChangeStreamDocument<TResult>> ChangeStream(ChangeStreamStageOptions options = null)
        {
            return FinalSource.ChangeStream(options);
        }

        IAggregateFluent<AggregateCountResult> IAggregateFluent<TResult>.Count()
        {
            return FinalSource.Count();
        }

        public IAggregateFluent<TResult> Densify(FieldDefinition<TResult> field, DensifyRange range, IEnumerable<FieldDefinition<TResult>> partitionByFields = null)
        {
            return FinalSource.Densify(field, range, partitionByFields);
        }

        public IAggregateFluent<TResult> Densify(FieldDefinition<TResult> field, DensifyRange range, params FieldDefinition<TResult>[] partitionByFields)
        {
            return FinalSource.Densify(field, range, partitionByFields);
        }

        public IAggregateFluent<TNewResult> Facet<TNewResult>(IEnumerable<AggregateFacet<TResult>> facets, AggregateFacetOptions<TNewResult> options = null)
        {
            return FinalSource.Facet(facets, options);
        }

        public IAggregateFluent<TNewResult> GraphLookup<TFrom, TConnectFrom, TConnectTo, TStartWith, TAsElement, TAs, TNewResult>(IMongoCollection<TFrom> from, FieldDefinition<TFrom, TConnectFrom> connectFromField, FieldDefinition<TFrom, TConnectTo> connectToField, AggregateExpressionDefinition<TResult, TStartWith> startWith, FieldDefinition<TNewResult, TAs> @as, FieldDefinition<TAsElement, int> depthField, AggregateGraphLookupOptions<TFrom, TAsElement, TNewResult> options = null) where TAs : IEnumerable<TAsElement>
        {
            return FinalSource.GraphLookup(from, connectFromField, connectToField, startWith, @as, depthField, options);
        }

        public IAggregateFluent<TNewResult> Group<TNewResult>(ProjectionDefinition<TResult, TNewResult> group)
        {
            return FinalSource.Group(group);
        }

        public IAggregateFluent<TResult> Limit(long limit)
        {
            return FinalSource.Limit(limit);
        }

        public IAggregateFluent<TNewResult> Lookup<TForeignDocument, TNewResult>(string foreignCollectionName, FieldDefinition<TResult> localField, FieldDefinition<TForeignDocument> foreignField, FieldDefinition<TNewResult> @as, AggregateLookupOptions<TForeignDocument, TNewResult> options = null)
        {
            return FinalSource.Lookup(foreignCollectionName, localField, foreignField, @as, options);
        }

        public IAggregateFluent<TNewResult> Lookup<TForeignDocument, TAsElement, TAs, TNewResult>(IMongoCollection<TForeignDocument> foreignCollection, BsonDocument let, PipelineDefinition<TForeignDocument, TAsElement> lookupPipeline, FieldDefinition<TNewResult, TAs> @as, AggregateLookupOptions<TForeignDocument, TNewResult> options = null) where TAs : IEnumerable<TAsElement>
        {
            return FinalSource.Lookup(foreignCollection, let, lookupPipeline, @as, options);
        }

        public IAggregateFluent<TResult> Match(FilterDefinition<TResult> filter)
        {
            return FinalSource.Match(filter);
        }

        public IAsyncCursor<TOutput> Merge<TOutput>(IMongoCollection<TOutput> outputCollection, MergeStageOptions<TOutput> mergeOptions = null, CancellationToken cancellationToken = default)
        {
            return FinalSource.Merge(outputCollection, mergeOptions, cancellationToken);
        }

        public Task<IAsyncCursor<TOutput>> MergeAsync<TOutput>(IMongoCollection<TOutput> outputCollection, MergeStageOptions<TOutput> mergeOptions = null, CancellationToken cancellationToken = default)
        {
            return FinalSource.MergeAsync(outputCollection, mergeOptions, cancellationToken);
        }

        public IAggregateFluent<TNewResult> OfType<TNewResult>(IBsonSerializer<TNewResult> newResultSerializer = null) where TNewResult : TResult
        {
            return FinalSource.OfType(newResultSerializer);
        }

        public IAsyncCursor<TResult> Out(IMongoCollection<TResult> outputCollection, CancellationToken cancellationToken = default)
        {
            return FinalSource.Out(outputCollection, cancellationToken);
        }

        public IAsyncCursor<TResult> Out(string collectionName, CancellationToken cancellationToken = default)
        {
            return FinalSource.Out(collectionName, cancellationToken);
        }

        public IAsyncCursor<TResult> Out(IMongoCollection<TResult> outputCollection, TimeSeriesOptions timeSeriesOptions, CancellationToken cancellationToken = default)
        {
            return FinalSource.Out(outputCollection, timeSeriesOptions, cancellationToken);
        }

        public IAsyncCursor<TResult> Out(string collectionName, TimeSeriesOptions timeSeriesOptions, CancellationToken cancellationToken = default)
        {
            return FinalSource.Out(collectionName, timeSeriesOptions, cancellationToken);
        }

        public Task<IAsyncCursor<TResult>> OutAsync(IMongoCollection<TResult> outputCollection, CancellationToken cancellationToken = default)
        {
            return FinalSource.OutAsync(outputCollection, cancellationToken);
        }

        public Task<IAsyncCursor<TResult>> OutAsync(string collectionName, CancellationToken cancellationToken = default)
        {
            return FinalSource.OutAsync(collectionName, cancellationToken);
        }

        public Task<IAsyncCursor<TResult>> OutAsync(IMongoCollection<TResult> outputCollection, TimeSeriesOptions timeSeriesOptions, CancellationToken cancellationToken = default)
        {
            return FinalSource.OutAsync(outputCollection, timeSeriesOptions, cancellationToken);
        }

        public Task<IAsyncCursor<TResult>> OutAsync(string collectionName, TimeSeriesOptions timeSeriesOptions, CancellationToken cancellationToken = default)
        {
            return FinalSource.OutAsync(collectionName, timeSeriesOptions, cancellationToken);
        }

        public IAggregateFluent<TNewResult> Project<TNewResult>(ProjectionDefinition<TResult, TNewResult> projection)
        {
            return FinalSource.Project(projection);
        }

        public IAggregateFluent<TNewResult> ReplaceRoot<TNewResult>(AggregateExpressionDefinition<TResult, TNewResult> newRoot)
        {
            return FinalSource.ReplaceRoot(newRoot);
        }

        public IAggregateFluent<TNewResult> ReplaceWith<TNewResult>(AggregateExpressionDefinition<TResult, TNewResult> newRoot)
        {
            return FinalSource.ReplaceWith(newRoot);
        }

        public IAggregateFluent<TResult> Sample(long size)
        {
            return FinalSource.Sample(size);
        }

        public IAggregateFluent<TResult> Set(SetFieldDefinitions<TResult> fields)
        {
            return FinalSource.Set(fields);
        }

        public IAggregateFluent<BsonDocument> SetWindowFields<TWindowFields>(AggregateExpressionDefinition<ISetWindowFieldsPartition<TResult>, TWindowFields> output)
        {
            return FinalSource.SetWindowFields(output);
        }

        public IAggregateFluent<TResult> Search(SearchDefinition<TResult> searchDefinition, SearchHighlightOptions<TResult> highlight = null, string indexName = null, SearchCountOptions count = null, bool returnStoredSource = false, bool scoreDetails = false)
        {
            return FinalSource.Search(searchDefinition, highlight, indexName, count, returnStoredSource, scoreDetails);
        }

        public IAggregateFluent<TResult> Search(SearchDefinition<TResult> searchDefinition, SearchOptions<TResult> searchOptions)
        {
            return FinalSource.Search(searchDefinition, searchOptions);
        }

        public IAggregateFluent<SearchMetaResult> SearchMeta(SearchDefinition<TResult> searchDefinition, string indexName = null, SearchCountOptions count = null)
        {
            return FinalSource.SearchMeta(searchDefinition, indexName, count);
        }

        public IAggregateFluent<BsonDocument> SetWindowFields<TPartitionBy, TWindowFields>(AggregateExpressionDefinition<TResult, TPartitionBy> partitionBy, AggregateExpressionDefinition<ISetWindowFieldsPartition<TResult>, TWindowFields> output)
        {
            return FinalSource.SetWindowFields(partitionBy, output);
        }

        public IAggregateFluent<BsonDocument> SetWindowFields<TPartitionBy, TWindowFields>(AggregateExpressionDefinition<TResult, TPartitionBy> partitionBy, SortDefinition<TResult> sortBy, AggregateExpressionDefinition<ISetWindowFieldsPartition<TResult>, TWindowFields> output)
        {
            return FinalSource.SetWindowFields(partitionBy, sortBy, output);
        }

        public IAggregateFluent<TResult> Skip(long skip)
        {
            return FinalSource.Skip(skip);
        }

        public IAggregateFluent<TResult> Sort(SortDefinition<TResult> sort)
        {
            return FinalSource.Sort(sort);
        }

        public IAggregateFluent<AggregateSortByCountResult<TId>> SortByCount<TId>(AggregateExpressionDefinition<TResult, TId> id)
        {
            return FinalSource.SortByCount(id);
        }

        public void ToCollection(CancellationToken cancellationToken = default)
        {
            FinalSource.ToCollection(cancellationToken);
        }

        public Task ToCollectionAsync(CancellationToken cancellationToken = default)
        {
            return FinalSource.ToCollectionAsync(cancellationToken);
        }

        public IAggregateFluent<TResult> UnionWith<TWith>(IMongoCollection<TWith> withCollection, PipelineDefinition<TWith, TResult> withPipeline = null)
        {
            return FinalSource.UnionWith(withCollection, withPipeline);
        }

        public IAggregateFluent<TNewResult> Unwind<TNewResult>(FieldDefinition<TResult> field, IBsonSerializer<TNewResult> newResultSerializer)
        {
            return FinalSource.Unwind(field, newResultSerializer);
        }

        public IAggregateFluent<TNewResult> Unwind<TNewResult>(FieldDefinition<TResult> field, AggregateUnwindOptions<TNewResult> options = null)
        {
            return FinalSource.Unwind(field, options);
        }

        public IAggregateFluent<TResult> VectorSearch(FieldDefinition<TResult> field, QueryVector queryVector, int limit, VectorSearchOptions<TResult> options = null)
        {
            return FinalSource.VectorSearch(field, queryVector, limit, options);
        }

        public IAsyncCursor<TResult> ToCursor(CancellationToken cancellationToken = default)
        {
            return FinalSource.ToCursor(cancellationToken);
        }

        public Task<IAsyncCursor<TResult>> ToCursorAsync(CancellationToken cancellationToken = default)
        {
            return FinalSource.ToCursorAsync(cancellationToken);
        }
    }

    /// <summary>
    /// A List That Just Applies Paging! For Projection The <see cref="PagedAggregate{TSource, TResult}"/> Is More Recomended!
    /// </summary>
    /// <typeparam name="TResult">The type of List Items</typeparam>
    public class PagedList<TResult> : PagedAggregate<TResult, TResult>
    {
        public PagedList(IAggregateFluent<TResult> source, int startCount, int endCount) : base(source, startCount, endCount)
        {
        }


        protected virtual IAggregateFluent<TResult> GetFinalSource()
        {
            if (_customizeBeforeProject is null)
                return Source.Skip(_startCount).Limit(_endCount - _startCount);

            return _customizeBeforeProject(Source.Skip(_startCount).Limit(_endCount - _startCount));
        }
    }
}
