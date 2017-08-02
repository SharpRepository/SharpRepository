using SharpRepository.Repository;

namespace SharpRepository.Tests.Integration.TestObjects
{
    // Ugly class for testing
    public class ContactType2
    {
        [RepositoryPrimaryKey(Order = 1)]
        public int ContactTypeId { get; set; }

        [RepositoryPrimaryKey(Order = 1)]
        public string Name { get; set; }

        public string Abbreviation { get; set; }
    }
}
    