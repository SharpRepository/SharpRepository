using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SharpRepository.Repository;

namespace MongoDbDriver.Models
{
    public class Patient : Hl7.Fhir.Model.Patient
    {
    }
}
