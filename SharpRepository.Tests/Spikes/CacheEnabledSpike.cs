﻿using System;
using NUnit.Framework;
using SharpRepository.InMemoryRepository;
using SharpRepository.Repository.Caching;
using SharpRepository.Tests.TestObjects;
using Should;

namespace SharpRepository.Tests.Spikes
{
	[TestFixture]
	public class CacheEnabledSpike : TestBase
	{
		[Test]
		public void CachingEnabled_Should_Be_False_With_NoCachingStrategy()
		{
			var repository = new InMemoryRepository<Contact, Int32>(new NoCachingStrategy<Contact, int>());
			repository.CachingEnabled.ShouldBeFalse();
		}

		[Test]
		public void CachingEnabled_Should_Be_True_With_TimeoutCachingStrategy()
		{
			var repository = new InMemoryRepository<Contact, Int32>(new TimeoutCachingStrategy<Contact, int>(60));
			repository.CachingEnabled.ShouldBeTrue();
		}

		[Test]
		public void CachingEnabled_Should_Be_True_With_StandardCachingStrategy()
		{
			var repository = new InMemoryRepository<Contact, Int32>(new StandardCachingStrategy<Contact, int>());
			repository.CachingEnabled.ShouldBeTrue();
		}

		[Test]
		public void CachingEnabled_Should_Be_True_When_CachingStrategy_Is_Changed_From_NoCachingStrategy()
		{
			var repository = new InMemoryRepository<Contact, Int32>(new NoCachingStrategy<Contact, int>());
			repository.CachingEnabled.ShouldBeFalse();
			repository.CachingStrategy = new TimeoutCachingStrategy<Contact, int>(60);
			repository.CachingEnabled.ShouldBeTrue();
		}

		[Test]
		public void CachingEnabled_Should_Be_False_When_CachingStrategy_Is_Changed_To_NoCachingStrategy()
		{
			var repository = new InMemoryRepository<Contact, Int32>(new TimeoutCachingStrategy<Contact, int>(60));
			repository.CachingEnabled.ShouldBeTrue();
			repository.CachingStrategy = new NoCachingStrategy<Contact, int>();
			repository.CachingEnabled.ShouldBeFalse();
		}
	}
}
