using System;
using System.Linq;
using System.Linq.Expressions;

namespace SharpRepository.Repository.Queries
{
    /// <summary>
    /// Used to define the paging criteria on queries run against a repository.
    /// </summary>
    /// <typeparam name="T">The entity type of the repository.</typeparam>
    /// <typeparam name="TSortKey">The type of the property that is being sorted.</typeparam>
    public class PagingOptions<T, TSortKey> : SortingOptions<T, TSortKey>
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int Skip { get { return (PageNumber - 1) * PageSize; } }
        public int Take { get { return PageSize; } }
        public int TotalItems { get; internal set; }

        public PagingOptions(int pageNumber, int pageSize, Expression<Func<T, TSortKey>> sortExpression, bool isDescending = false)
            : base(sortExpression, isDescending)
        {
            PageSize = pageSize;
            PageNumber = pageNumber;
        }

        /// <summary>
        /// Applies paging to the specified query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>Paged results.</returns>
        public override IQueryable<T> Apply(IQueryable<T> query)
        {
            query = base.Apply(query);

            TotalItems = query.Count();

            if (Skip > 0 || Take > 0)
            {
                return query.Skip(Skip).Take(Take);
            }
            
            return query;
        }

        /// <summary>
        /// Used in compiling a unique key for a query
        /// </summary>
        /// <returns>Unique key for a query</returns>
        public override string ToString()
        {
            return String.Format("PagingOptions<{0},{1}>\nPageSize: {2}\nPageNumber: {3}\nSort Expression: {4}\nIsDescending: {5}",
                (typeof(T)).Name,
                (typeof(TSortKey)).Name,
                PageSize,
                PageNumber,
                SortExpression == null ? "null" : SortExpression.ToString(),
                IsDescending
                );
        }
    }

    /// <summary>
    /// Used to define the paging criteria on queries run against a repository.
    /// </summary>
    /// <typeparam name="T">The entity type of the repository.</typeparam>
    public class PagingOptions<T> : SortingOptions<T>
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int Skip { get { return (PageNumber - 1) * PageSize; } }
        public int Take { get { return PageSize; } }
        public int TotalItems { get; set; }

        public PagingOptions(int pageNumber, int pageSize, string sortProperty, bool isDescending = false)
            : base(sortProperty, isDescending)
        {
            PageSize = pageSize;
            PageNumber = pageNumber;
        }


        /// <summary>
        /// Applies paging to the specified query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>Paged results.</returns>
        public override IQueryable<T> Apply(IQueryable<T> query)
        {
            query = base.Apply(query);

            TotalItems = query.Count();

            if (Skip > 0 || Take > 0)
            {
                return query.Skip(Skip).Take(Take);
            }

            return query;
        }

        /// <summary>
        /// Used in compiling a unique key for a query
        /// </summary>
        /// <returns>Unique key for a query</returns>
        public override string ToString()
        {
            return String.Format("PagingOptions<{0}>\nPageSize: {1}\nPageNumber: {2}\nSort Property: {3}\nIsDescending: {4}",
                (typeof(T)).Name,
                PageSize,
                PageNumber,
                SortProperty,
                IsDescending
                );
        }
    }
}