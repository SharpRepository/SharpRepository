using System.Collections.Generic;
using SharpRepository.Repository.Queries;
using SharpRepository.Repository.Specifications;

namespace SharpRepository.Repository.Caching
{
    public interface ICachingStrategy<T, TKey>
    {
        bool TryGetResult(TKey key, out T result);
        void SaveGetResult(TKey key, T result);

        bool TryGetAllResult(IQueryOptions<T> queryOptions, out IEnumerable<T> result);
        void SaveGetAllResult(IQueryOptions<T> queryOptions, IEnumerable<T> result);

        bool TryFindAllResult(ISpecification<T> criteria, IQueryOptions<T> queryOptions, out IEnumerable<T> result);
        void SaveFindAllResult(ISpecification<T> criteria, IQueryOptions<T> queryOptions, IEnumerable<T> result);

        bool TryFindResult(ISpecification<T> criteria, IQueryOptions<T> queryOptions, out T result);
        void SaveFindResult(ISpecification<T> criteria, IQueryOptions<T> queryOptions, T result);

        void Add(TKey key, T result);
        void Update(TKey key, T result);
        void Delete(TKey key, T result);
        void Save();
    }
}
