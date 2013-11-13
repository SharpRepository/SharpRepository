using System;
using System.Linq.Expressions;

namespace SharpRepository.Repository.Aspects
{
    public class RepositoryGetContext<T, TKey> : RepositoryGetContext<T, TKey, T> where T : class
    {
        public RepositoryGetContext(IRepository<T, TKey> repository, TKey id) : base(repository, id)
        {
        }
    }

    public class RepositoryGetContext<T, TKey, TResult> : RepositoryActionContext<T, TKey> where T : class
    {
        public RepositoryGetContext(IRepository<T, TKey> repository, TKey id, Expression<Func<T, TResult>> selector = null)
            : base(repository)
        {
            Id = id;
            Selector = selector;
        }

        public TKey Id { get; set; }
        public TResult Result { get; set; }

        public int NumberOfResults
        {
            get { return Result.Equals(default(TResult)) ? 0 : 1; }
        }

        public Expression<Func<T, TResult>> Selector { get; set; }
    }
}