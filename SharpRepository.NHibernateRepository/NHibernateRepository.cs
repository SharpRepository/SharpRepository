using System;
using NHibernate;
using SharpRepository.Repository.Caching;

namespace SharpRepository.NHibernateRepository
{
    /// <summary>
    /// Entity Framework repository layer
    /// </summary>
    /// <typeparam name="T">The Entity type</typeparam>
    /// <typeparam name="TKey">The type of the primary key.</typeparam>
    public class NHibernateRepository<T, TKey> : NHibernateRepositoryBase<T, TKey> where T : class, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NHibernateRepository&lt;T, TKey&gt;"/> class.
        /// </summary>
        /// <param name="sessionFactory">The NHibernate Session Factory.</param>
        /// <param name="cachingStrategy">The caching strategy to use.  Defaults to <see cref="NoCachingStrategy&lt;T, TKey&gt;" /></param>
        public NHibernateRepository(ISessionFactory sessionFactory, ICachingStrategy<T, TKey> cachingStrategy = null)
            : base(sessionFactory, cachingStrategy)
        {
            if (sessionFactory == null) throw new ArgumentNullException("sessionFactory");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NHibernateRepository&lt;T, TKey&gt;"/> class.
        /// </summary>
        /// <param name="session">The NHibernate Session.</param>
        /// <param name="cachingStrategy">The caching strategy to use.  Defaults to <see cref="NoCachingStrategy&lt;T, TKey&gt;" /></param>
        public NHibernateRepository(ISession session, ICachingStrategy<T, TKey> cachingStrategy = null)
            : base(session, cachingStrategy)
        {
            if (session == null) throw new ArgumentNullException("session");
        }
    }

    /// <summary>
    /// Entity Framework repository layer
    /// </summary>
    /// <typeparam name="T">The Entity type</typeparam>
    public class NHibernateRepository<T> : NHibernateRepositoryBase<T, int> where T : class, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NHibernateRepository&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="sessionFactory">The Entity Framework Session Factory.</param>
        /// <param name="cachingStrategy">The caching strategy to use.  Defaults to <see cref="NoCachingStrategy&lt;T, TKey&gt;" /></param>
        public NHibernateRepository(ISessionFactory sessionFactory, ICachingStrategy<T, int> cachingStrategy = null)
            : base(sessionFactory, cachingStrategy)
        {
            if (sessionFactory == null) throw new ArgumentNullException("sessionFactory");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NHibernateRepository&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="session">The NHibernate Session.</param>
        /// <param name="cachingStrategy">The caching strategy to use.  Defaults to <see cref="NoCachingStrategy&lt;T, TKey&gt;" /></param>
        public NHibernateRepository(ISession session, ICachingStrategy<T, int> cachingStrategy = null)
            : base(session, cachingStrategy)
        {
            if (session == null) throw new ArgumentNullException("session");
        }
    }
}
