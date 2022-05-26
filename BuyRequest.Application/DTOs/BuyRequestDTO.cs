using BuyRequest.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BuyRequest.Application.DTOs
{
    public class BuyRequestDTO
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public long Code { get; set; }
        public DateTime Date { get; set; }
        public DateTime DeliveryDate { get; set; }
        public List<ProductRequestDTO> Products { get; set; }
        public Guid ClientId { get; set; }
        public string ClientDescription { get; set; }
        public string ClientEmail { get; set; }
        public string ClientPhone { get; set; }
        public Status Status { get; set; }
        public string? StreetDescription { get; set; }
        public string? StreetNumber { get; set; }
        public string? Sector { get; set; }
        public string? Complement { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public decimal DiscountValue { get; set; }
        public decimal CostValue { get; set; }

        public decimal TotalValue => Products.Any() ? Products.Sum(x => x.Pvp * x.Quantity) - DiscountValue : 0;

        public decimal Price => Products.Any() ? Products.Sum(x => x.Pvp * x.Quantity) : 0;
    }
}
