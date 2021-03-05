using Microsoft.EntityFrameworkCore;
using SharpRepository.Samples.net5Mvc.Models;

namespace SharpRepository.Samples.net5Mvc
{
    public class ContactContext : DbContext
    {
        public ContactContext(DbContextOptions<ContactContext> options) : base(options)
        {
        }

        public DbSet<Contact> Contacts { get; set; }
    }
}