using SharpRepository.Benchmarks.Configuration.Models;
using SharpRepository.InMemoryRepository;
using SharpRepository.Repository;

namespace SharpRepository.Benchmarks.Configuration.Repositories
{
    public class UserFromConfigRepository : ConfigurationBasedRepository<User, int>, IRepository<User,int>
    {
    }

    public class UserRepository : InMemoryRepository<User, int>, IRepository<User, int>
    {}
}
