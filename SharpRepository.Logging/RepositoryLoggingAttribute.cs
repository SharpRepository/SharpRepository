using System;
using Common.Logging;
using SharpRepository.Repository;
using SharpRepository.Repository.Aspects;

namespace SharpRepository.Logging
{
    public class RepositoryLoggingAttribute : RepositoryActionBaseAttribute
    {
        private readonly ILog _logger;
        private LogLevel _logLevel = LogLevel.Debug;

        public RepositoryLoggingAttribute()
        {
            _logger = LogManager.GetLogger("SharpRepository");
        }

        public LogLevel LogLevel
        {
            get { return _logLevel; }
            set { _logLevel = value; }
        }

        private void Log(string message)
        {
            switch (_logLevel)
            {
                case LogLevel.Error:
                    _logger.Error(message);
                    break;
                case LogLevel.Info:
                    _logger.Info(message);
                    break;
                case LogLevel.Trace:
                    _logger.Trace(message);
                    break;
                default:
                    _logger.Debug(message);
                    break;
            }
        }

        public override void OnInitialized<T, TKey>(RepositoryActionContext<T, TKey> context)
        {
            Log(String.Format("Initialized IRepository<{0}, {1}>", typeof(T).Name, typeof(TKey).Name));
        }

        public override bool OnAddExecuting<T, TKey>(T entity, RepositoryActionContext<T, TKey> context)
        {
            Log(String.Format("Adding {0} entity", typeof(T).Name));
            Log(String.Format("   {0}", entity.ToString()));
            return true;
        }

        public override void OnAddExecuted<T, TKey>(T entity, RepositoryActionContext<T, TKey> context)
        {
            Log(String.Format("Added {0} entity", typeof(T).Name));
            Log(String.Format("   {0}", entity.ToString()));
        }

        public override bool OnUpdateExecuting<T, TKey>(T entity, RepositoryActionContext<T, TKey> context)
        {
            Log(String.Format("Updating {0} entity", typeof(T).Name));
            Log(String.Format("   {0}", entity.ToString()));

            return true;
        }

        public override void OnUpdateExecuted<T, TKey>(T entity, RepositoryActionContext<T, TKey> context)
        {
            Log(String.Format("Updated {0} entity", typeof(T).Name));
            Log(String.Format("   {0}", entity.ToString()));
        }

        public override bool OnDeleteExecuting<T, TKey>(T entity, RepositoryActionContext<T, TKey> context)
        {
            Log(String.Format("Deleting {0} entity", typeof(T).Name));
            Log(String.Format("   {0}", entity.ToString()));

            return true;
        }

        public override void OnDeleteExecuted<T, TKey>(T entity, RepositoryActionContext<T, TKey> context)
        {
            Log(String.Format("Deleted {0} entity", typeof(T).Name));
            Log(String.Format("   {0}", entity.ToString()));
        }

        public override bool OnSaveExecuting<T, TKey>(RepositoryActionContext<T, TKey> context)
        {
            Log(String.Format("Saving {0} entity", typeof(T).Name));

            return true;
        }

        public override void OnSaveExecuted<T, TKey>(RepositoryActionContext<T, TKey> context)
        {
            Log(String.Format("Saved {0} entity", typeof(T).Name));
        }

        public override void OnGetExecuting<T, TKey, TResult>(RepositoryGetContext<T, TKey, TResult> context)
        {
            var typeDisplay = RepositoryTypeDisplay(context.Repository);

            Log(String.Format("{0} Executing Get: Id = {1}", typeDisplay, context.Id));
        }

        public override void OnGetExecuted<T, TKey, TResult>(RepositoryGetContext<T, TKey, TResult> context)
        {
            var typeDisplay = RepositoryTypeDisplay(context.Repository);

            Log(String.Format("{0} Executed Get: Id = {1}", typeDisplay, context.Id));
            Log(context.Repository.TraceInfo);
            Log(String.Format("{0} Has Result: {1} Cache Used: {2}", typeDisplay, context.HasResult, context.Repository.CacheUsed));
        }

        public override void OnGetAllExecuting<T, TKey, TResult>(RepositoryQueryMultipleContext<T, TKey, TResult> context)
        {
            var typeDisplay = RepositoryTypeDisplay(context.Repository);

            Log(String.Format("{0} Executing GetAll", typeDisplay));
        }

        public override void OnGetAllExecuted<T, TKey, TResult>(RepositoryQueryMultipleContext<T, TKey, TResult> context)
        {
            var typeDisplay = RepositoryTypeDisplay(context.Repository);

            Log(String.Format("{0} Executed GetAll", typeDisplay));
            Log(context.Repository.TraceInfo);
            Log(String.Format("{0} Results: {1} Cache Used: {2}", typeDisplay, context.NumberOfResults, context.Repository.CacheUsed));
        }

        public override void OnFindExecuting<T, TKey, TResult>(RepositoryQuerySingleContext<T, TKey, TResult> context)
        {
            var typeDisplay = RepositoryTypeDisplay(context.Repository);

            Log(String.Format("{0} Executing Find: {1}", typeDisplay, context.Specification.Predicate));
        }

        public override void OnFindExecuted<T, TKey, TResult>(RepositoryQuerySingleContext<T, TKey, TResult> context)
        {
            var typeDisplay = RepositoryTypeDisplay(context.Repository);

            Log(String.Format("{0} Executed Find: {1}", typeDisplay, context.Specification.Predicate));
            Log(context.Repository.TraceInfo);
            Log(String.Format("{0} Results: {1} Cache Used: {2}", typeDisplay, context.NumberOfResults, context.Repository.CacheUsed));
        }

        public override void OnFindAllExecuting<T, TKey, TResult>(RepositoryQueryMultipleContext<T, TKey, TResult> context)
        {
            var typeDisplay = RepositoryTypeDisplay(context.Repository);

            Log(String.Format("{0} Executing FindAll: {1}", typeDisplay, context.Specification.Predicate));
        }

        public override void OnFindAllExecuted<T, TKey, TResult>(RepositoryQueryMultipleContext<T, TKey, TResult> context)
        {
            var typeDisplay = RepositoryTypeDisplay(context.Repository);

            Log(String.Format("{0} Executed FindAll: {1}", typeDisplay, context.Specification.Predicate));
            Log(context.Repository.TraceInfo);
            Log(String.Format("{0} Results: {1} Cache Used: {2}", typeDisplay, context.NumberOfResults, context.Repository.CacheUsed));
        }

        private static string RepositoryTypeDisplay<T, TKey>(IRepository<T, TKey> repository) where T : class
        {
            return String.Format("[{0}<{1},{2}>]", repository.GetType().Name, typeof(T).Name, typeof(TKey).Name);
        }
    }
}
