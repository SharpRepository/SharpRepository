using System;
using System.Linq;
using System.Linq.Expressions;
using SharpRepository.Repository.FetchStrategies;

namespace SharpRepository.Repository.Specifications
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Predicate { get; }

        IFetchStrategy<T> FetchStrategy { get; set; }

        T SatisfyingEntityFrom(IQueryable<T> query);

        IQueryable<T> SatisfyingEntitiesFrom(IQueryable<T> query);

        bool IsSatisfiedBy(T entity);

        ISpecification<T> And(ISpecification<T> specification);

        ISpecification<T> And(Expression<Func<T, bool>> predicate);

        ISpecification<T> AndAlso(ISpecification<T> specification);

        ISpecification<T> AndAlso(Expression<Func<T, bool>> predicate);

        ISpecification<T> Not();

        ISpecification<T> AndNot(ISpecification<T> specification);

        ISpecification<T> AndNot(Expression<Func<T, bool>> predicate);

        ISpecification<T> OrNot(ISpecification<T> specification);

        ISpecification<T> OrNot(Expression<Func<T, bool>> predicate);

        ISpecification<T> Or(ISpecification<T> specification);

        ISpecification<T> Or(Expression<Func<T, bool>> predicate);

        ISpecification<T> OrElse(ISpecification<T> specification);

        ISpecification<T> OrElse(Expression<Func<T, bool>> predicate);
    }
}