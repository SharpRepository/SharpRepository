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
    public class GenericFetchStrategy<T> : IFetchStrategy<T>
    {
        private readonly IList<string> _properties;

        public GenericFetchStrategy()
        {
            _properties = new List<string>();
        }

        public IEnumerable<string> IncludePaths
        {
            get { return _properties; }
        }

        public IFetchStrategy<T> Include(Expression<Func<T, object>> path)
        {
            _properties.Add(path.ToPropertyName());
            return this;
        }

        public IFetchStrategy<T> Include(string path)
        {
            _properties.Add(path);
            return this;
        }
    }
}