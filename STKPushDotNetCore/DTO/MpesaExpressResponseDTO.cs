using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STKPushDotNetCore.DTO
{
    public class MpesaExpressResponseDTO
    {
        public string MerchantRequestID { get; set; }

        public string CheckoutRequestID { get; set; }

        public string ResponseCode { get; set; }

        public string ResponseDescription { get; set; }

        public string CustomerMessage { get; set; }
    }
}