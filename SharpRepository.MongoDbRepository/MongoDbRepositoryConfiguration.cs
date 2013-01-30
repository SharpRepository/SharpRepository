using SharpRepository.Repository.Configuration;

namespace SharpRepository.MongoDbRepository
{
    public class MongoDbRepositoryConfiguration : RepositoryConfiguration
    {
        public MongoDbRepositoryConfiguration(string name, string connectionString) 
            : base(name)
        {
            ConnectionString = connectionString;
        }

        public string ConnectionString
        {
            set { Attributes["connectionString"] = value; }
        }
    }
}
