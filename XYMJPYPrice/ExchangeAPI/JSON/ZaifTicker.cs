using System.Runtime.Serialization;

namespace XYMJPYPrice.ExchangeAPI.JSON
{
    // Zaif ticker api document.
    // https://techbureau-api-document.readthedocs.io/ja/latest/public/2_individual/4_ticker.html

    [DataContract]
    public class ZaifTicker
    {
        [DataMember(Name = "last")]
        public float last { get; set; }
        [DataMember(Name = "high")]
        public float high { get; set; }
        [DataMember(Name = "low")]
        public float low { get; set; }
        [DataMember(Name = "vwap")]
        public float vwap { get; set; }
        [DataMember(Name = "volume")]
        public float volume { get; set; }
        [DataMember(Name = "bid")]
        public float bid { get; set; }
        [DataMember(Name = "ask")]
        public float ask { get; set; }
    }
}
