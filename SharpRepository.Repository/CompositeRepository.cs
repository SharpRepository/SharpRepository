using System;
using System.Linq;
using SharpRepository.Repository.FetchStrategies;

namespace SharpRepository.Repository
{
    public class CompositeRepository<T, TKey> : LinqRepositoryBase<T, TKey>, IRepositoryQueryable<T, TKey> where T : class, new()
    {
        private readonly IQueryable<T> _baseQuery;

        public CompositeRepository(IQueryable<T> baseQuery)
        {
            _baseQuery = baseQuery;
        }

        protected override IQueryable<T> BaseQuery(IFetchStrategy<T> fetchStrategy = null)
        {
            return _baseQuery;
        }

        protected override void AddItem(T entity)
        {
            throw new NotImplementedException();
        }

        protected override void DeleteItem(T entity)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateItem(T entity)
        {
            throw new NotImplementedException();
        }

        protected override void SaveChanges()
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            
        }

        //public void Dispose()
        //{
        //    _baseRepository.Dispose();
        //}

        //public IQueryable<T> AsQueryable()
        //{
        //    return _baseRepository.AsQueryable();
        //}

        //public IRepositoryQueryable<TResult, TResultKey> Join<TOuterKey, TInner, TResult, TResultKey>(IRepositoryQueryable<TInner, TOuterKey> innerRepository, Expression<Func<T, TOuterKey>> outerKeySelector, Expression<Func<TInner, TOuterKey>> innerKeySelector, Expression<Func<T, TInner, TResult, TResultKey>> resultSelector) where TInner : class where TResult : class
        //{
        //    return _baseRepository.Join(innerRepository, outerKeySelector, innerKeySelector, resultSelector);
        //}

        //public T Get(TKey key)
        //{
        //    return _baseRepository.Get(key);
        //}

        //public TResult Get<TResult>(TKey key, Expression<Func<T, TResult>> selector)
        //{
        //    return _baseRepository.Get(key, selector);
        //}

        //public IEnumerable<T> GetAll()
        //{
        //    return _baseRepository.GetAll();
        //}

        //public IEnumerable<T> GetAll(IQueryOptions<T> queryOptions)
        //{
        //    return _baseRepository.GetAll(queryOptions);
        //}

        //public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null)
        //{
        //    return _baseRepository.GetAll(selector, queryOptions);
        //}

        //public T Find(Expression<Func<T, bool>> predicate, IQueryOptions<T> queryOptions = null)
        //{
        //    return _baseRepository.Find(predicate, queryOptions);
        //}

        //public TResult Find<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null)
        //{
        //    return _baseRepository.Find(predicate, selector, queryOptions);
        //}

        //public T Find(ISpecification<T> criteria, IQueryOptions<T> queryOptions = null)
        //{
        //    return _baseRepository.Find(criteria, queryOptions);
        //}

        //public TResult Find<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null)
        //{
        //    return _baseRepository.Find(criteria, selector, queryOptions);
        //}

        //public IEnumerable<T> FindAll(Expression<Func<T, bool>> predicate, IQueryOptions<T> queryOptions = null)
        //{
        //    return _baseRepository.FindAll(predicate, queryOptions);
        //}

        //public IEnumerable<TResult> FindAll<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null)
        //{
        //    return _baseRepository.FindAll(predicate, selector, queryOptions);
        //}

        //public IEnumerable<T> FindAll(ISpecification<T> criteria, IQueryOptions<T> queryOptions = null)
        //{
        //    return _baseRepository.FindAll(criteria, queryOptions);
        //}

        //public IEnumerable<TResult> FindAll<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null)
        //{
        //    return _baseRepository.FindAll(criteria, selector, queryOptions);
        //}
    }
}
