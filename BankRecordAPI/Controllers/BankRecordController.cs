using AutoMapper;
using BankRecord.Application.DTOs;
using BankRecord.Application.Interfaces;
using BankRecord.Application.ViewModels;
using BankRecord.Data.Repositorys;
using BankRecord.Domain.Validators;
using BuyRequest.Data.Repositorys;
using Document.Data.Repository;
using Infrastructure.ErrorMessages;
using Infrastructure.Paging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace BankRecordAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankRecordController : ControllerBase
    {
        private readonly IBankRecordService _bankRecordService;
        private readonly IMapper _mapper;
        
        BankRecord.Domain.Entities.BankRecord bankRecord= new BankRecord.Domain.Entities.BankRecord();
        List<string> list = new();

        public BankRecordController(IBankRecordService bankRecordService, IMapper mapper)
        {
            _bankRecordService = bankRecordService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] BankRecordDTO input)
        {
            try
            {
                await _bankRecordService.PostAsync(input);
                return Ok(input);
            }
            catch (Exception ex)
            {
                list.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.BadRequest, new ErrorMessage<BankRecord.Domain.Entities.BankRecord>
                    (HttpStatusCode.BadRequest.GetHashCode().ToString(),list, bankRecord));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] Page page)
        {
            try
            {
                var result = await _bankRecordService.GetAll(page);
                return Ok(result);
            }
            catch (Exception ex)
            {
                list.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.BadRequest, new ErrorMessage<BankRecord.Domain.Entities.BankRecord>
                     (HttpStatusCode.BadRequest.GetHashCode().ToString(), list, bankRecord));
            }

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var result = await _bankRecordService.GetById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                list.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.BadRequest, new ErrorMessage<BankRecord.Domain.Entities.BankRecord>
                     (HttpStatusCode.BadRequest.GetHashCode().ToString(), list, bankRecord));
            }
        }

        [HttpGet("GetByDocumentIdOrOrderId")]
        public async Task<IActionResult> GetByDocumentIdorOrderId(Guid originId)
        {
            try
            {
                var result = await _bankRecordService.GetByDocumentIdorOrderId(originId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                list.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.BadRequest, new ErrorMessage<BankRecord.Domain.Entities.BankRecord>
                     (HttpStatusCode.BadRequest.GetHashCode().ToString(), list, bankRecord));
            }
        }

        [HttpPut("UpdateBankRecord/{id}")]
        public async Task<IActionResult> UpdateBankRecord(Guid id, [FromBody] BankRecordDTO bankRecordinput)
        {
            try
            {
                var bankRecordUpdate = await _bankRecordService.UpdateBankRecord(id, bankRecordinput);
                return Ok(bankRecordUpdate);
            }
            catch (Exception ex)
            {
                list.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.BadRequest, new ErrorMessage<BankRecord.Domain.Entities.BankRecord>
                     (HttpStatusCode.BadRequest.GetHashCode().ToString(), list, bankRecord));
            }
        }
    }
}
