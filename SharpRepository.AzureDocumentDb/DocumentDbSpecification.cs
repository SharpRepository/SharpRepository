using System;
using System.Linq;
using System.Linq.Expressions;
using SharpRepository.Repository.FetchStrategies;
using SharpRepository.Repository.Specifications;

namespace SharpRepository.AzureDocumentDb
{
    public class DocumentDbSpecification<T> : Specification<T>
    {
        public DocumentDbSpecification()
            : base((Expression<Func<T, bool>>)null)
        {
            // null Predicate means it matches everything
        }

        public DocumentDbSpecification(Expression<Func<T, bool>> predicate)
            : base(predicate)
        {
        }

        public DocumentDbSpecification(ISpecification<T> specification)
            : base(specification)
        {
        }

        public override T SatisfyingEntityFrom(IQueryable<T> query)
        {
            return SatisfyingEntitiesFrom(query).AsEnumerable().FirstOrDefault();
        }
    }
}