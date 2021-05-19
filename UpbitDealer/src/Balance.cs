using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpbitDealer.src
{
    [Serializable]
    public class Balance
    {
        public string CoinName { get; set; }
        public double KRWBalance { get; set; } = 0D;
        public double CoinVol { get; set; } = 0D;
        public double AvgBuyPrice { get; set; } = 0D;
        //public double CoinBalance { get; set; } = 0D;
        //public double TotalBalance { get; set; } = 0D;
        //public double KrwProfit { get; set; } = 0D;
        //public double KrwProfitRate { get; set; } = 0D;
    }
}
