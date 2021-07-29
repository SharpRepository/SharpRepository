using Microsoft.EntityFrameworkCore;

namespace SharpRepository.Tests.TestObjects
{
    public class TestObjectContextCore : DbContext
    {
        public TestObjectContextCore()
        { }

        public TestObjectContextCore(DbContextOptions<TestObjectContextCore> options)
        : base(options)
        { }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<PhoneNumber> PhoneNumbers { get; set; }
        public DbSet<EmailAddress> EmailAddresses { get; set; }
        public DbSet<CompoundTripleKeyItemInts> TripleCompoundKeyItems { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CompoundTripleKeyItemInts>()
                .HasKey(c => new { c.SomeId, c.AnotherId, c.LastId });
        }
    }
}
