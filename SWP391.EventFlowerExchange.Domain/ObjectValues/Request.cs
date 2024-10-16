﻿using System;
using System.Collections.Generic;

namespace SWP391.EventFlowerExchange.Domain.ObjectValues;

public partial class Request
{
    public int RequestId { get; set; }

    public string? UserId { get; set; }

    public string? RequestType { get; set; }

    public int? TransactionId { get; set; }

    public decimal? Amount { get; set; }

    public int? ProductId { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Product? Product { get; set; }

    public virtual Transaction? Transaction { get; set; }

    public virtual Account? User { get; set; }
}
