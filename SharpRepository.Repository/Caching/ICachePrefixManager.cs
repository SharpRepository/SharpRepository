namespace SharpRepository.Repository.Caching
{
    public interface ICachePrefixManager
    {
        int Counter { get; }
        void IncrementCounter();
    }
}
