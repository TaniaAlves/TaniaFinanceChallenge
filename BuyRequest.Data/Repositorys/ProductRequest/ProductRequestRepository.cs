using BuyRequest.Data.Context;
using Infrastructure.Repositorys.Generic;

namespace BuyRequest.Data.Repositorys.ProductRequest
{
    public class ProductRequestRepository : GenericRepository<Domain.Entities.ProductRequest>, IProductRequestRepository
    {
        private readonly BuyRequestDataContext _context;

        public ProductRequestRepository(BuyRequestDataContext context) : base(context)
        {
            _context = context;
        }

        //public async Task<Domain.Entities.ProductRequest> GetByIdAsync(Guid id)
        //{
        //    return await _context.Set<Domain.Entities.ProductRequest>()
        //        .AsNoTracking()
        //        .FirstOrDefaultAsync(e => e.Id == id);
        //}
        //public IQueryable<Domain.Entities.ProductRequest> GetAllByRequestId(Guid requestId)
        //{
        //    return _context.Set<Domain.Entities.ProductRequest>()
        //        .AsNoTracking()
        //        .Where(e => e.RequestId == requestId);
        //}
    }
}
