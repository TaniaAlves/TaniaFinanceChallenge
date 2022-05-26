using BuyRequest.Application.DTOs;
using BuyRequest.Domain.Entities.Enums;
using Infrastructure.Paging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BuyRequest.Application.Interfaces
{
    public interface IBuyRequestService
    {
        Task<Domain.Entities.BuyRequest> PostAsync(BuyRequestDTO orderInput);
        Task<IEnumerable<Domain.Entities.BuyRequest>> GetAll(Page page);
        Task<Domain.Entities.BuyRequest> GetById(Guid id);
        Task<Domain.Entities.BuyRequest> GetByClientIdAsync(Guid clientId);
        Task<Domain.Entities.BuyRequest> UpdateByIdAsync(BuyRequestDTO orderInput);
        Task<Domain.Entities.BuyRequest> UpdateState(Guid id, Status status);
        Task<Domain.Entities.BuyRequest> DeleteById(Guid id);
    }
}
