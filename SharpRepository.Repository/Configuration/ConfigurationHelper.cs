using System;

namespace SharpRepository.Repository.Configuration
{
    public static class ConfigurationHelper
    {
        public static void CheckForInterface(Type type, Type interfaceType)
        {
            if (type == null || interfaceType == null) return;

            if (Array.IndexOf<Type>(type.GetInterfaces(), interfaceType) == -1)
                throw new System.Configuration.ConfigurationErrorsException("The type " + type.AssemblyQualifiedName + " must implement " + interfaceType.AssemblyQualifiedName);
        }

        public static IRepository<T, TKey> GetDefaultInstance<T, TKey>(RepositoriesSectionGroup repositoriesSection) where T : class, new()
        {
            // see if there is a default repository section
            var defaultSection = repositoriesSection.Sections["default"];

            // get the first one that is of type IRepositoryElement
            //  this is totally hacky right now
            var defaultRepositoryElement = repositoriesSection.Sections[0] as IRepositoryElement;
            if (defaultRepositoryElement == null && repositoriesSection.Sections.Count > 1)
            {
                defaultRepositoryElement = repositoriesSection.Sections[1] as IRepositoryElement;
            }

            if (defaultSection != null)
            {
                var defaultName = ((DefaultSection)defaultSection).Name;

                if (!String.IsNullOrEmpty(defaultName) && defaultName != defaultRepositoryElement.Name)
                {
                    foreach (var section in repositoriesSection.Sections)
                    {
                        var tmp = section as IRepositoryElement;
                        if (tmp == null || tmp.Name != defaultName) continue;
                        defaultRepositoryElement = tmp;
                        break;
                    }
                }
            }

            return defaultRepositoryElement == null ? null : defaultRepositoryElement.GetInstance<T, TKey>();
        }
    }
}
