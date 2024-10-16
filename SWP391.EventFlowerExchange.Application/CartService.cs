using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using SWP391.EventFlowerExchange.Domain.Entities;
using SWP391.EventFlowerExchange.Domain.ObjectValues;
using SWP391.EventFlowerExchange.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.EventFlowerExchange.Application
{
    public class CartService : ICartService
    {
        private ICartRepository _repo;
        private IAccountRepository _accountRepository;
        private IProductRepository _productRepository;

        public CartService(ICartRepository repo, IAccountRepository accountRepository, IProductRepository productRepository)
        {
            _repo = repo;
            _accountRepository = accountRepository;
            _productRepository = productRepository;
        }

        public async Task<IdentityResult> CreateCartFromApiAsync(Account account)
        {
            var allCarts = await _repo.ViewAllCartAsync();
            var cart = allCarts.FirstOrDefault(c => c.BuyerId == account.Id);//néu không nhập buyerid thì xet như thế nào?

            if (cart == null)
            {
                await _repo.CreateCartAsync(new Account() { Id = account.Id });
                return IdentityResult.Success;
            }

            return IdentityResult.Failed();
        }

        public async Task<IdentityResult> CreateCartItemFromApiAsync(CreateCartItem cartItem)
        {
            var allCarts = await _repo.ViewAllCartAsync();
            var cart = allCarts.FirstOrDefault(c => c.BuyerId == cartItem.BuyerId);//néu không nhập buyerid thì xet như thế nào?

            if (cart == null)
            {
                await _repo.CreateCartAsync(new Account() { Id = cartItem.BuyerId });
                allCarts = await _repo.ViewAllCartAsync();
                cart = allCarts.FirstOrDefault(c => c.BuyerId == cartItem.BuyerId);
            }

            var resultList = await _productRepository.GetInProgressProductListAsync();
            var result = resultList.FirstOrDefault(c => c.ProductId == cartItem.ProductId);

            if (result != null && result.Status.Contains("Enable"))
            {
                var itemList = await _repo.ViewAllCartItemByUserIdAsync(new Account() { Id = cartItem.BuyerId });
                var existingItem = itemList.FirstOrDefault(c => c.ProductId == result.ProductId);

                var productImage = await _productRepository.SearchProductImageByIdAsync(result);

                if (existingItem == null)
                {
                    var newItem = new CartItem
                    {
                        CartId = cart.CartId,
                        BuyerId = cartItem.BuyerId,
                        ProductId = cartItem.ProductId,
                        Quantity = 1,
                        Price = result.Price,
                        ProductImage = productImage.LinkImage,
                        CreatedAt = DateTime.UtcNow
                    };

                    await _repo.CreateCartItemAsync(newItem);//ở phần này thêm item thì bên user ko được nhập cart id 
                    return IdentityResult.Success;
                }
            }

            return IdentityResult.Failed();
        }

        public async Task<IdentityResult> RemoveItemFromCartFromApiAsync(CartItem cartItem)
        {
            var allCarts = await _repo.ViewAllCartAsync();
            var cart = allCarts.FirstOrDefault(c => c.BuyerId == cartItem.BuyerId);

            if (cart != null)
            {
                var itemList = await _repo.ViewAllCartItemByUserIdAsync(new Account() { Id = cartItem.BuyerId });
                var itemToRemove = itemList.FirstOrDefault(c => c.ProductId == cartItem.ProductId);

                if (itemToRemove != null)
                {
                    await _repo.RemoveItemFromCartAsync(itemToRemove);
                    return IdentityResult.Success;
                }
            }

            return IdentityResult.Failed();
        }

        public async Task<List<CartItem>> ViewAllCartItemByUserIdFromApiAsync(Account account)
        {
            return await _repo.ViewAllCartItemByUserIdAsync(account);
        }
    }
}
