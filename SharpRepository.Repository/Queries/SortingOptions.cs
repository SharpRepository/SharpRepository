using System;
using System.Collections.Generic;
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
    public class SortingOptions<T, TSortKey> : SortingOptions<T>
    {
        public SortingOptions(Expression<Func<T, TSortKey>> sortExpression, bool isDescending = false) : base(sortExpression.ToString(), isDescending)
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
    /// Used to define the sorting on queries run against a repository.
    /// </summary>
    /// <typeparam name="T">The entity type of the repository.</typeparam>
    public class SortingOptions<T> : IQueryOptions<T>
    {
        protected Func<IQueryable<T>, IOrderedQueryable<T>> _primarySortAction;
        protected IList<Func<IOrderedQueryable<T>, IOrderedQueryable<T>>> _sortActions = new List<Func<IOrderedQueryable<T>, IOrderedQueryable<T>>>();

        // we need these fields because calling ToString on the Func's didn't include the details of the method being called so it was the same key for descending and ascending
        protected string _primarySortToString;
        protected IList<string> _sortActionsToString = new List<string>();

        public SortingOptions(string sortProperty, bool isDescending = false)
        {
            if (isDescending)
            {
                _primarySortAction = q => q.OrderByDescendingProperty(sortProperty);
            }
            else
            {
                _primarySortAction = q => q.OrderByProperty(sortProperty);
            }

            _primarySortToString = string.Format("{0}-{1}", sortProperty, isDescending);
        }

        public void ThenSortBy(string sortProperty, bool isDescending = false)
        {
            Func<IOrderedQueryable<T>, IOrderedQueryable<T>> sortAction = null;

            if (isDescending)
            {
                sortAction = q => q.ThenByDescendingProperty(sortProperty);
            }
            else
            {
                sortAction = q => q.ThenByProperty(sortProperty);
            }

            _sortActions.Add(sortAction);
            _sortActionsToString.Add(String.Format("{0}-{1}", sortProperty, isDescending));
        }

        public void ThenSortBy<TNewSortKey>(Expression<Func<T, TNewSortKey>> sortExpression, bool isDescending = false)
        {
            Func<IOrderedQueryable<T>, IOrderedQueryable<T>> sortAction = null;

            if (isDescending)
            {
                sortAction = q => q.ThenByDescending(sortExpression);
            }
            else
            {
                sortAction = q => q.ThenBy(sortExpression);
            }

            _sortActions.Add(sortAction);
            _sortActionsToString.Add(String.Format("{0}-{1}", sortExpression, isDescending));
        }


        /// <summary>
        /// Applies sorting to the specified query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>Sorted results.</returns>
        public virtual IQueryable<T> Apply(IQueryable<T> query)
        {
            // TODO: do we need to deal with the case where the user passes in "Name desc", should we strip the desc out, or let it override the isDescending param, or not deal with it and blame it on the user?

            IOrderedQueryable<T> sortedQuery = _primarySortAction != null 
                ? _primarySortAction(query) 
                : null;

            return _sortActions.Aggregate(sortedQuery, (current, sortAction) => sortAction(current));
        }

        /// <summary>
        /// Used in compiling a unique key for a query
        /// </summary>
        /// <returns>Unique key for a query</returns>
        public override string ToString()
        {
            var val = string.Format("SortingOptions<{0}>\nSort: {1}\nExtra: {2}",
                typeof(T).Name,
                _primarySortToString ?? "null",
                string.Join("-", _sortActionsToString)
                );
            return val;
        }
    }
}