using Microsoft.EntityFrameworkCore;
using SharpRepository.Samples.Core3Mvc.Models;

namespace SharpRepository.Samples.Core3Mvc
{
    public class ContactContext : DbContext
    {
        public ContactContext(DbContextOptions<ContactContext> options) : base(options)
        {
        }

        public DbSet<Contact> Contacts { get; set; }
    }
}