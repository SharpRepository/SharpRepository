using SharpRepository.Repository;

namespace SharpRepository.Tests.TestObjects
{
    public class CompoundKeyItemInts
    {
        [RepositoryPrimaryKey]
        public int SomeId { get; set; }

        [RepositoryPrimaryKey]
        public int AnotherId { get; set; }

        public string Title { get; set; }
    }
}
