using Microsoft.Extensions.Logging;
using SharpRepository.CoreMvc.Models;
using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;

namespace SharpRepository.Samples.CoreMvc
{
    public class UserRepository : ConfigurationBasedRepository<Contact, string>
    {
        public UserRepository(ISharpRepositoryConfiguration configuration, string repositoryName = null) : base(configuration, repositoryName) { }
    }

    public class UserService : IUserService
    {
        protected IRepository<Contact, string> _userRepository;

        public UserService(IRepository<Contact, string> userRepository)
        {
            _userRepository = userRepository;
        }

        public IRepository<Contact, string> GetRepository()
        {
            return _userRepository;
        }
    }

    public interface IUserService
    {
        IRepository<Contact, string> GetRepository();
    }

    public class UserServiceCustom : IUserServiceCustom
    {
        public UserRepository _userRepository;

        public UserServiceCustom(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IRepository<Contact, string> GetRepository()
        {
            return _userRepository;
        }
    }

    public interface IUserServiceCustom
    {
        IRepository<Contact, string> GetRepository();
    }
}
