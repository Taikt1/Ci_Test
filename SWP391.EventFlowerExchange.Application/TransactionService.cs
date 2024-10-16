using SWP391.EventFlowerExchange.Domain.Entities;
using SWP391.EventFlowerExchange.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.EventFlowerExchange.Application
{
    public class TransactionService : ITransactionService
    {
        private ITransactionRepository _repo;

        public TransactionService(ITransactionRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> CreateTransactionFromAPIAsync(CreateTransaction newValue)
        {
            return await _repo.CreateTransactionAsync(newValue);
        }
    }
}
