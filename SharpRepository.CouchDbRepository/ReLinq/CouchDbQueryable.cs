using System.Linq;
using System.Linq.Expressions;
using Remotion.Linq;
using Remotion.Linq.Parsing.Structure;

namespace SharpRepository.CouchDbRepository.ReLinq
{
    /// <summary>
    /// Provides the main entry point to a LINQ query.
    /// </summary>
    public class CouchDbQueryable<T> : QueryableBase<T>
    {
        private static IQueryExecutor CreateExecutor (string url, string databaseName)
        {
          return new CouchDbQueryExecutor (url, databaseName);
        }
    
        // This constructor is called by our users, create a new IQueryExecutor.
        public CouchDbQueryable (string url, string databaseName)
            : base (QueryParser.CreateDefault(), CreateExecutor (url, databaseName))
        {
        }

        // This constructor is called indirectly by LINQ's query methods, just pass to base.
        public CouchDbQueryable(IQueryProvider provider, Expression expression)
            : base(provider, expression)
        {
        }
    }
}
