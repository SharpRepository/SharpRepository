using System;

namespace SharpRepository.Tests.TestObjects
{
    [AuditAttributeMock(Order = 1)]
    [SpecificAudit(Order = 2)]
    internal class Product
    {
        public int ProductId { get; set; }
        public double Price { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}