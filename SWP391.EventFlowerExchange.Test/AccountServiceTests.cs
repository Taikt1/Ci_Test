using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SWP391.EventFlowerExchange.Application;
using SWP391.EventFlowerExchange.Domain.Entities;
using SWP391.EventFlowerExchange.Domain.ObjectValues;
using SWP391.EventFlowerExchange.Infrastructure;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SWP391.EventFlowerExchange.Domain;


[TestFixture]
//public class AccountServiceTests
//{
//    private Mock<IAccountRepository> _mockRepo;
//    private Mock<UserManager<Account>> _mockUserManager;
//    private Mock<RoleManager<IdentityRole>> _mockRoleManager;
//    private AccountService _accountService;

//    [SetUp]
//    public void Setup()
//    {
//        var store = new Mock<IUserStore<Account>>();
//        _mockUserManager = new Mock<UserManager<Account>>(store.Object, null, null, null, null, null, null, null, null);

//        _mockRepo = new Mock<IAccountRepository>();
//        _mockRoleManager = new Mock<RoleManager<IdentityRole>>(new Mock<IRoleStore<IdentityRole>>().Object, null, null, null, null);
//        _accountService = new AccountService(_mockRepo.Object, _mockUserManager.Object, _mockRoleManager.Object);
//    }

//    [Test]
//    public async Task SignUpBuyerFromAPIAsync_ValidData_ReturnsSuccess()
//    {
//        var signUpBuyer = new SignUpBuyer { Email = "buyer@example.com", Password = "Password123!" };
//        _mockRepo.Setup(repo => repo.SignUpBuyerAsync(signUpBuyer)).ReturnsAsync(IdentityResult.Success);

//        var result = await _accountService.SignUpBuyerFromAPIAsync(signUpBuyer);

//        Console.WriteLine($"SignUpBuyerFromAPIAsync result: {result.Succeeded}");
//    }

//    [Test]
//    public async Task SignInFromAPIAsync_ValidCredentials_ReturnsToken()
//    {
//        var signIn = new SignIn { Email = "test@example.com", Password = "password" };
//        _mockRepo.Setup(repo => repo.SignInAsync(signIn)).ReturnsAsync("token");

//        var result = await _accountService.SignInFromAPIAsync(signIn);

//        Console.WriteLine($"SignInFromAPIAsync result: {result}");
//    }

//    [Test]
//    public async Task DeleteAccountFromAPIAsync_AccountExists_ReturnsSuccess()
//    {
//        var account = new Account { Id = "1", Email = "test@example.com" };
//        _mockUserManager.Setup(um => um.GetRolesAsync(account)).ReturnsAsync(new List<string> { "buyer" });
//        _mockRepo.Setup(repo => repo.DeleteAccountAsync(account)).ReturnsAsync(IdentityResult.Success);

//        var result = await _accountService.DeleteAccountFromAPIAsync(account);

//        Console.WriteLine($"DeleteAccountFromAPIAsync result: {result.Succeeded}");
//    }

//    [Test]
//    public async Task GetUserByEmailFromAPIAsync_ValidEmail_ReturnsAccount()
//    {
//        var account = new Account { Email = "test@example.com" };
//        _mockRepo.Setup(repo => repo.GetUserByEmailAsync(account)).ReturnsAsync(account);

//        var result = await _accountService.GetUserByEmailFromAPIAsync(account);

//        Console.WriteLine($"GetUserByEmailFromAPIAsync result: {result?.Email}");
//    }

//    [Test]
//    public async Task ResetPasswordFromAPIAsync_ValidEmail_ReturnsTrue()
//    {
//        var email = "test@example.com";
//        var newPassword = "NewPassword123!";
//        _mockRepo.Setup(repo => repo.ResetPasswordAsync(email, newPassword)).ReturnsAsync(true);

//        var result = await _accountService.ResetPasswordFromAPIAsync(email, newPassword);

//        Console.WriteLine($"ResetPasswordFromAPIAsync result: {result}");
//    }
//}

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
        var signUpBuyer = new SignUpBuyer { Email = "buyer@example.com", Password = "Password123!" };
        _mockRepo.Setup(repo => repo.SignUpBuyerAsync(signUpBuyer)).ReturnsAsync(IdentityResult.Success);

        var result = await _accountService.SignUpBuyerFromAPIAsync(signUpBuyer);

        Console.WriteLine($"SignUpBuyerFromAPIAsync result: {result.Succeeded}");
    }

    [Test]
    public async Task SignInFromAPIAsync_ValidCredentials_ReturnsToken()
    {
        var signIn = new SignIn { Email = "test@example.com", Password = "password" };
        _mockRepo.Setup(repo => repo.SignInAsync(signIn)).ReturnsAsync("token");

        var result = await _accountService.SignInFromAPIAsync(signIn);

        Console.WriteLine($"SignInFromAPIAsync result: {result}");
    }

    [Test]
    public async Task DeleteAccountFromAPIAsync_AccountExists_ReturnsSuccess()
    {
        var account = new Account { Id = "1", Email = "test@example.com" };
        _mockUserManager.Setup(um => um.GetRolesAsync(account)).ReturnsAsync(new List<string> { "buyer" });
        _mockRepo.Setup(repo => repo.DeleteAccountAsync(account)).ReturnsAsync(IdentityResult.Success);

        var result = await _accountService.DeleteAccountFromAPIAsync(account);

        Console.WriteLine($"DeleteAccountFromAPIAsync result: {result.Succeeded}");
    }

    [Test]
    public async Task GetUserByEmailFromAPIAsync_ValidEmail_ReturnsAccount()
    {
        var account = new Account { Email = "test@example.com" };
        _mockRepo.Setup(repo => repo.GetUserByEmailAsync(account)).ReturnsAsync(account);

        var result = await _accountService.GetUserByEmailFromAPIAsync(account);

        Console.WriteLine($"GetUserByEmailFromAPIAsync result: {result?.Email}");
    }

    [Test]
    public async Task ResetPasswordFromAPIAsync_ValidEmail_ReturnsTrue()
    {
        var email = "test@example.com";
        var newPassword = "NewPassword123!";
        _mockRepo.Setup(repo => repo.ResetPasswordAsync(email, newPassword)).ReturnsAsync(true);

        var result = await _accountService.ResetPasswordFromAPIAsync(email, newPassword);

        Console.WriteLine($"ResetPasswordFromAPIAsync result: {result}");
    }
}