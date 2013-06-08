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

        [Test]
        public void SortingOptions_With_Multiple_Sorting_Properties()
        {
            var contacts = new List<Contact>();
            for (int i = 5; i >= 1; i--)
            {
                contacts.Add(new Contact { Name = "Test User " + (i % 2),ContactTypeId = i});
            }

            var qo = new SortingOptions<Contact>("Name");
            qo.ThenSortBy("ContactTypeId");

            IQueryable<Contact> queryable = qo.Apply(contacts.AsQueryable());
            queryable.Count().ShouldEqual(5);

            var contact = queryable.First();
            contact.Name.ShouldEqual("Test User 0");
            contact.ContactTypeId.ShouldEqual(2);
        }

        [Test]
        public void SortingOptions_Will_Sort_By_SortExpression_Asc()
        {
            var contacts = new List<Contact>();
            for (int i = 5; i >= 1; i--)
            {
                contacts.Add(new Contact { Name = "Test User " + i });
            }

            var qo = new SortingOptions<Contact, string>(x => x.Name);
            IQueryable<Contact> queryable = qo.Apply(contacts.AsQueryable());
            queryable.Count().ShouldEqual(5);
            queryable.First().Name.ShouldEqual("Test User 1");
        }

        [Test]
        public void SortingOptions_Will_Sort_By_SortExpression_Desc()
        {
            var contacts = new List<Contact>();
            for (int i = 1; i <= 5; i++)
            {
                contacts.Add(new Contact { Name = "Test User " + i });
            }

            var qo = new SortingOptions<Contact, string>(x => x.Name, isDescending: true);
            IQueryable<Contact> queryable = qo.Apply(contacts.AsQueryable());
            queryable.Count().ShouldEqual(5);
            queryable.First().Name.ShouldEqual("Test User 5");
        }

        [Test]
        public void SortingOptions_With_Multiple_SortExpression_Properties()
        {
            var contacts = new List<Contact>();
            for (int i = 5; i >= 1; i--)
            {
                contacts.Add(new Contact { Name = "Test User " + (i % 2),ContactTypeId = i});
            }

            var qo = new SortingOptions<Contact, string>(x => x.Name);
            qo.ThenSortBy(x => x.ContactTypeId);

            IQueryable<Contact> queryable = qo.Apply(contacts.AsQueryable());
            queryable.Count().ShouldEqual(5);

            var contact = queryable.First();
            contact.Name.ShouldEqual("Test User 0");
            contact.ContactTypeId.ShouldEqual(2);
        }

        // TODO: add tests to see how if an invalid magic string is handled properly
    }
}