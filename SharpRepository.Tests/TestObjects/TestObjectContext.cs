using System.Data.Entity;

namespace SharpRepository.Tests.TestObjects
{
    public class TestObjectEntities : DbContext
    {
        public TestObjectEntities(string connectionString) : base(connectionString)
        {

        }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<PhoneNumber> PhoneNumbers { get; set; }
        public DbSet<EmailAddress> EmailAddresses { get; set; }
        public DbSet<TripleCompoundKeyItemInts> TripleCompoundKeyItems { get; set; }
    }
}