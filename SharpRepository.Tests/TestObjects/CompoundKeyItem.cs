using SharpRepository.Repository;

namespace SharpRepository.Tests.TestObjects
{
    public class CompoundKeyItemInts
    {
        [RepositoryPrimaryKey(Order = 1)]
        public int SomeId { get; set; }

        [RepositoryPrimaryKey(Order = 2)]
        public int AnotherId { get; set; }

        public string Title { get; set; }
    }
}
