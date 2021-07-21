using SharpRepository.Repository;

namespace SharpRepository.Tests.TestObjects
{
    [AuditAttributeMock(Order = 1)]
    [SpecificAudit(Order = 2)]
    public class CompoundKeyItemInts
    {
        [RepositoryPrimaryKey(Order = 1)]
        public int SomeId { get; set; }

        [RepositoryPrimaryKey(Order = 2)]
        public int AnotherId { get; set; }

        public string Title { get; set; }
    }
}
