using BankRecord.Domain.Entities.Enums;
using System;
using System.Threading.Tasks;

namespace BankRecord.Communication.Interfaces
{
    public interface IBankRecordClient
    {
        Task<bool> PostBankRecord(Origin origin, Guid id, string description, Domain.Entities.Enums.Type type, decimal amount);

    }
}
