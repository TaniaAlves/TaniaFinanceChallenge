using BankRecord.Application.DTOs;
using BankRecord.Application.Interfaces;
using BankRecordAPI.Controllers;
using Infrastructure.Paging;
using Moq;
using Moq.AutoMock;
using System;
using System.Threading.Tasks;
using Xunit;

namespace BankRecordUnitTest
{
    public class BankRecordControllerTest
    {
        private readonly AutoMocker Mocker;
        public BankRecordControllerTest()
        {
            Mocker = new AutoMocker();
        }

        [Fact(DisplayName = "PostBankRecord Test")]
        public async Task PostBankRequest()
        {
            var bankRequest = new BankRecordDTO()
            {
                Origin = BankRecord.Domain.Entities.Enums.Origin.Null,
                OriginId = null,
                Description = "Post Testing",
                Type = BankRecord.Domain.Entities.Enums.Type.Receive,
                Amount = 10
            };

            var bankReqService = Mocker.GetMock<IBankRecordService>();
            bankReqService.Setup(X => X.PostAsync(bankRequest));

            var bankReqController = Mocker.CreateInstance<BankRecordController>();

            await bankReqController.PostAsync(bankRequest);

            bankReqService.Verify(x => x.PostAsync(It.IsAny<BankRecordDTO>()), Times.Once());

        }

        [Fact(DisplayName = "GetAllBankRecord Test")]
        public async Task GetAllBankRequest()
        {

            var bankReqService = Mocker.GetMock<IBankRecordService>();
            bankReqService.Setup(x => x.GetAll(null));

            var bankReqController = Mocker.CreateInstance<BankRecordController>();

            Page pageParameters = new Page();

            await bankReqController.GetAll(pageParameters);

            bankReqService.Verify(x => x.GetAll(pageParameters), Times.Once());

        }

        [Fact(DisplayName = "GetByIdBankRecord Test")]
        public async Task GetByIdBankRequest()
        {

            var bankRequest = new BankRecord.Domain.Entities.BankRecord();

            var bankReqService = Mocker.GetMock<IBankRecordService>();
            bankReqService.Setup(x => x.GetById(bankRequest.Id));

            var bankReqController = Mocker.CreateInstance<BankRecordController>();

            await bankReqController.GetById(bankRequest.Id);

            bankReqService.Verify(x => x.GetById(bankRequest.Id), Times.Once());

        }

        [Fact(DisplayName = "GetByDocumentIdorOrderIdBankRecord Test")]
        public async Task GetByOriginIdBankRequest()
        {
            var id = Guid.NewGuid();

            var bankRequest = new BankRecord.Domain.Entities.BankRecord();

            var bankReqService = Mocker.GetMock<IBankRecordService>();
            bankReqService.Setup(x => x.GetByDocumentIdorOrderId(id));

            var bankReqController = Mocker.CreateInstance<BankRecordController>();

            await bankReqController.GetByDocumentIdorOrderId(id);

            bankReqService.Verify(x => x.GetByDocumentIdorOrderId(id), Times.Once());

        }

        [Fact(DisplayName = "UpdateBankRecord Test")]

        public async Task UpdateBankRequest()
        {
            var id = Guid.NewGuid();
            var bankRequest = new BankRecord.Domain.Entities.BankRecord()
            {
                Origin = BankRecord.Domain.Entities.Enums.Origin.Null,
                OriginId = null,
                Description = "Update Testing",
                Type = BankRecord.Domain.Entities.Enums.Type.Payment,
                Amount = -10
            };

            var bankReqService = Mocker.GetMock<IBankRecordService>();
            bankReqService.Setup(x => x.GetById(bankRequest.Id)).ReturnsAsync(bankRequest);
            bankReqService.Setup(X => X.UpdateBankRecord(bankRequest.Id, new BankRecordDTO()));

            var bankReqController = Mocker.CreateInstance<BankRecordController>();

            await bankReqController.UpdateBankRecord(bankRequest.Id, new BankRecordDTO());

            bankReqService.Verify(x => x.UpdateBankRecord(bankRequest.Id, It.IsAny<BankRecordDTO>()), Times.Once());
        }
    }
}
