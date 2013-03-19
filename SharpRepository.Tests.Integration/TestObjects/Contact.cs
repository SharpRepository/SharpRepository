using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SharpRepository.Tests.Integration.TestObjects
{
    public class Contact
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ContactId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public int ContactTypeId { get; set; } // for partitioning on 

        public virtual ContactType ContactType { get; set; }

        public virtual List<EmailAddress> EmailAddresses { get; set; }
        public virtual List<PhoneNumber> PhoneNumbers { get; set; }
    }
}