using Infrastructure.Repositorys.Generic;

namespace Document.Data.Repository
{
    public interface IDocumentRepository : IGenericRepository<Domain.Entities.Document>
    {
        //Task<Domain.Entities.Document> GetByIdAsync(Guid id);

    }
}
