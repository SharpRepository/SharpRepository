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
                               RepositoryType.Dbo4,
                               RepositoryType.RavenDb,
                               RepositoryType.Xml,
                               RepositoryType.MongoDb,
                               RepositoryType.Ef5,
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
                               RepositoryType.Ef5,
                               RepositoryType.Cache
                           };
            }
        }
    }
}
