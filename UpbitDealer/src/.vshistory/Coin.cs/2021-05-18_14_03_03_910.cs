using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpbitDealer.src
{
    public class Coin
    {
        public static Dictionary<int, string> Coins = new Dictionary<int, string>()
        {
            {1, ac.CANDLE_MIN1 },
            {3, ac.CANDLE_MIN3 },
            {5, ac.CANDLE_MIN5 },
            {10, ac.CANDLE_MIN10 },
            {15, ac.CANDLE_MIN15 },
            {30, ac.CANDLE_MIN30 },
            {60, ac.CANDLE_HOUR1 }
        };

        public string Ticker { get; set; }
        public string CoinName { get; set; }
    }
}
