using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharpRepository.Tests.Integration.TestObjects
{
    public class EmailAddress
    {
        [Key]
        public int EmailAddressId { get; set; }
        
        public string ContactId { get; set; }

        public virtual Contact Contact { get; set; }

        public string Label { get; set; }
        public string Email { get; set; }
    }
}