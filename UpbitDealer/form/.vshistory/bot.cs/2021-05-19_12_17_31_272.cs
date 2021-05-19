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

namespace UpbitDealer.form
{
    public partial class Bot : Form
    {
        public React React { get; set; }
        public ApiData ApiData { get; set; }
        public Timer Timer { get; set; }
        public Algorithm Algorithm { get; set; }
        public CandleType CandleType { get; set; }
        public double FeeRate { get; set; }
        public int TradeRate { get; set; }
        public Coin Coin { get; set; }
        public int Interval { get; set; }
        public int CandleCount { get; set; }
        public double TriggerRate { get; set; }
        public DateTime LastBuyDate { get; set; } = DateTime.Now;
        public DateTime LastSellDate { get; set; } = DateTime.Now;
        public double StartKRW { get; set; } = 0;

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

            cmbAlgorithm.DataSource = new BindingSource(BotSetting.AlgorithmList, null);
            cmbAlgorithm.ValueMember = "Id";
            cmbAlgorithm.DisplayMember = "Name";
            cmbAlgorithm.SelectedValue = Settings.Default.algorithm;

            cmbCandle.DataSource = new BindingSource(BotSetting.CandleTypes, null);
            cmbCandle.ValueMember = "Minute";
            cmbCandle.DisplayMember = "Name";
            cmbCandle.SelectedValue = Settings.Default.candleType;

            cmbCoin.DataSource = new BindingSource(BotSetting.CoinList, null);
            cmbCoin.ValueMember = "Ticker";
            cmbCoin.DisplayMember = "CoinName";
            cmbCoin.SelectedValue = Settings.Default.coin;
        }

        private void Bot_Load(object sender, EventArgs e)
        {
            ReadSettings();
        }

        private void ReadSettings(bool bindControl = true)
        {
            Algorithm = BotSetting.AlgorithmList.Where(x => x.Id == Settings.Default.algorithm).First();
            CandleType = new CandleType(Settings.Default.candleType);
            FeeRate = Settings.Default.feeRate;
            TradeRate = Settings.Default.tradeRate;
            Coin = BotSetting.CoinList.Where(x => x.Ticker.Equals(Settings.Default.coin)).FirstOrDefault();
            Interval = Convert.ToInt32(Settings.Default.interval);
            TriggerRate = Settings.Default.triggerRate;
            CandleCount = Convert.ToInt32(Settings.Default.candleCount);

            if (bindControl)
            {
                cmbAlgorithm.SelectedItem = Algorithm;
                cmbCandle.SelectedItem = CandleType;
                txtFee.Text = FeeRate.ToString();
                txtTradeRate.Text = TradeRate.ToString();
                cmbCoin.SelectedItem = Coin;
                txtInterval.Text = Interval.ToString();
                txtTriggerRate.Text = TriggerRate.ToString();
                txtCandleCount.Text = CandleCount.ToString();
            }
        }

