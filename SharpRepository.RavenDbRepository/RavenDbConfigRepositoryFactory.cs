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

        public override IRepository<T, TKey> GetInstance<T, TKey>()
        {
            var documentStore =new DocumentStore();

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
    }
}
