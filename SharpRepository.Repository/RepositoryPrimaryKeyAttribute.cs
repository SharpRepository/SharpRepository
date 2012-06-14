using System;

namespace SharpRepository.Repository
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class RepositoryPrimaryKeyAttribute : Attribute
    {
    }
}
