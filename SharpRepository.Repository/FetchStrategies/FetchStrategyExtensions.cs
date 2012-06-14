using System;
using System.Linq.Expressions;

namespace SharpRepository.Repository.FetchStrategies
{
    /// <summary>
    /// Per Will Beattie's article, Specification Pattern, Entity Framework & LINQ
    /// <see cref="http://blog.willbeattie.net/2011/02/specification-pattern-entity-framework.html"/>
    /// </summary>
    public static class FetchStrategyExtensions
    {
        /// <summary>
        ///  Evaluates the Linq expression and returns the name of the property.
        /// </summary>
        /// <typeparam name="T">Type being evaluated</typeparam>
        /// <param name="selector">Name of the property per the Linq expression</param>
        /// <returns></returns>
        public static string ToPropertyName<T>(this Expression<Func<T, object>> selector)
        {
            var me = selector.Body as MemberExpression;
            if (me == null)
            {
                throw new ArgumentException("MemberExpression expected.");
            }

            var trimPrefix = me.ToString();
            return trimPrefix.Substring(trimPrefix.IndexOf('.') + 1);
        }
    }
}