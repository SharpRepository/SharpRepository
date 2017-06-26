using System;
using System.Linq;
using System.Linq.Expressions;
using SharpRepository.Repository.Caching.Hash;
using SharpRepository.Repository.FetchStrategies;
using System.Collections;
using System.Collections.Generic;

namespace SharpRepository.Repository.Specifications
{
    /// <summary>
    /// In simple terms, a specification is a small piece of logic which is independent and give an answer 
    /// to the question “does this match?”. With Specification, we isolate the logic that do the selection 
    /// into a reusable business component that can be passed around easily from the entity we are selecting.
    /// </summary>
    /// <see cref="http://en.wikipedia.org/wiki/Specification_pattern"/>
    /// 
    /// Inspired by Huy Nguyen's article, Entity Framework 4 POCO, Repository and Specification Pattern
    /// <see cref="http://huyrua.wordpress.com/2010/07/13/entity-framework-4-poco-repository-and-specification-pattern/"/>
    /// and open source project
    /// <see cref="http://code.google.com/p/ef4prs/downloads/list"/>
    /// 
    /// And Will Beattie's article, Specification Pattern, Entity Framework & LINQ
    /// <see cref="http://blog.willbeattie.net/2011/02/specification-pattern-entity-framework.html"/>
    /// 
    /// <typeparam name="T"></typeparam>
    public class Specification<T> : ISpecification<T>
    {
        public Specification()
            : this((Expression<Func<T, bool>>)null)
        {
            // null Predicate means it matches everything
        }

        public Specification(Expression<Func<T, bool>> predicate)
        {
            Predicate = predicate;
            FetchStrategy = new GenericFetchStrategy<T>();
        }

        public Specification(ISpecification<T> specification)
        {
            Predicate = specification.Predicate;
            FetchStrategy = specification.FetchStrategy;
        }

        #region ISpecification<T> Members

        public Expression<Func<T, bool>> Predicate { get; set; }

        public virtual T SatisfyingEntityFrom(IQueryable<T> query)
        {
            return SatisfyingEntitiesFrom(query).FirstOrDefault();
        }

        public virtual IQueryable<T> SatisfyingEntitiesFrom(IQueryable<T> query)
        {
            return Predicate == null ? query : query.Where(Predicate);
        }

        public bool IsSatisfiedBy(T entity)
        {
            return Predicate == null || new[] { entity }.AsQueryable().Any(Predicate);
        }

        public IFetchStrategy<T> FetchStrategy { get; set; }

        #endregion

        protected virtual Specification<T> Instanciate(Expression<Func<T, bool>> predicate, IFetchStrategy<T> strategy = null)
        {
            var specification = new Specification<T>(predicate);
            if (strategy != null)
                specification.FetchStrategy = strategy;

            return specification;
        }

        protected IFetchStrategy<T> InstanciateFetchStrategy(IFetchStrategy<T> strategy)
        {
            var thisPaths = FetchStrategy != null ? FetchStrategy.IncludePaths : new List<string>();
            var paramPaths = strategy != null ? strategy.IncludePaths : new List<string>();
            var includePaths = thisPaths.Union(paramPaths);

            var newStrategy = new GenericFetchStrategy<T>();
            foreach (var includePath in includePaths)
            {
                newStrategy.Include(includePath);
            }

            return newStrategy;
        }

        public ISpecification<T> And(ISpecification<T> specification)
        {
            return Instanciate(Predicate.And(specification.Predicate), InstanciateFetchStrategy(specification.FetchStrategy) );
        }

        public ISpecification<T> And(Expression<Func<T, bool>> predicate)
        {
            return Instanciate(Predicate.And(predicate), FetchStrategy);
        }

        public ISpecification<T> AndAlso(ISpecification<T> specification)
        {
            return Instanciate(Predicate.AndAlso(specification.Predicate), InstanciateFetchStrategy(specification.FetchStrategy));
        }

        public ISpecification<T> AndAlso(Expression<Func<T, bool>> predicate)
        {
            return Instanciate(Predicate.AndAlso(predicate), FetchStrategy);
        }

        public ISpecification<T> Not()
        {
            return Instanciate(Predicate.Not(), FetchStrategy);
        }

