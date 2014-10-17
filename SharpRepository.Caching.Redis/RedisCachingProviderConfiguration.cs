using System;
using SharpRepository.Repository.Configuration;

namespace SharpRepository.Caching.Redis
{
    public class RedisCachingProviderConfiguration : CachingProviderConfiguration
    {
        public RedisCachingProviderConfiguration(string name)
        {
            Name = name;
            Factory = typeof(RedisConfigCachingProviderFactory);
        }

        public RedisCachingProviderConfiguration(string name, string host, int port) : this(name, host, port, null)
        {
        }

        public RedisCachingProviderConfiguration(string name, string host, int port, string password, bool ssl = true)
        {
            Name = name;
            Host = host;
            Port = port;
            Password = password;
            Ssl = ssl;
            Factory = typeof (RedisConfigCachingProviderFactory);
        }

        public string Host
        {
            set { Attributes["host"] = value; }
        }

        public int Port
        {
            set { Attributes["port"] = value.ToString(); }
        }

        public string Password
        {
            set { Attributes["password"] = value; }
        }

        public bool Ssl
        {
            set { Attributes["ssl"] = value.ToString(); }
        }
    }
}
