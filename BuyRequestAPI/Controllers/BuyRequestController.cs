using AutoMapper;
using BuyRequest.Application.DTOs;
using BuyRequest.Application.Interfaces;
using BuyRequest.Domain.Entities.Enums;
using Infrastructure.ErrorMessages;
using Infrastructure.Paging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace BuyRequestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuyRequestController : ControllerBase
    {
        private readonly IBuyRequestService _buyRequestService;

        BuyRequest.Domain.Entities.BuyRequest buyRequest = new BuyRequest.Domain.Entities.BuyRequest();
        BankRecord.Domain.Entities.BankRecord bank = new();
        List<string> list = new();

        public BuyRequestController(IBuyRequestService buyRequestService, IMapper mapper)
        {
            _buyRequestService = buyRequestService;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] BuyRequestDTO orderInput)
        {
            try
            {
                var result = await _buyRequestService.PostAsync(orderInput);
                return Ok(result);
            }
            catch (Exception ex)
            {
                list.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.BadRequest, new ErrorMessage<BuyRequest.Domain.Entities.BuyRequest>
                    (HttpStatusCode.BadRequest.GetHashCode().ToString(), list, buyRequest));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] Page page)
        {
            try
            {
                var buyRequests =await _buyRequestService.GetAll(page);

                return Ok(buyRequests);
            }
            catch (Exception ex)
            {
                list.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.BadRequest, new ErrorMessage<BuyRequest.Domain.Entities.BuyRequest>
                    (HttpStatusCode.BadRequest.GetHashCode().ToString(), list, buyRequest));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var record = await _buyRequestService.GetById(id);
                return Ok(record);
            }
            catch (Exception ex)
            {
                list.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.BadRequest, new ErrorMessage<BuyRequest.Domain.Entities.BuyRequest>
                    (HttpStatusCode.BadRequest.GetHashCode().ToString(), list, buyRequest));
            }
        }


        [HttpGet("GetByClientIdAsync/{clientId}")]
        public async Task<IActionResult> GetByClientIdAsync(Guid clientId)
        {
            try
            {
                var record = await _buyRequestService.GetByClientIdAsync(clientId);
                return Ok(record);
            }
            catch (Exception ex)
            {
                list.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.BadRequest, new ErrorMessage<BuyRequest.Domain.Entities.BuyRequest>
                    (HttpStatusCode.BadRequest.GetHashCode().ToString(), list, buyRequest));
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateByIdAsync([FromBody] BuyRequestDTO orderInput)  //atualiza pelo id
        {
            try
            {
                var record = await _buyRequestService.UpdateByIdAsync(orderInput);
                return Ok(record);
                //COMUNICAÇÃO
                //if (findRequest.Status == Status.Finalized)
                //{
                //    var type = FinanceChallengeTania.Domain.Entities.Type.Receive;
                //    var recentValue = map.TotalValue;
                //    string description = $"Financial transaction order id: {findRequest.Id}";

                //    if (map.Status == oldStatus && map.Status == Status.Finalized && totalValueOld > map.TotalValue)
                //    {
                //        description = $"Diference purchase order id: {findRequest.Id}";
                //        recentValue = map.TotalValue - totalValueOld;
                //        type = FinanceChallengeTania.Domain.Entities.Type.Payment;
                //    }

                //    var response = await _bankRecordRepository.CreateBankRecord(Origin.PurchaseRequest, map.Id, description,
                //      type, recentValue);

                //    if (!response.IsSuccessStatusCode)
                //    {
                //        var result = _bankRecordRepository.BadRequestMessage(bank, response.Content.ToString());
                //        return StatusCode((int)HttpStatusCode.BadRequest, result);
                //    }
                //}
            }
            catch (Exception ex)
            {
                list.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.BadRequest, new ErrorMessage<BuyRequest.Domain.Entities.BuyRequest>
                    (HttpStatusCode.BadRequest.GetHashCode().ToString(), list, buyRequest));
            }
        }




        [HttpPut("updateState/{id}")]
        public async Task<IActionResult> UpdateState(Guid id, Status status)
        {
            try
            {
                var record = await _buyRequestService.UpdateState(id, status);
                return Ok(record);

                //tem comunicaçao
            }
            catch (Exception ex)
            {
                list.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.BadRequest, new ErrorMessage<BuyRequest.Domain.Entities.BuyRequest>
                    (HttpStatusCode.BadRequest.GetHashCode().ToString(), list, buyRequest));
            }

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteById(Guid id)
        {
            try
            {
                var record = await _buyRequestService.DeleteById(id);
                return Ok(record);
            }
            catch (Exception ex)
            {
                list.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.BadRequest, new ErrorMessage<BuyRequest.Domain.Entities.BuyRequest>
                    (HttpStatusCode.BadRequest.GetHashCode().ToString(), list, buyRequest));
            }
        }

    }
}
