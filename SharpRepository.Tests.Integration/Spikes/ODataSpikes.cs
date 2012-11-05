using System.Linq;
using NUnit.Framework;
using SharpRepository.ODataRepository;
using Should;

namespace SharpRepository.Tests.Integration.Spikes
{
    [TestFixture]
    public class ODataSpikes
    {
        [Test]
        public void NetflixTest()
        {
            var repository = new ODataRepository<Genre>("http://odata.netflix.com/v2/Catalog");
            var results = repository.GetAll();

            results.Count().ShouldEqual(326);
        }
    }

    public class Genre
    {
        public string Name { get; set; }
    }
}
