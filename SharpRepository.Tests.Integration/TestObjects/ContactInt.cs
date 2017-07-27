using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SharpRepository.Tests.Integration.TestObjects
{
    public class ContactInt : ICloneable
    {
        [Key]
        public int ContactIntId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public int ContactTypeId { get; set; } // for partitioning on 

        public decimal SumDecimal { get; set; }

        public virtual List<EmailAddress> EmailAddresses { get; set; }
        public virtual List<PhoneNumber> PhoneNumbers { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}