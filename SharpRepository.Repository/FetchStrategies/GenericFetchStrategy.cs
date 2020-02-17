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
    public class GenericFetchStrategy<T> : AbstractFetchStrategy<T>
    {
        private readonly IList<string> _properties;
        private bool _noTracking;

        public GenericFetchStrategy()
        {
            _properties = new List<string>();
            _noTracking = false;
        }

        public override IEnumerable<string> IncludePaths
        {
            get { return _properties; }
        }

        public override bool NoTracking
        {
            get { return _noTracking; }
        }



        public override IFetchStrategy<T> AsNoTracking()
        {
            _noTracking = true;

            return this;
        }

        public override IFetchStrategy<T> Include(Expression<Func<T, object>> path)
        {
            return Include(path.ToIncludeString());
        }

        public override IFetchStrategy<T> Include(string path)
        {
            _properties.Add(path);
            return this;
        }
    }
}