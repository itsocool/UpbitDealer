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

namespace UpbitDealer.form
{
    public partial class Bot : Form
    {
        public React React { get; set; }
        public ApiData ApiData { get; set; }
        public Timer Timer { get; set; }
        public Algorithm Algorithm { get; set; }
        public CandleType CandleType { get; set; }
        public double Fee { get; set; }
        public double TradeRate { get; set; }
        public Coin Coin { get; set; }
        public int Interval { get; set; }
        public double TriggerRatio { get; set; } 
        public int Ticks { get; set; }
        //public string CoinName { get; set; }
        //public int CandleCount { get; set; } = 10;
        //public string CandleType { get; set; }
        public DateTime LastBuyDate { get; set; } = DateTime.MinValue;
        public DateTime LastSellDate { get; set; } = DateTime.MinValue;

        public Bot()
        {
            InitializeComponent();

            cmbAlgorithm.DataSource = new BindingSource(BotSetting.Algorithms, null);
            cmbAlgorithm.ValueMember = "Key";
            cmbAlgorithm.DisplayMember = "Value";
            cmbAlgorithm.SelectedIndex = Settings.Default.algorithm;

            var algIdx = cmbAlgorithm.SelectedIndex;
            var idx = (algIdx > 0) ? algIdx : Convert.ToInt32(txtMinute.Text);

            CandleType = BotSetting.CandleTypes[idx];

            cmbCoin.DataSource = new BindingSource(BotSetting.Coins, null);
            cmbCoin.ValueMember = "Ticker";
            cmbCoin.DisplayMember = "CoinName";
            cmbCoin.SelectedValue = Settings.Default.coinName;

            CoinName = ((Coin)cmbCoin.SelectedItem).Ticker;
        }

        private void Bot_Load(object sender, EventArgs e)
        {
            var accessKey = Program.Accesskey;
            var secretEky = Program.Secretkey;
            Algorithm = cmbAlgorithm.SelectedIndex;
            ApiData = new ApiData(accessKey, secretEky);
            React = new React(accessKey, secretEky);

            var currPrice = ApiData.getCandle<List<Candle>>(CoinName, CandleType, 1).First().Close;
            var orderChance = GetOrderChance(ApiData, CoinName, currPrice);
        }

        private void ReadInput()
        {

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            var coinName = ((Coin)cmbCoin.SelectedItem).Ticker;
            var intervalString = txtInterval.Text;
            var interval = 0;
            var triggerRatioString = txtRatio.Text;
            var triggerRatio = 0D;
            var algIdx = cmbAlgorithm.SelectedIndex;
            var idx = (algIdx > 0) ? algIdx : Convert.ToInt32(txtMinute.Text);

            if (string.IsNullOrEmpty(coinName)
                || string.IsNullOrEmpty(intervalString) || !int.TryParse(intervalString, out interval)
                || string.IsNullOrEmpty(triggerRatioString) || !double.TryParse(triggerRatioString, out triggerRatio))
            {
                MessageBox.Show("코인명과 호출주기를 입력하세요");
                return;
            }

            CoinName = coinName;
            Interval = interval;
            TriggerRatio = triggerRatio;
            CandleType = BotSetting.CandleTypes[idx];

            Timer = new Timer();
            Timer.Interval = Interval * 1000;
            Timer.Tick += Timer_Tick;
            Timer.Start();

            Debug.WriteLine("#### START TIMER");
            WriteLog("#### START TIMER");

            btnStart.Enabled = false;
            btnFinish.Enabled = true;
        }

