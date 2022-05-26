using BankRecord.Domain.Entities.Enums;
using System;

namespace BankRecord.Application.DTOs
{
    public class BankRecordDTO
    {
        public Origin? Origin { get; set; }
        public Guid? OriginId { get; set; }
        public string Description { get; set; }
        public Domain.Entities.Enums.Type Type { get; set; }
        public decimal Amount { get; set; }
    }
}
