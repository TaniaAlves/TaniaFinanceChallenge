using BankRecord.Communication.Interfaces;
using BankRecord.Communication.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BankRecord.Communication.Configuration
{
    public static class BankRecordConfig
    {
        public static void AddBankRecordConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<BankRecordOptions>(options =>
            {
                options.BaseAddress = configuration["BankRecord.Communication:BaseAddress"];
                options.EndPoint = configuration["BankRecord.Communication:EndPoint"];
            });
            services.AddHttpClient<IBankRecordClient, BankRecordClient>();
        }
    }
}
