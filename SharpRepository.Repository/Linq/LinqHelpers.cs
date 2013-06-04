using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SharpRepository.Repository.Linq
{
    public static class LinqHelpers
    {
        private static readonly MethodInfo OrderByMethod = typeof(Queryable).GetMethods().Single(method => method.Name == "OrderBy" && method.GetParameters().Length == 2);
        private static readonly MethodInfo OrderByDescendingMethod = typeof(Queryable).GetMethods().Single(method => method.Name == "OrderByDescending" && method.GetParameters().Length == 2);

        public static IQueryable<TSource> OrderByProperty<TSource>(this IQueryable<TSource> source, string propertyName)
        {
            var parameter = Expression.Parameter(typeof(TSource), "x");
            var orderByProperty = Expression.Property(parameter, propertyName);

            var lambda = Expression.Lambda(orderByProperty, new[] { parameter });

            var genericMethod = OrderByMethod.MakeGenericMethod(new[] { typeof(TSource), orderByProperty.Type });
            
            return (IQueryable<TSource>)genericMethod.Invoke(null, new object[] { source, lambda });
        }

        public static IQueryable<TSource> OrderByDescendingProperty<TSource>(this IQueryable<TSource> source, string propertyName)
        {
            var parameter = Expression.Parameter(typeof(TSource), "x");
            var orderByProperty = Expression.Property(parameter, propertyName);

            var lambda = Expression.Lambda(orderByProperty, new[] { parameter });

            var genericMethod = OrderByDescendingMethod.MakeGenericMethod(new[] { typeof(TSource), orderByProperty.Type });
            
            return (IQueryable<TSource>)genericMethod.Invoke(null, new object[] { source, lambda });
        }
    }
}
