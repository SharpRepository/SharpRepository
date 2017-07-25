using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace SharpRepository.Tests.Integration.TestObjects
{
    [DbConfigurationType(typeof(TestConfiguration))]
    public class TestObjectEntities : DbContext
    {
        public TestObjectEntities(string connectionString) : base(connectionString)
        {
        }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<PhoneNumber> PhoneNumbers { get; set; }
        public DbSet<EmailAddress> EmailAddresses { get; set; }

        // set the Compound Key for the User object
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => new {u.Username, u.Age});
        }
    }

    public class TestConfiguration : DbConfiguration
    {
        public TestConfiguration()
        {
            SetDefaultConnectionFactory(new SqlCeConnectionFactory("System.Data.SqlServerCe.4.0"));
        }
    }
}
