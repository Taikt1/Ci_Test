﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
    public class RatingRepository : IRatingRepository
    {
        private Swp391eventFlowerExchangePlatformContext _context;

        public RatingRepository(Swp391eventFlowerExchangePlatformContext context)
        {
            _context = context;
        }

        public async Task<IdentityResult> PostRatingAsync(CreateRating rating)
        {
            _context = new Swp391eventFlowerExchangePlatformContext();

            // Tạo giỏ hàng mới
            var newRating = new Review
            {
                OrderId = rating.OrderId,
                BuyerId = rating.BuyerId,
                Rating = rating.Rating,
                Comment = rating.Comment,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Reviews.AddAsync(newRating);
            int result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return IdentityResult.Success;
            }

            return IdentityResult.Failed();
        }

        public async Task<List<Review>> ViewAllRatingByUserIdAsync(Account account)
        {
            _context = new Swp391eventFlowerExchangePlatformContext();

            var rating = await _context.Reviews.Where(x => x.BuyerId == account.Id).ToListAsync();
            if (rating != null)
            {
                return rating;
            }

            return null;
        }
    }
}
