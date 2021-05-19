using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpbitDealer.src
{
    [Serializable]
    public class Candle
    {
        [JsonProperty("opening_price")]
        public double Open { get; set; } = 0D;
        [JsonProperty("high_price")]
        public double High { get; set; } = 0D;
        [JsonProperty("low_price")]
        public double Low { get; set; } = 0D;
        [JsonProperty("trade_price")]
        public double Close { get; set; } = 0D;
        [JsonProperty("candle_acc_trade_volume")]
        public double Volume { get; set; } = 0D;
        public double Change { get => (Close - Open); }
        public double Ratio { get => Change / Close * 100; }
    }
}
