using BankRecord.Domain.Entities.Enums;
using Infrastructure.BaseClass;
using System;

namespace BankRecord.Domain.Entities
{
    public class BankRecord: EntityBase
    {
        public Origin? Origin { get; set; }
        public Guid? OriginId { get; set; }
        public string Description { get; set; }
        public Enums.Type Type { get; set; }
        public decimal Amount { get; set; }
    }
}
