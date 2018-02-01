namespace SharpRepository.Tests.Integration.TestAttributes
{
    public static class RepositoryTypes
    {
        public static RepositoryType[] All
        {
            get
            {
                return new[]
                {
                    RepositoryType.InMemory,
                    RepositoryType.EfCore,
                    RepositoryType.Ef,
                    RepositoryType.Dbo4,
                    RepositoryType.RavenDb,
                    RepositoryType.Xml,
                    RepositoryType.MongoDb,
                    RepositoryType.CouchDb
                };
            }
        }

        public static RepositoryType[] CompoundKey
        {
            get
            {
                return new[]
                {
                    RepositoryType.InMemory,
                    RepositoryType.Ef,
                    RepositoryType.EfCore,
                    RepositoryType.Cache
                };
            }
        }
    }
}
