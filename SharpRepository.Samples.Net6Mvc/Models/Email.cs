namespace SharpRepository.Samples.Net6Mvc.Models
{
    public class Email
    {
        public int Id { get; set; }
        public string EmailAddress { get; set; }
        public Contact Contact;
    }
}