        private void ReadInput(bool save = true)
        {
            Algorithm = cmbAlgorithm.SelectedItem as Algorithm;
            CandleType = cmbCandle.SelectedItem as CandleType;
            FeeRate = Convert.ToDouble(txtFee.Text);
            TradeRate = Convert.ToInt32(txtTradeRate.Text);
            Coin = cmbCoin.SelectedItem as Coin;
            Interval = Convert.ToInt32(txtInterval.Text);
            TriggerRate = Convert.ToDouble(txtTriggerRate.Text);
            CandleCount = Convert.ToInt32(txtCandleCount.Text);

            if (save)
            {
                Settings.Default.algorithm = Algorithm.Id;
                Settings.Default.candleType = CandleType.Minute;
                Settings.Default.feeRate = FeeRate;
                Settings.Default.tradeRate = TradeRate;
                Settings.Default.coin = Coin.Ticker;
                Settings.Default.interval = Interval;
                Settings.Default.triggerRate = TriggerRate;
                Settings.Default.candleCount = CandleCount;

                Settings.Default.Save();
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            StartKRW = 0;

            ReadInput();

            if (string.IsNullOrEmpty(Coin.Ticker) || Interval <= 0 || TriggerRate <= 0 || TradeRate <= 0 || CandleCount <= 0)
            {
                MessageBox.Show("거래 설정값을 모두 입력하세요");
                return;
            }

            Timer = new Timer();
            Timer.Interval = Interval * 1000;
            Timer.Tick += Timer_Tick;
            Timer.Start();

            WriteLog("#### START TIMER");

            btnStart.Enabled = false;
            btnFinish.Enabled = true;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            ReadInput(false);

            if (cmbAlgorithm.SelectedIndex == 0)
            {
                PointHalfStrategy();
            }
            else if (cmbAlgorithm.SelectedIndex == 1)
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
            var tradeRate = TradeRate;
            var triggerRate = TriggerRate;

            // 해당 코인 보유금액이 있으면 매도 없으면 매수
            var candles = ApiData.getCandle<List<Candle>>(coinName, candleType, candleCount);   // 캔들 조회
            //var currCandle = candles.First();                                                 // 현재 캔들
            //var currPrice = currCandle.Close;                                                 // 현재가
            var ticker = ApiData.getTicker(coinName).FirstOrDefault();
            var currPrice = (ticker != null) ? ticker.Value<double>("trade_price") : 0D;        // 현재가

            //var orderChance = GetOrderChance(ApiData, coinName, currPrice);                   // 주문 가능 정보
            var orderChance = GetBalance(ApiData, coinName, currPrice, StartKRW);
            var krwBalance = orderChance.KRWBalance;                                            // 보유 현금
            var coinBalance = orderChance.CoinBalance;                                          // 코인 보유금
            var coinVol = orderChance.CoinVol;                                                  // 보유 코인 수량
            var avgPrice = orderChance.AvgBuyPrice;                                             // 매수 평단가
            var coinBuyBalance = avgPrice * coinVol;                                              // 코인 매수금
            var minTradeKRW = Settings.Default.minTradeKRW;                                     // 최소 거래 금액

            // 분봉 N개를 기준으로 직전 시가 에서 현재 금액 등낙률을 가져온다.
            var prevCandle = candles[1];                                                        // 직전 캔들
            var lastCandle = candles.Last();                                                    // 마지막 캔들
            var prevPrice = prevCandle.Low;                                                     // 직전저가
            var highPrice = candles.GetRange(1, candles.Count - 1).Max(x => x.High);            // 최고가
            var changePrice = (currPrice - prevPrice);                                          // 변동가
            var changeRate = (changePrice / currPrice) * 100;                                   // 변동율
            var candlesChange = (currPrice - highPrice);                                        // 캔들 변동가
            var candlesRate = (candlesChange / highPrice) * 100;                                // 캔들 변동율
            var profit = coinBalance - coinBuyBalance;                                          // 수익
            //var tradeProfitRate = (avgPrice == 0) ? 0D : (profit / avgPrice) * 100;           // 수익율
            var targetProfit = coinBuyBalance + (coinBuyBalance * ((triggerRate / candleCount) + (feeRate * 2)) / 100);
            //var targetPrice = avgPrice + targetProfit;
            //var target = coinBuyPrice

            var result = null as JObject;
            var args = new object[] { coinName, currPrice, prevPrice, highPrice, changeRate, candlesRate, avgPrice, targetProfit };
            var buyTs = DateTime.Now - LastBuyDate;
            var sellTs = DateTime.Now - LastSellDate;
            var minutes = sellTs.TotalMinutes;

            WriteCurrent("{0} : 현재가 {1:N0}, 직전가 {2:N0}, 시작가 {3:N0}, 직전등락폭 {4:F6}, 등락폭 {5:F6}, 평단가 {6:N0}, 목표가 {7:N0}", args);

            if(StartKRW < minTradeKRW && krwBalance > minTradeKRW && coinBalance < minTradeKRW)
            {
                // 거래 시작 금액
                StartKRW = krwBalance;
            }

            try
            {
                if (coinBalance <= minTradeKRW && krwBalance <= minTradeKRW)
                {
                    // 보유현금과 보유 코인이 최소 거래금액 보다 적으면 거래 없음
                    WriteLog("#### 거래 불가 : 보유현금 {0}, 코인보유금 {1}, 최소 거래 금액 {2},", krwBalance, coinBalance, minTradeKRW);
                }
                else if (krwBalance > minTradeKRW
                    && sellTs.TotalSeconds >= CandleType.Minute * 60 / 2
                    && candlesRate <= -(triggerRate + (feeRate * 2))
                    && changeRate >= (feeRate * 2))
                {
                    // BUY
                    // 보유현금이 최소 거래금액 보다 많음
                    // 수익율 초과하여 떨어지다 수수료율 2배 이상 상승 했을때 거래비율 만큼 산다.
                    var total = ToOrderPrice(krwBalance);
                    result = React.executeDeal(true, false, coinName, 0, 0, total);
                    LastBuyDate = DateTime.Now;
                    WriteLog("#### {0} BUY : 금액 {1:N0}, 수량 {2:F6}", coinName, total, total / currPrice);
                }
                else if (coinBalance > minTradeKRW
                    && (buyTs.TotalSeconds >= CandleType.Minute * 60 * candleCount / 2
                    || targetProfit <= coinBalance ))
                {
                    // SELL
                    // 코인평가금 최소 거래금액 보다 많음
                    // 분봉 하나의 시간이 지나면 시장가에 전체 매도
                    // 현재가가 평단가 보다 (수익율/캔들수 + 수료율2배) 이상일때 전체 매도
                    var vol = coinVol;
                    vol = Math.Truncate(vol * 100000) / 100000;
                    result = React.executeDeal(false, false, coinName, vol, 0, 0);
                    LastSellDate = DateTime.Now;
                    WriteLog("#### {0} SELL : 금액 {1:N0}, 수량 {2:F6}", coinName, vol * currPrice, vol);
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.StackTrace);
            }

            if (result != null)
            {
                var uuid = result.Value<string>("uuid");
                var coinProfit = GetOrderResult(ApiData, uuid, coinName, currPrice, StartKRW);

                //WriteLog("#### RESULT : {0}", result.ToString());

                //var coinProfit = GetBalance(ApiData, coinName, StartKRW);

                //txtProfitPrice.Text = coinProfit.KrwProfit.ToString("N0");
                //txtProfitRate.Text = coinProfit.KrwProfitRate.ToString("F6");
                var profits = new object[] { coinName, StartKRW, coinProfit.TotalBalance, coinProfit.KrwProfit, coinProfit.KrwProfitRate };
                WriteLog("#### {0} 수익 : 거래시작금액 {1:N0}, 현재평가 금액 {2:N0}, 수익금액 {3:N0}, 수익율 {4:F6}", profits);
            }
        }

        private Balance GetOrderResult(ApiData apiData, string uuid, string coinName, double currentPrice, double startKrw)
        {
            System.Threading.Thread.Sleep(1500);
            var startTime = DateTime.Now;
            var order = apiData.checkOrder(uuid);
            var state = order.Value<string>("state");

            //while ((DateTime.Now - startTime).TotalSeconds <= 10 && !"donw".Equals(state))
            //{

            //    order = apiData.checkOrder(uuid);
            //    state = order.Value<string>("state");
            //}

            return GetBalance(apiData, coinName, currentPrice, startKrw);
        }

        private Balance GetBalance(ApiData apiData, string coinName, double currentPrice, double startKrw)
        {
            System.Threading.Thread.Sleep(500);
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
                balance.CoinBalance = balance.CoinVol * currentPrice;
            }

            balance.TotalBalance = balance.KRWBalance + balance.CoinBalance;
            balance.KrwProfit = balance.TotalBalance - startKrw;
            balance.KrwProfitRate = balance.KrwProfit / startKrw * 100;

            {
                txtKRW.Text = balance.KRWBalance.ToString("N0");
                txtCoinBalance.Text = balance.CoinBalance.ToString("N0");
                txtBuyBalance.Text = (balance.AvgBuyPrice * balance.CoinVol).ToString("N0");
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
            var log = $"[{DateTime.Now:T}] {string.Format(format, args)}{Environment.NewLine}";

            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate
                {
                    rtxtCurrent.Text = log;
                });
            }
            else
            {
                rtxtCurrent.Text = log;
            }
        }

        private void WriteLog(string format, params object[] args)
        {
            var log = $"[{DateTime.Now:T}] {string.Format(format, args)}{Environment.NewLine}";

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
                        txtLog.AppendText(log);
                        //txtLog.ScrollToCaret();
                    }
                    else
                    {
                        txtLog.Text += log;
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
                    txtLog.AppendText(log);
                    txtLog.ScrollToCaret();
                }
                else
                {
                    txtLog.Text += log;
                }
            }
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            if(Timer != null && Timer.Enabled)
            {
                StartKRW = 0;

                Timer.Stop();
                btnStart.Enabled = true;
                btnFinish.Enabled = false;

                Debug.WriteLine("#### STOP TIMER");
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

            //var total = ToOrderPrice(Convert.ToDouble(txtKRW.Text));
            //var result = React.executeDeal(true, false, Coin.Ticker, 0, 0, total);

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
