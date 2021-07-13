namespace SharpRepository.Repository.Aspects
{
    public class RepositoryActionContext<T, TKey> where T : class
    {
        public RepositoryActionContext(IRepository<T, TKey> repository)
        {
            Repository = repository;
        }

        public IRepository<T, TKey> Repository { get; set; }
    }

    public class RepositoryActionContext<T, TKey, TKey2> where T : class
    {
        public RepositoryActionContext(ICompoundKeyRepository<T, TKey, TKey2> repository)
        {
            Repository = repository;
        }

        public ICompoundKeyRepository<T, TKey, TKey2> Repository { get; set; }
    }

    public class RepositoryActionContext<T, TKey, TKey2, TKey3> where T : class
    {
        public RepositoryActionContext(ICompoundKeyRepository<T, TKey, TKey2, TKey3> repository)
        {
            Repository = repository;
        }

        public ICompoundKeyRepository<T, TKey, TKey2, TKey3> Repository { get; set; }
    }

    public class RepositoryActionContext<T> where T : class
    {
        public RepositoryActionContext(ICompoundKeyRepository<T> repository)
        {
            Repository = repository;
        }

        public ICompoundKeyRepository<T> Repository { get; set; }
    }
}