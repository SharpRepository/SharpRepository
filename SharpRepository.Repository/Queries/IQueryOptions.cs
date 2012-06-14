using System.Linq;

namespace SharpRepository.Repository.Queries
{
    /// <summary>
    /// Used to define the paging and/or sorting criteria on queries run against a repository.
    /// </summary>
    /// <typeparam name="T">The entity type of the repository.</typeparam>
    public interface IQueryOptions<T>
    {
        IQueryable<T> Apply(IQueryable<T> query);
    }
}
