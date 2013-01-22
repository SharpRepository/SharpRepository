using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using SharpRepository.Repository;
using SharpRepository.Repository.Caching;
using SharpRepository.Repository.FetchStrategies;

namespace SharpRepository.Ef5Repository
{
    public class Ef5CompoundKeyRepositoryBase<T, TKey, TKey2> : LinqCompoundKeyRepositoryBase<T, TKey, TKey2> where T : class, new()
    {
        protected IDbSet<T> DbSet { get; private set; }
        protected DbContext Context { get; private set; }

        internal Ef5CompoundKeyRepositoryBase(DbContext dbContext, ICompoundKeyCachingStrategy<T, TKey, TKey2> cachingStrategy = null)
            : base(cachingStrategy)
        {
            Initialize(dbContext);
        }

        private void Initialize(DbContext dbContext)
        {
            Context = dbContext;
            DbSet = Context.Set<T>();
        }

        protected override void AddItem(T entity)
        {
            if ((typeof(TKey) == typeof(Guid) || typeof(TKey) == typeof(string)) && (typeof(TKey2) == typeof(Guid) || typeof(TKey2) == typeof(string)))
            {
                TKey key;
                TKey2 key2;
                if (GetPrimaryKey(entity, out key, out key2) && Equals(key, default(TKey)) && Equals(key2, default(TKey2)))
                {
                    key = GenerateFirstPrimaryKey();
                    key2 = GenerateSecondPrimaryKey();
                    SetPrimaryKey(entity, key, key2);
                }
            }
            DbSet.Add(entity);
        }

        protected override void DeleteItem(T entity)
        {
            DbSet.Remove(entity);
        }

        protected override void UpdateItem(T entity)
        {
            // mark this entity as modified, in case it is not currently attached to this context
            Context.Entry(entity).State = EntityState.Modified;
        }

        protected override void SaveChanges()
        {
            Context.SaveChanges();
        }

        protected override IQueryable<T> BaseQuery(IFetchStrategy<T> fetchStrategy = null)
        {
            var query = DbSet.AsQueryable();
            return fetchStrategy == null ? query : fetchStrategy.IncludePaths.Aggregate(query, (current, path) => current.Include(path));
        }

        // we override the implementation fro LinqBaseRepository becausee this is built in and doesn't need to find the key column and do dynamic expressions, etc.
        protected override T GetQuery(TKey key, TKey2 key2)
        {
            return DbSet.Find(key, key2);
        }

        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            if (Context == null) return;

            Context.Dispose();
            Context = null;
        }
    }
}