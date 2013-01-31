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
        private readonly bool _ownSession = false;

        internal NHibernateRepositoryBase(ISessionFactory sessionFactory, ICachingStrategy<T, TKey> cachingStrategy = null) : base(cachingStrategy)
        {
            Initialize(sessionFactory.OpenSession());
            _ownSession = true;
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
            // TODO: check to see if this should be here, NHibernate has the ability to generate the key for you
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
//            using (var tx = Session.BeginTransaction())
//            {
//                Session.Save(entity);
//
//                tx.Commit();
//            }
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
            Session.Flush();
        }

        protected override IQueryable<T> BaseQuery(IFetchStrategy<T> fetchStrategy)
        {
            var query = Session.Query<T>();
            return query;
            //return fetchStrategy == null ? query : fetchStrategy.IncludePaths.Aggregate(query, (current, path) => current.(path));
        }

        // we override the implementation fro LinqBaseRepository becausee this is built in and doesn't need to find the key column and do dynamic expressions, etc.
        protected override T GetQuery(TKey key)
        {
            return Session.Get<T>(key);
        }

        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            if (Session == null) return;

            // only close out the session if we created it from the SessionFactory
            if (_ownSession)
            {
                Session.Close();
                Session.Dispose();
                Session = null;
            }
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

            throw new InvalidOperationException("Primary key could not be generated. This only works for GUID and String.");
        }
    }
}
