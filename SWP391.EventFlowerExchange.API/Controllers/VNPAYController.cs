using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using SWP391.EventFlowerExchange.Application;
using SWP391.EventFlowerExchange.Domain.Entities;
using SWP391.EventFlowerExchange.Domain.ObjectValues;

namespace SWP391.EventFlowerExchange.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VNPAYController : ControllerBase
    {

        private readonly IVnPayService _vnPayservice;
        private ITransactionService _service;

        public VNPAYController(IVnPayService vnPayservice, ITransactionService service)
        {
            _vnPayservice = vnPayservice;
            _service = service;
        }

        [EnableCors("MyCors")]
        [HttpPost("create-payment-link")]
        public IActionResult CreatePaymentLink( VnPayRequest model)
        {
            if (model == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid payment request.");
            }
            string paymentURL = _vnPayservice.CreatePaymentUrl(HttpContext, model);
            return Ok(paymentURL);
        }

        [HttpGet("payment-callback")]
        public async Task<IActionResult> PaymentCallback()
        {
            var response = _vnPayservice.PaymentExecute(Request.Query);
            Console.WriteLine(response);
            

            if (response == null || response.Code != "00" || response.Status != true)
            {
                response.CreatedAt = DateTime.Now;
                await _service.CreateTransactionFromAPIAsync(response);
                return Redirect("https://event-flower-exchange.vercel.app/");
            }

            response.CreatedAt = DateTime.Now;
            // Here you can save the order to the database if needed
            await _service.CreateTransactionFromAPIAsync(response);

            //return Ok(new { Message = "Payment successful", check});
            return Redirect("https://anime47.tv/xem-phim-kekkon-yubiwa-monogatari-ep-02/204546.html");
        }
    }
}
