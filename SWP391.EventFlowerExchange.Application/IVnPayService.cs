﻿using Microsoft.AspNetCore.Http;
using SWP391.EventFlowerExchange.Domain.Entities;
using SWP391.EventFlowerExchange.Domain.ObjectValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.EventFlowerExchange.Application
{
    public interface IVnPayService
    {
        public string CreatePaymentUrl(HttpContext context, VnPayRequest model);
        public CreateTransaction PaymentExecute(IQueryCollection collections);
    }
}
