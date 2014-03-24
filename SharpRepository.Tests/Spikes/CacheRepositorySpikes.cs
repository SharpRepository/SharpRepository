using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SharpRepository.CacheRepository;
using SharpRepository.Repository;
using SharpRepository.Repository.Caching;
using SharpRepository.Repository.Configuration;
using SharpRepository.Tests.TestObjects;
using Should;

namespace SharpRepository.Tests.Spikes
{
    [TestFixture]
    class CacheRepositorySpikes
    {
        private IRepository<Contact, int> _repository;

        [SetUp]
        public void Initialize()
        {
            var sharpRepositoryConfiguration = new SharpRepositoryConfiguration();
            sharpRepositoryConfiguration.AddCachingStrategy(new StandardCachingStrategyConfiguration("standard"));
            sharpRepositoryConfiguration.AddCachingProvider(new InMemoryCachingProviderConfiguration("inmemory"));
            sharpRepositoryConfiguration.AddRepository(new CacheRepositoryConfiguration("textFilter", "TextFilter", "standard", "inmemory"));

            _repository = sharpRepositoryConfiguration.GetInstance<Contact, int>("textFilter");
        }

        [TearDown]
        public void Cleanup()
        {
            _repository.Delete(x => true);
        }

        [Test]
        public void Add_Multiple_Should_Work()
        {
            var list = new List<Contact>()
                       {
                           new Contact() {ContactTypeId = 1, Name = "Test 1"},
                           new Contact() {ContactTypeId = 1, Name = "Test 2"},
                           new Contact() {ContactTypeId = 1, Name = "Test 3"}
                       };
            _repository.Add(list);

            _repository.GetAll().Count().ShouldEqual(3);
        }
    }
}
