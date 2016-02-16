using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SharpRepository.Repository.FetchStrategies
{
    public abstract class AbstractFetchStrategy<T> : IFetchStrategy<T>
    {
        public abstract IEnumerable<string> IncludePaths { get; }
        public abstract IFetchStrategy<T> Include(Expression<Func<T, object>> path);
        public abstract IFetchStrategy<T> Include(string path);

        public override string ToString()
        {
            return String.Format("Type: {0} Includes: {1}",
                    GetType().Name,
                    String.Join(",", IncludePaths)
                );
        }
    }
}
