// TODO: I want to use the ICanGet<> trait so that they aren't defined in 2 places but I can't because the GetAll is in IRepositoryQueryable and not in here, but it needs to be in ICanGet
namespace SharpRepository.Repository
{
    /// <summary>
    /// Repository that acesses <typeparamref name="T"/> entities and has a primary key of type <typeparamref name="TKey"/>
    /// </summary>
    /// <typeparam name="T">The entity type that the repository acts on.</typeparam>
    /// <typeparam name="TKey">The type of the primary key.</typeparam>
    public interface IRepository<T, TKey> : ICrudRepository<T, TKey>, IRepositoryQueryable<T>, IRepositoryAggregates<T> where T : class
    {
        
    }

    /// <summary>
    /// Defaults to int as the Primary Key
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> : IRepository<T, int> where T : class
    {
    }
}