using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpbitDealer.src
{
    [Serializable]
    public class Profit
    {
        public string CoinName { get; set; }
        public double KRWBalance { get; set; } = 0D;
        public double CoinVol { get; set; } = 0D;
        public double AvgBuyPrice { get; set; } = 0D;
        public double CoinAmount { get; set; } = 0D;
        public double TotalKrw { get; set; } = 0D;
        public double KrwProfit { get; set; } = 0D;
        public double KrwProfitRate { get; set; } = 0D;

        public Profit(ApiData apiData, string coinName, double startKrw)
        {
            CoinName = coinName;

            var asset = apiData.getAsset();
            var krwAsset = asset.Where(x => "KRW".Equals(x.Value<string>("currency"))).FirstOrDefault();
            var coinAsset = asset.Where(x => coinName.Equals(x.Value<string>("currency"))).FirstOrDefault();

            if (krwAsset != null)
            {
                KRWBalance = krwAsset.Value<double>("balance");
            }

            if (coinAsset != null)
            {
                CoinVol = coinAsset.Value<double>("balance");
                AvgBuyPrice = coinAsset.Value<double>("avg_buy_price");
                CoinAmount = CoinVol * AvgBuyPrice;
            }

            TotalKrw = KRWBalance + CoinAmount;
            KrwProfit = TotalKrw - startKrw;
            KrwProfitRate = KrwProfit / startKrw * 100;
        }
    }
}
