using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SharpRepository.Repository.Queries;
using SharpRepository.Tests.TestObjects;
using Should;

namespace SharpRepository.Tests.QueryOptions
{
    [TestFixture]
    public class SortingOptionsTests : TestBase
    {
        [Test]
        public void SortOptions_SortProperty_Will_Be_Set_In_Constructor()
        {
            new SortingOptions<Contact>("Name").SortProperty.ShouldEqual("Name");
            new SortingOptions<Contact, String>(m => m.Name).SortExpression.GetPropertyName().ShouldEqual("Name");
        }

        [Test]
        public void SortingOptions_SortDirection_Will_Be_Set_In_Constructor()
        {
            new SortingOptions<Contact>("Name").IsDescending.ShouldBeFalse();
            new SortingOptions<Contact, String>(m => m.Name).IsDescending.ShouldBeFalse();

            new SortingOptions<Contact>("Name", true).IsDescending.ShouldBeTrue();
            new SortingOptions<Contact, String>(m => m.Name, true).IsDescending.ShouldBeTrue();
        }

        [Test]
        public void SortingOptions_Will_Sort_By_SortProperty_Asc()
        {
            var contacts = new List<Contact>();
            for (int i = 5; i >= 1; i--)
            {
                contacts.Add(new Contact { Name = "Test User " + i });
            }

            var qo = new SortingOptions<Contact>("Name");
            IQueryable<Contact> queryable = qo.Apply(contacts.AsQueryable());
            queryable.Count().ShouldEqual(5);
            queryable.First().Name.ShouldEqual("Test User 1");
        }

        [Test]
        public void SortingOptions_Will_Sort_By_SortProperty_Desc()
        {
            var contacts = new List<Contact>();
            for (int i = 1; i <= 5; i++)
            {
                contacts.Add(new Contact { Name = "Test User " + i });
            }

            var qo = new SortingOptions<Contact>("Name", true);
            IQueryable<Contact> queryable = qo.Apply(contacts.AsQueryable());
            queryable.Count().ShouldEqual(5);
            queryable.First().Name.ShouldEqual("Test User 5");
        }

        // TODO: add tests to see how if an invalid magic string is handled properly
    }
}