using BankRecord.Application.DTOs;
using BankRecord.Application.ViewModels;
using Infrastructure.Paging;
using System;
using System.Threading.Tasks;

namespace BankRecord.Application.Interfaces
{
    public interface IBankRecordService
    {
        Task PostAsync(BankRecordDTO input);
        Task<BankRecordViewModel> GetAll(Page page);
        Task<Domain.Entities.BankRecord> GetById(Guid id);
        Task<Domain.Entities.BankRecord> GetByDocumentIdorOrderId(Guid originId);
        Task<Domain.Entities.BankRecord> UpdateBankRecord(Guid id, BankRecordDTO bankRecord);
    }
}
