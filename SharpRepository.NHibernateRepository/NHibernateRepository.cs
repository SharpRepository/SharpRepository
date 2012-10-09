using System;
using SharpRepository.Repository.Caching;
using NHibernate;

namespace SharpRepository.NHibernateRepository
{
    /// <summary>
    /// RavenDb repository layer
    /// </summary>
    /// <typeparam name="T">The type of object the repository acts on.</typeparam>
    /// <typeparam name="TKey">The type of the primary key.</typeparam>
    public class NHibernateRepository<T, TKey> : NHibernateRepositoryBase<T, TKey> where T : class, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref=" NHibernateRepository&lt;T, TKey&gt;"/> class.
        /// </summary>
        /// <param name="session">NHibernate session.</param>
        /// <param name="cachingStrategy">The caching strategy.  Defaults to <see cref="NoCachingStrategy&lt;T, TKey&gt;" />.</param>
        public NHibernateRepository(ISession session, ICachingStrategy<T, TKey> cachingStrategy = null)
            : base(session, cachingStrategy) 
        {
            if (session == null) throw new ArgumentNullException("session");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref=" NHibernateRepository&lt;T, TKey&gt;"/> class.
        /// </summary>
        /// <param name="sessionFactory">The NHibernate Session Factory.</param>
        /// <param name="cachingStrategy">The caching strategy.  Defaults to <see cref="NoCachingStrategy&lt;T, TKey&gt;" />.</param>
        public NHibernateRepository(ISessionFactory sessionFactory, ICachingStrategy<T, TKey> cachingStrategy = null)
            : base(sessionFactory, cachingStrategy) 
        {
            if (sessionFactory == null) throw new ArgumentNullException("sessionFactory");
        }  
    }

    /// <summary>
    /// RavenDb repository layer
    /// </summary>
    /// <typeparam name="T">The type of object the repository acts on.</typeparam>
    public class NHibernateRepository<T> : NHibernateRepositoryBase<T, string> where T : class, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref=" NHibernateRepository&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="session">The Nibernate Session.</param>
        /// <param name="cachingStrategy">The caching strategy.  Defaults to <see cref="NoCachingStrategy&lt;T&gt;" />.</param>
        public NHibernateRepository(ISession session, ICachingStrategy<T, string> cachingStrategy = null)
            : base(session, cachingStrategy)
        {
            if (session == null) throw new ArgumentNullException("session");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref=" NHibernateRepository&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="sessionFactory">The NHibernate Sesison Factory.</param>
        /// <param name="cachingStrategy">The caching strategy.  Defaults to <see cref="NoCachingStrategy&lt;T&gt;" />.</param>
        public NHibernateRepository(ISessionFactory sessionFactory, ICachingStrategy<T, string> cachingStrategy = null)
            : base(sessionFactory, cachingStrategy)
        {
            if (sessionFactory == null) throw new ArgumentNullException("sessionFactory");
        }  
    }
}
