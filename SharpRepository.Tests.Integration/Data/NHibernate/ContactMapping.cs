using FluentNHibernate.Mapping;
using SharpRepository.Tests.Integration.TestObjects;

namespace SharpRepository.Tests.Integration.Data.NHibernate
{
    public class ContactMapping : ClassMap<Contact>
    {
        public ContactMapping()
        {
            Map(x => x.ContactId).Not.Nullable().Length(50);
            Map(x => x.Name).Not.Nullable().Length(100);
            Map(x => x.Title).Not.Nullable().Length(100);
        }
    }
}
