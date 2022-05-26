using AutoMapper;
using BankRecord.Communication.Interfaces;
using BankRecord.Domain.Entities.Enums;
using Document.Application.DTOs;
using Document.Application.Interfaces;
using Document.Data.Repository;
using Document.Domain.Validators;
using Infrastructure.ErrorMessages;
using Infrastructure.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Document.Application.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IBankRecordClient _bankRecordClient;
        private readonly IMapper _mapper;

        Domain.Entities.Document document = new();
        public DocumentService(IDocumentRepository documentRepository, IBankRecordClient bankRecordClient, IMapper mapper)
        {
            _documentRepository = documentRepository;
            _bankRecordClient = bankRecordClient;
            _mapper = mapper;
        }

        public async Task<Domain.Entities.Document> Post(DocumentRequestDTO input)
        {
            var mapperDoc = _mapper.Map(input, document);

            var validator = new DocumentValidator();
            var valid = validator.Validate(mapperDoc);

            if (valid.IsValid)
            {
                if (mapperDoc.Paid == true)     //comunicação
                {
                    var type = new BankRecord.Domain.Entities.Enums.Type();

                    if (mapperDoc.Operation == Domain.Entities.Enums.Operation.Entry)
                        type = BankRecord.Domain.Entities.Enums.Type.Receive;
                    else
                        type = BankRecord.Domain.Entities.Enums.Type.Payment;

                    await _documentRepository.CreateAsync(mapperDoc);

                    var response = await _bankRecordClient.PostBankRecord(Origin.Document, mapperDoc.Id,
                       $"Financial Transaction (id: {mapperDoc.Id})", type, mapperDoc.Total);

                    if (response == false)
                    {
                        var errors = _documentRepository.BadRequestMessage(document, "Communication Error. There was an error trying to communicate with BankRecordAPI.");
                        var error = ValidatorErrors(errors);
                        throw new Exception(error);
                    }
                }
                else
                    await _documentRepository.CreateAsync(mapperDoc);
            }
            else
            {
                var errors = new ErrorMessage<Domain.Entities.Document>(HttpStatusCode.BadRequest.GetHashCode().ToString(),
                    valid.Errors.ConvertAll(x => x.ErrorMessage.ToString()), document);
                var error = ValidatorErrors(errors);
                throw new Exception(error);
            }

            return mapperDoc;

        }
        public string ValidatorErrors(ErrorMessage<Domain.Entities.Document> errorList)
        {
            string error = "";
            foreach (var item in errorList.Message)
                error += item + " ";

            return error;
        }


        public async Task<IEnumerable<Domain.Entities.Document>> GetAll(Page page)
        {

            var documents = await _documentRepository.GetAllWithPaging(page);

            if (documents.Count() == 0)
            {
                var errors = _documentRepository.NotFoundMessage(document);
                var error = ValidatorErrors(errors);
                throw new Exception(error);
            }

            return documents;
        }

        public async Task<Domain.Entities.Document> UpdateDocument(Guid id, DocumentRequestDTO input)
        {
            var originalDoc = await _documentRepository.GetByIdAsync(id);

            if (originalDoc == null)
            {
                var errors = _documentRepository.NotFoundMessage(document);
                var error = ValidatorErrors(errors);
                throw new Exception(error);
            }

            if (originalDoc.Paid == true && input.Paid != true)
            {
                var errors = _documentRepository.BadRequestMessage(document, "That data can only have the type 'Paid'");
                var error = ValidatorErrors(errors);
                throw new Exception(error);
            }

            var oldTotal = originalDoc.Total;
            var mapperDocument = _mapper.Map(input, originalDoc);

            var validator = new DocumentValidator();
            var valid = validator.Validate(mapperDocument);

            if (valid.IsValid)
            {
                var TotalUpdated = originalDoc.Total - oldTotal;

                if (TotalUpdated != oldTotal && originalDoc.Paid == true || originalDoc.Paid == false && input.Paid == true)
                {
                    string msg = $"Diference in Document id: {originalDoc.Id}";
                    var type = BankRecord.Domain.Entities.Enums.Type.Revert;
                    decimal total = TotalUpdated;

                    if (originalDoc.Paid == false && input.Paid == true)
                    {
                        msg = $"Financial Transaction (id: {originalDoc.Id})";
                        type = BankRecord.Domain.Entities.Enums.Type.Receive;
                        total = input.Total;
                    }

                    var response = await _bankRecordClient.PostBankRecord(Origin.Document, mapperDocument.Id, msg, type, total);

                    if (response == false)
                    {
                        var errors = _documentRepository.BadRequestMessage(document, "Communication Error. There was an error trying to communicate with BankRecordAPI.");
                        var error = ValidatorErrors(errors);
                        throw new Exception(error);
                    }
                }

                await _documentRepository.UpdateAsync(mapperDocument);
                return mapperDocument;
            }
            else
            {
                var errors = new ErrorMessage<Domain.Entities.Document>(HttpStatusCode.BadRequest.GetHashCode().ToString(),
                    valid.Errors.ConvertAll(x => x.ErrorMessage.ToString()), document);
                var error = ValidatorErrors(errors);
                throw new Exception(error);
            }
        }



        public async Task<Domain.Entities.Document> UpdateState(Guid id, bool Status)
        {
            var doc = await _documentRepository.GetByIdAsync(id);
            if (doc == null)
            {
                var errors = _documentRepository.NotFoundMessage(document);
                var error = ValidatorErrors(errors);
                throw new Exception(error);
            }

            if (doc.Paid == true)
            {
                var errors = _documentRepository.BadRequestMessage(document, "You can only delete data with type 'Paid'");
                var error = ValidatorErrors(errors);
                throw new Exception(error);

            }
            doc.Paid = Status;

            await _documentRepository.UpdateAsync(doc);

            if (doc.Paid == true)         //comunicação
            {
                var operation = BankRecord.Domain.Entities.Enums.Type.Payment;
                if (doc.Operation == Domain.Entities.Enums.Operation.Entry)
                    operation = BankRecord.Domain.Entities.Enums.Type.Receive;

                var response = await _bankRecordClient.PostBankRecord(Origin.Document, id, $"Financial Transaction (id: {doc.Id})",
                    operation, doc.Total);

                if (response == false)
                {
                    var errors = _documentRepository.BadRequestMessage(document, "Communication Error. There was an error trying to communicate with BankRecordAPI.");
                    var error = ValidatorErrors(errors);
                    throw new Exception(error);
                }
            }

            return doc;
        }


        public async Task<Domain.Entities.Document> GetById(Guid id)
        {
            var document = await _documentRepository.GetByIdAsync(id);

            if (document == null)
            {
                var errors = _documentRepository.NotFoundMessage(document);
                var error = ValidatorErrors(errors);
                throw new Exception(error);
            }

            return document;
        }

        public async Task<Domain.Entities.Document> DeleteById(Guid id)
        {

            var doc = await _documentRepository.GetByIdAsync(id);
            if (doc == null)
            {
                var errors = _documentRepository.NotFoundMessage(document);
                var error = ValidatorErrors(errors);
                throw new Exception(error);
            }
            else
                await _documentRepository.DeleteAsync(doc);

            if (doc.Paid == true)         //comunicação
            {
                var response = await _bankRecordClient.PostBankRecord(Origin.Document, doc.Id, $"Revert Document order id: {doc.Id}",
                    BankRecord.Domain.Entities.Enums.Type.Revert, -doc.Total);

                if (response == false)
                {
                    var errors = _documentRepository.BadRequestMessage(document, "Communication Error. There was an error trying to communicate with BankRecordAPI.");
                    var error = ValidatorErrors(errors);
                    throw new Exception(error);
                }
            }

            return doc;
        }

    }
}
