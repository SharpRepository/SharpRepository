using System;
using SharpRepository.Repository.Aspects;
using NLog;

namespace SharpRepository.Logging.NLog
{
    public class NLogRepositoryLoggingAttribute : RepositoryActionBaseAttribute
    {
        private readonly Logger _logger;

        public NLogRepositoryLoggingAttribute()
        {
            _logger = LogManager.GetLogger("SharpRepository");
        }

        public override void OnInitialized<T, TKey>(RepositoryActionContext<T, TKey> context)
        {
            _logger.Debug(String.Format("Initialized IRepository<{0}, {1}>", typeof(T).Name, typeof(TKey).Name));
        }

        public override bool OnAddExecuting<T, TKey>(RepositoryActionContext<T, TKey> context)
        {
            _logger.Debug(String.Format("Adding {0} entity", typeof(T).Name));
            return true;
        }

        public override void OnAddExecuted<T, TKey>(RepositoryActionContext<T, TKey> context)
        {
            _logger.Debug(String.Format("Added {0} entity", typeof(T).Name));
        }

        public override bool OnUpdateExecuting<T, TKey>(RepositoryActionContext<T, TKey> context)
        {
            _logger.Debug(String.Format("Updating {0} entity", typeof(T).Name));

            return true;
        }

        public override void OnUpdateExecuted<T, TKey>(RepositoryActionContext<T, TKey> context)
        {
            _logger.Debug(String.Format("Updated {0} entity", typeof(T).Name));
        }

        public override bool OnDeleteExecuting<T, TKey>(RepositoryActionContext<T, TKey> context)
        {
            _logger.Debug(String.Format("Deleting {0} entity", typeof(T).Name));

            return true;
        }

        public override void OnDeleteExecuted<T, TKey>(RepositoryActionContext<T, TKey> context)
        {
            _logger.Debug(String.Format("Deleted {0} entity", typeof(T).Name));
        }

        public override bool OnSaveExecuting<T, TKey>(RepositoryActionContext<T, TKey> context)
        {
            _logger.Debug(String.Format("Saving {0} entity", typeof(T).Name));

            return true;
        }

        public override void OnSaveExecuted<T, TKey>(RepositoryActionContext<T, TKey> context)
        {
            _logger.Debug(String.Format("Saved {0} entity", typeof(T).Name));
        }
    }
}
