using Document.Domain.Entities.Enums;
using System;

namespace Document.Application.DTOs
{
    public class DocumentRequestDTO
    {
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public DocumentType DocType { get; set; }
        public Operation Operation { get; set; }
        public bool Paid { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string Description { get; set; }
        public decimal Total { get; set; }
        public string? Observation { get; set; }
    }
}
