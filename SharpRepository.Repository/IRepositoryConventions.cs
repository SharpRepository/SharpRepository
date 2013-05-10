using System;

namespace SharpRepository.Repository
{
    public interface IRepositoryConventions
    {
        Func<Type, string> GetPrimaryKeyName { get; set; } 
    }
}
