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

        public override void OnInitializedBase<T>()
        {
            OnInitializedCalled = true;
        }

        public override bool OnGetExecutingBase<T, TResult>()
        {
            OnGetExecutingCalled = true;
            return OnGetExecutingCalled;
        }

        public override void OnGetExecutedBase<T, TResult>()
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

        public override void OnGetExecuted<T, TKey, TKey2, TResult>(CompoundKeyRepositoryGetContext<T, TKey, TKey2, TResult> context)
        {
            Thread.Sleep(50);
            base.OnGetExecuted(context);
        }

        public override void OnGetExecuted<T, TKey, TKey2, TKey3, TResult>(CompoundTripleKeyRepositoryGetContext<T, TKey, TKey2, TKey3, TResult> context)
        {
            Thread.Sleep(50);
            base.OnGetExecuted(context);
        }

        public override void OnGetExecuted<T, TResult>(CompoundKeyRepositoryGetContext<T, TResult> context)
        {
            Thread.Sleep(50);
            base.OnGetExecuted(context);
        }
    }
}