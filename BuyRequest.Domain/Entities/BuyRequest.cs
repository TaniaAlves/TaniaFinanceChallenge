using BuyRequest.Domain.Entities.Enums;
using Infrastructure.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BuyRequest.Domain.Entities
{
    public class BuyRequest :EntityBase
    {
        //private decimal _totalValue;

        //public Guid Id { get; set; } = Guid.NewGuid();
        public long Code { get; set; }
        public DateTimeOffset Date { get; set; }
        public DateTimeOffset DeliveryDate { get; set; }
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
        public decimal Price { get; set; }
        public decimal DiscountValue { get; set; }
        public decimal CostValue { get; set; }  //custo para produzir 
        public decimal TotalValue { get; set; }
        
        private List<ProductRequest> _products = new List<ProductRequest>();
        public List<ProductRequest> Products
        {
            get { return _products; }
            set { _products = value ?? new List<ProductRequest>(); }
        }

        //[JsonIgnore]
        //public virtual ICollection<ProductRequest> ProductRequests { get; set; }
    }
}
