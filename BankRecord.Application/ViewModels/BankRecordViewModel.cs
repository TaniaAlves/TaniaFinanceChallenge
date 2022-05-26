using System.Collections.Generic;

namespace BankRecord.Application.ViewModels
{
    public class BankRecordViewModel
    {
        public IEnumerable<Domain.Entities.BankRecord> Models { get; set; }
        public decimal Total { get; set; }
    }
}
