using System.ComponentModel.DataAnnotations.Schema;

namespace SharpRepository.Samples.Core3Mvc.Models
{
    public class Email
    {
        public int Id { get; set; }
        public string EmailAddress { get; set; }
        public Contact Contact;
    }
}