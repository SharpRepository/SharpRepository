using Microsoft.EntityFrameworkCore;
using SharpRepository.Tests.Integration.TestObjects;

public class TestObjectContext : DbContext
{
    public TestObjectContext()
    { }

    public TestObjectContext(DbContextOptions<TestObjectContext> options)
        : base(options)
    { }

    public DbSet<Contact> Contacts { get; set; }
    public DbSet<PhoneNumber> PhoneNumbers { get; set; }
    public DbSet<EmailAddress> EmailAddresses { get; set; }

    // set the Compound Key for the User object
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasKey(u => new { u.Username, u.Age });
    }
}