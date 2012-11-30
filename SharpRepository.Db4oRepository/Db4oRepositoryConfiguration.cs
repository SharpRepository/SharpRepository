using SharpRepository.Repository.Configuration;

namespace SharpRepository.Db4oRepository
{
    public class Db4oRepositoryConfiguration : RepositoryConfiguration
    {
        public Db4oRepositoryConfiguration(string name, string directory)
            : base(name)
        {
            Directory = directory;
            Factory = typeof(Db4oConfigRepositoryFactory);
        }

        public string Directory
        {
            set { Attributes["directory"] = value; }
        }
    }
}
