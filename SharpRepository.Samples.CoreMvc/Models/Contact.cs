using SharpRepository.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SharpRepository.CoreMvc.Models
{
    public class Contact
    {
        [RepositoryPrimaryKey] //Autogenrates value for strings
        [Key] //Ef primary key
        public string Id { get; set; }
        public string Name { get; set; }

        [UIHint("_Emails")]
        public virtual List<Email> Emails { get; set; }
    }
}
