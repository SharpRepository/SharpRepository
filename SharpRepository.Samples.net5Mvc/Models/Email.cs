using System.ComponentModel.DataAnnotations.Schema;

namespace SharpRepository.Samples.net5Mvc.Models
{
    public class Email
    {
        public int Id { get; set; }
        public string EmailAddress { get; set; }
        public Contact Contact;
    }
}