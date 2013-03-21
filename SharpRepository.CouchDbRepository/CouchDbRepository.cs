
namespace SharpRepository.CouchDbRepository
{
    public class CouchDbRepository<T> : CouchDbRepositoryBase<T> where T : class, new()
    {
        public CouchDbRepository()
        {
        }

        public CouchDbRepository(string host) : base(host)
        {
        }

        public CouchDbRepository(string host, int port, string database = null, string username = null, string password = null)
            : base(host, port, database, username, password)
        {
        }
    }
}
