using System;

namespace SharpRepository.Repository
{
    public class RepositoryConventions : IRepositoryConventions
    {
        public Func<Type, string> GetPrimaryKeyName { get; set; } 

        public RepositoryConventions()
        {
            GetPrimaryKeyName = DefaultRepositoryConventions.GetPrimaryKeyName;
        }
    }
}
