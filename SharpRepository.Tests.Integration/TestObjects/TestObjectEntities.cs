using System;
using System.Data.Entity;
using System.Diagnostics;

namespace SharpRepository.Tests.Integration.TestObjects
{
    public class TestObjectEntities : DbContext
    {
        public TestObjectEntities(string connectionString) : base(connectionString)
        {
            Database.Log = sql => Debug.WriteLine(sql);
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
}
