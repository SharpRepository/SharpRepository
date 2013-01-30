using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SharpRepository.Tests.Integration.TestObjects
{
    public class Contact
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public virtual string ContactId { get; set; }
        public virtual string Name { get; set; }
        public virtual string Title { get; set; }
        public virtual int ContactTypeId { get; set; } // for partitioning on 

        public virtual List<EmailAddress> EmailAddresses { get; set; }
        public virtual List<PhoneNumber> PhoneNumbers { get; set; }
    }
}