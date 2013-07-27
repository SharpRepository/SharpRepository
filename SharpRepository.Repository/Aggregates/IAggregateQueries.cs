using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SharpRepository.Repository.Specifications;

namespace SharpRepository.Repository.Aggregates
{
    public interface IAggregateQueries<T> where T : class
    {
        IDictionary<TGroupKey, int> GroupCounts<TGroupKey>(Func<T, TGroupKey> keySelector, ISpecification<T> criteria = null);
        IDictionary<TGroupKey, int> GroupCounts<TGroupKey>(Func<T, TGroupKey> keySelector, Expression<Func<T, bool>> predicate);
        IDictionary<TGroupKey, long> GroupLongCounts<TGroupKey>(Func<T, TGroupKey> keySelector, ISpecification<T> criteria = null);
        IDictionary<TGroupKey, long> GroupLongCounts<TGroupKey>(Func<T, TGroupKey> keySelector, Expression<Func<T, bool>> predicate);

        IEnumerable<GroupItem<TGroupKey, TGroupResult>> GroupItems<TGroupKey, TGroupResult>(Func<T, TGroupKey> keySelector, Func<T, TGroupResult> resultSelector, ISpecification<T> criteria = null);
        IEnumerable<GroupItem<TGroupKey, TGroupResult>> GroupItems<TGroupKey, TGroupResult>(Func<T, TGroupKey> keySelector, Func<T, TGroupResult> resultSelector, Expression<Func<T, bool>> predicate);

        long LongCount(ISpecification<T> criteria = null);
        long LongCount(Expression<Func<T, bool>> predicate);
        int Count(ISpecification<T> criteria = null);
        int Count(Expression<Func<T, bool>> predicate);

        int Sum(Expression<Func<T, int>> selector, ISpecification<T> criteria = null);
        int Sum(Expression<Func<T, int>> selector, Expression<Func<T, bool>> predicate);
        int? Sum(Expression<Func<T, int?>> selector, ISpecification<T> criteria = null);
        int? Sum(Expression<Func<T, int?>> selector, Expression<Func<T, bool>> predicate);
        long Sum(Expression<Func<T, long>> selector, ISpecification<T> criteria = null);
        long Sum(Expression<Func<T, long>> selector, Expression<Func<T, bool>> predicate);
        long? Sum(Expression<Func<T, long?>> selector, ISpecification<T> criteria = null);
        long? Sum(Expression<Func<T, long?>> selector, Expression<Func<T, bool>> predicate);
        decimal Sum(Expression<Func<T, decimal>> selector, ISpecification<T> criteria = null);
        decimal Sum(Expression<Func<T, decimal>> selector, Expression<Func<T, bool>> predicate);
        decimal? Sum(Expression<Func<T, decimal?>> selector, ISpecification<T> criteria = null);
        decimal? Sum(Expression<Func<T, decimal?>> selector, Expression<Func<T, bool>> predicate);
        double Sum(Expression<Func<T, double>> selector, ISpecification<T> criteria = null);
        double Sum(Expression<Func<T, double>> selector, Expression<Func<T, bool>> predicate);
        double? Sum(Expression<Func<T, double?>> selector, ISpecification<T> criteria = null);
        double? Sum(Expression<Func<T, double?>> selector, Expression<Func<T, bool>> predicate);
        float Sum(Expression<Func<T, float>> selector, ISpecification<T> criteria = null);
        float Sum(Expression<Func<T, float>> selector, Expression<Func<T, bool>> predicate);
        float? Sum(Expression<Func<T, float?>> selector, ISpecification<T> criteria = null);
        float? Sum(Expression<Func<T, float?>> selector, Expression<Func<T, bool>> predicate);

        TResult Min<TResult>(Expression<Func<T, TResult>> selector, ISpecification<T> criteria = null);
        TResult Min<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> predicate);

        TResult Max<TResult>(Expression<Func<T, TResult>> selector, ISpecification<T> criteria = null);
        TResult Max<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> predicate);
    }
}