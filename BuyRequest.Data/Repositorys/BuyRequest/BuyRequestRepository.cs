using BuyRequest.Data.Context;
using Infrastructure.Repositorys.Generic;
using Microsoft.EntityFrameworkCore;

namespace BuyRequest.Data.Repositorys.BuyRequest
{
    public class BuyRequestRepository : GenericRepository<Domain.Entities.BuyRequest>, IBuyRequestRepository
    {
        private readonly BuyRequestDataContext _context;

        public BuyRequestRepository(BuyRequestDataContext context) : base(context)
        {
            _context = context;
            SetInclude(x => x.Include(i => i.Products));
        }

    }
}
