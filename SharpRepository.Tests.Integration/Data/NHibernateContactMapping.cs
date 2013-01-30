using FluentNHibernate.Mapping;
using SharpRepository.Tests.Integration.TestObjects;

namespace SharpRepository.Tests.Integration.Data
{
    public class NHibernateContactMapping : ClassMap<Contact>
    {
        public NHibernateContactMapping()
        {
            Id<string>("ContactId").GeneratedBy.UuidString();
//            Id(x => x.ContactId).Not.Nullable().Length(50);
            Map(x => x.ContactTypeId).Not.Nullable();
            Map(x => x.Name).Nullable().Length(100);
            Map(x => x.Title).Nullable().Length(100);
        }
    }
}
