using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using XYMJPYPrice.Base;
using XYMJPYPrice.ExchangeAPI.JSON;
using XYMJPYPrice.Net;

namespace XYMJPYPrice.ExchangeAPI
{
    public class ZaifAPI : BaseTickerAPI
    {

        private Http HttpAccess { get; set; }
        private ZaifTicker DeserializedResponse { get; set; }

        private float _ask;
        private float _bid;
        private float _last;
        private int _statusCode;

        public ZaifAPI()
        {
            if (HttpAccess == null) { HttpAccess = new Http(); }
        }

        public override void GetTicker()
        {
            string endpoint = "https://api.zaif.jp/api/1";
            string api = "ticker";
            string currency_pair = "xym_jpy";

            HttpAccess.Url = $"{endpoint}/{api}/{currency_pair}";
            (int statusCode, string response) = HttpAccess.HttpContent().Result;

            if (HttpStatusCode.OK == (HttpStatusCode)statusCode)
            {
                DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(ZaifTicker));
                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(response), false))
                {
                    DeserializedResponse = js.ReadObject(ms) as ZaifTicker;
                }
                _ask = DeserializedResponse.ask;
                _bid = DeserializedResponse.bid;
                _last = DeserializedResponse.last;
                _statusCode = statusCode;
            }
        }

        public override float GetAsk() { return _ask; }

        public override float GetBid() { return _bid; }

        public override float GetLast() { return _last; }

        public override int GetStatusCode() { return _statusCode; }
    }
}
