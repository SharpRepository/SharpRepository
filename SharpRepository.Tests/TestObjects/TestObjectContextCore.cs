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
        public DbSet<TripleCompoundKeyItemInts> TripleCompoundKeyItems { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TripleCompoundKeyItemInts>()
                .HasKey(c => new { c.SomeId, c.AnotherId, c.LastId });
        }
    }
}
