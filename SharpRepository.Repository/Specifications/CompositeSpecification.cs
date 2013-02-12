using System;
using System.Linq;
using System.Linq.Expressions;
using SharpRepository.Repository.FetchStrategies;

namespace SharpRepository.Repository.Specifications
{
    /// <summary>
    /// http://devlicio.us/blogs/jeff_perrin/archive/2006/12/13/the-specification-pattern.aspx
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class CompositeSpecification<T> : ISpecification<T>
    {
        protected CompositeSpecification(Expression<Func<T, bool>> predicate)
        {
            FetchStrategy = new GenericFetchStrategy<T>();
            Predicate = predicate;
        }

        #region ISpecification<T> Members

        public Expression<Func<T, bool>> Predicate { get; set; }

        public T SatisfyingEntityFrom(IQueryable<T> query)
        {
            return SatisfyingEntitiesFrom(query).FirstOrDefault();
        }

        public IQueryable<T> SatisfyingEntitiesFrom(IQueryable<T> query)
        {
            return Predicate == null ? query : query.Where(Predicate);
        }

        public bool IsSatisfiedBy(T entity)
        {
            if (Predicate == null) return true;

            return new[] { entity }.AsQueryable().Any(Predicate);
        }

        public IFetchStrategy<T> FetchStrategy { get; set; }

        #endregion
    }
}