using System.Data.Entity;

namespace SharpRepository.Samples.MVC5.Models
{
    public class ContactsDbContext : DbContext
    {
        public ContactsDbContext(string connectionString) : base(connectionString)
        {
            var cs = connectionString;
        }

        public virtual DbSet<Contact> Contacts { get; set; }
    }
}