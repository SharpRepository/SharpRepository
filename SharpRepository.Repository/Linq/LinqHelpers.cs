using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SharpRepository.Repository.Linq
{
    public static class LinqHelpers
    {
        private static readonly MethodInfo OrderByMethod = typeof(Queryable).GetMethods().Single(method => method.Name == "OrderBy" && method.GetParameters().Length == 2);
        private static readonly MethodInfo OrderByDescendingMethod = typeof(Queryable).GetMethods().Single(method => method.Name == "OrderByDescending" && method.GetParameters().Length == 2);
        private static readonly MethodInfo ThenByMethod = typeof(Queryable).GetMethods().Single(method => method.Name == "ThenBy" && method.GetParameters().Length == 2);
        private static readonly MethodInfo ThenByDescendingMethod = typeof(Queryable).GetMethods().Single(method => method.Name == "ThenByDescending" && method.GetParameters().Length == 2);

        public static IOrderedQueryable<TSource> ApplyOrdering<TSource>(IQueryable<TSource> source, string propertyName, MethodInfo orderingMethod)
        {
            var parameter = Expression.Parameter(typeof(TSource), "x");
            var orderByProperty = Expression.Property(parameter, propertyName);

            var lambda = Expression.Lambda(orderByProperty, new[] { parameter });

            var genericMethod = orderingMethod.MakeGenericMethod(new[] { typeof(TSource), orderByProperty.Type });

            return (IOrderedQueryable<TSource>)genericMethod.Invoke(null, new object[] { source, lambda });
        }

        public static IOrderedQueryable<TSource> OrderByProperty<TSource>(this IQueryable<TSource> source, string propertyName)
        {
            return ApplyOrdering(source, propertyName, OrderByMethod);
        }

        public static IOrderedQueryable<TSource> OrderByDescendingProperty<TSource>(this IQueryable<TSource> source, string propertyName)
        {
            return ApplyOrdering(source, propertyName, OrderByDescendingMethod);
        }

        public static IOrderedQueryable<TSource> ThenByProperty<TSource>(this IOrderedQueryable<TSource> source, string propertyName)
        {
            return ApplyOrdering(source, propertyName, ThenByMethod);
        }

        public static IOrderedQueryable<TSource> ThenByDescendingProperty<TSource>(this IOrderedQueryable<TSource> source, string propertyName)
        {
            return ApplyOrdering(source, propertyName, ThenByDescendingMethod);
        }
    }
}
