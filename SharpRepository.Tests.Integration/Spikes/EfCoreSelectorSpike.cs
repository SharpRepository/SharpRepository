using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using SharpRepository.EfCoreRepository;
using SharpRepository.Tests.Integration.Data;
using SharpRepository.Tests.Integration.TestObjects;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SharpRepository.Tests.Integration.Spikes
{
    [TestFixture]
    public class EfCoreSelectorSpike
    {
        public static IConfigurationRoot GetIConfigurationRoot(string outputPath)
        {
            return new ConfigurationBuilder()
                .SetBasePath(outputPath)
                .AddJsonFile("appsettings.json", optional: true)
                .AddUserSecrets("627a7ed1-b2c9-408a-a341-c01fc197a606")
                .Build();
        }

        private TestObjectContextCore dbContext;
        private Func<string, bool> filterSelects = q => q.StartsWith("Executing DbCommand") && q.Contains("SELECT") && !q.Contains("sqlite_master");


        [TearDown]
        public void TearDown()
        {
            dbContext.Database.EnsureDeleted();
        }

        [SetUp]
        public void SetupRepository()
        {
            var configurationRoot = GetIConfigurationRoot(TestContext.CurrentContext.TestDirectory);

            var connectionString = configurationRoot.GetConnectionString("EfCoreConnectionString");

            var options = new DbContextOptionsBuilder<TestObjectContextCore>()
                .UseLazyLoadingProxies()
                .UseSqlServer(connectionString)
                .Options;

            // Create the schema in the database
            dbContext = new TestObjectContextCore(options);
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
            const int totalItems = 5;

            for (int i = 1; i <= totalItems; i++)
            {
                dbContext.Contacts.Add(
                    new Contact
                    {
                        ContactId = i.ToString(),
                        Name = "Test User " + i,
                        EmailAddresses = new List<EmailAddress>()
                    });
            }
            
            dbContext.SaveChanges();

            foreach (var contact in dbContext.Contacts)
            {
                contact.EmailAddresses.Add(new EmailAddress
                {
                    Email = "test.addr." + contact.ContactId + "@email.com",
                    Label = "test.addr." + contact.ContactId
                });
            }

            dbContext.SaveChanges();

            dbContext = new TestObjectContextCore(options); // there is some kind of cache of inserted objects in dbContext
        }
        
        [Test]
        public void EfCore_GetAll_With_Selector_Selects_Only_Specified_Columns()
        {
            var repository = new EfCoreRepository<EmailAddress, int>(dbContext);

            var emailAddress = repository.GetAll(s => new { s.ContactId, s.EmailAddressId, s.Email }).First();
            emailAddress.Email.ShouldStartWith("test.addr.");
            dbContext.QueryLog.Count(filterSelects).ShouldBe(1, "A select was executed");
            string queryLogElement = dbContext.QueryLog.Where(filterSelects).First();
            Regex regex = new Regex("SELECT(.+)FROM.+", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            var selectMatches = regex.Matches(queryLogElement);
            selectMatches.Count.ShouldBe(1, "One regex match must be found for the select pattern");
            var selectPart = selectMatches[0].Groups[1].Value;
            selectPart.ShouldContain(nameof(EmailAddress.ContactId), customMessage: "ContactId was selected") ;
            selectPart.ShouldContain(nameof(EmailAddress.EmailAddressId), customMessage: "EmailAddressId was notselected");
            selectPart.ShouldContain(nameof(EmailAddress.Email), customMessage: "Email was not selected");
            selectPart.ShouldNotContain(nameof(EmailAddress.Label), customMessage: "Label was selected");
        }
        
        [Test]
        public void EfCore_Get_With_Selector_Selects_Only_Specified_Columns()
        {
            var repository = new EfCoreRepository<EmailAddress, int>(dbContext);

            var emailAddress = repository.Find(m => m.Email == "test.addr.1@email.com", s => new { s.ContactId, s.EmailAddressId, s.Email });
            emailAddress.Email.ShouldBe("test.addr.1@email.com");
            dbContext.QueryLog.Count(filterSelects).ShouldBe(1, "One SELECT must be executed");
            string queryLogElement = dbContext.QueryLog.Where(filterSelects).First();
            Regex regex = new Regex("SELECT(.+)FROM.+", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            var selectMatches = regex.Matches(queryLogElement);
            selectMatches.Count.ShouldBe(1, "One regex match must be found for the select pattern");
            var selectPart = selectMatches[0].Groups[1].Value;
            selectPart.ShouldContain(nameof(EmailAddress.ContactId), customMessage: "ContactId was selected");
            selectPart.ShouldContain(nameof(EmailAddress.EmailAddressId), customMessage: "EmailAddressId was selected");
            selectPart.ShouldContain(nameof(EmailAddress.Email), customMessage: "Email was selected");
            selectPart.ShouldNotContain(nameof(EmailAddress.Label), customMessage: "Label was not selected");
        }
    }
}
