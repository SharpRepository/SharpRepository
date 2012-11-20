using System;
using System.ComponentModel;
using System.Configuration;
using System.Data.Entity;
using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;

namespace SharpRepository.EfRepository
{
    public class Config : ConfigurationSection, IRepositoryElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)base["name"]; }
            set { base["name"] = value; }
        }

        [ConfigurationProperty("connectionString", IsRequired = true)]
        public string ConnectionString
        {
            get { return (string)base["connectionString"]; }
            set { base["connectionString"] = value; }
        }

        /// <summary>
        /// Gets or sets the type of the specific DbContext type.
        /// </summary>
        [ConfigurationProperty("dbContextType"), TypeConverter(typeof(TypeNameConverter))]
        public Type DbContextType
        {
            get { return (Type)base["dbContextType"]; }
            set
            {
                base["dbContextType"] = value;
            }
        }

        public IRepository<T, TKey> GetInstance<T, TKey>() where T : class, new()
        {
            // TODO: look at FastActivator (from Enyim.Caching configuratio bits) and how it caches, see about implementing cache or expanding FastActivator to take parameters
            var dbContext = DbContextType == null ?
                new DbContext(ConnectionString) : 
                (DbContext)Activator.CreateInstance(DbContextType, ConnectionString);

            return new EfRepository<T, TKey>(dbContext);
        }
    }
}
