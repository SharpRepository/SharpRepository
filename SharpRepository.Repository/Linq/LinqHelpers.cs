using System;
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

        public static IOrderedQueryable<T> ApplyOrdering<T>(IQueryable<T> source, string propertyName, MethodInfo orderingMethod)
        {
            var props = propertyName.Split('.');
            var type = typeof(T);
            var arg = Expression.Parameter(type, "x");
            Expression expr = arg;
            foreach (var pi in props.Select(prop => type.GetProperty(prop)))
            {
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }
            var delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            var lambda = Expression.Lambda(delegateType, expr, arg);

            return (IOrderedQueryable<T>)orderingMethod
                .MakeGenericMethod(typeof(T), type)
                .Invoke(null, new object[] { source, lambda });
        }

        public static IOrderedQueryable<T> OrderByProperty<T>(this IQueryable<T> source, string propertyName)
        {
            return ApplyOrdering(source, propertyName, OrderByMethod);
        }

        public static IOrderedQueryable<T> OrderByDescendingProperty<T>(this IQueryable<T> source, string propertyName)
        {
            return ApplyOrdering(source, propertyName, OrderByDescendingMethod);
        }

        public static IOrderedQueryable<T> ThenByProperty<T>(this IOrderedQueryable<T> source, string propertyName)
        {
            return ApplyOrdering(source, propertyName, ThenByMethod);
        }

        public static IOrderedQueryable<T> ThenByDescendingProperty<T>(this IOrderedQueryable<T> source, string propertyName)
        {
            return ApplyOrdering(source, propertyName, ThenByDescendingMethod);
        }
    }
}
