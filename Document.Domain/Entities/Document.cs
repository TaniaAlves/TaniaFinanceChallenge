﻿using Document.Domain.Entities.Enums;
using Infrastructure.BaseClass;
using System;

namespace Document.Domain.Entities
{
    public class Document : EntityBase
    {
        private DateTimeOffset? _date;
        //public Guid Id { get; set; } = Guid.NewGuid();
        public string Number { get; set; }
        public DateTimeOffset Date { get; set; }
        public DocumentType DocType { get; set; }
        public Operation Operation { get; set; }
        public bool Paid { get; set; }
        public DateTimeOffset? PaymentDate
        {
            get
            {
                if (Paid == true)
                {
                    return _date = DateTimeOffset.Now;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                _date = value;
            }
        }
        public string Description { get; set; }
        public decimal Total { get; set; }
        public string? Observation { get; set; }
    }
}
