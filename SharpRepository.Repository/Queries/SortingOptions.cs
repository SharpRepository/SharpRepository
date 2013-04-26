using System;
using System.Linq;
using System.Linq.Expressions;
using SharpRepository.Repository.Linq;

namespace SharpRepository.Repository.Queries
{
    /// <summary>
    /// Used to define the sorting on queries run against a repository.
    /// </summary>
    /// <typeparam name="T">The entity type of the repository.</typeparam>
    /// <typeparam name="TSortKey">The type of the property that is being sorted.</typeparam>
    public class SortingOptions<T, TSortKey> : IQueryOptions<T>
    {
        public Expression<Func<T, TSortKey>> SortExpression { get; set; }
        public bool IsDescending { get; set; }

        public SortingOptions(Expression<Func<T, TSortKey>> sortExpression, bool isDescending = false)
        {
            SortExpression = sortExpression;
            IsDescending = isDescending;
        }

        /// <summary>
        /// Applies sorting to the specified query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>Sorted results.</returns>
        public virtual IQueryable<T> Apply(IQueryable<T> query)
        {
            if (SortExpression != null)
            {
                query = IsDescending 
                            ? query.OrderByDescending(SortExpression) 
                            : query.OrderBy(SortExpression);
            }
            
            return query;
        }

        /// <summary>
        /// Used in compiling a unique key for a query
        /// </summary>
        /// <returns>Unique key for a query</returns>
        public override string ToString()
        {
            return String.Format("SortingOptions<{0},{1}>\nSort Expression: {2}\nIsDescending: {3}",
                (typeof(T)).Name,
                (typeof(TSortKey)).Name,
                SortExpression == null ? "null" : SortExpression.ToString(),
                IsDescending
                );
        }
    }

    /// <summary>
    /// Used to define the sorting on queries run against a repository.
    /// </summary>
    /// <typeparam name="T">The entity type of the repository.</typeparam>
    public class SortingOptions<T> : IQueryOptions<T>
    {
        public string SortProperty { get; set; }
        public bool IsDescending { get; set; }

        public SortingOptions(string sortProperty, bool isDescending = false)
        {
            SortProperty = sortProperty;
            IsDescending = isDescending;
        }

        /// <summary>
        /// Applies sorting to the specified query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>Sorted results.</returns>
        public virtual IQueryable<T> Apply(IQueryable<T> query)
        {
            if (!String.IsNullOrEmpty(SortProperty))
            {
                // TODO: do we need to deal with the case where the user passes in "Name desc", should we strip the desc out, or let it override the isDescending param, or not deal with it and blame it on the user?
                var sortString = String.Format("{0}{1}", SortProperty, IsDescending ? " desc" : "");
                query = query.OrderBy(sortString);
            }

            return query;
        }

        /// <summary>
        /// Used in compiling a unique key for a query
        /// </summary>
        /// <returns>Unique key for a query</returns>
        public override string ToString()
        {
            return String.Format("SortingOptions<{0}>\nSort Property: {1}\nIsDescending: {2}",
                (typeof(T)).Name,
                SortProperty,
                IsDescending
                );
        }
    }
}