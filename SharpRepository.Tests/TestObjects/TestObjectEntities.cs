using Microsoft.EntityFrameworkCore;

namespace SharpRepository.Tests.TestObjects
{
    public class TestObjectContext : DbContext
    {
        public TestObjectContext()
        { }

        public TestObjectContext(DbContextOptions<TestObjectContext> options)
        : base(options)
        { }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<PhoneNumber> PhoneNumbers { get; set; }
        public DbSet<EmailAddress> EmailAddresses { get; set; }
        public DbSet<TripleCompoundKeyItemInts> TripleCompoundKeyItems { get; set; }
    }
}
