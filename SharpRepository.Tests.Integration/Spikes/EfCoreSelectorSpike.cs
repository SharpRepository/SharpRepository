
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
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
        private TestObjectContextCore dbContext;
        private TestObjectContextCore dbContext2;
        private Func<string, bool> filterSelects = q => q.StartsWith("Executing DbCommand") && q.Contains("SELECT") && !q.Contains("sqlite_master");

        [SetUp]
        public void SetupRepository()
        {
            var dbPath = EfDataDirectoryFactory.Build();

            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<TestObjectContextCore>()
                .UseSqlite(connection)
                .Options;
            
            // Create the schema in the database
            dbContext = new TestObjectContextCore(options);
            dbContext.Database.EnsureCreated();
            const int totalItems = 5;

            for (int i = 1; i <= totalItems; i++)
            {
                dbContext.Contacts.Add(
                    new Contact
                    {
                        ContactId = i.ToString(),
                        Name = "Test User " + i,
                        EmailAddresses = new List<EmailAddress> {
                            new EmailAddress {
                                ContactId = i.ToString(),
                                EmailAddressId = i,
                                Email = "omar.piani." + i.ToString() + "@email.com",
                                Label = "omar.piani." + i.ToString()
                            }
                        }
                    });
            }

            dbContext.SaveChanges();

            dbContext2 = new TestObjectContextCore(options); // there is some kind of cache of inserted objects in dbContext
        }
        
        [Test]
        public void EfCore_GetAll_With_Selector_Selects_Only_Specified_Columns()
        {
            var repository = new EfCoreRepository<EmailAddress, int>(dbContext);

            var emailAddress = repository.GetAll(s => new { s.ContactId, s.EmailAddressId, s.Email }).First();
            emailAddress.Email.ShouldStartWith("omar.piani.");
            dbContext.QueryLog.Count(filterSelects).ShouldBe(1, "A select was executed");
            string queryLogElement = dbContext.QueryLog.Where(filterSelects).First();
            Regex regex = new Regex("SELECT(.+)FROM.+", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            var selectMatches = regex.Matches(queryLogElement);
            selectMatches.Count.ShouldBe(1, "One regex match must be found for the select pattern");
            var selectPart = selectMatches[0].Groups[1].Value;
            selectPart.ShouldContain(nameof(EmailAddress.ContactId), "ContactId was selected");
            selectPart.ShouldContain(nameof(EmailAddress.EmailAddressId), "EmailAddressId was notselected");
            selectPart.ShouldContain(nameof(EmailAddress.Email), "Email was not selected");
            selectPart.ShouldNotContain(nameof(EmailAddress.Label), "Label was selected");
        }
        
        [Test]
        public void EfCore_Get_With_Selector_Selects_Only_Specified_Columns()
        {
            var repository = new EfCoreRepository<EmailAddress, int>(dbContext2);

            var emailAddress = repository.Find(m => m.Email == "omar.piani.1@email.com", s => new { s.ContactId, s.EmailAddressId, s.Email });
            emailAddress.Email.ShouldBe("omar.piani.1@email.com");
            dbContext2.QueryLog.Count(filterSelects).ShouldBe(1, "One SELECT must be executed");
            string queryLogElement = dbContext2.QueryLog.Where(filterSelects).First();
            Regex regex = new Regex("SELECT(.+)FROM.+", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            var selectMatches = regex.Matches(queryLogElement);
            selectMatches.Count.ShouldBe(1, "One regex match must be found for the select pattern");
            var selectPart = selectMatches[0].Groups[1].Value;
            selectPart.ShouldContain(nameof(EmailAddress.ContactId), "ContactId was selected");
            selectPart.ShouldContain(nameof(EmailAddress.EmailAddressId), "EmailAddressId was selected");
            selectPart.ShouldContain(nameof(EmailAddress.Email), "Email was selected");
            selectPart.ShouldNotContain(nameof(EmailAddress.Label), "Label was not selected");
        }
    }
}
