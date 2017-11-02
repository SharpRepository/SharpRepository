
using System;

namespace SharpRepository.CouchDbRepository
{
    public class CouchDbRepository<T,TKey> : CouchDbRepositoryBase<T, TKey> where T : class, new()
    {
        public CouchDbRepository()
        {
            if (typeof(TKey) != typeof(string))
            {
                throw new Exception("The key type TKey must be string in CouchDbRepository");
            }
        }

        public CouchDbRepository(string host) : base(host)
        {
            if (typeof(TKey) != typeof(string))
            {
                throw new Exception("The key type TKey must be string in CouchDbRepository");
            }
        }

        public CouchDbRepository(string host, int port, string database = null, string username = null, string password = null)
            : base(host, port, database, username, password)
        {
        }
    }
}
