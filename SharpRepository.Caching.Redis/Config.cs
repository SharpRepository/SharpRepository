using System;
using System.Configuration;
using SharpRepository.Repository.Caching;
using SharpRepository.Repository.Configuration;

namespace SharpRepository.Caching.Redis
{
    public class Config : ConfigurationSection, ICachingProviderElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)base["name"]; }
            set { base["name"] = value; }
        }

        [ConfigurationProperty("host")]
        public string Host
        {
            get { return (string) base["host"]; }
            set { base["host"] = value; }
        }

        [ConfigurationProperty("port")]
        public int Port
        {
            get { return (int)base["port"]; }
            set { base["port"] = value; }
        }

        [ConfigurationProperty("password")]
        public string Password
        {
            get { return (string)base["password"]; }
            set { base["password"] = value; }
        }

        public ICachingProvider GetInstance()
        {
            // this seems like a dumb way to do this :)
            if (!String.IsNullOrEmpty(Password))
            {
                return new RedisCachingProvider(Host, Port, Password);
            }

            if (Port != default(int))
            {
                return new RedisCachingProvider(Host, Port);
            }

            if (!String.IsNullOrEmpty(Host))
            {
                return new RedisCachingProvider(Host);
            }

            return new RedisCachingProvider();
        }
    }
}
