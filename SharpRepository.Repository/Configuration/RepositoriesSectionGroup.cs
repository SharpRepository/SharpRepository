#if NET451
using System.Configuration;
#endif

namespace SharpRepository.Repository.Configuration
{
#if NET451
    public class RepositoriesSectionGroup : ConfigurationSectionGroup
#elif NETSTANDARD1_4
    public class RepositoriesSectionGroup
#endif
    {
        
//        [ConfigurationProperty("defaultRepository", IsRequired = true)]
//        public string DefaultRepository
//        {
//            get { return (string)base["defaultRepository"]; }
//            set
//            {
//                base["defaultRepository"] = value;
//            }
//        }

       // [ConfigurationProperty("")]
    }
}
