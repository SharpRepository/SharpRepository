using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SharpRepository.Repository.Aspects
{
    public class RepositoryGetContext<T, TKey> : RepositoryGetContext<T, TKey, T> where T : class
    {
        public RepositoryGetContext(IRepository<T, TKey> repository, TKey id) : base(repository, id)
        {
        }
    }
    public class RepositoryGetContext<T, TKey, TKey2> : RepositoryGetContext<T, TKey, TKey2, T> where T : class
    {
        public RepositoryGetContext(ICompoundKeyRepository<T, TKey, TKey2> repository, TKey id) : base(repository, id)
        {
        }
    }
    public class RepositoryGetContext<T, TKey, TKey2, TKey3> : RepositoryGetContext<T, TKey, TKey2, TKey3, T> where T : class
    {
        public RepositoryGetContext(ICompoundKeyRepository<T, TKey, TKey2, TKey3> repository, TKey id, TKey2 id2, TKey3 id3) : base(repository, id, id2, id3)
        {
        }
    }
    public class RepositoryGetContext<T> : RepositoryGetContext<T, T> where T : class
    {
        public RepositoryGetContext(IRepository<T> repository, IEnumerable<object> ids) : base(repository, ids)
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

        public bool HasResult
        {
            get { return Result != null && !Result.Equals(default(TResult)); }
        }

        public Expression<Func<T, TResult>> Selector { get; set; }
    }

    public class RepositoryGetContext<T, TKey, TKey2, TResult> : RepositoryActionContext<T, TKey, TKey2> where T : class
    {
        public RepositoryGetContext(IRepository<T, TKey> repository, TKey id, TKey2 id2, Expression<Func<T, TResult>> selector = null)
            : base(repository)
        {
            Id = id;
            Selector = selector;
        }

        public TKey Id { get; set; }
        public TResult Result { get; set; }

        public bool HasResult
        {
            get { return Result != null && !Result.Equals(default(TResult)); }
        }

        public Expression<Func<T, TResult>> Selector { get; set; }
    }
}