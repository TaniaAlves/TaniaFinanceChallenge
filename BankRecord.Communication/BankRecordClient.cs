using BankRecord.Application.DTOs;
using BankRecord.Communication.Interfaces;
using BankRecord.Communication.Options;
using BankRecord.Domain.Entities.Enums;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BankRecord.Communication
{
    public class BankRecordClient : IBankRecordClient
    {
        private readonly HttpClient _client;
        private readonly BankRecordOptions _options;
        //private readonly ILogger _logger;
        public BankRecordClient(HttpClient client, IOptions<BankRecordOptions> options)
        {
            _client = client;
            _options = options.Value;
        }

        public async Task<bool> PostBankRecord(Origin origin, Guid id, string description, Domain.Entities.Enums.Type type, decimal amount )
        {
            //implementacao de envio
            //utilizar option para obter BaseUrl e EndPoint do appsettings
            var options = _options.GetBankRecordEndPoint();

            BankRecordDTO bankRecord = new()
            {
                Origin = origin,
                OriginId = id,
                Description = description,
                Type = type,
                Amount = amount
            };

            var response = await _client.PostAsJsonAsync(options, bankRecord);

            if (!response.IsSuccessStatusCode)
            {
                var error = response.Content.ToString();
                throw new Exception(error);
            }
            return response != null && response.IsSuccessStatusCode;
        }

    }
}
