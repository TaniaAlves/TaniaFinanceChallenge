using Infrastructure.Repositorys.Generic;
using System;
using System.Threading.Tasks;

namespace BankRecord.Data.Repositorys
{
    public interface IBankRecordRepository : IGenericRepository<Domain.Entities.BankRecord>
    {
        Task<Domain.Entities.BankRecord> GetByIdAsync(Guid id);
        Task<Domain.Entities.BankRecord> GetByOriginIdAsync(Guid id);
        //Task<HttpResponseMessage> CreateBankRecord(Origin origin, Guid originId, string description, FinanceChallengeTania.Domain.Entities.Type type, decimal amount);
    }
}
