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
    }
}