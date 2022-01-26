using System;
using System.Runtime.Serialization;

namespace XYMJPYPrice.ExchangeAPI.JSON
{
    // GMOコイン ticker api document.
    // https://api.coin.z.com/docs/#ticker

    [DataContract]
    public class GMOCoinTicker
    {
        [DataMember(Name = "status")]
        public int status { get; set; }
        [DataMember(Name = "data")]
        public Datum[] data { get; set; }
        [DataMember(Name = "responsetime")]
        public DateTime responsetime { get; set; }

        [DataContract]
        public class Datum
        {
            [DataMember(Name = "ask")]
            public string ask { get; set; }
            [DataMember(Name = "bid")]
            public string bid { get; set; }
            [DataMember(Name = "high")]
            public string high { get; set; }
            [DataMember(Name = "last")]
            public string last { get; set; }
            [DataMember(Name = "low")]
            public string low { get; set; }
            [DataMember(Name = "symbol")]
            public string symbol { get; set; }
            [DataMember(Name = "timestamp")]
            public DateTime timestamp { get; set; }
            [DataMember(Name = "volume")]
            public string volume { get; set; }
        }
    }
}
