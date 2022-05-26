using Infrastructure.Repositorys.Generic;

namespace BuyRequest.Data.Repositorys.ProductRequest
{
    public interface IProductRequestRepository : IGenericRepository<Domain.Entities.ProductRequest>
    {
        //Task<Domain.Entities.ProductRequest> GetByIdAsync(Guid id);
        //IQueryable<Domain.Entities.ProductRequest> GetAllByRequestId(Guid requestId);
    }
}
