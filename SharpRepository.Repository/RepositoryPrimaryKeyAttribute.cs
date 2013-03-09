using System;

namespace SharpRepository.Repository
{
    /// <summary>
    /// This attribute is used to identify an object property as the primary key when it will not be found based on conventions.  It is not needed if the property name is either Id or [ClassName]Id (e.g. the Contact object would match a ContactId property automatically).
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class RepositoryPrimaryKeyAttribute : Attribute
    {
        public int Order { get; set; }
    }
}
