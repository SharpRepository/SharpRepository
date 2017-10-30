
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SharpRepository.Samples.MVC5.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Email> Emails { get; set; }
    }
}
