using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace SharpRepository.CoreMvc.Models
{
    public class Email
    {
        public int Id { get; set; }
        public string EmailAddress { get; set; }
        [XmlIgnore]
        public Contact Contact;
    }
}