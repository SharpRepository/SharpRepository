using SharpRepository.Repository.Caching;

namespace SharpRepository.Repository.Configuration
{
    public interface ICachingProviderElement
    {
        string Name { get; set;  }

        ICachingProvider GetInstance();
    }
}
