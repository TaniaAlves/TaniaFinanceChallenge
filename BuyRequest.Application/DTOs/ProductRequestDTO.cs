using BuyRequest.Domain.Entities.Enums;
using System;

namespace BuyRequest.Application.DTOs
{
    public class ProductRequestDTO
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string ProductDescription { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public decimal Quantity { get; set; }
        public decimal Pvp { get; set; }
        public decimal Total => Pvp * Quantity;
    }
}
