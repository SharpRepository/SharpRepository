using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;

namespace SharpRepository.Tests.Integration.TestObjects
{
    public class TestObjectContext : DbContext
    {
        public TestObjectContext(DbConnection connection) : base(connection, false)
        {
        }

        public TestObjectContext(string connectionString) : base(connectionString)
        {
        }

        public DbSet<ContactInt> ContactInts { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<PhoneNumber> PhoneNumbers { get; set; }
        public DbSet<EmailAddress> EmailAddresses { get; set; }
        public DbSet<Node> Nodes { get; set; }

        // set the Compound Key for the User object
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => new {u.Username, u.Age});

            modelBuilder.Entity<Node>()
                .HasKey(n => n.Id)
                .Property(n => n.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Node>()
                .HasOptional(n => n.Parent)
                .WithMany()
                .HasForeignKey(n => n.ParentId);
        }
    }

    public class TestConfiguration : DbConfiguration
    {
        public TestConfiguration()
        {
            SetDefaultConnectionFactory(new SqlConnectionFactory("Effort.EF6"));
        }
    }
}
