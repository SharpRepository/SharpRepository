using System.Collections.Generic;
// using SharpRepository.Logging;

namespace SharpRepository.Tests.TestObjects
{
    //[RepositoryLogging]
    public class Contact
    {
        public int ContactId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public int ContactTypeId { get; set; } // for partitioning on 

        public List<EmailAddress> EmailAddresses { get; set; }
        public List<PhoneNumber> PhoneNumbers { get; set; }

        public ContactType ContactType { get; set; }

        public byte[] Image { get; set; }

        public override bool Equals(object obj)
        {
            var contact = obj as Contact;

            return GetHashCode() == contact.GetHashCode();
        }

        public override int GetHashCode()
        {
            var hashCode = -1084825632;
            hashCode = hashCode * -1521134295 + ContactId.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Title);
            hashCode = hashCode * -1521134295 + ContactTypeId.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<List<EmailAddress>>.Default.GetHashCode(EmailAddresses);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<PhoneNumber>>.Default.GetHashCode(PhoneNumbers);
            hashCode = hashCode * -1521134295 + EqualityComparer<ContactType>.Default.GetHashCode(ContactType);
            hashCode = hashCode * -1521134295 + EqualityComparer<byte[]>.Default.GetHashCode(Image);
            return hashCode;
        }        
    }
}