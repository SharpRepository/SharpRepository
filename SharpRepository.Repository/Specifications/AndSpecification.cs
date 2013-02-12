namespace SharpRepository.Repository.Specifications
{
    public class AndSpecification<T> : CompositeSpecification<T>
    {
        public AndSpecification(ISpecification<T> leftSide, ISpecification<T> rightSide)
            : base(leftSide.Predicate.And(rightSide.Predicate))
        {
        }
    }
}