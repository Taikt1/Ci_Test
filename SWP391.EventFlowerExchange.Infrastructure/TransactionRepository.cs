using SWP391.EventFlowerExchange.Domain;
using SWP391.EventFlowerExchange.Domain.Entities;
using SWP391.EventFlowerExchange.Domain.ObjectValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.EventFlowerExchange.Infrastructure
{
    public class TransactionRepository : ITransactionRepository
    {
        private Swp391eventFlowerExchangePlatformContext _context;


        public async Task<bool> CreateTransactionAsync(CreateTransaction newValue)
        {
            Transaction transaction = new Transaction() { 
                Amount = newValue.Amount,
                CreatedAt = newValue.CreatedAt,
                OrderId = newValue.OrderId,
                UserId = newValue.UserId,
                Status = newValue.Status,
                TransactionCode = newValue.TransactionCode, 
                TransactionContent = newValue.TransactionContent,
            };
            _context = new Swp391eventFlowerExchangePlatformContext();
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
