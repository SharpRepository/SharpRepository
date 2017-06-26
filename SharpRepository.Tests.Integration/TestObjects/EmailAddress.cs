using System.ComponentModel.DataAnnotations.Schema;

namespace SharpRepository.Tests.Integration.TestObjects
{
    public class EmailAddress
    {
        public int EmailAddressId { get; set; }
        
        public string ContactId { get; set; }

        public Contact Contact { get; set; }

        public string Label { get; set; }
        public string Email { get; set; }
    }
}