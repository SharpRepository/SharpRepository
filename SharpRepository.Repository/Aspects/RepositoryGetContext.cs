namespace SharpRepository.Repository.Aspects
{
    public class RepositoryGetContext<T, TKey> : RepositoryActionContext<T, TKey> where T : class
    {
        public RepositoryGetContext(IRepository<T, TKey> repository, TKey id, int numberOfResults = 0)
            : base(repository)
        {
            Id = id;
            NumberOfResults = numberOfResults;
        }

        public TKey Id { get; set; }
        public int NumberOfResults { get; set; }
    }
}