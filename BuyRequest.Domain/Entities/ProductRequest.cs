using BuyRequest.Domain.Entities.Enums;
using Infrastructure.BaseClass;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuyRequest.Domain.Entities
{
    public class ProductRequest : EntityBase
    {
        private decimal _total;

        //public Guid Id { get; set; } = Guid.NewGuid();

        //[ForeignKey("FK_BuyRequests")]
        public Guid BuyRequestId { get; set; }
        public virtual BuyRequest BuyRequest { get; set; }
        public Guid ProductId { get; set; } = Guid.NewGuid();
        public string ProductDescription { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public decimal Quantity { get; set; }
        public decimal Pvp { get; set; }
        //public decimal Total { get; set; }

        public decimal Total
        {
            get { return _total; }
            set { _total = Convert.ToDecimal((Quantity * Pvp).ToString("N2")); }
        }

        //[JsonIgnore]
        //public virtual BuyRequest BuyRequests { get; set; }
    }
}
