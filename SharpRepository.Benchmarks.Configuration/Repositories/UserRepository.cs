using Microsoft.Extensions.Options;
using SharpRepository.Benchmarks.Configuration.Models;
using SharpRepository.InMemoryRepository;
using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;

namespace SharpRepository.Benchmarks.Configuration.Repositories
{
    public interface IUserRepository : IRepository<User, int>
    {
        User GetAdminUser();
    }

    public class UserFromConfigRepository : ConfigurationBasedRepository<User, int>, IUserRepository
    {
        public UserFromConfigRepository(ISharpRepositoryConfiguration configuration, string repositoryName = null) : base(configuration, repositoryName)
        {
        }

        public User GetAdminUser()
        {
            return Find(x => x.Email == "admin@admin.com");
        }
    }

    public class UserRepository : InMemoryRepository<User, int>, IUserRepository
    {
        public User GetAdminUser()
        {
            return Find(x => x.Email == "admin@admin.com");
        }
    }
}
