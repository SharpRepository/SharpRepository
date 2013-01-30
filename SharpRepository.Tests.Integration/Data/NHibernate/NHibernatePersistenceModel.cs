using FluentNHibernate;

namespace SharpRepository.Tests.Integration.Data.NHibernate
{
    public class NHibernatePersistenceModel : PersistenceModel
    {
        public NHibernatePersistenceModel()
        {
            AddMappingsFromThisAssembly();
        }
    }
}
