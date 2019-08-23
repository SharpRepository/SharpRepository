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
    public class PagingOptions<T, TSortKey> : PagingOptions<T>
    {
        public PagingOptions(int pageNumber, int pageSize, Expression<Func<T, TSortKey>> sortExpression, bool isDescending = false)
            : base(pageNumber, pageSize, sortExpression.ToString(), isDescending)
        {
            if (isDescending)
            {
                _primarySortAction = q => q.OrderByDescending(sortExpression);
            }
            else
            {
                _primarySortAction = q => q.OrderBy(sortExpression);
            }
        }
    }

    /// <summary>
    /// Used to define the paging criteria on queries run against a repository.
    /// </summary>
    /// <typeparam name="T">The entity type of the repository.</typeparam>
    public class PagingOptions<T> : SortingOptions<T>, IPagingOptions
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
            return string.Format("PagingOptions<{0}>\nPageSize: {1}\nPageNumber: {2}\nSort: {3}",
                typeof(T).Name,
                PageSize,
                PageNumber,
                base.ToString()
                );
        }
    }
}