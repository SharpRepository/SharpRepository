using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SharpRepository.MongoDbRepository;
using SharpRepository.Repository;
using System.ComponentModel.DataAnnotations;

namespace SharpRepository.Samples.Net6Mvc.Models
{
    [MongoDbCollectionName("Contacts")]
    public class Contact
    {
        [BsonId] // Needed for MongoDB
        [BsonRepresentation(BsonType.ObjectId)] // Needed for MongoDB
        [RepositoryPrimaryKey] //Autogenrates value for strings
        [Key] //Ef primary key
        public string Id { get; set; }
        public string Name { get; set; }

        [UIHint("_Emails")]
        public virtual ICollection<Email> Emails { get; set; }
    }
}
