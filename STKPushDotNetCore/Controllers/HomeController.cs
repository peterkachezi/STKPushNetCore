using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using STKPushDotNetCore.DTO;
using STKPushDotNetCore.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace STKPushDotNetCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationDbContext context;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;

            this.context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> SaveCallBack([FromBody] DarajaResponseAfterUserEntersPin darajaResponse)
        {
            try
            {
                if (darajaResponse.Body.stkCallback.ResultCode == 0)
                {
                    var s = new PaymentDTO
                    {
                        CheckoutRequestID = darajaResponse.Body.stkCallback.CheckoutRequestID,

                        MerchantRequestID = darajaResponse.Body.stkCallback.MerchantRequestID,

                        ResultCode = darajaResponse.Body.stkCallback.ResultCode,

                        ResultDesc = darajaResponse.Body.stkCallback.ResultDesc,

                        Amount = Convert.ToDecimal(darajaResponse.Body.stkCallback.CallbackMetadata.Item.Where(p => p.Name.Contains("Amount")).FirstOrDefault().Value.ToString()),

                        TransactionNumber = darajaResponse.Body.stkCallback.CallbackMetadata.Item.Where(p => p.Name.Contains("MpesaReceiptNumber")).FirstOrDefault().Value.ToString(),

                        TransactionDate = darajaResponse.Body.stkCallback.CallbackMetadata.Item.Where(p => p.Name.Contains("TransactionDate")).FirstOrDefault().Value.ToString(),

                        PhoneNumber = darajaResponse.Body.stkCallback.CallbackMetadata.Item.Where(p => p.Name.Contains("PhoneNumber")).FirstOrDefault().Value.ToString(),

                    };


                    var result = await Save(s);

                }
                return Json(darajaResponse);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        public async Task<PaymentDTO> Save(PaymentDTO paymentDTO)
        {
            try
            {

                long timestamp = long.Parse(paymentDTO.TransactionDate);

                DateTime NewTransactionDate = GetDateTimeFromInt(timestamp).Value;

                var s = new MpesaPayment

                {
                    CheckoutRequestID = paymentDTO.CheckoutRequestID,

                    MerchantRequestID = paymentDTO.MerchantRequestID,

                    ResultCode = paymentDTO.ResultCode,

                    ResultDesc = paymentDTO.ResultDesc,

                    Amount = paymentDTO.Amount,

                    TransactionNumber = paymentDTO.TransactionNumber,

                    TransactionDate = NewTransactionDate.ToString(),

                    PhoneNumber = paymentDTO.PhoneNumber,

                    FirstName = paymentDTO.FirstName,

                    LastName = paymentDTO.LastName,

                };

                context.MpesaPayments.Add(s);

                await context.SaveChangesAsync();

                return paymentDTO;


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        private static DateTime? GetDateTimeFromInt(long? dateAsLong, bool hasTime = true)
        {
            if (dateAsLong.HasValue && dateAsLong > 0)
            {
                if (hasTime)
                {
                    // sometimes input is 14 digit and sometimes 16
                    var numberOfDigits = (int)Math.Floor(Math.Log10(dateAsLong.Value) + 1);

                    if (numberOfDigits > 14)
                    {
                        dateAsLong /= (int)Math.Pow(10, (numberOfDigits - 14));
                    }
                }

                if (DateTime.TryParseExact(dateAsLong.ToString(), hasTime ? "yyyyMMddHHmmss" : "yyyyMMdd",
                                          CultureInfo.InvariantCulture,
                                          DateTimeStyles.None, out DateTime dt))
                {
                    return dt;
                }
            }

            return null;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
