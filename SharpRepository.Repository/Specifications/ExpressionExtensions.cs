using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

namespace SharpRepository.Repository.Specifications
{
    /// <summary>
    /// Based on LINQ to Entities: Combining Predicates article by Colin Meek 
    /// <see cref="http://blogs.msdn.com/b/meek/archive/2008/05/02/linq-to-entities-combining-predicates.aspx"/>,
    /// ExpressionExtensions provide for composition of Specifications.
    /// </summary>
    public static class ExpressionExtensions
    {
        public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second,
                                               Func<Expression, Expression, Expression> merge)
        {
            // build parameter map (from parameters of second to parameters of first)
            Dictionary<ParameterExpression, ParameterExpression> map =
                first.Parameters.Select((f, i) => new {f, s = second.Parameters[i]}).ToDictionary(p => p.s, p => p.f);

            // replace parameters in the second lambda expression with parameters from the first
            Expression secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            // apply composition of lambda expression bodies to parameters from the first expression 
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first,
                                                       Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.And);
        }

        public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> first,
                                                       Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.AndAlso);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first,
                                                      Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.Or);
        }

        public static Expression<Func<T, bool>> OrElse<T>(this Expression<Func<T, bool>> first,
                                                      Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.OrElse);
        }

        public static Expression<Func<T, bool>> AndNot<T>(this Expression<Func<T, bool>> first,
                                                          Expression<Func<T, bool>> second)
        {
            return first.Compose(Not(second), Expression.And);
        }

        public static Expression<Func<T, bool>> OrNot<T>(this Expression<Func<T, bool>> first,
                                                         Expression<Func<T, bool>> second)
        {
            return first.Compose(second.Not(), Expression.Or);
        }

        public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expression)
        {
            ReadOnlyCollection<ParameterExpression> parameters = expression.Parameters;
            UnaryExpression condition = Expression.Not(expression.Body);
            LambdaExpression lambda = Expression.Lambda(condition, parameters);
            return (Expression<Func<T, bool>>) lambda;
        }
    }

    /// <summary>
    /// ParameterRebinder manually rebinds  parameters since LINQ to Entities does not support InvocationExpressions.  
    /// Based on LINQ to Entities: Combining Predicates article by Colin Meek 
    /// <see cref="http://blogs.msdn.com/b/meek/archive/2008/05/02/linq-to-entities-combining-predicates.aspx"/> and
    /// Matt Warren’s series of articles on Buildin an IQueryable provider: 
    /// <see cref="http://blogs.msdn.com/b/mattwar/archive/2007/07/30/linq-building-an-iqueryable-provider-part-i.aspx"/>
    /// <see cref="http://blogs.msdn.com/b/mattwar/archive/2007/07/31/linq-building-an-iqueryable-provider-part-ii.aspx"/>
    /// </summary>
    public class ParameterRebinder : ExpressionVisitor
    {
        private readonly Dictionary<ParameterExpression, ParameterExpression> _map;

        public ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
        {
            _map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map,
                                                   Expression exp)
        {
            return new ParameterRebinder(map).Visit(exp);
        }

        protected override Expression VisitParameter(ParameterExpression p)
        {
            if (_map.TryGetValue(p, out ParameterExpression replacement))
            {
                p = replacement;
            }
            return base.VisitParameter(p);
        }
    }
}