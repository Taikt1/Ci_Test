﻿using SWP391.EventFlowerExchange.Domain.ObjectValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.EventFlowerExchange.Domain.Entities
{
    public class CreateTransaction
    {
        public int? OrderId { get; set; }

        public string Code { get; set; }

        public string? UserId { get; set; }

        public string? TransactionCode { get; set; }

        public string? TransactionContent { get; set; }

        public decimal? Amount { get; set; }

        public bool? Status { get; set; }

        public DateTime? CreatedAt { get; set; }

    }
}
