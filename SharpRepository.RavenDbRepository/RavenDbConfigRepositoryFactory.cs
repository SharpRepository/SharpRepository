using System;
using Raven.Client.Document;
using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;

namespace SharpRepository.RavenDbRepository
{
    public class RavenDbConfigRepositoryFactory : ConfigRepositoryFactory
    {
        public RavenDbConfigRepositoryFactory(IRepositoryConfiguration config)
            : base(config)
        {
        }

        public override IRepository<T> GetInstance<T>()
        {
            throw new NotImplementedException("RavenDbRepository does not support using IRepository<T> directly to reference a IRepository<T, string>");
        }

        public override IRepository<T, TKey> GetInstance<T, TKey>()
        {
            var documentStore = new DocumentStore();

            if (!String.IsNullOrEmpty(RepositoryConfiguration["connectionStringName"]))
            {
                documentStore.ConnectionStringName = RepositoryConfiguration["connectionStringName"];
            }
            else if (!String.IsNullOrEmpty(RepositoryConfiguration["url"]))
            {
                documentStore.Url = RepositoryConfiguration["url"];
            }

            return new RavenDbRepository<T, TKey>(documentStore);
        }

        public override ICompoundKeyRepository<T, TKey, TKey2> GetInstance<T, TKey, TKey2>()
        {
            throw new NotImplementedException();
//            var documentStore =new DocumentStore();
//
//            if (!String.IsNullOrEmpty(RepositoryConfiguration["connectionStringName"]))
//            {
//                documentStore.ConnectionStringName = RepositoryConfiguration["connectionStringName"];
//            }
//            else if (!String.IsNullOrEmpty(RepositoryConfiguration["url"]))
//            {
//                documentStore.Url = RepositoryConfiguration["url"];
//            }
//
//            return new RavenDbRepository<T, TKey, TKey2>(documentStore);
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
