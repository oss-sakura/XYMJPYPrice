using System.Runtime.Serialization;

namespace XYMJPYPrice.ExchangeAPI.JSON
{
    // API Document
    // https://lightning.bitflyer.com/docs?lang=ja#ticker

    [DataContract]
    public class BitbankTicker
    {
        [DataMember(Name = "success")]
        public int success { get; set; }
        [DataMember(Name = "data")]
        public Data data { get; set; }

        [DataContract]
        public class Data
        {
            [DataMember(Name = "sell")]
            public string sell { get; set; }
            [DataMember(Name = "buy")]
            public string buy { get; set; }
            [DataMember(Name = "open")]
            public string open { get; set; }
            [DataMember(Name = "high")]
            public string high { get; set; }
            [DataMember(Name = "low")]
            public string low { get; set; }
            [DataMember(Name = "last")]
            public string last { get; set; }
            [DataMember(Name = "vol")]
            public string vol { get; set; }
            [DataMember(Name = "timestamp")]
            public long timestamp { get; set; }
        }
    }
}
