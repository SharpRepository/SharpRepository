using System.Linq;
using NUnit.Framework;
using SharpRepository.Repository.FetchStrategies;
using SharpRepository.Tests.TestObjects;
using Should;

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
            strategy.IncludePaths.Count().ShouldEqual(2);
        }
    }
}