using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpbitDealer.src
{
    [Serializable]
    public class TradeResult
    {
        public string Side { get; set; }
        public double Price { get; set; } = 0D;
        public double AvgPrice { get; set; } = 0D;
        public double Volume { get; set; } = 0D;
    }
}
