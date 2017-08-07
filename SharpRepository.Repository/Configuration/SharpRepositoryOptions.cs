using Microsoft.Extensions.Options;

namespace SharpRepository.Repository.Configuration
{
    public class SharpRepositoryOptions : IOptions<SharpRepositoryConfiguration>
    {
        public SharpRepositoryOptions(SharpRepositoryConfiguration configuration)
        {
            Value = configuration;
        }

        public SharpRepositoryConfiguration Value { get; protected set; }
    }
}
