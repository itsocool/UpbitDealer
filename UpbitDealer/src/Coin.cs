using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpbitDealer.src
{
    [Serializable]
    public class Coin
    {
        public string Ticker { get; set; }
        public string CoinName { get; set; }
    }
}
