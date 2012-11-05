using System;
using System.Linq;
using SharpRepository.ODataRepository.Linq;
using SharpRepository.Repository;
using SharpRepository.Repository.FetchStrategies;

namespace SharpRepository.ODataRepository
{
    public class ODataRepositoryBase<T> : LinqRepositoryBase<T, string> where T : class, new()
    {
        //protected ODataClient<T> Client;
        private readonly string _serverUrl;
        private readonly string _database;
        private string _typeName;

        //private readonly ODataQueryProvider _provider;
        //private readonly IQueryable<T> _baseQuery;

        internal ODataRepositoryBase(string url, string collectionName = null)
         {
             _typeName = typeof(T).Name;

             if (String.IsNullOrEmpty(collectionName))
             {
                 // generate based on the type name
                 collectionName = _typeName + "s"; // TODO: do we need to take into account Person to People, or things like that
             }
         }

        protected override IQueryable<T> BaseQuery(IFetchStrategy<T> fetchStrategy = null)
        {
            return ODataQueryFactory.Queryable<T>(_serverUrl, _database);
            //return Client.GetAllDocuments().AsQueryable();
        }

        // we override the implementation fro LinqBaseRepository becausee this is built in 

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
