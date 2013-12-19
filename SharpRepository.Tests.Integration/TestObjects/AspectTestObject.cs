using System;
using SharpRepository.Repository.Aspects;

namespace SharpRepository.Tests.Integration.TestObjects
{
    public class AuditAttribute : RepositoryActionBaseAttribute
    {
        public override bool OnAddExecuting<T, TKey>(T entity, RepositoryActionContext<T, TKey> context)
        {
            var tmp = entity as IAuditable;

            if (tmp != null)
            {
                tmp.Created = DateTime.UtcNow;
            }

            return true;
        }

        public override bool OnUpdateExecuting<T, TKey>(T entity, RepositoryActionContext<T, TKey> context)
        {
            var tmp = entity as IAuditable;

            if (tmp != null)
            {
                tmp.Modified = DateTime.UtcNow;
            }

            return true;
        }
    }

    [Audit]
    public class AspectTestObject : IAuditable
    {
        public int AspectTestObjectId { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
    }

    public interface IAuditable
    {
        DateTime Created { get; set; }
        DateTime? Modified { get; set; }
    }
}
