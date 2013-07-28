using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SharpRepository.Repository.Specifications;

namespace SharpRepository.Repository.Aggregates
{
    public interface IAggregateQueries<T> where T : class
    {
        IDictionary<TGroupKey, int> GroupCounts<TGroupKey>( Func<T, TGroupKey> keySelector);
        IDictionary<TGroupKey, int> GroupCounts<TGroupKey>(ISpecification<T> criteria, Func<T, TGroupKey> keySelector);
        IDictionary<TGroupKey, int> GroupCounts<TGroupKey>(Expression<Func<T, bool>> predicate, Func<T, TGroupKey> keySelector);
        IDictionary<TGroupKey, long> GroupLongCounts<TGroupKey>(Func<T, TGroupKey> keySelector);
        IDictionary<TGroupKey, long> GroupLongCounts<TGroupKey>(ISpecification<T> criteria, Func<T, TGroupKey> keySelector);
        IDictionary<TGroupKey, long> GroupLongCounts<TGroupKey>(Expression<Func<T, bool>> predicate, Func<T, TGroupKey> keySelector);

        IEnumerable<GroupItem<TGroupKey, TGroupResult>> GroupItems<TGroupKey, TGroupResult>(Func<T, TGroupKey> keySelector, Func<T, TGroupResult> resultSelector);
        IEnumerable<GroupItem<TGroupKey, TGroupResult>> GroupItems<TGroupKey, TGroupResult>(ISpecification<T> criteria, Func<T, TGroupKey> keySelector, Func<T, TGroupResult> resultSelector);
        IEnumerable<GroupItem<TGroupKey, TGroupResult>> GroupItems<TGroupKey, TGroupResult>(Expression<Func<T, bool>> predicate, Func<T, TGroupKey> keySelector, Func<T, TGroupResult> resultSelector);

        long LongCount();
        long LongCount(ISpecification<T> criteria);
        long LongCount(Expression<Func<T, bool>> predicate);
        int Count();
        int Count(ISpecification<T> criteria);
        int Count(Expression<Func<T, bool>> predicate);

        int Sum(Expression<Func<T, int>> selector);
        int Sum(ISpecification<T> criteria, Expression<Func<T, int>> selector);
        int Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> selector);
        int? Sum(Expression<Func<T, int?>> selector);
        int? Sum(ISpecification<T> criteria, Expression<Func<T, int?>> selector);
        int? Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, int?>> selector);
        long Sum(Expression<Func<T, long>> selector);
        long Sum(ISpecification<T> criteria, Expression<Func<T, long>> selector);
        long Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, long>> selector);
        long? Sum(Expression<Func<T, long?>> selector);
        long? Sum(ISpecification<T> criteria, Expression<Func<T, long?>> selector);
        long? Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, long?>> selector);
        decimal Sum(Expression<Func<T, decimal>> selector);
        decimal Sum(ISpecification<T> criteria, Expression<Func<T, decimal>> selector);
        decimal Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, decimal>> selector);
        decimal? Sum(Expression<Func<T, decimal?>> selector);
        decimal? Sum(ISpecification<T> criteria, Expression<Func<T, decimal?>> selector);
        decimal? Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, decimal?>> selector);
        double Sum(Expression<Func<T, double>> selector);
        double Sum(ISpecification<T> criteria, Expression<Func<T, double>> selector);
        double Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, double>> selector);
        double? Sum(Expression<Func<T, double?>> selector);
        double? Sum(ISpecification<T> criteria, Expression<Func<T, double?>> selector);
        double? Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, double?>> selector);
        float Sum(Expression<Func<T, float>> selector);
        float Sum(ISpecification<T> criteria, Expression<Func<T, float>> selector);
        float Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, float>> selector);
        float? Sum(Expression<Func<T, float?>> selector);
        float? Sum(ISpecification<T> criteria, Expression<Func<T, float?>> selector);
        float? Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, float?>> selector);

        TResult Min<TResult>(Expression<Func<T, TResult>> selector);
        TResult Min<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector);
        TResult Min<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector);

        TResult Max<TResult>(Expression<Func<T, TResult>> selector);
        TResult Max<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector);
        TResult Max<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector);
    }
}