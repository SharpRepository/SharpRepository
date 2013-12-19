using System.Collections.Generic;
using SharpRepository.Logging;

namespace SharpRepository.Tests.TestObjects
{
    [RepositoryLogging]
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
    }
}