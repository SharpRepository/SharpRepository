using SharpRepository.Repository;

namespace SharpRepository.Tests.Integration.TestObjects
{
    public class User
    {
        [RepositoryPrimaryKey(Order = 1)]
        public string Username { get; set; }

        [RepositoryPrimaryKey(Order = 1)]
        public int Age { get; set; }

        public string FullName { get; set; }

        public int ContactTypeId { get; set; }
    }
}
