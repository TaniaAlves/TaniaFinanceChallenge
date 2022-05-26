using Document.Application.DTOs;
using Document.Application.Interfaces;
using Infrastructure.ErrorMessages;
using Infrastructure.Paging;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace DocumentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        Document.Domain.Entities.Document document = new();
        List<string> list = new();
        public DocumentController(IDocumentService documentService)
        {
            _documentService = documentService;

        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DocumentRequestDTO input)
        {
            try
            {
                var doc = await _documentService.Post(input);
                return Ok(doc);
            }
            catch (Exception ex)
            {
                list.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.BadRequest, new ErrorMessage<Document.Domain.Entities.Document>
                    (HttpStatusCode.BadRequest.GetHashCode().ToString(), list, document));
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] Page page)
        {
            try
            {
                var doc = await _documentService.GetAll(page);
                return Ok(doc);
            }
            catch (Exception ex)
            {
                list.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.BadRequest, new ErrorMessage<Document.Domain.Entities.Document>
                    (HttpStatusCode.BadRequest.GetHashCode().ToString(), list, document));
            }
        }

        [HttpPut("UpdateDocument/{id}")]
        public async Task<IActionResult> UpdateDocument(Guid id, [FromBody] DocumentRequestDTO input)
        {
            try
            {
                var doc = await _documentService.UpdateDocument(id, input);
                return Ok(doc);
            }
            catch (Exception ex)
            {
                list.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.BadRequest, new ErrorMessage<Document.Domain.Entities.Document>
                    (HttpStatusCode.BadRequest.GetHashCode().ToString(), list, document));
            }

        }


        [HttpPut("updateState/{id}")]
        public async Task<IActionResult> UpdateState(Guid id, bool status)
        {
            try
            {
                var doc = await _documentService.UpdateState(id, status);
                return Ok(doc);
            }
            catch (Exception ex)
            {
                list.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.BadRequest, new ErrorMessage<Document.Domain.Entities.Document>
                    (HttpStatusCode.BadRequest.GetHashCode().ToString(), list, document));
            }

        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var doc = await _documentService.GetById(id);
                return Ok(doc);
            }
            catch (Exception ex)
            {
                list.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.BadRequest, new ErrorMessage<Document.Domain.Entities.Document>
                    (HttpStatusCode.BadRequest.GetHashCode().ToString(), list, document));
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteById(Guid id)
        {
            try
            {
                var doc = await _documentService.DeleteById(id);
                return Ok(doc);
            }
            catch (Exception ex)
            {
                list.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.BadRequest, new ErrorMessage<Document.Domain.Entities.Document>
                    (HttpStatusCode.BadRequest.GetHashCode().ToString(), list, document));
            }

        }
    }
}
