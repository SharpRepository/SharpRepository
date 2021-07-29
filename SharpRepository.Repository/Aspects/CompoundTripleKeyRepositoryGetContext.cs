using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SharpRepository.Repository.Aspects
{
    public class CompoundTripleKeyRepositoryGetContext<T, TKey, TKey2, TKey3> : CompoundTripleKeyRepositoryGetContext<T, TKey, TKey2, TKey3, T> where T : class
    {
        public CompoundTripleKeyRepositoryGetContext(ICompoundKeyRepository<T, TKey, TKey2, TKey3> repository, TKey id, TKey2 id2, TKey3 id3) : base(repository, id, id2, id3)
        {
        }
    }

    public class CompoundTripleKeyRepositoryGetContext<T, TKey, TKey2, TKey3, TResult> : RepositoryActionContext<T, TKey, TKey2, TKey3> where T : class
    {
        public CompoundTripleKeyRepositoryGetContext(ICompoundKeyRepository<T, TKey, TKey2, TKey3> repository, TKey id, TKey2 id2, TKey3 id3, Expression<Func<T, TResult>> selector = null)
            : base(repository)
        {
            Id = id;
            Id2 = id2;
            Id3 = id3;
            Selector = selector;
        }

        public TKey Id { get; set; }
        public TKey2 Id2 { get; set; }
        public TKey3 Id3 { get; set; }
        public TResult Result { get; set; }

        public bool HasResult
        {
            get { return Result != null && !Result.Equals(default(TResult)); }
        }

        public Expression<Func<T, TResult>> Selector { get; set; }
    }
}