using Document.Application.DTOs;
using Infrastructure.Paging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Document.Application.Interfaces
{
    public interface IDocumentService
    {
        Task<Domain.Entities.Document> Post(DocumentRequestDTO input);

        Task<IEnumerable<Domain.Entities.Document>> GetAll(Page page);
        Task<Domain.Entities.Document> UpdateDocument(Guid id, DocumentRequestDTO input);
        Task<Domain.Entities.Document> UpdateState(Guid id, bool Status);
        Task<Domain.Entities.Document> GetById(Guid id);
        Task<Domain.Entities.Document> DeleteById(Guid id);
    }
}
