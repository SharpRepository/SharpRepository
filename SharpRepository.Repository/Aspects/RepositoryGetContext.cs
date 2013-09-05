namespace SharpRepository.Repository.Aspects
{
    public class RepositoryGetContext<T, TKey> : RepositoryActionContext<T, TKey> where T : class
    {
        public RepositoryGetContext(IRepository<T, TKey> repository, T id, int numberOfResults)
            : base(repository)
        {
            Id = id;
            NumberOfResults = numberOfResults;
        }

        public T Id { get; set; }
        public int NumberOfResults { get; set; }
    }
}