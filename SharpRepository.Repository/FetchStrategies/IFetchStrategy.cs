using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SharpRepository.Repository.FetchStrategies
{
    /// <summary>
    /// Per Will Beattie's article, Specification Pattern, Entity Framework & LINQ
    /// <see cref="http://blog.willbeattie.net/2011/02/specification-pattern-entity-framework.html"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IFetchStrategy<T>
    {
        IEnumerable<string> IncludePaths { get; }

        bool NoTracking { get; }

        IFetchStrategy<T> Include(Expression<Func<T, object>> path);

        IFetchStrategy<T> Include(string path);

        IFetchStrategy<T> AsNoTracking();
    }
}