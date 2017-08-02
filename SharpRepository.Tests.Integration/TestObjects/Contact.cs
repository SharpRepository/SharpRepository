using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace SharpRepository.Tests.Integration.TestObjects
{
    public class Contact
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [Key]
        public string ContactId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public int ContactTypeId { get; set; } // for partitioning on 

        public decimal SumDecimal { get; set; }

        public virtual List<EmailAddress> EmailAddresses { get; set; }
        public virtual List<PhoneNumber> PhoneNumbers { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}