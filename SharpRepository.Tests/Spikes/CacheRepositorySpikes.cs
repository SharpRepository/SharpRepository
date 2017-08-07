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
using Shouldly;

namespace SharpRepository.Tests.Spikes
{
    [TestFixture]
    class CacheRepositorySpikes
    {
        private IRepository<TempTestObject, int> _repository;

        [SetUp]
        public void Initialize()
        {
            var sharpRepositoryConfiguration = new SharpRepositoryConfiguration();
            sharpRepositoryConfiguration.AddCachingStrategy(new StandardCachingStrategyConfiguration("standard"));
            sharpRepositoryConfiguration.AddCachingProvider(new InMemoryCachingProviderConfiguration("inmemory"));
            sharpRepositoryConfiguration.AddRepository(new CacheRepositoryConfiguration("textFilter", "TextFilter", "standard", "inmemory"));

            _repository = sharpRepositoryConfiguration.GetInstance<TempTestObject, int>("textFilter");
        }

        [TearDown]
        public void Cleanup()
        {
            _repository.Delete(x => true);
        }

        [Test]
        public void Add_Multiple_Should_Work()
        {
            var list = new List<TempTestObject>()
                       {
                           new TempTestObject() { Name = "Test 1"},
                           new TempTestObject() {Name = "Test 2"},
                           new TempTestObject() {Name = "Test 3"}
                       };
            _repository.Add(list);

            _repository.GetAll().Count().ShouldBe(3);
        }

        [Test]
        public void Add_Multiple_Should_Work2()
        {
            var list = new List<TempTestObject>()
                       {
                           new TempTestObject() {Name = "Test 4"},
                           new TempTestObject() {Name = "Test 5"}
                       };
            _repository.Add(list);

            _repository.GetAll().Count().ShouldBe(2);
        }
    }

    public class TempTestObject
    {
        [RepositoryPrimaryKey]
        public int TestTempKey { get; set; }
        public string Name { get; set; }
    }
}
