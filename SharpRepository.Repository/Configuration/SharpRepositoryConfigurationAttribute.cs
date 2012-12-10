using System;

namespace SharpRepository.Repository.Configuration
{
     [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class SharpRepositoryConfigurationAttribute : Attribute
     {
         public SharpRepositoryConfigurationAttribute(string repositoryName)
         {
             RepositoryName = repositoryName;
         }

         public string RepositoryName { get; private set; }
//         public string CachingStrategyName { get; set; }
//         public string CachingProviderName { get; set; }

     }
}
