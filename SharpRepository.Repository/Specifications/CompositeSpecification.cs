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
        protected readonly Specification<T> LeftSide;
        protected readonly Specification<T> RightSide;

        protected CompositeSpecification(Specification<T> leftSide, Specification<T> rightSide)
        {
            LeftSide = leftSide;
            RightSide = rightSide;
            FetchStrategy = new GenericFetchStrategy<T>();
        }

        #region ISpecification<T> Members

        public abstract Expression<Func<T, bool>> Predicate { get; }

        public abstract T SatisfyingEntityFrom(IQueryable<T> query);

        public abstract IQueryable<T> SatisfyingEntitiesFrom(IQueryable<T> query);

        public abstract bool IsSatisfiedBy(T entity);

        public IFetchStrategy<T> FetchStrategy { get; set; }

        #endregion
    }
}