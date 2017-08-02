using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SharpRepository.Repository.Helpers;

namespace SharpRepository.Repository
{
    public static class DefaultRepositoryConventions
    {
        public static string PrimaryKeySuffix = "Id";

        public static string CachePrefix = "#Repo";

        public static Func<Type, string> GetPrimaryKeyName = entityType =>
                                                                 {
                                                                     var propInfo = entityType.GetRuntimeProperties().FirstOrDefault(x => x.HasAttribute<RepositoryPrimaryKeyAttribute>());
                                                                     if (propInfo != null) return propInfo.Name;

                                                                     foreach (var propertyName in GetPrimaryKeyNameChecks(entityType))
                                                                     {
                                                                         propInfo = GetPropertyCaseInsensitive(entityType, propertyName);

                                                                         if (propInfo != null) return propInfo.Name;
                                                                     }

                                                                     return null;
                                                                 };

        private static readonly Func<Type, IEnumerable<string>> GetPrimaryKeyNameChecks = type =>
                                                                 {
                                                                     var suffix = PrimaryKeySuffix;
                                                                     return new[] {suffix, type.Name + suffix};
                                                                 };
        
        private static PropertyInfo GetPropertyCaseInsensitive(Type type, string propertyName)
        {
            return type.GetRuntimeProperties().Where(pi => pi.Name.ToLowerInvariant() == propertyName.ToLowerInvariant()).FirstOrDefault();
        }
    }
}
