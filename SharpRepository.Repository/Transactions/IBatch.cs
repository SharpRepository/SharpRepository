using System;
using System.Collections.Generic;

namespace SharpRepository.Repository.Transactions
{
    public interface IBatch<T> : IDisposable
    {
        IList<IBatchItem<T>> BatchActions { get; }
        void Add(T entity);
        void Add(IEnumerable<T> entities);
        void Update(T entity);
        void Update(IEnumerable<T> entities);
        void Delete(T entity);
        void Delete(IEnumerable<T> entities);
        void Commit();
        void Rollback();
    }
}