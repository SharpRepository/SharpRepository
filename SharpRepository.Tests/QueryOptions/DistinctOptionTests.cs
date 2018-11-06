using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SharpRepository.Repository.Queries;
using SharpRepository.Tests.TestObjects;
using Shouldly;

namespace SharpRepository.Tests.QueryOptions
{
    [TestFixture]
    public class DistinctOptionTests : TestBase
    {
        [Test]
        public void DistinctOption_Will_Hide_Duplicates()
        {
            var contacts = new List<Contact>();
            for (var i = 5; i >= 1; i--)
            {
                contacts.Add(new Contact { Name = "Test User " + i });
                contacts.Add(new Contact { Name = "Test User " + i });
            }

            var qo = new DistinctOption<Contact>();
            var queryable = qo.Apply(contacts.AsQueryable());
            queryable.Count().ShouldBe(5);
            queryable.First().Name.ShouldBe("Test User 5");
        }
    }
}