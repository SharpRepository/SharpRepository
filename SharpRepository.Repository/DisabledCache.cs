using System;

namespace SharpRepository.Repository
{
    public abstract partial class RepositoryBase<T, TKey>
    {
        private sealed class DisabledCache : IDisabledCache
        {
            private readonly RepositoryBase<T, TKey> _repository;

            public DisabledCache(RepositoryBase<T, TKey> repository)
            {
                _repository = repository;
                _repository.CachingEnabled = false;
            }

            private bool _disposed;

            private void Dispose(bool disposing)
            {
                if (!_disposed)
                {
                    if (disposing)
                    {
                        _repository.CachingEnabled = true;
                    }
                }
                _disposed = true;
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }
    }
}
