using System;
using SharpRepository.Repository.Aspects;

namespace SharpRepository.RavenDbRepository
{
    public class RavenDbAdvancedConfigurationAttribute : RepositoryActionBaseAttribute
    {
        public bool? UseOptimisticConcurency { get; set; }
        public bool? AllowNonAuthoritativeInformation { get; set; }
        public TimeSpan? NonAuthoritativeInformationTimeout { get; set; }
        public int? MaxNumberOfRequestsPerSession { get; set; }

        public RavenDbAdvancedConfigurationAttribute()
        {
        }

        public RavenDbAdvancedConfigurationAttribute(bool? useOptimisticConcurrency, bool? allowNonAuthoritativeInformation = null, TimeSpan? nonAuthoritativeInfoTimeout = null, int? maxRequestsPerSession = null)
        {
            UseOptimisticConcurency = useOptimisticConcurrency;
            MaxNumberOfRequestsPerSession = maxRequestsPerSession;
            AllowNonAuthoritativeInformation = allowNonAuthoritativeInformation;
            NonAuthoritativeInformationTimeout = nonAuthoritativeInfoTimeout;
        }

        public override void OnInitialized<T, TKey>(RepositoryActionContext<T, TKey> context)
        {
            var ravenDbRepository = context.Repository as RavenDbRepository<T, TKey>;

            if (ravenDbRepository == null) return;

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
