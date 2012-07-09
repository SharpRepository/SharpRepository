using System;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using SharpRepository.Repository;
using SharpRepository.Repository.Caching;
using SharpRepository.Repository.FetchStrategies;

// Reference: http://www.d80.co.uk/post/2011/02/20/Linq-to-NHibernate-Tutorial.aspx
namespace SharpRepository.NHibernateRepository
{
    public class NHibernateRepositoryBase<T, TKey> : LinqRepositoryBase<T, TKey> where T : class, new()
    {
        protected ISession Session { get; private set; }

        internal NHibernateRepositoryBase(ISessionFactory sessionFactory, ICachingStrategy<T, TKey> cachingStrategy = null) : base(cachingStrategy)
        {
            Initialize(sessionFactory.OpenSession());
        }

        internal NHibernateRepositoryBase(ISession session, ICachingStrategy<T, TKey> cachingStrategy = null)
            : base(cachingStrategy)
        {
            Initialize(session);
        }

        private void Initialize(ISession session)
        {
            Session = session;
        }

        protected override void AddItem(T entity)
        {
            if (typeof(TKey) == typeof(Guid) || typeof(TKey) == typeof(string))
            {
                TKey id;
                if (GetPrimaryKey(entity, out id) && Equals(id, default(TKey)))
                {
                    id = GeneratePrimaryKey();
                    SetPrimaryKey(entity, id);
                }
            }

            // TODO: Do we need to wrap these each in a transaction (session.BeginTransaction() and trans.Committ())?

            Session.Save(entity);
        }

        protected override void DeleteItem(T entity)
        {
            Session.Delete(entity);
        }

        protected override void UpdateItem(T entity)
        {
            Session.Update(entity);
        }

        protected override void SaveChanges()
        {
            // TODO: is anything needed here
        }

        protected override IQueryable<T> BaseQuery(IFetchStrategy<T> fetchStrategy)
        {
            var query = Session.Linq<T>();
            return query;
            //return fetchStrategy == null ? query : fetchStrategy.IncludePaths.Aggregate(query, (current, path) => current.(path));
        }

        // we override the implementation fro LinqBaseRepository becausee this is built in and doesn't need to find the key column and do dynamic expressions, etc.
        protected override T GetQuery(TKey key)
        {
            return Session.Get<T>(key);
        }

        // TODO: use logic like this to override GetPrimaryKey
        //  below is using the older EF stuff and it doesn't translate exactly
        //private EntityKey GetEntityKey(object keyValue)
        //{
        //    var entitySetName = GetEntityName();
        //    var keyPropertyName = _dbSet.EntitySet.ElementType.KeyMembers[0].ToString();
        //    return new EntityKey(entitySetName, new[] { new EntityKeyMember(keyPropertyName, keyValue) });

        //}

        //private string GetEntityName()
        //{
        //    return string.Format("{0}.{1}", Context.DefaultContainerName, QueryBase.EntitySet.Name);
        //}

        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            if (Session == null) return;

            Session.Dispose();
            Session = null;
        }

        private static TKey GeneratePrimaryKey()
        {
            if (typeof(TKey) == typeof(Guid))
            {
                return (TKey)Convert.ChangeType(Guid.NewGuid(), typeof(TKey));
            }

            if (typeof(TKey) == typeof(string))
            {
                return (TKey)Convert.ChangeType(Guid.NewGuid().ToString(), typeof(TKey));
            }

            throw new InvalidOperationException("Primary key could not be generated. This only works for GUID, Int32 and String.");
        }
    }
}
