using System.Linq;
using NUnit.Framework;
using SharpRepository.Repository.FetchStrategies;
using SharpRepository.Tests.TestObjects;
using Shouldly;

namespace SharpRepository.Tests.FetchStrategies
{
    [TestFixture]
    public class FetchStrategyTests : TestBase
    {
        [Test]
        public void FetchStrategy_May_Include_Multiple_References()
        {
            var strategy = new GenericFetchStrategy<Contact>()
                .Include(p => p.EmailAddresses)
                .Include(p => p.PhoneNumbers);

            strategy.IncludePaths.ShouldContain("EmailAddresses");
            strategy.IncludePaths.ShouldContain("PhoneNumbers");
            strategy.IncludePaths.Count().ShouldBe(2);
        }

        [Test]
        public void FetchStrategy_May_Include_String_Property_Names()
        {
            // This is a non-sense example because the Email property is not another table, but it works the exact same
            var strategy = new GenericFetchStrategy<Contact>()
                .Include("EmailAddresses")
                .Include("PhoneNumbers");

            strategy.IncludePaths.ShouldContain("EmailAddresses");
            strategy.IncludePaths.ShouldContain("PhoneNumbers");
            strategy.IncludePaths.Count().ShouldBe(2);
        }

        [Test]
        public void FetchStrategy_May_Include_Multiple_Levels()
        {
            // This is a non-sense example because the Email property is not another table, but it works the exact same
            var strategy = new GenericFetchStrategy<Contact>()
                .Include(p => p.EmailAddresses.Select(e => e.Email));

            strategy.IncludePaths.ShouldContain("EmailAddresses.Email");
        }

        [Test]
        public void FetchStrategy_May_Include_Multiple_Levels_First_Syntax()
        {
            // This is a non-sense example because the Email property is not another table, but it works the exact same
            var strategy = new GenericFetchStrategy<Contact>()
                .Include(p => p.EmailAddresses.First().Email);

            strategy.IncludePaths.ShouldContain("EmailAddresses.Email");
        }
    }
}