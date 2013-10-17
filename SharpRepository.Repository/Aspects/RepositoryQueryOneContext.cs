using SharpRepository.Repository.Queries;
using SharpRepository.Repository.Specifications;

namespace SharpRepository.Repository.Aspects
{
    public class RepositoryQueryOneContext<T, TKey> : RepositoryQueryOneContext<T, TKey, T> where T : class
    {
        public RepositoryQueryOneContext(IRepository<T, TKey> repository, ISpecification<T> specification, IQueryOptions<T> queryOptions)
            : base(repository, specification, queryOptions)
        {
        }
    }

    public class RepositoryQueryOneContext<T, TKey, TResult> : RepositoryQueryContext<T, TKey, TResult> where T : class
    {
        public RepositoryQueryOneContext(IRepository<T, TKey> repository, ISpecification<T> specification,
                                         IQueryOptions<T> queryOptions)
            : base(repository, specification, queryOptions)
        {
        }

        public TResult Result { get; set; }
        public override int NumberOfResults
        {
            get { return Result.Equals(default(TResult)) ? 0 : 1; }
        }
    }
}
