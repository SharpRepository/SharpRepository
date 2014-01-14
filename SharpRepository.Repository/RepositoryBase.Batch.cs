using System;
using System.Collections.Generic;
using SharpRepository.Repository.Transactions;

namespace SharpRepository.Repository
{
    public abstract partial class RepositoryBase<T, TKey> 
    {
        private sealed class Batch : IBatch<T> 
        {
            private readonly RepositoryBase<T, TKey> _repository;
            private readonly IList<IBatchItem<T>> _items = new List<IBatchItem<T>>();

            public Batch(RepositoryBase<T, TKey> repository)
            {
                _repository = repository;
            }

            // TODO: the only reason this is public is for the unit tests, this doesn't feel right because we don't want people manipulating this themselves, they should only use the Add, Update and Delete methods
            public IList<IBatchItem<T>> BatchActions
            {
                get { return _items; }
            }

            public void Add(T entity)
            {
                if (entity == null) throw new ArgumentNullException("entity");

                _items.Add(new BatchItem { Action = BatchAction.Add, Item = entity});
            }

            public void Add(IEnumerable<T> entities)
            {
                if (entities == null) throw new ArgumentNullException("entities");

                foreach (var entity in entities)
                {
                    Add(entity);
                }
            }

            public void Update(T entity)
            {
                if (entity == null) throw new ArgumentNullException("entity");

                _items.Add(new BatchItem { Action = BatchAction.Update, Item = entity });
            }

            public void Update(IEnumerable<T> entities)
            {
                if (entities == null) throw new ArgumentNullException("entities");

                foreach (var entity in entities)
                {
                    Update(entity);
                }
            }

            public void Delete(T entity)
            {
                if (entity == null) throw new ArgumentNullException("entity");

                _items.Add(new BatchItem { Action = BatchAction.Delete, Item = entity });
            }

            public void Delete(IEnumerable<T> entities)
            {
                if (entities == null) throw new ArgumentNullException("entities");

                foreach (var entity in entities)
                {
                    Delete(entity);
                }
            }

            public void Commit()
            {
				_repository.BatchMode = true;
				
				foreach (var batchItem in _items)
                {
                    switch (batchItem.Action)
                    {
                        case BatchAction.Add:
                            _repository.Add(batchItem.Item);
                            break;

                        case BatchAction.Update:
                            _repository.Update(batchItem.Item);
							break;

                        case BatchAction.Delete:
                            _repository.Delete(batchItem.Item);
                            break;
                    }
                }

                _repository.Save();

				// call QueryManager.OnItem{action} for each item in the batch only after saving the whole batch
				foreach (var batchItem in _items)
	            {
		            switch (batchItem.Action)
		            {
						case BatchAction.Add:
							_repository.NotifyQueryManagerOfAddedEntity(batchItem.Item);
							break;

						case BatchAction.Update:
							_repository.NotifyQueryManagerOfUpdatedEntity(batchItem.Item);
							break;

						case BatchAction.Delete:
							_repository.NotifyQueryManagerOfDeletedEntity(batchItem.Item);
							break;
		            }
					
	            }
                _repository.BatchMode = false;
                _items.Clear();
            }

            public void Rollback()
            {
                _repository.BatchMode = false;
                _items.Clear();
            }

            private bool _disposed;

            private void Dispose(bool disposing)
            {
                if (!_disposed)
                {
                    if (disposing)
                    {
                        _repository.BatchMode = false;
                        _items.Clear();
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