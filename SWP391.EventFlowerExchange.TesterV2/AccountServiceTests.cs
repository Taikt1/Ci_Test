using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using SWP391.EventFlowerExchange.Application;
using SWP391.EventFlowerExchange.Domain.Entities;
using SWP391.EventFlowerExchange.Domain.ObjectValues;
using SWP391.EventFlowerExchange.Domain;
using SWP391.EventFlowerExchange.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.EventFlowerExchange.Test
{
    public class AccountServiceTests
    {
        private Mock<IAccountRepository> _mockRepo;
        private Mock<UserManager<Account>> _mockUserManager;
        private Mock<RoleManager<IdentityRole>> _mockRoleManager;
        private AccountService _accountService;
        private DbContextOptions<Swp391eventFlowerExchangePlatformContext> _dbContextOptions;

        [SetUp]
        public void Setup()
        {
            _dbContextOptions = new DbContextOptionsBuilder<Swp391eventFlowerExchangePlatformContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            var store = new Mock<IUserStore<Account>>();
            _mockUserManager = new Mock<UserManager<Account>>(store.Object, null, null, null, null, null, null, null, null);
            _mockRepo = new Mock<IAccountRepository>();
            _mockRoleManager = new Mock<RoleManager<IdentityRole>>(new Mock<IRoleStore<IdentityRole>>().Object, null, null, null, null);
            _accountService = new AccountService(_mockRepo.Object, _mockUserManager.Object, _mockRoleManager.Object);
        }

        [Test]
        public async Task SignUpBuyerFromAPIAsync_ValidData_ReturnsSuccess()
        {
            // Arrange
            var signUpBuyer = new SignUpBuyer { Email = "buyer@example.com", Password = "Password123!" };
            _mockRepo.Setup(repo => repo.SignUpBuyerAsync(signUpBuyer)).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _accountService.SignUpBuyerFromAPIAsync(signUpBuyer);

            // Assert
            Assert.IsNotNull(result); // Kiểm tra kết quả không null
            Assert.IsTrue(result.Succeeded); // Kết quả phải thành công
        }

        [Test]
        public async Task SignInFromAPIAsync_ValidCredentials_ReturnsToken()
        {
            // Arrange
            var signIn = new SignIn { Email = "test@example.com", Password = "password" };
            _mockRepo.Setup(repo => repo.SignInAsync(signIn)).ReturnsAsync("token");

            // Act
            var result = await _accountService.SignInFromAPIAsync(signIn);

            // Assert
            Assert.IsNotNull(result); // Kết quả không null
            Assert.AreEqual("token", result); // Token phải chính xác
        }

        [Test]
        public async Task DeleteAccountFromAPIAsync_AccountExists_ReturnsSuccess()
        {
            // Arrange
            var account = new Account { Id = "1", Email = "test@example.com" };
            _mockUserManager.Setup(um => um.GetRolesAsync(account)).ReturnsAsync(new List<string> { "buyer" });
            _mockRepo.Setup(repo => repo.DeleteAccountAsync(account)).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _accountService.DeleteAccountFromAPIAsync(account);

            // Assert
            Assert.IsNotNull(result); // Kết quả không null
            Assert.IsTrue(result.Succeeded); // Xóa thành công
        }

        [Test]
        public async Task GetUserByEmailFromAPIAsync_ValidEmail_ReturnsAccount()
        {
            // Arrange
            var account = new Account { Email = "test@example.com" };
            _mockRepo.Setup(repo => repo.GetUserByEmailAsync(account)).ReturnsAsync(account);

            // Act
            var result = await _accountService.GetUserByEmailFromAPIAsync(account);

            // Assert
            Assert.IsNotNull(result); // Tài khoản phải không null
            Assert.AreEqual("test@example.c" +
                "", result?.Email); // Kiểm tra email chính xác
        }

        [Test]
        public async Task ResetPasswordFromAPIAsync_ValidEmail_ReturnsTrue()
        {
            // Arrange
            var email = "test@example.com";
            var newPassword = "NewPassword123!";
            _mockRepo.Setup(repo => repo.ResetPasswordAsync(email, newPassword)).ReturnsAsync(true);

            // Act
            var result = await _accountService.ResetPasswordFromAPIAsync(email, newPassword);

            // Assert
            Assert.IsTrue(result); // Kiểm tra rằng reset password thành công
        }
    }
}

