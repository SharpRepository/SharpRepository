using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SharpRepository.Repository.Specifications;

namespace SharpRepository.Repository
{
    public interface IRepositoryAggregates<T> where T : class
    {
        IEnumerable<TResult> GroupBy<TGroupKey, TResult>(Expression<Func<T, TGroupKey>> keySelector, Expression<Func<IGrouping<TGroupKey, T>, TResult>> resultSelector);
        IEnumerable<TResult> GroupBy<TGroupKey, TResult>(ISpecification<T> criteria, Expression<Func<T, TGroupKey>> keySelector, Expression<Func<IGrouping<TGroupKey, T>, TResult>> resultSelector);
        IEnumerable<TResult> GroupBy<TGroupKey, TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TGroupKey>> keySelector, Expression<Func<IGrouping<TGroupKey, T>, TResult>> resultSelector);

        int Count();
        int Count(ISpecification<T> criteria);
        int Count(Expression<Func<T, bool>> predicate);

        long LongCount();
        long LongCount(ISpecification<T> criteria);
        long LongCount(Expression<Func<T, bool>> predicate);

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

        double Average(Expression<Func<T, int>> selector);
        double Average(ISpecification<T> criteria, Expression<Func<T, int>> selector);
        double Average(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> selector);
        double? Average(Expression<Func<T, int?>> selector);
        double? Average(ISpecification<T> criteria, Expression<Func<T, int?>> selector);
        double? Average(Expression<Func<T, bool>> predicate, Expression<Func<T, int?>> selector);
        double Average(Expression<Func<T, long>> selector);
        double Average(ISpecification<T> criteria, Expression<Func<T, long>> selector);
        double Average(Expression<Func<T, bool>> predicate, Expression<Func<T, long>> selector);
        double? Average(Expression<Func<T, long?>> selector);
        double? Average(ISpecification<T> criteria, Expression<Func<T, long?>> selector);
        double? Average(Expression<Func<T, bool>> predicate, Expression<Func<T, long?>> selector);
        decimal Average(Expression<Func<T, decimal>> selector);
        decimal Average(ISpecification<T> criteria, Expression<Func<T, decimal>> selector);
        decimal Average(Expression<Func<T, bool>> predicate, Expression<Func<T, decimal>> selector);
        decimal? Average(Expression<Func<T, decimal?>> selector);
        decimal? Average(ISpecification<T> criteria, Expression<Func<T, decimal?>> selector);
        decimal? Average(Expression<Func<T, bool>> predicate, Expression<Func<T, decimal?>> selector);
        double Average(Expression<Func<T, double>> selector);
        double Average(ISpecification<T> criteria, Expression<Func<T, double>> selector);
        double Average(Expression<Func<T, bool>> predicate, Expression<Func<T, double>> selector);
        double? Average(Expression<Func<T, double?>> selector);
        double? Average(ISpecification<T> criteria, Expression<Func<T, double?>> selector);
        double? Average(Expression<Func<T, bool>> predicate, Expression<Func<T, double?>> selector);
        float Average(Expression<Func<T, float>> selector);
        float Average(ISpecification<T> criteria, Expression<Func<T, float>> selector);
        float Average(Expression<Func<T, bool>> predicate, Expression<Func<T, float>> selector);
        float? Average(Expression<Func<T, float?>> selector);
        float? Average(ISpecification<T> criteria, Expression<Func<T, float?>> selector);
        float? Average(Expression<Func<T, bool>> predicate, Expression<Func<T, float?>> selector);

        TResult Min<TResult>(Expression<Func<T, TResult>> selector);
        TResult Min<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector);
        TResult Min<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector);

        TResult Max<TResult>(Expression<Func<T, TResult>> selector);
        TResult Max<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector);
        TResult Max<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector);

        // convenience methods
        IDictionary<TGroupKey, int> GroupCount<TGroupKey>(Expression<Func<T, TGroupKey>> selector);
        IDictionary<TGroupKey, int> GroupCount<TGroupKey>(ISpecification<T> criteria, Expression<Func<T, TGroupKey>> selector);
        IDictionary<TGroupKey, int> GroupCount<TGroupKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TGroupKey>> selector);

        IDictionary<TGroupKey, long> GroupLongCount<TGroupKey>(Expression<Func<T, TGroupKey>> selector);
        IDictionary<TGroupKey, long> GroupLongCount<TGroupKey>(ISpecification<T> criteria, Expression<Func<T, TGroupKey>> selector);
        IDictionary<TGroupKey, long> GroupLongCount<TGroupKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TGroupKey>> selector);

        // I can't get these to work with Expression<Func<T, TResult>> selector so it's throwing an error in EF5 when trying to use the Func<T. TResult> selector
        //  so for now I'm going to leave these convenience methods out and we can use GroupBy to handle it ourselves
//        IDictionary<TGroupKey, TResult> GroupMin<TGroupKey, TResult>(Expression<Func<T, TGroupKey>> groupSelector, Func<T, TResult> selector);
//        IDictionary<TGroupKey, TResult> GroupMin<TGroupKey, TResult>(ISpecification<T> criteria, Expression<Func<T, TGroupKey>> groupSelector, Func<T, TResult> selector);
//        IDictionary<TGroupKey, TResult> GroupMin<TGroupKey, TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TGroupKey>> groupSelector, Func<T, TResult> selector);
//
//        IDictionary<TGroupKey, TResult> GroupMax<TGroupKey, TResult>(Expression<Func<T, TGroupKey>> groupSelector, Func<T, TResult> selector);
//        IDictionary<TGroupKey, TResult> GroupMax<TGroupKey, TResult>(ISpecification<T> criteria, Expression<Func<T, TGroupKey>> groupSelector, Func<T, TResult> selector);
//        IDictionary<TGroupKey, TResult> GroupMax<TGroupKey, TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TGroupKey>> groupSelector, Func<T, TResult> selector);


    }
}