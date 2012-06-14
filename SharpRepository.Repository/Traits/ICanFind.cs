using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SharpRepository.Repository.Queries;
using SharpRepository.Repository.Specifications;

namespace SharpRepository.Repository.Traits
{
    /// <summary>
    /// Based on the Interface Segregation Principle (ISP), the
    /// ICanFind interface exposes only the "Find" methods of the
    /// Repository.   
    /// <see cref="http://richarddingwall.name/2009/01/19/irepositoryt-one-size-does-not-fit-all/"/>       
    /// </summary>
    /// <typeparam name="T">Generic repository entity type</typeparam>
    public interface ICanFind<T>
    {
        T Find(Expression<Func<T, bool>> predicate, IQueryOptions<T> queryOptions = null);
        TResult Find<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null);
        T Find(ISpecification<T> criteria, IQueryOptions<T> queryOptions = null);
        TResult Find<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null);

        IEnumerable<T> FindAll(Expression<Func<T, bool>> predicate, IQueryOptions<T> queryOptions = null);
        IEnumerable<TResult> FindAll<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null);
        IEnumerable<T> FindAll(ISpecification<T> criteria, IQueryOptions<T> queryOptions = null);
        IEnumerable<TResult> FindAll<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null);
    }
}