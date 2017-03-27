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
#if NET451
                                                                     var propInfo = entityType.GetProperties().FirstOrDefault(x => x.HasAttribute<RepositoryPrimaryKeyAttribute>());
#elif NETSTANDARD1_6
                                                                     var propInfo = entityType.GetRuntimeProperties().FirstOrDefault(x => x.HasAttribute<RepositoryPrimaryKeyAttribute>());
#endif

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

#if NET451
        private static PropertyInfo GetPropertyCaseInsensitive(IReflect type, string propertyName)
#elif NETSTANDARD1_6
        private static PropertyInfo GetPropertyCaseInsensitive(Type type, string propertyName)
#endif
        {
#if NET451
            // make the property reflection lookup case insensitive
            const BindingFlags bindingFlags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;

            return type.GetProperty(propertyName, bindingFlags);
#elif NETSTANDARD1_6
            return type.GetRuntimeProperties().Where(pi => pi.Name.ToLowerInvariant() == propertyName.ToLowerInvariant()).FirstOrDefault();
#endif
        }

    }
}
