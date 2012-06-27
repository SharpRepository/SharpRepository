using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using SharpRepository.MongoDbRepository;

namespace SharpRepository.Tests.Integration.TestObjects
{
    public class Contact
    {
        [BsonId(IdGenerator = typeof(EmployeeIdGenerator))]
        public int ContactId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public int ContactTypeId { get; set; } // for partitioning on 

        public List<EmailAddress> EmailAddresses { get; set; }
        public List<PhoneNumber> PhoneNumbers { get; set; }
    }
}