using System;
using System.Linq;
using SharpRepository.Repository.FetchStrategies;

namespace SharpRepository.Repository
{
    // right now int is hard coded but it's sloppy and need to fix this inheritance
    public class CompositeRepository<T> : LinqRepositoryBase<T, int> where T : class
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
    }
}
