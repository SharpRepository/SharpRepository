using System;
using System.Linq;
using System.Linq.Expressions;

namespace SharpRepository.Repository.Queries
{
    /// <summary>
    /// Used to define the sorting on queries with distinct run against a repository.
    /// </summary>
    /// <typeparam name="T">The entity type of the repository.</typeparam>
    /// <typeparam name="TSortKey">The type of the property that is being sorted.</typeparam>
    public class DistinctSortingOptions<T, TSortKey> : SortingOptions<T, TSortKey>
    {
        public DistinctSortingOptions(Expression<Func<T, TSortKey>> sortExpression, bool isDescending = false) : base(sortExpression, isDescending)
        {
        }

        /// <summary>
        /// Applies sorting to the specified query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>Sorted results.</returns>
        public override IQueryable<T> Apply(IQueryable<T> query)
        {
            return base.Apply(query).Distinct();
        }

        /// <summary>
        /// Used in compiling a unique key for a query
        /// </summary>
        /// <returns>Unique key for a query</returns>
        public override string ToString()
        {
            return "Distinct" + base.ToString();
        }
    }

    /// <summary>
    /// Used to define the sorting on queries with distinct run against a repository.
    /// </summary>
    /// <typeparam name="T">The entity type of the repository.</typeparam>
    public class DistinctSortingOptions<T> : SortingOptions<T>
    {
        public DistinctSortingOptions(string sortProperty, bool isDescending = false) : base(sortProperty, isDescending) { }

        /// <summary>
        /// Applies sorting to the specified query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>Sorted results.</returns>
        public override IQueryable<T> Apply(IQueryable<T> query)
        {
            return base.Apply(query).Distinct();
        }

        /// <summary>
        /// Used in compiling a unique key for a query
        /// </summary>
        /// <returns>Unique key for a query</returns>
        public override string ToString()
        {
            return "Distinct" + base.ToString();
        }
    }
}