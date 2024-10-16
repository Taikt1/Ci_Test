using SWP391.EventFlowerExchange.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.EventFlowerExchange.Infrastructure
{
    public interface ITransactionRepository
    {
        public Task<bool> CreateTransactionAsync(CreateTransaction newValue);


    }
}
