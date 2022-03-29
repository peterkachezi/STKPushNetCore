using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using STKPushDotNetCore.DTO;
using STKPushDotNetCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace STKPushDotNetCore.Controllers
{
    public class STKPaymentController : Controller
    {
        private readonly ApplicationDbContext context;

        public STKPaymentController(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> MpesaPayment()
        {
            try
            {
                //var msisdn = formatPhoneNumber(PhoneNumber);

                string PhoneNumber = "254113456740";

                string url = @"https://api.safaricom.co.ke/mpesa/stkpush/v1/processrequest";

                HttpClient client = new HttpClient();

                var key = "QQGUDGoGV9WoXMbTzxPRMnguUkL8SqG5";

                var secrete = "0HkuMBkfO00JIhF2";

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + await generateAccessToken(key, secrete));



                var mpesaExpressRequestDTO = new MpesaExpressRequestDTO
                {
                    BusinessShortCode = 328108,

                    Password = GeneratePassword(),

                    Timestamp = DateTime.Now.ToString("yyyyMMddHHmmss"),

                    TransactionType = "CustomerPayBillOnline",

                    Amount = 1,

                    PartyA = PhoneNumber,

                    PartyB = 328108,

                    PhoneNumber = PhoneNumber,

                    CallBackURL = "https://899d-41-139-157-251.ngrok.io/Home/SaveCallBack",

                    AccountReference = "ref",

                    TransactionDesc = "Testing stk push on api"

                };

                // HttpResponseMessage result = await client.PostAsJsonAsync(url, mpesaExpressRequestDTO);

                string post_params = JsonConvert.SerializeObject(mpesaExpressRequestDTO);

                HttpContent content = new StringContent(post_params, Encoding.UTF8, "application/json");

                HttpResponseMessage result = await client.PostAsync(url, content);



                result.EnsureSuccessStatusCode();

                var response = await result.Content.ReadAsStringAsync();

                var mpesaExpressResponse = JsonConvert.DeserializeObject<MpesaExpressResponseDTO>(response);

                var mpesaResponse = new MpesaExpressResponseDTO
                {
                    MerchantRequestID = mpesaExpressResponse.MerchantRequestID,

                    CheckoutRequestID = mpesaExpressResponse.CheckoutRequestID,

                    ResponseCode = mpesaExpressResponse.ResponseCode,

                    ResponseDescription = mpesaExpressResponse.ResponseDescription,

                    CustomerMessage = mpesaExpressResponse.CustomerMessage,
                };

                var h = await SaveResponse(mpesaResponse);

                return View("STKSuccess");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }

        }

        public async Task<MpesaExpressResponseDTO> SaveResponse(MpesaExpressResponseDTO mpesaExpressResponseDTO)
        {
            try
            {              

                    var s = new CheckoutRequest
                    {
                        MerchantRequestID = mpesaExpressResponseDTO.MerchantRequestID,

                        CheckoutRequestID = mpesaExpressResponseDTO.CheckoutRequestID,

                        ResponseCode = mpesaExpressResponseDTO.ResponseCode,

                        ResponseDescription = mpesaExpressResponseDTO.ResponseDescription,

                        CustomerMessage = mpesaExpressResponseDTO.CustomerMessage,
                    };

                    context.CheckoutRequests.Add(s);

                    await context.SaveChangesAsync();

                    return mpesaExpressResponseDTO;
              
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        public static string GeneratePassword()
        {
            try
            {
                var lipa_time = DateTime.Now.ToString("yyyyMMddHHmmss");

                var passkey = "c308930e0ea0919259e32eeaf0d04f10e79d26954f52eced2436c8a7ff2ed0fc";

                int BusinessShortCode = 328108;

                var timestamp = lipa_time;

                var passwordBytes = System.Text.Encoding.UTF8.GetBytes(BusinessShortCode + passkey + timestamp);

                var password = Convert.ToBase64String(passwordBytes);

                return password;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public async Task<string> generateAccessToken(string key, string secrete)
        {
            try
            {
                var url = @"https://api.safaricom.co.ke/oauth/v1/generate?grant_type=client_credentials";

                HttpClient client = new HttpClient();

                var byteArray = System.Text.Encoding.ASCII.GetBytes(key + ":" + secrete);

                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                HttpResponseMessage response = await client.GetAsync(url);

                HttpContent content = response.Content;

                string result = await content.ReadAsStringAsync();

                var mpesaAccessToken = JsonConvert.DeserializeObject<MpesaAccessTokenDTO>(result);

                return mpesaAccessToken.access_token;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
    }
}
