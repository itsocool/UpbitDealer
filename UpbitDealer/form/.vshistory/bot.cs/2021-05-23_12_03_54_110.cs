﻿using UpbitDealer.src;
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
        public DateTime LastSellDate { get; set; } = DateTime.MinValue;
        public double KRWBalance { get; set; } = 0;
        public double StartKRW { get; set; } = 0;
        public bool IsWaiting { get; set; } = false;
        public JObject LastOrderResult { get; set; }
        public int MyProperty { get; set; }

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
                Task.Run(async () =>
                {
                    await GridHalfStrategyAsync();
                });
            }
            else if (Algorithm.Id == 1)
            {
                Task.Run(async () =>
                {
                    await MinuteCandleByTicksAsync();
                });
            }
        }

        private async Task GridHalfStrategyAsync()
        {
            
        }

        private async Task MinuteCandleByTicksAsync()
        {
            // 설정값
            var coinName = Coin.Ticker;
            var candleType = CandleType.Name;
            var candleMinute = CandleType.Minute;
            var candleCount = CandleCount;
            var feeRate = FeeRate;
            var tradeRate = OrderRate;
            var triggerRate = TriggerRate;

            // 캔들 갯수 많큼 캔들 가져오기
            var candles = ApiData.getCandle<List<Candle>>(coinName, candleType, candleCount * 2);   // 캔들 조회 (2배로 여유롭게)
            var currPrice = candles.First().Close;                                                  // 현재가
            var prevPrice = candles[1].Low;                                                         // 직전저가
            var highPrice = candles.GetRange(1, candleCount - 1).Max(x => x.High);                  // 최고가
            var downPrice = highPrice * (triggerRate + (feeRate * (candleCount - 1))) / 100;        // 하락가
            var triggerDownPrice = highPrice - downPrice;                                           // 매수 하락 촉발가
            var downRate = Math.Min(0D, (currPrice - highPrice) * 100 / highPrice);                 // 하락율
            var upPrice = prevPrice * (feeRate * (candleCount - 1)) / 100;                          // 반등가
            var triggerUpPrice = prevPrice + upPrice;                                               // 매수 반등 촉발가
            var upRate = (currPrice - prevPrice) * 100 / prevPrice;                                 // 반등 상승율
            var downUpRate = upRate <= 0 && downRate <= 0 ? 0D : (-downRate / upRate);              // 반등율

            // 보유현금 및 주문 정보
            var orderChance = ApiData.getOrdersChance(coinName);                                    // 주문 가능 정보
            var bid = orderChance["bid_account"];
            var ask = orderChance["ask_account"];
            var krwBalance = bid.Value<double>("balance");                                          // 보유 현금
            var coinVol = ask.Value<double?>("balance") ?? 0D;                                      // 보유 코인 수량
            var avgPrice = ask.Value<double?>("avg_buy_price") ?? 0D;                               // 매수 평단가
            var buyLock = bid.Value<double?>("locked") ?? 0D;                                       // 매수 대기 금액
            var sellLock = ask.Value<double?>("locked") ?? 0D;                                      // 매도 대기 금액
            var coinBuyBalance = avgPrice * coinVol;                                                // 코인 매수금
            var coinBalance = currPrice * coinVol;                                                  // 코인 평가금
            var totalBalance = krwBalance + coinBalance;                                            // 현재 자산
            var minTradeKRW = Settings.Default.minTradeKRW;                                         // 최소 거래 금액
            var fee = currPrice * feeRate / 100;                                                    // 거래 수수료
            var sellFee = coinBalance * feeRate / 100;                                              // 매도 총 수수료
            var buyFee = totalBalance * feeRate / 100;                                              // 매수 총 수수료
            var buyUnitPrice = GetOrderUnitPrice(krwBalance);                                       // 매수 주문 가격 단위
            var sellUnitPrice = GetOrderUnitPrice(coinBalance);                                     // 매도 주문 가격 단위

            // 거래 쿨다운 타임
            var buyTs = (DateTime.Now - LastSellDate).TotalSeconds;                                 // 마지막 매도 이후 경과 시간 (초)
            var buyCoolDownSec = CandleType.Minute * 60 * (candleCount - 2)/(candleCount - 1);      // 매수 쿨다운 시간 (초)
            var buyRemainCoolDownSec = buyCoolDownSec - buyTs;                                      // 매수 쿨다운 시간 (초)

            var sellTs = (DateTime.Now - LastBuyDate).TotalSeconds;                                 // 마지막 매수 이후 경과 시간 (초)
            var sellCoolDownSec = (CandleType.Minute + (1/candleCount)) * 60;                       // 매도 쿨다운 시간 (초)
            var sellRemainCoolDownSec = sellCoolDownSec - sellTs;                                   // 매도 까지 남은 시간 (초)
            var targetRate = ((triggerRate / 2) + (feeRate * 2)) / 100;                             // 목표 수익율
            var targetSellPrice = Math.Round(avgPrice * (1 + targetRate) / 10) * 10;                // 매도 목표가

            var now = DateTime.Now;
            var line1 = $"[{now:T}] {coinName}";
            line1 += $" : 현재/직전/최고 {currPrice:N0}/{prevPrice:N0}/{highPrice:N0}";
            line1 += $", 하락율/반등율 {downRate:F2}/{upRate:F2}, 평단가 {avgPrice:N0}";
            line1 += (sellRemainCoolDownSec > 0) ? $", 매수쿨 {sellRemainCoolDownSec:N0}(초)" : "";
            line1 += (buyRemainCoolDownSec > 0) ? $", 매도쿨 {buyRemainCoolDownSec:N0}(초)" : "";
            var line2 = $"[{now:T}] {coinName}";
            line2 += $" : 시작자산 {StartKRW:N0}, 현재자산 {totalBalance:N0}";
            line2 += $", 매수촉발가(하락/반등) {triggerDownPrice:N0} / {triggerUpPrice:N0}, 매도 목표가 {targetSellPrice:N0}, 매수 수수료 {totalBalance * feeRate / 100:F2}";

            WriteCurrent("{0}\r\n{1}", line1, line2);

            if (StartKRW < minTradeKRW && krwBalance > minTradeKRW && coinBalance < minTradeKRW)
            {
                // 거래 시작 금액
                StartKRW = krwBalance;
            }

            if (InvokeRequired)
            {
                BeginInvoke(new Action(() =>
                {
                    txtKRWBalance.Text = krwBalance.ToString("N0");
                    txtCoinBalance.Text = coinBalance.ToString("N0");
                    txtBalance.Text = totalBalance.ToString("N0");
                    txtStartKRW.Text = StartKRW.ToString("N0");
                }));
            }
            else
            {
                txtKRWBalance.Text = krwBalance.ToString("N0");
                txtCoinBalance.Text = coinBalance.ToString("N0");
                txtBalance.Text = totalBalance.ToString("N0");
                txtStartKRW.Text = StartKRW.ToString("N0");
            }

            try
            {
                var orderResult = null as JObject;
                var order = null as JObject;
                var uuid = "";
                var side = "";
                var state = "";

                if (IsWaiting)
                {
                    // 거래 대기 중일때
                    uuid = LastOrderResult.Value<string>("uuid");
                    side = LastOrderResult.Value<string>("side");
                    state = LastOrderResult.Value<string>("state");
                    WriteLog("#### 거래 불가(거래 대기중) : side {0}, state {1}, uuid {2}", side, state, uuid);
                    return;
                }

                if (coinBalance <= minTradeKRW && krwBalance <= minTradeKRW)
                {
                    // 보유현금과 보유 코인이 최소 거래금액 보다 적으면 거래 없음
                    WriteLog("#### 거래 불가(금액 부족) : 보유현금 {0}, 코인보유금 {1}, 매도 대기 금액 {2}, 매수 대기 금액 {3}, 최소 거래 금액 {4}", krwBalance, coinBalance, buyLock, sellLock, minTradeKRW);
                    return;
                }

                if (krwBalance > minTradeKRW                                                                        // 보유 현금이 최소거래 금액 보다 클때
                    && buyRemainCoolDownSec <= 0                                                                    // 매도 유예시간이 지났을때
                    && currPrice <= Math.Truncate(triggerDownPrice) && currPrice >= Math.Truncate(triggerUpPrice)   // 현재가가 촉발 금액 사이에서 반등하고
                    && downUpRate <= candleCount * 2)                                                               // 반등율이 캔들갯수의 2배수가 넘지 않아야 함
                {
                    // BUY
                    var unitPrice = GetOrderUnitPrice(currPrice);
                    var total = Math.Truncate(coinBalance / unitPrice) * unitPrice;
                    var avgBuyPrice = currPrice;
                    orderResult = React.executeDeal(true, false, coinName, 0, 0, total);
                    IsWaiting = true;
                    LastBuyDate = DateTime.Now;
                    WriteLog("#### BUY : {0}", orderResult.ToString(Formatting.Indented));
                    
                    var buyUUID = orderResult.Value<string>("uuid");
                    var buyOrderResult = await GetBuyResultAsync(buyUUID);

                    // SELL
                    if(buyOrderResult != null)
                    {
                        var buyPrice = buyOrderResult.Value<double>("price");
                        var targetBuyPrice = GetOrderUnitPrice(buyPrice + Math.Max((buyPrice * 0.5 / 100) + (fee * 2), 10));
                        var buyVol = buyOrderResult.Value<double>("executed_volume");
                        var sellResult = await GetSellResultAsync(coinName, targetBuyPrice, buyVol);
                        IsWaiting = false;
                        LastBuyDate = DateTime.Now;
                        WriteLog("#### SELL : {0}", sellResult.ToString(Formatting.Indented));

                        if (sellResult != null)
                        {
                            var sellUuid = sellResult.Value<string>("uuid");
                            var sellState = sellResult.Value<string>("state");
                            var sellPrice = sellResult.Value<double?>("price") ?? 0D;
                            var sellVolume = sellResult.Value<double?>("volume") ?? 0D;

                            var tradeCount = sellResult.Value<double>("trade_count");
                            var trades = order["trades"] as JArray;
                            var tradePrice = 0D;
                            var tradeVolume = 0D;
                            var tradeFunds = 0D;

                            if (trades != null && trades.Count > 0)
                            {
                                tradePrice = trades[0].Value<double?>("price") ?? 0D;
                                tradeVolume = trades[0].Value<double?>("volume") ?? 0D;
                                tradeFunds = trades[0].Value<double?>("funds") ?? 0D;
                            }

                            // SELL
                            WriteLog("#### {0} SELL : 금액 {1:N0} : 평단가 {2:N0} : 수량 {3:F6}", coinName, tradeFunds, tradePrice, tradeVolume);

                            // 수익
                            var balance = GetBalance(coinName);
                            krwBalance = balance.KRWBalance;
                            coinBalance = currPrice * balance.CoinVol;
                            totalBalance = krwBalance + coinBalance;
                            var profit = totalBalance - StartKRW;
                            var profitRate = (StartKRW == 0) ? 0D : profit / StartKRW * 100;

                            if (InvokeRequired)
                            {
                                BeginInvoke(new Action(() =>
                                {
                                    txtKRWBalance.Text = krwBalance.ToString("N0");
                                    txtCoinBalance.Text = coinBalance.ToString("N0");
                                    txtBalance.Text = totalBalance.ToString("N0");
                                    txtProfitPrice.Text = profit.ToString("N0");
                                    txtProfitRate.Text = profitRate.ToString("F4");
                                }));
                            }
                            else
                            {
                                txtKRWBalance.Text = krwBalance.ToString("N0");
                                txtCoinBalance.Text = coinBalance.ToString("N0");
                                txtBalance.Text = totalBalance.ToString("N0");
                                txtProfitPrice.Text = profit.ToString("N0");
                                txtProfitRate.Text = profitRate.ToString("F4");
                            }

                            var args = new object[] { coinName, StartKRW, totalBalance, profit, profitRate, krwBalance, coinBalance };
                            WriteLog("#### {0} 수익 : 거래시작금액 {1:N0}, 현재평가 금액 {2:N0}, 수익금액 {3:N0}, 수익율 {4:F6}, 보유현금 {5:N0}, 코인 평가 {6:N0}", args);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.StackTrace);
            }
        }

        private async Task<JObject> GetBuyResultAsync(string uuid)
        {
            await Task.Delay(500);

            var apiData = ApiData;
            var result = apiData.checkOrder(uuid);

            if(result != null)
            {
                WriteLog("checkOrder result : ", result.ToString(Formatting.Indented));
                var count = 0;
                var state = result.Value<string>("state");
                var succeeded = "done".Equals(state);

                try
                {
                    while (succeeded)
                    {
                        await Task.Delay(500);
                        result = apiData.checkOrder(uuid);
                        state = result?.Value<string>("state");
                        count++;

                        if(result != null && "done".Equals(state))
                        {
                            succeeded = true;
                            WriteLog("#### checkOrder : uuid {0}, state {1}", uuid, state);
                        }
                        else
                        {
                            WriteLog("#### checkOrder null");
                        }

                        if(count > 10)
                        {
                            succeeded = true;
                        }
                    }

                    return result;
                }
                catch (Exception ex)
                {
                    WriteLog(ex.StackTrace);
                }
            }

            return result;
        }

        private async Task<JObject> GetSellResultAsync(string coinName, double price, double vol, int timeOutSec = 100)
        {
            await Task.Delay(500);

            var apiData = ApiData;
            var orderResult = React.executeDeal(false, true, coinName, vol, price, 0);
            var result = null as JObject;

            if (orderResult != null)
            {
                WriteLog("### price sell orderResult : ", orderResult.ToString(Formatting.Indented));
                var orderDateTime = DateTime.Now;
                var uuid = orderResult?.Value<string>("uuid");
                var state = "";
                var succeeded = "done".Equals(state);

                try
                {
                    while (succeeded)
                    {
                        await Task.Delay(500);
                        var ts = DateTime.Now - orderDateTime;
                        result = apiData.checkOrder(uuid);
                        state = result?.Value<string>("state");

                        if (result != null && "done".Equals(state))
                        {
                            succeeded = true;
                            WriteLog("#### price sell result : {0}", result.ToString(Formatting.Indented));
                        }
                        else
                        {
                            WriteLog("#### price sell result null");
                        }

                        if (ts.TotalSeconds > timeOutSec)
                        {
                            apiData.cancelOrder(uuid);
                            orderResult = React.executeDeal(false, false, coinName, vol, 0, 0);
                            await Task.Delay(1500);
                            uuid = orderResult.Value<string>("uuid");
                            result = apiData.checkOrder(uuid);
                            WriteLog("#### market sell checkOrder : {0}", result.ToString(Formatting.Indented));
                            succeeded = true;
                            break;
                        }
                    }

                    return result;
                }
                catch (Exception ex)
                {
                    WriteLog(ex.StackTrace);
                }
            }

            return result;
        }

        private Balance GetBalance(string coinName)
        {
            var apiData = ApiData;
            var balance = new Balance();
            var asset = apiData.getAsset();
            var krwAsset = asset.Where(x => "KRW".Equals(x.Value<string>("currency"))).FirstOrDefault();
            var coinAsset = asset.Where(x => coinName.Equals(x.Value<string>("currency")) && "KRW".Equals(x.Value<string>("unit_currency"))).FirstOrDefault();

            WriteLog("#### asset : {0}", asset.ToString(Formatting.Indented));

            if (krwAsset != null)
            {
                balance.KRWBalance = krwAsset.Value<double?>("balance") ?? 0D;
            }

            if (coinAsset != null)
            {
                balance.CoinVol = coinAsset.Value<double?>("balance") ?? 0D;
                balance.AvgBuyPrice = coinAsset.Value<double?>("avg_buy_price") ?? 0D;
            }

            return balance;
        }

        private double GetOrderUnitPrice(double orderPrice)
        {
            var unitPrice = 1D;

            // 원화 마켓 주문 가격 단위
            if (orderPrice >= 2_000_000)
            {
                unitPrice = 1000;
            }
            else if (orderPrice >= 1_000_000 && orderPrice < 2_000_000)
            {
                unitPrice = 500;
            }
            else if (orderPrice >= 500_000 && orderPrice < 1_000_000)
            {
                unitPrice = 100;
            }
            else if (orderPrice >= 100_000 && orderPrice < 500_000)
            {
                unitPrice = 50;
            }
            else if (orderPrice >= 10_000 && orderPrice < 100_000)
            {
                unitPrice = 10;
            }
            else if (orderPrice >= 1_000 && orderPrice < 10_000)
            {
                unitPrice = 5;
            }

            return unitPrice;
        }

        private double ToOrderPrice(double price, double x)
        {
            var orderKRW = price - (price * FeeRate / 100);
            var unitPrice = GetOrderUnitPrice(orderKRW);
            var orderPrice = Math.Truncate(orderKRW / unitPrice) * unitPrice;

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
            IsWaiting = false;
            LastBuyDate = DateTime.MinValue;
            LastSellDate = DateTime.MinValue;
            LastOrderResult = null;
            ReadInput();

            if (Timer != null)
            {
                Timer.Stop();
                WriteLog("#### FINISH TIMER");
            }
        }

        private void butClear_Click(object sender, EventArgs e)
        {
            txtLog.Text = "";
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            IsWaiting = false;
            LastBuyDate = DateTime.MinValue;
            LastSellDate = DateTime.MinValue;
            LastOrderResult = null;
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