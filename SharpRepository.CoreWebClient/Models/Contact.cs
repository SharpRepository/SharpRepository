using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpRepository.CoreWebClient.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Email> Emails { get; set; }
    }
}
