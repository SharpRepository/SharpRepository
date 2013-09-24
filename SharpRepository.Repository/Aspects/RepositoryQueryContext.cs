using SharpRepository.Repository.Queries;
using SharpRepository.Repository.Specifications;

namespace SharpRepository.Repository.Aspects
{
    public class RepositoryQueryContext<T, TKey> : RepositoryActionContext<T, TKey> where T : class
    {
        public RepositoryQueryContext(IRepository<T, TKey> repository, ISpecification<T> specification, IQueryOptions<T> queryOptions, int numberOfResults = 0)
            : base(repository)
        {
            Specification = specification;
            QueryOptions = queryOptions;
            NumberOfResults = numberOfResults;
        }

        // should I include the results so they can be manipulated?

        public ISpecification<T> Specification { get; set; }
        public IQueryOptions<T> QueryOptions { get; set; }
        public int NumberOfResults { get; set; }
    }
}