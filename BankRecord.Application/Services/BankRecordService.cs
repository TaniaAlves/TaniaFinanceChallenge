using AutoMapper;
using BankRecord.Application.DTOs;
using BankRecord.Application.Interfaces;
using BankRecord.Application.ViewModels;
using BankRecord.Data.Repositorys;
using BankRecord.Domain.Entities.Enums;
using BankRecord.Domain.Validators;
using Infrastructure.ErrorMessages;
using Infrastructure.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BankRecord.Application.Services
{

    public class BankRecordService : IBankRecordService
    {
        private readonly IBankRecordRepository _bankRecordRepository;
        private readonly IMapper _mapper;

        Domain.Entities.BankRecord bankRecord = new Domain.Entities.BankRecord();
        public BankRecordService(IBankRecordRepository bankRecordRepository, IMapper mapper)
        {
            _bankRecordRepository = bankRecordRepository;
            _mapper = mapper;
        }

        public async Task PostAsync(BankRecordDTO input)
        {
            var map = _mapper.Map<Domain.Entities.BankRecord>(input);

            var validator = new BankRecordValidator();
            var valid = validator.Validate(map);

            if (input.Origin == Origin.Null)
                map.OriginId = null;

            if (valid.IsValid)
                await _bankRecordRepository.CreateAsync(map);
            else
            {
                var errors = new ErrorMessage<Domain.Entities.BankRecord>(HttpStatusCode.BadRequest.GetHashCode().ToString(),
                    valid.Errors.ConvertAll(x => x.ErrorMessage.ToString()), bankRecord);

                var error = ValidatorErrors(errors);

                throw new Exception(error);
            }

        }

        public string ValidatorErrors(ErrorMessage<Domain.Entities.BankRecord> errorList)
        {
            string error = "";
            foreach (var item in errorList.Message)
                error += item + " ";

            return error;
        }

        public async Task<BankRecordViewModel> GetAll(Page page)
        {
            BankRecordViewModel recordsModel = new BankRecordViewModel();
            recordsModel.Models = await _bankRecordRepository.GetAllWithPaging(page);

            if (recordsModel.Models.Count() != 0)
                recordsModel.Total = recordsModel.Models.Sum(x => x.Amount);
            else
            {
                var errors = _bankRecordRepository.NotFoundMessage(bankRecord);

                var error = ValidatorErrors(errors);
                throw new Exception(error);
            }

            return recordsModel;
        }

        public async Task<Domain.Entities.BankRecord> GetById(Guid id)
        {
            var records = await _bankRecordRepository.GetByIdAsync(id);

            if (records == null)
            {
                var errors = _bankRecordRepository.NotFoundMessage(bankRecord);
                var error = ValidatorErrors(errors);
                throw new Exception(error);
            }
            return records;
        }

        public async Task<Domain.Entities.BankRecord> GetByDocumentIdorOrderId(Guid originId)
        {
            var records = await _bankRecordRepository.GetAsync(x=> x.OriginId == originId);

            if (records == null)
            {
                var errors = _bankRecordRepository.NotFoundMessage(bankRecord);
                var error = ValidatorErrors(errors);
                throw new Exception(error);
            }
            return records;
        }

        public async Task<Domain.Entities.BankRecord> UpdateBankRecord(Guid id, BankRecordDTO bankRecordinput)
        {
            var bankRecordUpdate = await _bankRecordRepository.GetByIdAsync(id);
            if (bankRecordUpdate == null)
            {
                var errors = _bankRecordRepository.NotFoundMessage(bankRecord);
                var error = ValidatorErrors(errors);
                throw new Exception(error);
            }

            if (bankRecordUpdate.Origin == Origin.Null)
            {
                bankRecordUpdate.OriginId = null;
                bankRecordinput.OriginId = null;
                bankRecordUpdate.Origin = Origin.Null;
            }
            else
            {
                var errors = _bankRecordRepository.BadRequestMessage(bankRecord, "That data can't be changed.");
                var error = ValidatorErrors(errors);
                throw new Exception(error);
            }

            var mapBankRecord = _mapper.Map(bankRecordinput, bankRecordUpdate);

            var validator = new BankRecordValidator();
            var valid = validator.Validate(mapBankRecord);

            if (valid.IsValid)
                await _bankRecordRepository.UpdateAsync(bankRecordUpdate);
            else
            {
                var errors = new ErrorMessage<Domain.Entities.BankRecord>(HttpStatusCode.BadRequest.GetHashCode().ToString(),
                    valid.Errors.ConvertAll(x => x.ErrorMessage.ToString()), bankRecord);
                var error = ValidatorErrors(errors);

                throw new Exception(error);
            }

            return bankRecordUpdate;
        }
    }
}
