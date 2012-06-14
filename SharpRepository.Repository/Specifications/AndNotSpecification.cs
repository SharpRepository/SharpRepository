using System;
using System.Linq;
using System.Linq.Expressions;

namespace SharpRepository.Repository.Specifications
{
    public class AndNotSpecification<T> : CompositeSpecification<T>
    {
        public AndNotSpecification(Specification<T> leftSide, Specification<T> rightSide) : base(leftSide, rightSide)
        {
        }

        public override Expression<Func<T, bool>> Predicate
        {
            get { return LeftSide.Predicate.AndNot(RightSide.Predicate); }
        }

        public override T SatisfyingEntityFrom(IQueryable<T> query)
        {
            return SatisfyingEntitiesFrom(query).FirstOrDefault();
        }

        public override IQueryable<T> SatisfyingEntitiesFrom(IQueryable<T> query)
        {
            return query.Where(Predicate);
        }

        public override bool IsSatisfiedBy(T entity)
        {
            return new[] {entity}.AsQueryable().Any(Predicate);
        }
    }
}