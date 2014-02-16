using SharpRepository.Repository;

namespace SharpRepository.Tests.Integration.TestObjects
{
    public class Node
    {
        [RepositoryPrimaryKey(Order = 1)]
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public virtual Node Parent { get; private set; }
    }
}