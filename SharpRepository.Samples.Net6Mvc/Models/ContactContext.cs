using Microsoft.EntityFrameworkCore;

namespace SharpRepository.Samples.Net6Mvc.Models
{
    public class ContactContext : DbContext
    {
        public ContactContext(DbContextOptions<ContactContext> options) : base(options)
        {
        }

        public DbSet<Contact> Contacts { get; set; }
    }
}