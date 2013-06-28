using System;
using SharpRepository.Repository;
using SharpRepository.Repository.Aspects;

namespace SharpRepository.RavenDbRepository
{
    public class AdvancedConfiguration<T, TKey> : RepositoryAspect<T, TKey> where T : class, new()
    {
        public bool? UseOptimisticConcurency { get; set; }
        public bool? AllowNonAuthoritativeInformation { get; set; }
        public TimeSpan? NonAuthoritativeInformationTimeout { get; set; }
        public int? MaxNumberOfRequestsPerSession { get; set; }

        public AdvancedConfiguration()
        {
        }

        public AdvancedConfiguration(bool? useOptimisticConcurrency, bool? allowNonAuthoritativeInformation = null, TimeSpan? nonAuthoritativeInfoTimeout = null, int? maxRequestsPerSession = null)
        {
            UseOptimisticConcurency = useOptimisticConcurrency;
            MaxNumberOfRequestsPerSession = maxRequestsPerSession;
            AllowNonAuthoritativeInformation = allowNonAuthoritativeInformation;
            NonAuthoritativeInformationTimeout = nonAuthoritativeInfoTimeout;
        }

        public override void OnInitialize(IRepository<T, TKey> repository)
        {
            var ravenDbRepository = (RavenDbRepository<T, TKey>) repository;

            if (UseOptimisticConcurency.HasValue)
                ravenDbRepository.Session.Advanced.UseOptimisticConcurrency = UseOptimisticConcurency.Value;

            if (AllowNonAuthoritativeInformation.HasValue)
                ravenDbRepository.Session.Advanced.AllowNonAuthoritativeInformation = AllowNonAuthoritativeInformation.Value;

            if (NonAuthoritativeInformationTimeout.HasValue)
                ravenDbRepository.Session.Advanced.NonAuthoritativeInformationTimeout = NonAuthoritativeInformationTimeout.Value;

            if (MaxNumberOfRequestsPerSession.HasValue)
                ravenDbRepository.Session.Advanced.MaxNumberOfRequestsPerSession = MaxNumberOfRequestsPerSession.Value;

        }
    }
}
