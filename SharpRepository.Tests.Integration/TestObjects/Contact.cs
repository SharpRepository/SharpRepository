using System;
using System.Collections.Generic;
using Norm;

namespace SharpRepository.Tests.Integration.TestObjects
{
    public class Contact
    {
        [MongoIdentifier]
        public int ContactId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public int ContactTypeId { get; set; } // for partitioning on 

        public List<EmailAddress> EmailAddresses { get; set; }
        public List<PhoneNumber> PhoneNumbers { get; set; }
    }
}