        private void PointHalfStrategy()
        {
            // 1분봉 3개를 기준으로 이전 시가 에서 현재 금액 등낙률을 가져온다.
            // 등낙률 0.5% 이상이 되면 오르면 팔고/ 내리면 산다.
            // 거래시 보유 현금, 보유 코인의 절반을 거래하되 거래 금액이 만원 미만인 경우 전체 금액으로 거래한다.
            var algIdx = cmbAlgorithm.SelectedIndex;
            var idx = (algIdx > 0) ? algIdx : Convert.ToInt32(txtMinute.Text);
            var candleType = BotSetting.CandleTypes[idx];
            var candles = ApiData.getCandle<List<Candle>>(CoinName, candleType, 3);
            var prevCandle = candles.Last();            // 시가 봉
            var currCandle = candles.First();           // 현재가 봉
            var currPrice = currCandle.Close;
            var prevPrice = prevCandle.Close;
            var orderChance = GetOrderChance(ApiData, CoinName, currPrice);
            //var ask = orderChance["ask_account"];
            //var bid = orderChance["bid_account"];
            var krwBalance = orderChance.KRWBalance;                            // 보유 현금
            var coinVol = orderChance.CoinVol;                                  // 보유 코인 수량
            var avgPrice = orderChance.AvgBuyPrice;                             // 매수 평단가
            var coinPrice = orderChance.CoinBalance;                            // 보유 코인 금액

            //var upRatio = (currPrice - avgPrice) / currPrice * 100;     // 평단 대비 상승폭 (%)
            //var downRatio = (currPrice - prevPrice) / currPrice * 100;  // 현재가 대비 하락폭 (%)
            var ratio = (currPrice - prevPrice) / currPrice * 100;          // 등락폭 (%)
            var tradeRate = Convert.ToDouble(txtTradeRate.Text) / 100D;     // 거래 비중 (%)
            var result = null as JObject;                                   // 거래 결과

            //txtKRW.Text = krwBalance.ToString("N0");
            //txtCoinBalance.Text = coinPrice.ToString("N0");

            Debug.WriteLine("이전가: {0:N0}, 현재가 {1:N0}, ratio : {2:F4}%", prevPrice, currPrice, ratio);
            WriteLog("이전가: {0:N0}, 현재가 {1:N0}, ratio : {2:F4}%", prevPrice, currPrice, ratio);

            try
            {
                if (avgPrice + (2 * (avgPrice * TriggerRatio / 100)) <= currPrice && coinPrice >= 5000)
                {
                    // 현재가가 평단가보다 등락폭 2배 이상 올랐을때 전량 매도
                    var vol = coinVol;
                    vol = Math.Truncate(vol * 100000) / 100000;
                    Debug.WriteLine("#### {2} ALL SELL : 금액 {0:N0}, 수량 {1:F6}", vol * currPrice, vol, CoinName);
                    WriteLog("#### {2} ALL SELL : 금액 {0:N0}, 수량 {1:F6}", vol * currPrice, vol, CoinName);
                    result = React.executeDeal(false, false, CoinName, vol, 0, 0);
                    LastSellDate = DateTime.Now;
                }
                else if (ratio >= TriggerRatio && coinPrice >= 5000 && ((DateTime.Now - LastSellDate).TotalMinutes >= idx || ratio >= TriggerRatio * 1.5))
                {
                    // 올랐을때 코인 금액 절반 팔기
                    var vol = (coinPrice * tradeRate) > 5000 ? coinVol * tradeRate : coinVol;
                    vol = Math.Truncate(vol * 100000) / 100000;
                    Debug.WriteLine("#### {2} SELL : 금액 {0:N0}, 수량 {1:F6}", vol * currPrice, vol, CoinName);
                    WriteLog("#### {2} SELL : 금액 {0:N0}, 수량 {1:F6}", vol * currPrice, vol, CoinName);
                    result = React.executeDeal(false, false, CoinName, vol, 0, 0);
                    LastSellDate = DateTime.Now;
                }
                else if (ratio <= -(TriggerRatio * 1.2) && krwBalance > 5000 && (DateTime.Now - LastBuyDate).TotalMinutes >= idx)
                {
                    // 1.2배 내렸을때 보유 현금 절반으로 코인 사기
                    var total = (krwBalance * tradeRate) > 5000 ? krwBalance * tradeRate : krwBalance;
                    total = Math.Truncate(total * 1000) / 1000;
                    Debug.WriteLine("#### {2} BUY : 금액 {0:N0}, 수량 {1:F6}", total, total / currPrice, CoinName);
                    WriteLog("#### {2} BUY : 금액 {0:N0}, 수량 {1:F6}", total, total / currPrice, CoinName);
                    result = React.executeDeal(true, false, CoinName, 0, 0, total);
                    LastBuyDate = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                WriteLog(ex.StackTrace);
            }

            orderChance = GetOrderChance(ApiData, CoinName, currPrice);

            if (result != null)
            {
                Debug.WriteLine("#### RESULT : {0}", result.ToString());
                WriteLog("#### RESULT : {0}", result.ToString());
            }
        }

        private void MinuteCandleByTicks()
        {
            // 1분봉 3개를 기준으로 이전 시가 에서 현재 금액 등낙률을 가져온다.
            // 등낙률 0.5% 이상이 되면 오르면 팔고/ 내리면 산다.
            // 거래시 보유 현금, 보유 코인의 절반을 거래하되 거래 금액이 만원 미만인 경우 전체 금액으로 거래한다.
            var algIdx = cmbAlgorithm.SelectedIndex;
            var idx = (algIdx > 0) ? algIdx : Convert.ToInt32(txtMinute.Text);
            var candleType = BotSetting.CandleTypes[idx];
            var candles = ApiData.getCandle<List<Candle>>(CoinName, candleType, 3);
            var prevCandle = candles.Last();            // 시가 봉
            var currCandle = candles.First();           // 현재가 봉
            var currPrice = currCandle.Close;
            var prevPrice = prevCandle.Close;
            var orderChance = GetOrderChance(ApiData, CoinName, currPrice);
            //var ask = orderChance["ask_account"];
            //var bid = orderChance["bid_account"];
            var krwBalance = orderChance.KRWBalance;                            // 보유 현금
            var coinVol = orderChance.CoinVol;                                  // 보유 코인 수량
            var avgPrice = orderChance.AvgBuyPrice;                             // 매수 평단가
            var coinPrice = orderChance.CoinBalance;                            // 보유 코인 금액

            //var upRatio = (currPrice - avgPrice) / currPrice * 100;     // 평단 대비 상승폭 (%)
            //var downRatio = (currPrice - prevPrice) / currPrice * 100;  // 현재가 대비 하락폭 (%)
            var ratio = (currPrice - prevPrice) / currPrice * 100;          // 등락폭 (%)
            var tradeRate = Convert.ToDouble(txtTradeRate.Text) / 100D;     // 거래 비중 (%)
            var result = null as JObject;                                   // 거래 결과

            //txtKRW.Text = krwBalance.ToString("N0");
            //txtCoinBalance.Text = coinPrice.ToString("N0");

            Debug.WriteLine("이전가: {0:N0}, 현재가 {1:N0}, ratio : {2:F4}%", prevPrice, currPrice, ratio);
            WriteLog("이전가: {0:N0}, 현재가 {1:N0}, ratio : {2:F4}%", prevPrice, currPrice, ratio);

            try
            {
                if (avgPrice + (2 * (avgPrice * TriggerRatio / 100)) <= currPrice && coinPrice >= 5000)
                {
                    // 현재가가 평단가보다 등락폭 2배 이상 올랐을때 전량 매도
                    var vol = coinVol;
                    vol = Math.Truncate(vol * 100000) / 100000;
                    Debug.WriteLine("#### {2} ALL SELL : 금액 {0:N0}, 수량 {1:F6}", vol * currPrice, vol, CoinName);
                    WriteLog("#### {2} ALL SELL : 금액 {0:N0}, 수량 {1:F6}", vol * currPrice, vol, CoinName);
                    result = React.executeDeal(false, false, CoinName, vol, 0, 0);
                    LastSellDate = DateTime.Now;
                }
                else if (ratio >= TriggerRatio && coinPrice >= 5000 && ((DateTime.Now - LastSellDate).TotalMinutes >= idx || ratio >= TriggerRatio * 1.5))
                {
                    // 올랐을때 코인 금액 절반 팔기
                    var vol = (coinPrice * tradeRate) > 5000 ? coinVol * tradeRate : coinVol;
                    vol = Math.Truncate(vol * 100000) / 100000;
                    Debug.WriteLine("#### {2} SELL : 금액 {0:N0}, 수량 {1:F6}", vol * currPrice, vol, CoinName);
                    WriteLog("#### {2} SELL : 금액 {0:N0}, 수량 {1:F6}", vol * currPrice, vol, CoinName);
                    result = React.executeDeal(false, false, CoinName, vol, 0, 0);
                    LastSellDate = DateTime.Now;
                }
                else if (ratio <= -(TriggerRatio * 1.2) && krwBalance > 5000 && (DateTime.Now - LastBuyDate).TotalMinutes >= idx)
                {
                    // 1.2배 내렸을때 보유 현금 절반으로 코인 사기
                    var total = (krwBalance * tradeRate) > 5000 ? krwBalance * tradeRate : krwBalance;
                    total = Math.Truncate(total * 1000) / 1000;
                    Debug.WriteLine("#### {2} BUY : 금액 {0:N0}, 수량 {1:F6}", total, total / currPrice, CoinName);
                    WriteLog("#### {2} BUY : 금액 {0:N0}, 수량 {1:F6}", total, total / currPrice, CoinName);
                    result = React.executeDeal(true, false, CoinName, 0, 0, total);
                    LastBuyDate = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                WriteLog(ex.StackTrace);
            }

            orderChance = GetOrderChance(ApiData, CoinName, currPrice);

            if (result != null)
            {
                Debug.WriteLine("#### RESULT : {0}", result.ToString());
                WriteLog("#### RESULT : {0}", result.ToString());
            }
        }

        private OrderChance GetOrderChance(ApiData apiData, string coinName, double currentPrice, bool setText = true)
        {
            var orderChance = apiData.getOrdersChance(coinName);
            var ask = orderChance["ask_account"];
            var bid = orderChance["bid_account"];
            var result = new OrderChance(currentPrice)
            {
                KRWBalance = bid.Value<double>("balance"),              // 보유 현금
                CoinVol = ask.Value<double>("balance"),                 // 보유 코인 수량
                AvgBuyPrice = ask.Value<double>("avg_buy_price"),       // 매수 평단가
            };

            if (setText)
            {
                txtKRW.Text = result.KRWBalance.ToString("N0");
                txtCoinBalance.Text = result.CoinBalance.ToString("N0");
            }

            return result;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (cmbAlgorithm.SelectedIndex == 0)
            {
                PointHalfStrategy();
            }
            else if (cmbAlgorithm.SelectedIndex == 1)
            {

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
                        txtLog.ScrollToCaret();
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
                Timer.Stop();
                btnStart.Enabled = true;
                btnFinish.Enabled = false;

                Debug.WriteLine("#### STOP TIMER");
                WriteLog("#### STOP TIMER");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var coinName = cmbCoin.Text;
                var total = Convert.ToDouble(txtCoinBalance.Text);
                var result = React.executeDeal(true, false, coinName, 3, 0, total);
                Debug.WriteLine("#### RESULT : {0}", result.ToString());
            }
            catch (Exception ex)
            {
                _ = ex;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            txtLog.Text = "";
        }
    }
}
