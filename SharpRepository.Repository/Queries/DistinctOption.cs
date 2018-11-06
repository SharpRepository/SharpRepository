using System.Linq;

namespace SharpRepository.Repository.Queries
{
    /// <summary>
    /// Used to define the paging and/or sorting criteria on queries run against a repository.
    /// </summary>
    /// <typeparam name="T">The entity type of the repository.</typeparam>
    public class DistinctOption<T> : IQueryOptions<T>, IPostSelectorQueryOptions<T>
    {
        public IQueryable<T> Apply(IQueryable<T> query)
        {
            return query.Distinct();
        }

        public IQueryable<TResult> Apply<TResult>(IQueryable<TResult> query)
        {
            return query.Distinct();
        }
    }
}
