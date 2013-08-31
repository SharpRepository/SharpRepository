using System;
using SharpRepository.Repository;
using SharpRepository.Repository.Aspects;
using NLog;
using SharpRepository.Repository.Specifications;

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

        public override bool OnAddExecuting<T, TKey>(T entity, RepositoryActionContext<T, TKey> context)
        {
            _logger.Debug(String.Format("Adding {0} entity", typeof(T).Name));
            return true;
        }

        public override void OnAddExecuted<T, TKey>(T entity, RepositoryActionContext<T, TKey> context)
        {
            _logger.Debug(String.Format("Added {0} entity", typeof(T).Name));
        }

        public override bool OnUpdateExecuting<T, TKey>(T entity, RepositoryActionContext<T, TKey> context)
        {
            _logger.Debug(String.Format("Updating {0} entity", typeof(T).Name));

            return true;
        }

        public override void OnUpdateExecuted<T, TKey>(T entity, RepositoryActionContext<T, TKey> context)
        {
            _logger.Debug(String.Format("Updated {0} entity", typeof(T).Name));
        }

        public override bool OnDeleteExecuting<T, TKey>(T entity, RepositoryActionContext<T, TKey> context)
        {
            _logger.Debug(String.Format("Deleting {0} entity", typeof(T).Name));

            return true;
        }

        public override void OnDeleteExecuted<T, TKey>(T entity, RepositoryActionContext<T, TKey> context)
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

        public override void OnFindAllExecuting<T, TKey>(RepositoryQueryContext<T, TKey> context)
        {
            var typeDisplay = RepositoryTypeDisplay(context.Repository);

            _logger.Debug(String.Format("{0} Executing FindAll: {1}", typeDisplay, context.Specification.Predicate));
        }

        public override void OnFindAllExecuted<T, TKey>(RepositoryQueryContext<T, TKey> context)
        {
            var typeDisplay = RepositoryTypeDisplay(context.Repository);

            _logger.Debug(String.Format("{0} Executed FindAll: {1}", typeDisplay, context.Specification.Predicate));
            _logger.Debug(String.Format("{0} Results: {1} Cache Used: {2}", typeDisplay, context.NumberOfResults, context.Repository.CacheUsed));
        }

        private static string RepositoryTypeDisplay<T, TKey>(IRepository<T, TKey> repository) where T : class
        {
            return String.Format("[{0}<{1},{2}>]", repository.GetType().Name, typeof (T).Name, typeof (TKey).Name);
        }
    }
}
