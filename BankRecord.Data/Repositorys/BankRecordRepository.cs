using BankRecord.Data.Context;
using Infrastructure.Repositorys.Generic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BankRecord.Data.Repositorys
{
    public class BankRecordRepository : GenericRepository<Domain.Entities.BankRecord>, IBankRecordRepository
    {
        private readonly BankRecordDataContext _context;

        public BankRecordRepository(BankRecordDataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Domain.Entities.BankRecord> GetByIdAsync(Guid id)
        {
            return await _context.Set<Domain.Entities.BankRecord>()
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Domain.Entities.BankRecord> GetByOriginIdAsync(Guid id)
        {
            return await _context.Set<Domain.Entities.BankRecord>()
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.OriginId == id);
        }
        //public async Task<HttpResponseMessage> CreateBankRecord(Origin origin, Guid originId, string description, FinanceChallengeTania.Domain.Entities.Type type, decimal amount)
        //{
        //    var client = new HttpClient();
        //    string ApiUrl = "https://localhost:44320/api/BankRecord";

        //    var bankRecord = new BankRecordDTO()
        //    {
        //        Origin = origin,
        //        OriginId = originId,
        //        Description = description,
        //        Type = type,
        //        Amount = amount
        //    };

        //    var response = await client.PostAsJsonAsync(ApiUrl, bankRecord);
        //    return response;
        //}
    }
}
