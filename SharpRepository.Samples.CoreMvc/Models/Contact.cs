using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SharpRepository.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SharpRepository.CoreMvc.Models
{
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
