using System;
using Common.Logging;
using SharpRepository.Repository;
using SharpRepository.Repository.Aspects;

namespace SharpRepository.Logging
{
    public class RepositoryLoggingAttribute : RepositoryActionBaseAttribute
    {
        private readonly ILog _logger;

        public RepositoryLoggingAttribute()
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
            _logger.Debug(String.Format("   {0}", entity.ToString()));
            return true;
        }

        public override void OnAddExecuted<T, TKey>(T entity, RepositoryActionContext<T, TKey> context)
        {
            _logger.Debug(String.Format("Added {0} entity", typeof(T).Name));
            _logger.Debug(String.Format("   {0}", entity.ToString()));
        }

        public override bool OnUpdateExecuting<T, TKey>(T entity, RepositoryActionContext<T, TKey> context)
        {
            _logger.Debug(String.Format("Updating {0} entity", typeof(T).Name));
            _logger.Debug(String.Format("   {0}", entity.ToString()));

            return true;
        }

        public override void OnUpdateExecuted<T, TKey>(T entity, RepositoryActionContext<T, TKey> context)
        {
            _logger.Debug(String.Format("Updated {0} entity", typeof(T).Name));
            _logger.Debug(String.Format("   {0}", entity.ToString()));
        }

        public override bool OnDeleteExecuting<T, TKey>(T entity, RepositoryActionContext<T, TKey> context)
        {
            _logger.Debug(String.Format("Deleting {0} entity", typeof(T).Name));
            _logger.Debug(String.Format("   {0}", entity.ToString()));

            return true;
        }

        public override void OnDeleteExecuted<T, TKey>(T entity, RepositoryActionContext<T, TKey> context)
        {
            _logger.Debug(String.Format("Deleted {0} entity", typeof(T).Name));
            _logger.Debug(String.Format("   {0}", entity.ToString()));
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

        public override void OnGetExecuting<T, TKey, TResult>(RepositoryGetContext<T, TKey, TResult> context)
        {
            var typeDisplay = RepositoryTypeDisplay(context.Repository);

            _logger.Debug(String.Format("{0} Executing Get: Id = {1}", typeDisplay, context.Id));
        }

        public override void OnGetExecuted<T, TKey, TResult>(RepositoryGetContext<T, TKey, TResult> context)
        {
            var typeDisplay = RepositoryTypeDisplay(context.Repository);

            _logger.Debug(String.Format("{0} Executed Get: Id = {1}", typeDisplay, context.Id));
            _logger.Debug(context.Repository.TraceInfo);
            _logger.Debug(String.Format("{0} Has Result: {1} Cache Used: {2}", typeDisplay, context.HasResult, context.Repository.CacheUsed));
        }

        public override void OnGetAllExecuting<T, TKey, TResult>(RepositoryQueryMultipleContext<T, TKey, TResult> context)
        {
            var typeDisplay = RepositoryTypeDisplay(context.Repository);

            _logger.Debug(String.Format("{0} Executing GetAll", typeDisplay));
        }

        public override void OnGetAllExecuted<T, TKey, TResult>(RepositoryQueryMultipleContext<T, TKey, TResult> context)
        {
            var typeDisplay = RepositoryTypeDisplay(context.Repository);

            _logger.Debug(String.Format("{0} Executed GetAll", typeDisplay));
            _logger.Debug(context.Repository.TraceInfo);
            _logger.Debug(String.Format("{0} Results: {1} Cache Used: {2}", typeDisplay, context.NumberOfResults, context.Repository.CacheUsed));
        }

        public override void OnFindExecuting<T, TKey, TResult>(RepositoryQuerySingleContext<T, TKey, TResult> context)
        {
            var typeDisplay = RepositoryTypeDisplay(context.Repository);

            _logger.Debug(String.Format("{0} Executing Find: {1}", typeDisplay, context.Specification.Predicate));
        }

        public override void OnFindExecuted<T, TKey, TResult>(RepositoryQuerySingleContext<T, TKey, TResult> context)
        {
            var typeDisplay = RepositoryTypeDisplay(context.Repository);

            _logger.Debug(String.Format("{0} Executed Find: {1}", typeDisplay, context.Specification.Predicate));
            _logger.Debug(context.Repository.TraceInfo);
            _logger.Debug(String.Format("{0} Results: {1} Cache Used: {2}", typeDisplay, context.NumberOfResults, context.Repository.CacheUsed));
        }

        public override void OnFindAllExecuting<T, TKey, TResult>(RepositoryQueryMultipleContext<T, TKey, TResult> context)
        {
            var typeDisplay = RepositoryTypeDisplay(context.Repository);

            _logger.Debug(String.Format("{0} Executing FindAll: {1}", typeDisplay, context.Specification.Predicate));
        }

        public override void OnFindAllExecuted<T, TKey, TResult>(RepositoryQueryMultipleContext<T, TKey, TResult> context)
        {
            var typeDisplay = RepositoryTypeDisplay(context.Repository);

            _logger.Debug(String.Format("{0} Executed FindAll: {1}", typeDisplay, context.Specification.Predicate));
            _logger.Debug(context.Repository.TraceInfo);
            _logger.Debug(String.Format("{0} Results: {1} Cache Used: {2}", typeDisplay, context.NumberOfResults, context.Repository.CacheUsed));
        }

        private static string RepositoryTypeDisplay<T, TKey>(IRepository<T, TKey> repository) where T : class
        {
            return String.Format("[{0}<{1},{2}>]", repository.GetType().Name, typeof(T).Name, typeof(TKey).Name);
        }
    }
}
