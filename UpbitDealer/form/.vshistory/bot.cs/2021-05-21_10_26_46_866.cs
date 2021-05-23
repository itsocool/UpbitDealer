using UpbitDealer.src;
using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;
using UpbitDealer.Properties;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Activities.Statements;
using System.Threading.Tasks;
using log4net;
using Newtonsoft.Json;

namespace UpbitDealer.form
{
    public partial class Bot : Form
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Bot));
        public React React { get; set; }
        public ApiData ApiData { get; set; }
        public Timer Timer { get; set; }
        public List<Algorithm> AlgorithmList { get; set; }
        public List<CandleType> CandleTypeList { get; set; }
        public List<Coin> CoinList { get; set; }
        public Algorithm Algorithm { get; set; }
        public CandleType CandleType { get; set; }
        public double FeeRate { get; set; }
        public int OrderRate { get; set; }
        public Coin Coin { get; set; }
        public int Interval { get; set; }
        public int CandleCount { get; set; }
        public double TriggerRate { get; set; }
        public DateTime LastBuyDate { get; set; } = DateTime.Now;
        public DateTime LastSellDate { get; set; } = DateTime.Now;
        public double KRWBalance { get; set; } = 0;
        public double StartKRW { get; set; } = 0;
        public bool IsWaiting { get; set; } = false;
        public string LastTradeUUID { get; set; }
        public JObject LastTradeResult { get; set; }

        public Bot()
        {
            InitializeComponent();
            PostInit();
        }

        private void PostInit()
        {
            var accessKey = Program.Accesskey;
            var secretEky = Program.Secretkey;

            ApiData = new ApiData(accessKey, secretEky);
            React = new React(accessKey, secretEky);

            AlgorithmList = BotSetting.AlgorithmList;
            CandleTypeList = BotSetting.CandleTypeList;
            CoinList = BotSetting.CoinList;

            algorithmBindingSource.DataSource = AlgorithmList;
            candleTypeBindingSource.DataSource = CandleTypeList;
            coinBindingSource.DataSource = CoinList;

            Algorithm = AlgorithmList.Where(x => x.Id == Settings.Default.algorithm).FirstOrDefault();
            CandleType = CandleTypeList.Where(x => x.Minute == Settings.Default.candleType).FirstOrDefault();
            Coin = CoinList.Where(x => x.Ticker.Equals(Settings.Default.coin)).FirstOrDefault();

            FeeRate = Settings.Default.feeRate;
            OrderRate = Settings.Default.tradeRate;
            Interval = Convert.ToInt32(Settings.Default.interval);
            TriggerRate = Settings.Default.triggerRate;
            CandleCount = Convert.ToInt32(Settings.Default.candleCount);
        }

        private void Bot_Load(object sender, EventArgs e)
        {
            cmbAlgorithm.SelectedItem = Algorithm;
            cmbCandle.SelectedItem = CandleType;
            cmbCoin.SelectedItem = Coin;
            txtFee.Text = FeeRate.ToString();
            txtTradeRate.Text = OrderRate.ToString();
            txtInterval.Text = Interval.ToString();
            txtTriggerRate.Text = TriggerRate.ToString();
            txtCandleCount.Text = CandleCount.ToString();
        }

        private void ReadInput(bool save = true)
        {
            Algorithm = cmbAlgorithm.SelectedItem as Algorithm;
            CandleType = cmbCandle.SelectedItem as CandleType;
            Coin = cmbCoin.SelectedItem as Coin;

            FeeRate = Convert.ToDouble(txtFee.Text);
            OrderRate = Convert.ToInt32(txtTradeRate.Text);
            Interval = Convert.ToInt32(txtInterval.Text);
            TriggerRate = Convert.ToDouble(txtTriggerRate.Text);
            CandleCount = Convert.ToInt32(txtCandleCount.Text);

            if (save)
            {
                Settings.Default.algorithm = Algorithm.Id;
                Settings.Default.candleType = CandleType.Minute;
                Settings.Default.feeRate = FeeRate;
                Settings.Default.tradeRate = OrderRate;
                Settings.Default.coin = Coin.Ticker;
                Settings.Default.interval = Interval;
                Settings.Default.triggerRate = TriggerRate;
                Settings.Default.candleCount = CandleCount;

                Settings.Default.Save();
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            StartKRW = 0;       // 거래 시작 금액 초기화

            ReadInput();

            if (string.IsNullOrEmpty(Coin.Ticker) || Interval <= 0 || TriggerRate <= 0 || OrderRate <= 0 || CandleCount <= 0)
            {
                MessageBox.Show("거래 설정값을 모두 입력하세요");
                return;
            }

            Timer = new Timer();
            Timer.Interval = Interval * 1000;
            Timer.Tick += Timer_Tick;
            Timer.Start();

            WriteLog("#### START TIMER");
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            ReadInput(false);

            if (Algorithm.Id == 0)
            {
                PointHalfStrategy();
            }
            else if (Algorithm.Id == 1)
            {
                MinuteCandleByTicks();
            }
        }

        private void PointHalfStrategy()
        {
            //// 1분봉 3개를 기준으로 이전 시가 에서 현재 금액 등낙률을 가져온다.
            //// 등낙률 0.5% 이상이 되면 오르면 팔고/ 내리면 산다.
            //// 거래시 보유 현금, 보유 코인의 절반을 거래하되 거래 금액이 만원 미만인 경우 전체 금액으로 거래한다.
            //var algIdx = cmbAlgorithm.SelectedIndex;
            //var idx = (algIdx > 0) ? algIdx : Convert.ToInt32(txtMinute.Text);
            //var candleType = BotSetting.CandleTypes[idx];
            //var candles = ApiData.getCandle<List<Candle>>(CoinName, candleType, 3);
            //var prevCandle = candles.Last();            // 시가 봉
            //var currCandle = candles.First();           // 현재가 봉
            //var currPrice = currCandle.Close;
            //var prevPrice = prevCandle.Close;
            //var orderChance = GetOrderChance(ApiData, CoinName, currPrice);
            ////var ask = orderChance["ask_account"];
            ////var bid = orderChance["bid_account"];
            //var krwBalance = orderChance.KRWBalance;                            // 보유 현금
            //var coinVol = orderChance.CoinVol;                                  // 보유 코인 수량
            //var avgPrice = orderChance.AvgBuyPrice;                             // 매수 평단가
            //var coinPrice = orderChance.CoinBalance;                            // 보유 코인 금액

            ////var upRatio = (currPrice - avgPrice) / currPrice * 100;     // 평단 대비 상승폭 (%)
            ////var downRatio = (currPrice - prevPrice) / currPrice * 100;  // 현재가 대비 하락폭 (%)
            //var ratio = (currPrice - prevPrice) / currPrice * 100;          // 등락폭 (%)
            //var tradeRate = Convert.ToDouble(txtTradeRate.Text) / 100D;     // 거래 비중 (%)
            //var result = null as JObject;                                   // 거래 결과

            ////txtKRW.Text = krwBalance.ToString("N0");
            ////txtCoinBalance.Text = coinPrice.ToString("N0");

            //Debug.WriteLine("직전가: {0:N0}, 현재가 {1:N0}, ratio : {2:F4}%", prevPrice, currPrice, ratio);
            //WriteLog("직전가: {0:N0}, 현재가 {1:N0}, ratio : {2:F4}%", prevPrice, currPrice, ratio);

            //try
            //{
            //    if (avgPrice + (2 * (avgPrice * Rate / 100)) <= currPrice && coinPrice >= 5000)
            //    {
            //        // 현재가가 평단가보다 등락폭 2배 이상 올랐을때 전량 매도
            //        var vol = coinVol;
            //        vol = Math.Truncate(vol * 100000) / 100000;
            //        Debug.WriteLine("#### {2} ALL SELL : 금액 {0:N0}, 수량 {1:F6}", vol * currPrice, vol, CoinName);
            //        WriteLog("#### {2} ALL SELL : 금액 {0:N0}, 수량 {1:F6}", vol * currPrice, vol, CoinName);
            //        result = React.executeDeal(false, false, CoinName, vol, 0, 0);
            //        LastSellDate = DateTime.Now;
            //    }
            //    else if (ratio >= Rate && coinPrice >= 5000 && ((DateTime.Now - LastSellDate).TotalMinutes >= idx || ratio >= Rate * 1.5))
            //    {
            //        // 올랐을때 코인 금액 절반 팔기
            //        var vol = (coinPrice * tradeRate) > 5000 ? coinVol * tradeRate : coinVol;
            //        vol = Math.Truncate(vol * 100000) / 100000;
            //        Debug.WriteLine("#### {2} SELL : 금액 {0:N0}, 수량 {1:F6}", vol * currPrice, vol, CoinName);
            //        WriteLog("#### {2} SELL : 금액 {0:N0}, 수량 {1:F6}", vol * currPrice, vol, CoinName);
            //        result = React.executeDeal(false, false, CoinName, vol, 0, 0);
            //        LastSellDate = DateTime.Now;
            //    }
            //    else if (ratio <= -(Rate * 1.2) && krwBalance > 5000 && (DateTime.Now - LastBuyDate).TotalMinutes >= idx)
            //    {
            //        // 1.2배 내렸을때 보유 현금 절반으로 코인 사기
            //        var total = (krwBalance * tradeRate) > 5000 ? krwBalance * tradeRate : krwBalance;
            //        total = Math.Truncate(total * 1000) / 1000;
            //        Debug.WriteLine("#### {2} BUY : 금액 {0:N0}, 수량 {1:F6}", total, total / currPrice, CoinName);
            //        WriteLog("#### {2} BUY : 금액 {0:N0}, 수량 {1:F6}", total, total / currPrice, CoinName);
            //        result = React.executeDeal(true, false, CoinName, 0, 0, total);
            //        LastBuyDate = DateTime.Now;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine(ex);
            //    WriteLog(ex.StackTrace);
            //}

            //orderChance = GetOrderChance(ApiData, CoinName, currPrice);

            //if (result != null)
            //{
            //    Debug.WriteLine("#### RESULT : {0}", result.ToString());
            //    WriteLog("#### RESULT : {0}", result.ToString());
            //}
            WriteLog("#### DO NOTHING");
        }

        private void MinuteCandleByTicks()
        {
            // 설정값
            var coinName = Coin.Ticker;
            var candleType = CandleType.Name;
            var candleCount = CandleCount;
            var feeRate = FeeRate;
            var tradeRate = OrderRate;
            var triggerRate = TriggerRate;

            // 캔들 갯수 많큼 캔들 가져오기
            var candles = ApiData.getCandle<List<Candle>>(coinName, candleType, candleCount * 2);   // 캔들 조회 (2배로 여유롭게)
            var currPrice = candles.First().Close;                                                  // 현재가
            var prevPrice = candles[1].Close;                                                       // 직전종가
            var highPrice = candles.GetRange(1, candleCount - 1).Max(x => x.High);                  // 최고가
            var downPrice = highPrice * (triggerRate + (feeRate * 2)) / 100;                        // 하락가
            var triggerDownPrice = highPrice - downPrice;                                           // 매수 하락 촉발가
            var downRate = Math.Min(0D, (currPrice - highPrice) * 100 / highPrice);                 // 하락율
            var upPrice = prevPrice * ((triggerRate / candleCount) + (feeRate * 2)) / 100;          // 반등가
            var triggerUpPrice = prevPrice + upPrice;                                               // 매수 반등 촉발가
            var upRate = Math.Max(0D, (currPrice - prevPrice) * 100 / prevPrice);                   // 반등 상승율
            var downUpRate = upRate == 0 ? 0D : (-downRate / upRate);                               // 반등율

            // 보유현금 및 주문 정보
            var orderChance = ApiData.getOrdersChance(coinName);                                    // 주문 가능 정보
            var bid = orderChance["bid_account"];
            var ask = orderChance["ask_account"];
            var krwBalance = bid.Value<double>("balance");                                          // 보유 현금
            var coinVol = ask.Value<double>("balance");                                             // 보유 코인 수량
            var avgPrice = ask.Value<double>("avg_buy_price");                                      // 매수 평단가
            var buyLock = bid.Value<double?>("locked") ?? 0D;                                       // 매수 대기 금액
            var sellLock = ask.Value<double?>("locked") ?? 0D;                                      // 매도 대기 금액
            var coinBuyBalance = avgPrice * coinVol;                                                // 코인 매수금
            var coinBalance = currPrice * coinVol;                                                  // 코인 평가금
            var totalBalance = krwBalance + coinBalance;                                            // 현재 자산
            var minTradeKRW = Settings.Default.minTradeKRW;                                         // 최소 거래 금액
            avgPrice = avgPrice < minTradeKRW ? currPrice : avgPrice;

            // 거래 쿨다운 타임
            var buyTs = (DateTime.Now - LastSellDate).TotalSeconds;                                 // 마지막 매도 이후 경과 시간 (초)
            var buyCoolDownSec = CandleType.Minute * 60;                                            // 매수 쿨다운 시간 (초)
            var buyCountDownSec = buyCoolDownSec - buyTs;                                           // 매수 쿨다운 시간 (초)

            var sellTs = (DateTime.Now - LastBuyDate).TotalSeconds;                                 // 마지막 매수 이후 경과 시간 (초)
            var sellCoolDownSec = (CandleType.Minute + (1/candleCount)) * 60;                       // 매도 쿨다운 시간 (초)
            var sellCountDownSec = sellCoolDownSec - sellTs;                                        // 매도 까지 남은 시간 (초)

            var targetRate = ((triggerRate / (candleCount - 1)) + (feeRate * 2)) / 100;             // 목표 수익율
            var targetSellPrice = avgPrice + (avgPrice * targetRate);                               // 매도 목표가

            var args = new object[] {
                DateTime.Now,
                coinName,
                currPrice,
                prevPrice,
                highPrice,
                downRate,
                upRate,
                avgPrice,
                StartKRW,
                totalBalance,
                triggerDownPrice,
                triggerUpPrice,
                targetSellPrice,
                sellCountDownSec,
                buyCoolDownSec,
            };

            log.Debug(JsonConvert.SerializeObject(candles));
            log.Debug(JsonConvert.SerializeObject(orderChance));

            string format = "[{0:T}] {1} : 현재가 {2:N0}, 직전가 {3:N0}, 최고가 {4:N0}, 하락율 {5:F6}, 반등율 {6:F6}, 평단가 {7:N0}";
            format += (sellCountDownSec > 0) ? ", 매수 까지 남은 시간(초) {13:N0}" : "";
            format += (buyCoolDownSec > 0) ? ", 매도 까지 남은 시간(초) {14:N0}" : "";
            format += "\r\n[{0:T}] {1} : 시작자산 {8:N0}, 현재자산 {9:N0} 매수 하락 촉발가 {10:N0}, 매수 반등 촉발가 {11:N0}, 매도 목표가 {12:N0}";
            WriteCurrent(format, args);

            if (StartKRW < minTradeKRW && krwBalance > minTradeKRW && coinBalance < minTradeKRW)
            {
                // 거래 시작 금액
                StartKRW = krwBalance;
            }

            txtKRWBalance.Text = krwBalance.ToString("N0");
            txtCoinBalance.Text = coinBalance.ToString("N0");
            txtBalance.Text = totalBalance.ToString("N0");
            txtStartKRW.Text = StartKRW.ToString("N0");

            try
            {
                var tradeResult = null as JObject;
                var uuid = "";
                var side = "";
                var state = "";

                if (coinBalance <= minTradeKRW && krwBalance <= minTradeKRW)
                {
                    // 보유현금과 보유 코인이 최소 거래금액 보다 적으면 거래 없음
                    WriteLog("#### 거래 불가(금액 부족) : 보유현금 {0}, 코인보유금 {1}, 매도 대기 금액 {2}, 매수 대기 금액 {3}, 최소 거래 금액 {4}", krwBalance, coinBalance, buyLock, sellLock, minTradeKRW);
                    return;
                }

                if (sellCountDownSec <= 0 && buyCountDownSec <= 0)
                {
                    // 거래 쿨타임 일때
                    WriteLog("#### 거래 불가(쿨타임) : 매수 까지 남은 시간(초) {0}, 매수 까지 남은 시간(초) {1}, 마지막 매수 {2:T}, 마지막 매도 {3:T}", buyCountDownSec, sellCountDownSec, LastSellDate, LastBuyDate);
                    return;
                }

                if (IsWaiting)
                {
                    // 거래 대기 중일때
                    uuid = LastTradeResult.Value<string>("uuid");
                    side = LastTradeResult.Value<string>("side");
                    state = LastTradeResult.Value<string>("state");
                    WriteLog("#### 거래 불가(거래 대기중) : side {0}, state {1}, uuid {2}", side, state, uuid);
                    return;
                }

                if (krwBalance > minTradeKRW                                                                        // 보유 현금이 최소거래 금액 보다 클때
                    && currPrice <= Math.Truncate(triggerDownPrice) && currPrice >= Math.Truncate(triggerUpPrice)   // 현재가가 촉발 금액 사이에서 반등하고
                    && downUpRate <= candleCount * 2)                                                               // 반등율이 캔들갯수의 2배수가 넘지 않아야 함
                {
                    // BUY
                    var total = ToOrderPrice(krwBalance);
                    var avgBuyPrice = currPrice;
                    result = React.executeDeal(true, false, coinName, 0, 0, total);
                    LastBuyDate = DateTime.Now;
                    WriteLog("#### BUY : {0}", result.ToString(Formatting.Indented));

                }
                else if (coinBalance > minTradeKRW                                                                  // 코인 보유금이 최소거래 금액 보다 클때
                    && currPrice >= Math.Truncate(targetSellPrice))                                                 // 현재가가 목표 매수 금액보다 클때
                {
                    // SELL
                    var vol = coinVol;
                    vol = Math.Truncate(vol * 100000) / 100000;
                    result = React.executeDeal(false, false, coinName, vol, 0, 0);
                    LastSellDate = DateTime.Now;
                    WriteLog("#### SELL : {0}", result.ToString(Formatting.Indented));
                }

                if(result != null)
                {
                    IsWaiting = true;
                    LastTradeResult = result;
                    uuid = result.Value<string>("uuid");
                    side = result.Value<string>("side");
                    state = result.Value<string>("state");

                    System.Threading.Thread.Sleep(1500);
                    var order = ApiData.checkOrder(uuid);

                    if(order != null)
                    {
                        IsWaiting = false;
                        WriteLog("#### ORDER RESULT : {0}", order.ToString(Formatting.Indented));
                        //result = GetOrderResultAsync(uuid);
                        //result = GetOrderResult(uuid);
                        //WriteLog("#### FINISH GetOrderResultAsync : {0}", uuid);
                    }

                    if (result != null && !"cancel".Equals(buySates))
                    {


                        WriteLog("#### START GetOrderResultAsync : {0}", uuid);
                        //result = GetOrderResultAsync(uuid);
                        result = GetOrderResult(uuid);
                        WriteLog("#### FINISH GetOrderResultAsync : {0}", uuid);

                        if (result == null)
                        {
                            WriteLog("#### 거래 결과를 가져올수 없습니다.");
                            return;
                        }

                        var state = result.Value<string>("state");
                        var price = result.Value<double?>("price") ?? 0D;
                        var volume = result.Value<double?>("volume") ?? 0D;
                        var tradeCount = result.Value<double>("trade_count");
                        var trades = result["trades"] as JArray;
                        var tradePrice = trades[0].Value<double>("price");
                        var tradeVolume = trades[0].Value<double>("volume");
                        var tradeFunds = trades[0].Value<double>("funds");

                        args = new object[] { uuid, side, state, price, volume, tradeCount, tradePrice, tradeVolume, tradeFunds };
                        WriteLog("#### uuid {0}, side {1}, state {2}, price {3:N0}, volume {4:F6}, tradeCount {5:N0}, tradePrice {6:N0}, tradeVolume {7:N0}, tradeFunds {8:N0}", args);

                        if ("bid".Equals(side))
                        {
                            // BUY
                            LastBuyDate = DateTime.Now;
                            WriteLog("#### {0} BUY : 매수금 {1:N0} : 매수 평단가 {1:N0} : 수량 {3:F6}", coinName, price * volume, price, volume);

                        }
                        else if ("ask".Equals(side))
                        {
                            // SELL
                            LastSellDate = DateTime.Now;
                            WriteLog("#### {0} SELL : 매도금 {1:N0} : 매도 평단가 {1:N0} : 수량 {3:F6}", coinName, price * volume, price, volume);
                        }

                        // 수익
                        var balance = GetBalance(coinName);
                        krwBalance = balance.KRWBalance;
                        coinBalance = currPrice * balance.CoinVol;
                        totalBalance = krwBalance + coinBalance;
                        var profit = totalBalance - StartKRW;
                        var profitRate = (StartKRW == 0) ? 0D : profit / StartKRW * 100;

                        txtKRWBalance.Text = krwBalance.ToString("N0");
                        txtCoinBalance.Text = coinBalance.ToString("N0");
                        txtBalance.Text = totalBalance.ToString("N0");
                        txtProfitPrice.Text = profit.ToString("N0");
                        txtProfitRate.Text = profitRate.ToString("F4");

                        args = new object[] { coinName, StartKRW, totalBalance, profit, profitRate, krwBalance, coinBalance };
                        WriteLog("#### {0} 수익 : 거래시작금액 {1:N0}, 현재평가 금액 {2:N0}, 수익금액 {3:N0}, 수익율 {4:F6}, 보유현금 {5:N0}, 코인 평가 {6:N0}", args);
                    }

                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.StackTrace);
            }
        }



        private async Task<JObject> GetOrderResultAsync(string uuid)
        {
            await Task.Delay(500);

            var apiData = ApiData;
            var result = apiData.checkOrder(uuid);
            var state = result?.Value<string>("state");

            log.Debug("checkOrder async : " + JsonConvert.SerializeObject(result));

            try
            {
                while (!"done".Equals(state))
                {
                    await Task.Delay(500);
                    result = apiData.checkOrder(uuid);
                    state = result.Value<string>("state");
                    WriteLog("#### uuid {0}, state {1}", uuid, state);
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.StackTrace);
            }

            return result;
        }

        private JObject GetOrderResult(string uuid)
        {
            System.Threading.Thread.Sleep(500);

            var apiData = ApiData;
            var result = apiData.checkOrder(uuid);
            var state = result?.Value<string>("state");

            log.Debug("checkOrder : " + JsonConvert.SerializeObject(result));

            //while (!"done".Equals(state))
            //{
                System.Threading.Thread.Sleep(1000);
                result = apiData.checkOrder(uuid);
                state = result.Value<string>("state");
                WriteLog("#### uuid {0}, state {1}", uuid, state);
            //}

            return result;
        }


        private Balance GetBalance(string coinName)
        {
            var apiData = ApiData;
            var balance = new Balance();
            var asset = apiData.getAsset();
            var krwAsset = asset.Where(x => "KRW".Equals(x.Value<string>("currency"))).FirstOrDefault();
            var coinAsset = asset.Where(x => coinName.Equals(x.Value<string>("currency")) && "KRW".Equals(x.Value<string>("unit_currency"))).FirstOrDefault();

            if (krwAsset != null)
            {
                balance.KRWBalance = krwAsset.Value<double>("balance");
            }

            if (coinAsset != null)
            {
                balance.CoinVol = coinAsset.Value<double>("balance");
                balance.AvgBuyPrice = coinAsset.Value<double>("avg_buy_price");
            }

            return balance;
        }

        private double ToOrderPrice(double krw)
        {
            var fee = krw * FeeRate / 100;
            var orderKRW = krw - fee;
            var orderPrice = Math.Truncate(orderKRW / 1000) * 1000;     // 1000단위 절사
            var rest = orderKRW - orderPrice;

            // 5 단위 처리
            if (orderPrice >= 1_000_000 && orderPrice < 2_000_000)
            {
                orderPrice += Math.Truncate(rest / 500) * 500;
            }
            else if (orderPrice >= 100_000 && orderPrice < 500_000)
            {
                orderPrice += Math.Truncate(rest / 50) * 50;
            }

            return orderPrice;
        }

        private void WriteCurrent(string format, params object[] args)
        {
            var logText = $"{string.Format(format, args)}{Environment.NewLine}";
            log.Debug(logText);

            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate
                {
                    rtxtCurrent.Text = logText;
                });
            }
            else
            {
                rtxtCurrent.Text = logText;
            }
        }

        private void WriteLog(string format, params object[] args)
        {
            var logText = $"[{DateTime.Now:T}] {string.Format(format, args)}{Environment.NewLine}";
            log.Info(logText);

            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate
                {
                    if (txtLog.Text.Length > 1024 * 1024)
                    {
                        txtLog.Text = txtLog.Text.Substring(0, 1024);
                    }

                    if (chkAutoScroll.Checked)
                    {
                        txtLog.AppendText(logText);
                        //txtLog.ScrollToCaret();
                    }
                    else
                    {
                        txtLog.Text += logText;
                    }
                });
            }
            else
            {
                if (txtLog.Text.Length > 1024 * 1024)
                {
                    txtLog.Text = txtLog.Text.Substring(0, 1024);
                }

                if (chkAutoScroll.Checked)
                {
                    txtLog.AppendText(logText);
                    txtLog.ScrollToCaret();
                }
                else
                {
                    txtLog.Text += logText;
                }
            }
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            if(Timer != null && Timer.Enabled)
            {
                StartKRW = 0;
                Timer.Stop();
                WriteLog("#### STOP TIMER");
            }
        }

        private void butClear_Click(object sender, EventArgs e)
        {
            txtLog.Text = "";
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            ReadInput();

            if(Timer != null && Timer.Enabled)
            {
                Timer.Stop();
                Timer.Interval = Interval * 1000;
                Timer.Start();
                WriteLog("#### RESTART TIMER");
            }
            else
            {
                WriteLog("#### APPLY");
            }
        }
    }
}
