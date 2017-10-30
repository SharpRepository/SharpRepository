using Microsoft.EntityFrameworkCore;
using SharpRepository.CoreMvc.Models;

namespace SharpRepository.CoreMvc
{
    public class ContactContext : DbContext
    {
        public ContactContext(DbContextOptions<ContactContext> options) : base(options)
        {
        }

        public DbSet<Contact> Contacts { get; set; }
    }
}