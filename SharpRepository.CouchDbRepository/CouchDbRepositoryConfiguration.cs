using System;
using SharpRepository.Repository.Configuration;

namespace SharpRepository.CouchDbRepository
{
    public class CouchDbRepositoryConfiguration : RepositoryConfiguration
    {
        public CouchDbRepositoryConfiguration(string name) : this(name, null, null, null, null)
        {
        }

        public CouchDbRepositoryConfiguration(string name, string host)
            : this(name, host, null, null, null)
        {
        }

        public CouchDbRepositoryConfiguration(string name, string host, string port, string username, string password, string cachingStrategy = null, string cachingProvider = null)
            : base(name)
        {
            Host = host;
            Port = port;
            Username = username;
            Password = password;
            CachingStrategy = cachingStrategy;
            CachingProvider = cachingProvider;
            Factory = typeof(CouchDbConfigRepositoryFactory);
        }

        public string Host
        {
            set { Attributes["host"] = String.IsNullOrEmpty(value) ? null : value; }
        }

        public string Port
        {
            set { Attributes["port"] = String.IsNullOrEmpty(value) ? null : value; }
        }

        public string Username
        {
            set { Attributes["username"] = String.IsNullOrEmpty(value) ? null : value; }
        }

        public string Password
        {
            set { Attributes["password"] = String.IsNullOrEmpty(value) ? null : value; }
        }
    }
}
