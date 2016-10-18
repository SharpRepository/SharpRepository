using System;
using System.Threading;
using SharpRepository.Repository.Aspects;

namespace SharpRepository.Tests.TestObjects
{
    internal class AuditAttributeMock : RepositoryActionBaseAttribute
    {
        public bool OnInitializedCalled { get; set; }
        public bool OnGetExecutingCalled { get; set; }
        public bool OnGetExecutedCalled { get; set; }

        public DateTime ExecutedOn { get; set; }

        public override void OnInitialized<T, TKey>(RepositoryActionContext<T, TKey> context)
        {
            OnInitializedCalled = true;
        }

        public override bool OnGetExecuting<T, TKey, TResult>(RepositoryGetContext<T, TKey, TResult> context)
        {
            OnGetExecutingCalled = true;
            return OnGetExecutingCalled;
        }

        public override void OnGetExecuted<T, TKey, TResult>(RepositoryGetContext<T, TKey, TResult> context)
        {
            OnGetExecutedCalled = true;
            ExecutedOn = DateTime.UtcNow;
        }
    }

    internal sealed class SpecificAuditAttribute : AuditAttributeMock
    {
        public override void OnGetExecuted<T, TKey, TResult>(RepositoryGetContext<T, TKey, TResult> context)
        {
            Thread.Sleep(50);
            base.OnGetExecuted(context);
        }
    }


}