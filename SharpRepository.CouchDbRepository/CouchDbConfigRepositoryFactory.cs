using System;
using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;

namespace SharpRepository.CouchDbRepository
{
    public class CouchDbConfigRepositoryFactory : ConfigRepositoryFactory
    {
        public CouchDbConfigRepositoryFactory(IRepositoryConfiguration config)
            : base(config)
        {
        }

        public override IRepository<T> GetInstance<T>()
        {
            throw new NotImplementedException();
        }

        public override IRepository<T, TKey> GetInstance<T, TKey>()
        {
            return new CouchDbRepository<T,TKey>(RepositoryConfiguration["host"], int.Parse(RepositoryConfiguration["port"]), RepositoryConfiguration["database"], RepositoryConfiguration["username"], RepositoryConfiguration["password"]);
        }

        public override ICompoundKeyRepository<T, TKey, TKey2> GetInstance<T, TKey, TKey2>()
        {
            throw new NotImplementedException();
        }

        public override ICompoundKeyRepository<T, TKey, TKey2, TKey3> GetInstance<T, TKey, TKey2, TKey3>()
        {
            throw new NotImplementedException();
        }

        public override ICompoundKeyRepository<T> GetCompoundKeyInstance<T>()
        {
            throw new NotImplementedException();
        }
    }
}
