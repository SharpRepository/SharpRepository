using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using SharpRepository.Repository.Helpers;

namespace SharpRepository.Repository.FetchStrategies
{
    /// <summary>
    /// Per Will Beattie's article, Specification Pattern, Entity Framework & LINQ
    /// <see cref="http://blog.willbeattie.net/2011/02/specification-pattern-entity-framework.html"/>
    /// <see cref="http://www.codetuning.net/blog/post/Entity-Framework-compile-safe-Includes.aspx"/>
    /// </summary>
    public static class FetchStrategyExtensions
    {
        /// <summary>
        ///  Evaluates the Linq expression and returns the name of the property or the multiple level deep string representation of the Expression (i.e. prop.Collection.Property).
        /// </summary>
        /// <typeparam name="T">Type being evaluated</typeparam>
        /// <param name="selector">Name of the property per the Linq expression</param>
        /// <returns></returns>
        public static string ToIncludeString<T>(this Expression<Func<T, object>> selector)
        {
            // Retrieve member path:
            var members = new List<PropertyInfo>();
            ExpressionHelper.CollectRelationalMembers(selector, members);

            // Build string path:
            var sb = new StringBuilder();
            var separator = "";
            foreach (var member in members)
            {
                sb.Append(separator);
                sb.Append(member.Name);
                separator = ".";
            }

            // return concatenated string
            return sb.ToString();
        }
    }
}