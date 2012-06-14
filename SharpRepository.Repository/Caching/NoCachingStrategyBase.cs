using System.Collections.Generic;
using SharpRepository.Repository.Queries;
using SharpRepository.Repository.Specifications;

namespace SharpRepository.Repository.Caching
{
    public abstract class NoCachingStrategyBase<T, TKey> : ICachingStrategy<T, TKey>
    {
        internal NoCachingStrategyBase()
        {
        }

        public bool TryGetResult(TKey key, out T result)
        {
            result = default(T);
            return false;
        }

        public void SaveGetResult(TKey key, T result)
        {

        }

        public bool TryGetAllResult(IQueryOptions<T> queryOptions, out IEnumerable<T> result)
        {
            result = default(IEnumerable<T>);
            return false;
        }

        public void SaveGetAllResult(IQueryOptions<T> queryOptions, IEnumerable<T> result)
        {

        }

        public bool TryFindAllResult(ISpecification<T> criteria, IQueryOptions<T> queryOptions, out IEnumerable<T> result)
        {
            result = default(IEnumerable<T>);
            return false;
        }

        public void SaveFindAllResult(ISpecification<T> criteria, IQueryOptions<T> queryOptions, IEnumerable<T> result)
        {

        }

        public bool TryFindResult(ISpecification<T> criteria, IQueryOptions<T> queryOptions, out T result)
        {
            result = default(T);
            return false;
        }

        public void SaveFindResult(ISpecification<T> criteria, IQueryOptions<T> queryOptions, T result)
        {

        }

        public void Add(TKey key, T result)
        {

        }

        public void Update(TKey key, T result)
        {

        }

        public void Delete(TKey key, T result)
        {

        }

        public void Save()
        {

        }
    }
}
