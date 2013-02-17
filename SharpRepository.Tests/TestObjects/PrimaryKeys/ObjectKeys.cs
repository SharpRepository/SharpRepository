using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace SharpRepository.Tests.TestObjects.PrimaryKeys
{
    public class ObjectKeys
    {
        public int Id { get; set; }

        [BsonId]
        public int KeyInt1 { get; set; }

        [Key]
        public int KeyInt2 { get; set; }

        public Guid KeyGuid { get; set; }

        public string KeyString { get; set; }
    }
}
