using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpbitDealer.src
{
    [Serializable]
    public class OrderChance
    {
        public double KRWBalance { get; set; } = 0D;
        public double CoinVol { get; set; } = 0D;
        public double AvgBuyPrice { get; set; } = 0D;
    }
}
