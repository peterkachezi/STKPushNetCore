using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STKPushDotNetCore.DTO
{
    public class DarajaResponseAfterUserEntersPin
    {
        public Body Body { get; set; }
    }

    public class Body
    {
        public Stkcallback stkCallback { get; set; }
    }

    public class Stkcallback
    {
        public string MerchantRequestID { get; set; }
        public string CheckoutRequestID { get; set; }
        public int ResultCode { get; set; }
        public string ResultDesc { get; set; }
        public Callbackmetadata CallbackMetadata { get; set; }
    }

    public class Callbackmetadata
    {
        public Item[] Item { get; set; }
    }

    public class Item
    {
        public string Name { get; set; }
        public object Value { get; set; }
    }

}