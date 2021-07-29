using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SharpRepository.Repository.Aspects
{
    public class CompoundKeyRepositoryGetContext<T, TKey, TKey2> : CompoundKeyRepositoryGetContext<T, TKey, TKey2, T> where T : class
    {
        public CompoundKeyRepositoryGetContext(ICompoundKeyRepository<T, TKey, TKey2> repository, TKey id, TKey2 id2) : base(repository, id, id2)
        {
        }
    }

    public class CompoundKeyRepositoryGetContext<T> : CompoundKeyRepositoryGetContext<T, T> where T : class
    {
        public CompoundKeyRepositoryGetContext(ICompoundKeyRepository<T> repository, object[] ids) : base(repository, ids)
        {
        }
    }

    public class CompoundKeyRepositoryGetContext<T, TKey, TKey2, TResult> : RepositoryActionContext<T, TKey, TKey2> where T : class
    {
        public CompoundKeyRepositoryGetContext(ICompoundKeyRepository<T, TKey, TKey2> repository, TKey id, TKey2 id2, Expression<Func<T, TResult>> selector = null)
            : base(repository)
        {
            Id = id;
            Id2 = id2;
            Selector = selector;
        }

        public TKey Id { get; set; }
        public TKey2 Id2 { get; set; }
        public TResult Result { get; set; }

        public bool HasResult
        {
            get { return Result != null && !Result.Equals(default(TResult)); }
        }

        public Expression<Func<T, TResult>> Selector { get; set; }
    }

    public class CompoundKeyRepositoryGetContext<T, TResult> : RepositoryActionContext<T> where T : class
    {
        public CompoundKeyRepositoryGetContext(ICompoundKeyRepository<T> repository, object[] ids, Expression<Func<T, TResult>> selector = null)
            : base(repository)
        {
            Ids = ids;
            Selector = selector;
        }

        public object[] Ids { get; set; }
        public TResult Result { get; set; }

        public bool HasResult
        {
            get { return Result != null && !Result.Equals(default(TResult)); }
        }

        public Expression<Func<T, TResult>> Selector { get; set; }
    }
}