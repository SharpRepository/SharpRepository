using System.Collections.Generic;
using SharpRepository.Logging.Log4net;

namespace SharpRepository.Tests.TestObjects
{
    [Log4NetRepositoryLogging()]
    public class Contact
    {
        public int ContactId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public int ContactTypeId { get; set; } // for partitioning on 

        public List<EmailAddress> EmailAddresses { get; set; }
        public List<PhoneNumber> PhoneNumbers { get; set; }

        public byte[] Image { get; set; }
    }
}