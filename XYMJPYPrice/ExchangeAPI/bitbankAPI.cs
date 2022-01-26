using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using XYMJPYPrice.Base;
using XYMJPYPrice.ExchangeAPI.JSON;
using XYMJPYPrice.Net;

namespace XYMJPYPrice.ExchangeAPI
{
    public class BitbankAPI : BaseTickerAPI
    {
        private Http HttpAccess { get; set; }
        private BitbankTicker DeserializedResponse { get; set; }

        private float _ask;
        private float _bid;
        private float _last;
        private int _statusCode;

        public BitbankAPI()
        {
            if (HttpAccess == null) { HttpAccess = new Http(); }
        }

        public override void GetTicker()
        {
            string endpoint = "https://public.bitbank.cc";
            string pair = "xym_jpy";
            string api = "ticker";

            HttpAccess.Url = $"{endpoint}/{pair}/{api}";
            (int statusCode, string response) = HttpAccess.HttpContent().Result;

            if (HttpStatusCode.OK == (HttpStatusCode)statusCode)
            {
                DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(BitbankTicker));
                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(response), false))
                {
                    DeserializedResponse = js.ReadObject(ms) as BitbankTicker;
                }
                _ = float.TryParse(DeserializedResponse.data.sell, out _ask);
                _ = float.TryParse(DeserializedResponse.data.buy, out _bid);
                _ = float.TryParse(DeserializedResponse.data.last, out _last);
                _statusCode = statusCode;
            }
        }

        public override float GetAsk() { return _ask; }

        public override float GetBid() { return _bid; }

        public override float GetLast() { return _last; }

        public override int GetStatusCode() { return _statusCode; }
    }
}
