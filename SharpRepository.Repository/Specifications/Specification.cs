﻿using System;
using System.Linq;
using System.Linq.Expressions;
using SharpRepository.Repository.Caching.Hash;
using SharpRepository.Repository.FetchStrategies;

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
        public Specification() : this((Expression<Func<T, bool>>)null)
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

        public T SatisfyingEntityFrom(IQueryable<T> query)
        {
            return SatisfyingEntitiesFrom(query).FirstOrDefault();
        }

        public IQueryable<T> SatisfyingEntitiesFrom(IQueryable<T> query)
        {
            return Predicate == null ? query :  query.Where(Predicate);
        }

        public bool IsSatisfiedBy(T entity)
        {
            return Predicate == null || new[] {entity}.AsQueryable().Any(Predicate);
        }

        public IFetchStrategy<T> FetchStrategy { get; set; }

        #endregion

        public ISpecification<T> And(ISpecification<T> specification)
        {
            return new Specification<T>(Predicate.And(specification.Predicate));
        }

        public ISpecification<T> And(Expression<Func<T, bool>> predicate)
        {
            return new Specification<T>(Predicate.And(predicate));
        }

        public ISpecification<T> AndAlso(ISpecification<T> specification)
        {
            return new Specification<T>(Predicate.AndAlso(specification.Predicate));
        }

        public ISpecification<T> AndAlso(Expression<Func<T, bool>> predicate)
        {
            return new Specification<T>(Predicate.AndAlso(predicate));
        }

        public ISpecification<T> Not()
        {
            return new Specification<T>(Predicate.Not());
        }

        public ISpecification<T> AndNot(ISpecification<T> specification)
        {
            return new Specification<T>(Predicate.AndNot(specification.Predicate));
        }

        public ISpecification<T> AndNot(Expression<Func<T, bool>> predicate)
        {
            return new Specification<T>(Predicate.AndNot(predicate));
        }

        public ISpecification<T> OrNot(ISpecification<T> specification)
        {
            return new Specification<T>(Predicate.OrNot(specification.Predicate));
        }

        public ISpecification<T> OrNot(Expression<Func<T, bool>> predicate)
        {
            return new Specification<T>(Predicate.OrNot(predicate));
        }

        public ISpecification<T> Or(ISpecification<T> specification)
        {
            return new Specification<T>(Predicate.Or(specification.Predicate));
        }

        public ISpecification<T> Or(Expression<Func<T, bool>> predicate)
        {
            return new Specification<T>(Predicate.Or(predicate));
        }

        public ISpecification<T> OrElse(ISpecification<T> specification)
        {
            return new Specification<T>(Predicate.OrElse(specification.Predicate));
        }

        public ISpecification<T> OrElse(Expression<Func<T, bool>> predicate)
        {
            return new Specification<T>(Predicate.OrElse(predicate));
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
                                 (typeof (T)).Name,
                                 HashGenerator.FromPredicate(Predicate),
                                 FetchStrategy
                );
        }
    }
}