        public ISpecification<T> AndNot(ISpecification<T> specification)
        {
            return Instanciate(Predicate.AndNot(specification.Predicate), InstanciateFetchStrategy(specification.FetchStrategy));
        }

        public ISpecification<T> AndNot(Expression<Func<T, bool>> predicate)
        {
            return Instanciate(Predicate.AndNot(predicate), FetchStrategy);
        }

        public ISpecification<T> OrNot(ISpecification<T> specification)
        {
            return Instanciate(Predicate.OrNot(specification.Predicate), InstanciateFetchStrategy(specification.FetchStrategy));
        }

        public ISpecification<T> OrNot(Expression<Func<T, bool>> predicate)
        {
            return Instanciate(Predicate.OrNot(predicate), FetchStrategy);
        }

        public ISpecification<T> Or(ISpecification<T> specification)
        {
            return Instanciate(Predicate.Or(specification.Predicate), InstanciateFetchStrategy(specification.FetchStrategy));
        }

        public ISpecification<T> Or(Expression<Func<T, bool>> predicate)
        {
            return Instanciate(Predicate.Or(predicate), FetchStrategy);
        }

        public ISpecification<T> OrElse(ISpecification<T> specification)
        {
            return Instanciate(Predicate.OrElse(specification.Predicate), InstanciateFetchStrategy(specification.FetchStrategy));
        }

        public ISpecification<T> OrElse(Expression<Func<T, bool>> predicate)
        {
            return Instanciate(Predicate.OrElse(predicate), FetchStrategy);
        }

        public static ISpecification<T> And(ISpecification<T> specification, ISpecification<T> specification2)
        {
            return new Specification<T>(specification.And(specification2));
        }

        public static ISpecification<T> And(Expression<Func<T, bool>> predicate, Expression<Func<T, bool>> predicate2)
        {
            return new Specification<T>(predicate.And(predicate2));
        }

        public static ISpecification<T> AndAlso(ISpecification<T> specification, ISpecification<T> specification2)
        {
            return new Specification<T>(specification.AndAlso(specification));
        }

        public static ISpecification<T> AndAlso(Expression<Func<T, bool>> predicate, Expression<Func<T, bool>> predicate2)
        {
            return new Specification<T>(predicate.AndAlso(predicate2));
        }

        public static ISpecification<T> Not(ISpecification<T> specification)
        {
            return new Specification<T>(specification.Not());
        }

        public static ISpecification<T> AndNot(ISpecification<T> specification, ISpecification<T> specification2)
        {
            return new Specification<T>(specification.AndNot(specification2));
        }

        public static ISpecification<T> AndNot(Expression<Func<T, bool>> predicate, Expression<Func<T, bool>> predicate2)
        {
            return new Specification<T>(predicate.AndNot(predicate2));
        }

        public static ISpecification<T> OrNot(ISpecification<T> specification, ISpecification<T> specification2)
        {
            return new Specification<T>(specification.OrNot(specification2));
        }

        public static ISpecification<T> OrNot(Expression<Func<T, bool>> predicate, Expression<Func<T, bool>> predicate2)
        {
            return new Specification<T>(predicate.OrNot(predicate2));
        }

        public static ISpecification<T> Or(ISpecification<T> specification, ISpecification<T> specification2)
        {
            return new Specification<T>(specification.Or(specification2));
        }

        public static ISpecification<T> Or(Expression<Func<T, bool>> predicate, Expression<Func<T, bool>> predicate2)
        {
            return new Specification<T>(predicate.Or(predicate2));
        }

        public static ISpecification<T> OrElse(ISpecification<T> specification, ISpecification<T> specification2)
        {
            return new Specification<T>(specification.OrElse(specification2));
        }

        public static ISpecification<T> OrElse(Expression<Func<T, bool>> predicate, Expression<Func<T, bool>> predicate2)
        {
            return new Specification<T>(predicate.OrElse(predicate2));
        }

        /// <summary>
        /// Used to get a unique string for this particular specification. The Generational key uses a MD5 of this value.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("Specification Type: {0}\nEntity Type: {1}\nPredicate: {2}\nFetchStrategy: {3}",
                                 GetType().Name,
                                 (typeof(T)).Name,
                                 HashGenerator.FromPredicate(Predicate),
                                 FetchStrategy
                );
        }
    }
}