using System;
using System.Linq.Expressions;
using SharpRepository.Repository.Specifications;

namespace SharpRepository.Repository.Caching.Hash
{
    public class HashGenerator
    {
        public static string FromSpecification<T>(ISpecification<T> specification)
        {
            return FromPredicate(specification.Predicate);
        }

        public static string FromPredicate<T>(Expression<Func<T,bool>> predicate)
        {
            return FromPredicate(predicate.Body);
        }

        public static string FromPredicate(Expression expression)
        {
            return new HashGeneratorHelper(expression).GetHash();
        }
    }

    internal class HashGeneratorHelper
    {
        private readonly Expression _expression;

        public HashGeneratorHelper(Expression expression)
        {
            _expression = expression;
        }

        // special thanks to Pete Montgomery's post here: http://petemontgomery.wordpress.com/2008/08/07/caching-the-results-of-linq-queries/
        public string GetHash()
        {
            if (_expression == null)
                return null;

            var expression = _expression;

             // locally evaluate as much of the query as possible
            expression = Evaluator.PartialEval(expression);

            // support local collections
            expression = LocalCollectionExpander.Rewrite(expression);

            // use the string representation of the expression for the cache key
            return expression.ToString();
        }
    }

}
