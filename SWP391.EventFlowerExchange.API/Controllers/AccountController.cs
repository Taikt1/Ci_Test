﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SWP391.EventFlowerExchange.Application;
using SWP391.EventFlowerExchange.Domain.Entities;
using SWP391.EventFlowerExchange.Domain.ObjectValues;
using SWP391.EventFlowerExchange.Infrastructure;
using System.IdentityModel.Tokens.Jwt;

namespace SWP391.EventFlowerExchange.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IAccountService _service;
        private SmtpSetting _smtpSettings;

        public AccountController(IAccountService service, IOptionsMonitor<SmtpSetting> option)
        {
            _service = service;
            _smtpSettings = option.CurrentValue;
        }

        [HttpPost("SignUp/Buyer")]
        public async Task<IActionResult> SignUpBuyerFromAPIAsync(SignUpBuyer model)
        {
            var account = new Account()
            {
                Email = model.Email
            };
            if (await _service.GetUserByEmailFromAPIAsync(account) != null) 
            {
                return Ok("Email had been registered.");
            }

            
            var result = await _service.SignUpBuyerFromAPIAsync(model);
            if (result.Succeeded)
            {
                return Ok(result.Succeeded);
            }
            return Ok("Password must include digits, uppercase letters, lowercase letters, and special characters!");
        }

        [HttpPost("SignUp/Seller")]
        public async Task<ActionResult<bool>> SignUpSeller(SignUpSeller model)
        {
            var result = await _service.SignUpSellerFromAPIAsync(model);
            if (result.Succeeded)
            {
                return true;
            }
            return false;
        }

        [HttpPost("CreateAccount/Staff")]
        [Authorize(Roles = ApplicationRoles.Manager)]
        public async Task<ActionResult<bool>> CreateStaff(SignUpStaff model)
        {
            var result = await _service.CreateStaffAccountFromAPIAsync(model);
            if (result.Succeeded)
            {
                return true;
            }
            return false;
        }

        [HttpPost("CreateAccount/Shipper")]
        [Authorize(Roles = ApplicationRoles.Manager)]
        public async Task<ActionResult<bool>> CreateShipper(SignUpShipper model)
        {
            var result = await _service.CreateShipperAccountFromAPIAsync(model);
            if (result.Succeeded)
            {
                return true;
            }
            return false;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> SignInFromAPIAsync(SignIn model)
        {
            var result = await _service.SignInFromAPIAsync(model);
            if (string.IsNullOrEmpty(result))
            {
                return Unauthorized();
            }
            return Ok(result);
        }

        [HttpPost("LoginByGoogle/{token}")]
        public async Task<IActionResult> LoginGoogleFromAPIAsync(string token)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var jsonToken = jwtTokenHandler.ReadToken(token) as JwtSecurityToken;
            var email = jsonToken.Claims.First(claims => claims.Type == "email").Value;
            var name = jsonToken.Claims.First(claim => claim.Type == "name").Value;
            var picture = jsonToken.Claims.First(claim => claim.Type == "picture").Value;

            //Neu tim ko thay se tao
            if (await _service.GetUserByEmailFromAPIAsync(new Account() { Email = email }) == null)
            {
                SignUpBuyer account = new SignUpBuyer()
                {
                    Email = email,
                    Name = name,
                    Balance = 0,
                    Password = "Abc123@"
                };
                await _service.SignUpBuyerFromAPIAsync(account);
            }

            var result = await _service.SignInEmailFromAPIAsync(new SignIn() { Email = email });
            if (string.IsNullOrEmpty(result))
            {
                return Unauthorized();
            }
            return Ok(result);

        }

        [HttpPost("SendOTP")]
        [Authorize]
        public async Task <ActionResult<bool>> SendOTPFromAPIAsync(string email)
        {
            return await _service.SendOTPFromAPIAsync(email);
        }

        [HttpPost("VerifyOTP")]
        [Authorize]
        public async Task<ActionResult<bool>> VerifyOTPFromAPIAsync(string email, string otp)
        {
            return await _service.VerifyOTPFromAPIAsync(email, otp);
        }

        [HttpPost("ResetPassword")]
        [Authorize]
        public async Task<ActionResult<bool>> ResetPassword(string email, string newPassword)
        {
            return await _service.ResetPasswordFromAPIAsync(email, newPassword);
        }


        [HttpGet("GetAccountByEmail/{email}")]
        [Authorize]
        public async Task<ActionResult<Account>> GetAccountByEmailFromAPIAsync(string email)
        {
            var account = await _service.GetUserByEmailFromAPIAsync(new Account() { Email = email });
            if (account != null)
            {
                return account;
            }
            return StatusCode(StatusCodes.Status404NotFound);
        }

        [HttpGet("GetAccountById/{id}")]
        [Authorize]
        public async Task<ActionResult<Account>> GetAccountByIdFromAPIAsync(string id)
        {
            var account = await _service.GetUserByIdFromAPIAsync(new Account() { Id = id });
            if (account != null)
            {
                return account;
            }
            return StatusCode(StatusCodes.Status404NotFound);
        }


        [HttpGet("ViewAllAccount")]
        [Authorize(Roles = ApplicationRoles.Manager)]
        public async Task<IActionResult> ViewAllAccount()
        {
            try
            {
                return Ok(await _service.ViewAllAccountFromAPIAsync());
            }
            catch
            {
                return Ok("Not found!");
            }
        }


        [HttpGet("ViewAllAccount/{role}")]
        [Authorize(Roles = ApplicationRoles.Manager)]
        public async Task<IActionResult> ViewAllAccountByRole(string role)
        {

            var accounts = await _service.ViewAllAccountByRoleFromAPIAsync(role);

            if (accounts != null) return Ok(accounts);

            return Ok("Not found!");
        }

        [HttpGet("SearchAccounts/{address}")]
        [Authorize(Roles = ApplicationRoles.Manager)]
        public async Task<IActionResult> SearchAccountsByAddress(string address)
        {
            var accounts = await _service.SearchAccountsByAddressFromAPIAsync(address);

            if (accounts != null) return Ok(accounts);

            return Ok("Not found!");
        }

        [HttpGet("SearchShipper/{address}")]
        [Authorize(Roles = ApplicationRoles.Manager)]
        public async Task<IActionResult> SearchShipperByAddress(string address)
        {
            var accounts = await _service.SearchShipperByAddressFromAPIAsync(address);

            if (accounts != null) return Ok(accounts);

            return Ok("Not found!");
        }

        [HttpGet("SearchAccounts/{minSalary}/{maxSalary}")]
        [Authorize(Roles = ApplicationRoles.Manager)]
        public async Task<IActionResult> SearchAccountsBySalary(float minSalary, float maxSalary)
        {
            if (minSalary < 0 || maxSalary < 0)
            {
                return Ok("minSalary or maxSalary must not be negative number");
            }

            if (minSalary > maxSalary)
            {
                return Ok("minSalary must be less than or equal to maxSalary");
            }

            var accounts = await _service.SearchAccountsBySalaryFromAPIAsync(minSalary, maxSalary);

            if (accounts != null) return Ok(accounts);

            return Ok("Not found!");
        }

        [HttpPut("UpdateAccount")]
        [Authorize]
        public async Task<ActionResult<bool>> UpdateAccountFromAPIAsync(Account account)
        {
            var user = await _service.GetUserByIdFromAPIAsync(new Account() { Id = account.Id });
            if (user != null)
            {
                await _service.UpdateAccountFromAPIAsync(account);
                return true;
            }
            return false;
        }

        [HttpDelete("RemoveAccount/{id}")]
        [Authorize(Roles = ApplicationRoles.Manager)]
        public async Task<ActionResult<bool>> RemoveAccount(string id)
        {
            Account acc = new Account();
            acc.Id = id;
            var deleteAccount = await _service.GetUserByIdFromAPIAsync(acc);


            if (deleteAccount != null)
            {
                var result = await _service.RemoveAccountFromAPIAsync(deleteAccount);
                if (result.Succeeded)
                {
                    return true;
                }
            }

            return false;
        }

        [HttpDelete("DisableAccount/{id}")]
        [Authorize(Roles = ApplicationRoles.Manager)]
        public async Task<ActionResult<bool>> DeleteAccount(string id)
        {
            Account acc = new Account();
            acc.Id = id;
            var disableAccount = await _service.GetUserByIdFromAPIAsync(acc);

            if (disableAccount != null)
            {
                var result = await _service.DeleteAccountFromAPIAsync(disableAccount);
                if (result.Succeeded)
                {
                    return true;
                }
            }

            return false;
        }

    }
}
