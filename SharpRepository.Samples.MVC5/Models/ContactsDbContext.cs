using System.Data.Entity;

namespace SharpRepository.Samples.MVC5.Models
{
    public class ContactsDbContext : DbContext
    {
        public ContactsDbContext()
            : base("name=ContactsDbContext")
        {
        }
        
        public virtual DbSet<Contact> Contacts { get; set; }
    }
}