using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace BankRecord.Data.Context
{
    public class BankRecordDataContext : DataContext
    {
        public DbSet<Domain.Entities.BankRecord> BankRecords { get; set; }
        public BankRecordDataContext(DbContextOptions<BankRecordDataContext> options) : base(options)
        {

        }
    }
}
