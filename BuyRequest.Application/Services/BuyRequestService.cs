using AutoMapper;
using BankRecord.Communication.Interfaces;
using BankRecord.Domain.Entities;
using BankRecord.Domain.Entities.Enums;
using BuyRequest.Application.DTOs;
using BuyRequest.Application.Interfaces;
using BuyRequest.Data.Repositorys;
using BuyRequest.Domain.Entities.Enums;
using BuyRequest.Domain.Validators;
using Infrastructure.ErrorMessages;
using Infrastructure.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BuyRequest.Application.Services
{
    public class BuyRequestService : IBuyRequestService
    {
        private readonly IBuyRequestRepository _buyRequestRepository;
        private readonly IProductRequestService _productRequestService;
        private readonly IMapper _mapper;
        private readonly IBankRecordClient _bankRecordClient;
        Domain.Entities.BuyRequest buyRequest = new();
        public BuyRequestService(IBuyRequestRepository buyRequestRepository,IProductRequestService productRequestService, IMapper mapper, IBankRecordClient bankRecordClient)
        {
            _buyRequestRepository = buyRequestRepository;
            _productRequestService = productRequestService;
            _mapper = mapper;
            _bankRecordClient = bankRecordClient;
        }

        public async Task<Domain.Entities.BuyRequest> PostAsync(BuyRequestDTO orderInput)
        {
            var map = _mapper.Map(orderInput, buyRequest);

            var validator = new BuyRequestValidator();
            var valid = validator.Validate(map);

            if (valid.IsValid)
            {
                await _buyRequestRepository.CreateAsync(map);

                //decimal totalPrice = await _productRequestService.PostProduct(orderInput.Products, map.Id);
                //buyRequest.Price = totalPrice;

                //map.TotalValue = buyRequest.Price - (buyRequest.Price * (buyRequest.DiscountValue / 100));
                //map.Status = Status.Received;

                //await _buyRequestRepository.UpdateAsync(map);
            }
            else
            {
                var errors = new ErrorMessage<Domain.Entities.BuyRequest>(HttpStatusCode.BadRequest.GetHashCode().ToString(),
                   valid.Errors.ConvertAll(x => x.ErrorMessage.ToString()), buyRequest);

                var error = ValidatorErrors(errors);
                throw new Exception(error);
            }

            return map;
        }

        public string ValidatorErrors(ErrorMessage<Domain.Entities.BuyRequest> errorList)
        {
            string error = "";
            foreach (var item in errorList.Message)
                error += item + " ";

            return error;
        }

        public async Task<IEnumerable<Domain.Entities.BuyRequest>> GetAll( Page page)
        {
            var buyRequests = await _buyRequestRepository.GetAllWithPaging(page);

            if (buyRequests.Count() == 0)
            {
                var errors = _buyRequestRepository.NotFoundMessage(buyRequest);
                var error = ValidatorErrors(errors);
                throw new Exception(error);
            }

            return buyRequests;
           
        }
        
        public async Task<Domain.Entities.BuyRequest> GetById(Guid id)
        {
            var record = await _buyRequestRepository.GetByIdAsync(id);

            if (record == null)
            {
                var errors = _buyRequestRepository.NotFoundMessage(buyRequest);
                var error = ValidatorErrors(errors);
                throw new Exception(error);
            }
            return record;
        }

        public async Task<Domain.Entities.BuyRequest> GetByClientIdAsync(Guid clientId)
        {
            var record = await _buyRequestRepository.GetAsync(x => x.ClientId == clientId);

            if (record == null)
            {
                var errors = _buyRequestRepository.NotFoundMessage(buyRequest);
                var error = ValidatorErrors(errors);
                throw new Exception(error);
            }

            return record;
        }

        public async Task<Domain.Entities.BuyRequest> UpdateByIdAsync(BuyRequestDTO orderInput)  
        {
            
            var findRequest = await _buyRequestRepository.GetByIdAsync(orderInput.Id);
            //var findProducts = _productRequestRepository.GetAllByRequestId(id).ToList();

            #region Validations
            if (findRequest == null)
            {
                var errors = _buyRequestRepository.NotFoundMessage(buyRequest);
                var error = ValidatorErrors(errors);
                throw new Exception(error);
            }

            if (findRequest.Status == Status.Finalized && orderInput.Status != Status.Finalized)
            {
                var errors = _buyRequestRepository.BadRequestMessage(buyRequest, "Requests with status 'finalized' must maintain the same status");
                var error = ValidatorErrors(errors);
                throw new Exception(error);
            }
            #endregion

            var oldStatus = findRequest.Status;
            var totalValueOld = findRequest.TotalValue;

            //atualiza os produtos
            //var findRequestPrice = await _productRequestService.UpdateByIdAsync(id, orderInput);
            //findRequest.Price = findRequestPrice;

            //findRequest.TotalValue = findRequest.Price - (findRequest.Price * (findRequest.DiscountValue / 100));

            var map = _mapper.Map(orderInput, findRequest);

            var validator = new BuyRequestValidator();
            var valid = validator.Validate(map);

            if (valid.IsValid)
                await _buyRequestRepository.UpdateAsync(map);
            else
            {
                var errors = new ErrorMessage<Domain.Entities.BuyRequest>(HttpStatusCode.BadRequest.GetHashCode().ToString(),
                   valid.Errors.ConvertAll(x => x.ErrorMessage.ToString()), buyRequest);
                var error = ValidatorErrors(errors);
                throw new Exception(error);
            }

            //comunicação

            if (findRequest.Status == Status.Finalized)
            {
                var type = BankRecord.Domain.Entities.Enums.Type.Receive;
                var recentValue = map.TotalValue;
                string description = $"Financial transaction order id: {findRequest.Id}";

                if (map.Status == oldStatus && map.Status == Status.Finalized && totalValueOld > map.TotalValue)
                {
                    description = $"Diference purchase order id: {findRequest.Id}";
                    recentValue = map.TotalValue - totalValueOld;
                    type = BankRecord.Domain.Entities.Enums.Type.Receive;
                }
                if (map.Status == oldStatus && map.Status == Status.Finalized && map.TotalValue > totalValueOld)
                {
                    description = $"Diference purchase order id: {findRequest.Id}";
                    recentValue = map.TotalValue - totalValueOld;
                    type = BankRecord.Domain.Entities.Enums.Type.Payment;
                }
                else
                {
                    var errors = _buyRequestRepository.BadRequestMessage(buyRequest, "There was no change on the total amount.");
                    var error = ValidatorErrors(errors);
                    throw new Exception(error);
                }

                var response = await _bankRecordClient.PostBankRecord(Origin.PurchaseRequest, map.Id, description, type, recentValue);

                if (!response == true)
                {
                    var errors = _buyRequestRepository.BadRequestMessage(buyRequest, "There was an error while communicating with the BankRecordAPI, please try again.");
                    var error = ValidatorErrors(errors);
                    throw new Exception(error);
                }
            }
            return map;
           
        }

        public async Task<Domain.Entities.BuyRequest> UpdateState(Guid id, Status status)
        {
            var requestToUpdate = await _buyRequestRepository.GetByIdAsync(id);

            #region Validations
            if (requestToUpdate == null)
            {
                var errors = _buyRequestRepository.NotFoundMessage(buyRequest);
                var error = ValidatorErrors(errors);
                throw new Exception(error);
            }

            if (requestToUpdate.Status == Status.Finalized)
            {

                var errors = _buyRequestRepository.BadRequestMessage(buyRequest, "Finalized requests can only be deleted!");
                var error = ValidatorErrors(errors);
                throw new Exception(error);
            }

            //await _productRequestService.UpdateState(id, status);
            #endregion

            requestToUpdate.Status = status;
            await _buyRequestRepository.UpdateAsync(requestToUpdate);

            ////se ficar finalizado -> entrada                      //Comunicação
            if (requestToUpdate.Status == Status.Finalized)
            {
                var response = await _bankRecordClient.PostBankRecord(Origin.PurchaseRequest, id, $"Purshase order id: {requestToUpdate.Id}",
                   BankRecord.Domain.Entities.Enums.Type.Receive, requestToUpdate.TotalValue);

                if (!response == true)
                {
                    var errors = _buyRequestRepository.BadRequestMessage(buyRequest, "There was an error while communicating with the BankRecordAPI, please try again.");
                    var error = ValidatorErrors(errors);
                    throw new Exception(error);
                }
            }

            return requestToUpdate;
        }


        public async Task<Domain.Entities.BuyRequest> DeleteById(Guid id)
        {
            var buyRequest = await _buyRequestRepository.GetByIdAsync(id);

            if (buyRequest == null)
            {
                var errors = _buyRequestRepository.NotFoundMessage(buyRequest);
                var error = ValidatorErrors(errors);
                throw new Exception(error);
            }

            await _buyRequestRepository.DeleteAsync(buyRequest);
            //comunicação
            if (buyRequest.Status == Status.Finalized)
            {
                var response = await _bankRecordClient.PostBankRecord(Origin.PurchaseRequest, id, $"Revert Purshase order id: {buyRequest.Id}",
                    BankRecord.Domain.Entities.Enums.Type.Revert, -buyRequest.TotalValue);

                if (!response == true)
                {
                    var errors = _buyRequestRepository.BadRequestMessage(buyRequest, "There was an error while communicating with the BankRecordAPI, please try again.");
                    var error = ValidatorErrors(errors);
                    throw new Exception(error);
                }
            }
            return buyRequest;
           
        }
    }
}
