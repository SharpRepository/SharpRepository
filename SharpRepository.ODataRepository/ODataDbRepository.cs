
namespace SharpRepository.ODataRepository
{
    public class ODataRepository<T> : ODataRepositoryBase<T> where T : class, new()
    {
        public ODataRepository(string host) : base(host)
        {
        }

        public ODataRepository(string host,string collectionName)
            : base(host, collectionName)
        {
        }
    }
}
