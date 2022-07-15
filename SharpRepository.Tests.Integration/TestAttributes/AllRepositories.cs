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
                    RepositoryType.MongoDb
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
                    RepositoryType.EfCore
                };
            }
        }
    }
}
