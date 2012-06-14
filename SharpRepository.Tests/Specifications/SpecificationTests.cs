﻿using NUnit.Framework;
using SharpRepository.Repository.FetchStrategies;
using SharpRepository.Repository.Specifications;
using SharpRepository.Tests.TestObjects;
using Should;

namespace SharpRepository.Tests.Specifications
{
    [TestFixture]
    public class SpecificationTests : TestBase
    {
        [Test]
        public void Specification_Will_Default_To_GenericFetchStrategy()
        {
            var spec = new Specification<Contact>(p => p.ContactId == 1);
            spec.FetchStrategy.ShouldBeType<GenericFetchStrategy<Contact>>();
        }

        [Test]
        public void Specification_May_Be_Chained_By_And()
        {
            var spec = new Specification<Contact>(p => p.ContactId == 1)
                .And(new Specification<Contact>(p => p.Name.Equals("test")));

            var contact = new Contact() {ContactId = 1, Name = "test"};
            spec.IsSatisfiedBy(contact).ShouldBeTrue();

            contact = new Contact() { ContactId = 2, Name = "test" };
            spec.IsSatisfiedBy(contact).ShouldBeFalse();

            contact = new Contact() { ContactId = 1, Name = "nottest" };
            spec.IsSatisfiedBy(contact).ShouldBeFalse();
        }

        [Test]
        public void Specification_May_Be_Chained_By_Or()
        {
            var spec = new Specification<Contact>(p => p.ContactId == 1)
                .Or(new Specification<Contact>(p => p.Name.Equals("test")));

            var contact = new Contact() { ContactId = 1, Name = "test" };
            spec.IsSatisfiedBy(contact).ShouldBeTrue();

            contact = new Contact() { ContactId = 2, Name = "test" };
            spec.IsSatisfiedBy(contact).ShouldBeTrue();

            contact = new Contact() { ContactId = 1, Name = "nottest" };
            spec.IsSatisfiedBy(contact).ShouldBeTrue();

            contact = new Contact() { ContactId = 2, Name = "nottest" };
            spec.IsSatisfiedBy(contact).ShouldBeFalse();
        }
    }
}