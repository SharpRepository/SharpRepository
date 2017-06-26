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
                    RepositoryType.MongoDb,
                    //RepositoryType.Dbo4,
                    //RepositoryType.RavenDb,
                    //RepositoryType.Xml,
                    //RepositoryType.Ef5,
                    //RepositoryType.CouchDb
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
                    RepositoryType.EfCore,
                    //RepositoryType.Cache
                };
            }
        }
    }
}
