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
        public int Algorithm { get; set; } = 0;
        public List<Coin> Coins { get; set; } = BotSetting.Coins;
        public List<Algorithm> AlgorithmList { get; set; } = BotSetting.AlgorithmList;
        public double MaxProfit { get; set; }
        public double Ratio { get; set; }
        public React React { get; set; }
        public ApiData ApiData { get; set; }
        public Timer Timer { get; set; }
        public string CoinName { get; set; }
        public int Interval { get; set; } = 1;
        public int CandleCount { get; set; } = 10;
        public double TriggerRatio { get; set; }
        public string CandleType { get; set; } = ac.CANDLE_MIN3;

        public Bot()
        {
            InitializeComponent();

            cmbAlgorithm.DataSource = new BindingSource(BotSetting.AlgorithmList, null);
            cmbAlgorithm.ValueMember = "Key";
            cmbAlgorithm.DisplayMember = "Value";
            cmbAlgorithm.SelectedIndex = Settings.Default.algorithm;

            cmbCoin.DataSource = new BindingSource(BotSetting.Coins, null);
            cmbCoin.ValueMember = "Ticker";
            cmbCoin.DisplayMember = "CoinName";
            cmbCoin.SelectedValue = Settings.Default.coinName;
        }

        private void Bot_Load(object sender, EventArgs e)
        {
            var accessKey = Program.Accesskey;
            var secretEky = Program.Secretkey;
            Algorithm = cmbAlgorithm.SelectedIndex;
            ApiData = new ApiData(accessKey, secretEky);
            React = new React(accessKey, secretEky);
        }

        private void WriteLog(string format, params object[] args)
        {
            var log = string.Format(format, args);

            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker) delegate
                {
                    txtLog.AppendText(log + Environment.NewLine);
                });
            }
            else
            {
                txtLog.AppendText(log + Environment.NewLine);
            }

            if (chkAutoScroll.Checked)
            {
                txtLog.ScrollToCaret();
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            btnFinish.Enabled = true;

            var coinName = cmbCoin.SelectedText;
            var intervalString = txtInterval.Text;
            var interval = 0;
            var triggerRatioString = txtRatio.Text;
            var triggerRatio = 0D;

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
            CandleType = BotSetting.CandleTypes[cmbAlgorithm.SelectedIndex];

            Timer = new Timer();
            Timer.Interval = Interval * 1000;
            Timer.Tick += Timer_Tick;
            Timer.Start();

            Debug.WriteLine("#### START TIMER");
            WriteLog("#### START TIMER");
        }

        private void PointHalfStrategy()
        {
            // 1분봉 3개를 기준으로 이전 시가 에서 현재 금액 등낙률을 가져온다.
            // 등낙률 0.5% 이상이 되면 오르면 팔고/ 내리면 산다.
            // 거래시 보유 현금, 보유 코인의 절반을 거래하되 거래 금액이 만원 미만인 경우 전체 금액으로 거래한다.

            var KRWAccount = ApiData.getAsset().Where(x => "KRW".Equals(x.Value<string>("currency"))).First();
            var krwBalance = KRWAccount.Value<double>("balance");                                           // 보유 현금
            txtKRW.Text = krwBalance.ToString("N0");
            var candles = ApiData.getCandle<List<Candle>>(CoinName, CandleType, CandleCount);
            var prevCandle = candles[2];        // 시가 봉
            var currCandle = candles[0];        // 현재가 봉
            var currPrice = currCandle.Close;
            var prevPrice = prevCandle.Close;

            var orderChance = ApiData.getOrdersChance(CoinName);
            var ask = orderChance["ask_account"];
            var coinVol = ask.Value<double>("balance");                                                     // 보유 코인 수량
            var avgPrice = (coinVol * currPrice < 5000) ? currPrice : ask.Value<double>("avg_buy_price");   // 매수 평단가

            var coinPrice = avgPrice * coinVol;                         // 보유 코인 금액
            var upRatio = (currPrice - avgPrice) / currPrice * 100;     // 평단 대비 상승폭 (%)
            var downRatio = (currPrice - prevPrice) / currPrice * 100;  // 현재가 대비 하락폭 (%)
            var result = null as JObject;                               // 거래 결과

            Debug.WriteLine("upRatio : {0}, downRatio {1}", upRatio, downRatio);
            WriteLog("upRatio : {0}, downRatio {1}", upRatio, downRatio);

            try
            {
                if (upRatio >= TriggerRatio && coinPrice > 5000)
                {
                    // 올랐을때 코인 금액 절반 팔기
                    var vol = (coinPrice / 2) > 5000 ? coinVol / 2 : coinVol;
                    vol = Math.Truncate(vol * 100000) / 100000;
                    Debug.WriteLine("#### {2} SELL : 금액 {0}, 수량 {1}", vol * currPrice, vol, CoinName);
                    WriteLog("#### {2} SELL : 금액 {0}, 수량 {1}", vol * currPrice, vol, CoinName);
                    result = React.executeDeal(false, false, CoinName, vol, 0, 0);
                }
                else if (downRatio <= -(TriggerRatio) && krwBalance > 5000)
                {
                    // 내렸을때 보유 현금 절반으로 코인 사기
                    var total = (krwBalance / 2) > 5000 ? krwBalance / 2 : krwBalance;
                    total = Math.Truncate(total * 1000) / 1000;
                    Debug.WriteLine("#### {2} BUY : 금액 {0}, 수량 {1}", total, total / currPrice, CoinName);
                    WriteLog("#### {2} BUY : 금액 {0}, 수량 {1}", total, total / currPrice, CoinName);
                    result = React.executeDeal(true, false, CoinName, 0, 0, total);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                WriteLog(ex.StackTrace);
            }

            if (result != null)
            {
                Debug.WriteLine("#### RESULT : {0}", result.ToString());
                WriteLog("#### RESULT : {0}", result.ToString());
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if(cmbAlgorithm.SelectedIndex == 0)
            {
                PointHalfStrategy();
            }
            else if (cmbAlgorithm.SelectedIndex == 1)
            {
                var KRWAccount = ApiData.getAsset().Where(x => "KRW".Equals(x.Value<string>("currency"))).First();
                var krwBalance = KRWAccount.Value<double>("balance");                                           // 보유 현금
                var candles = ApiData.getCandle<List<Candle>>(CoinName, CandleType, CandleCount);
                var prevCandle = candles[2];
                var currCandle = candles[0];
                var currPrice = currCandle.Close;
                var prevPrice = prevCandle.Close;

                var orderChance = ApiData.getOrdersChance(CoinName);
                var ask = orderChance["ask_account"];
                var coinVol = ask.Value<double>("balance");                                                     // 보유 코인 수량
                var avgPrice = (coinVol * currPrice < 5000) ? currPrice : ask.Value<double>("avg_buy_price");   // 매수 평단가

                var coinPrice = avgPrice * coinVol;                         // 보유 코인 금액
                var upRatio = (currPrice - avgPrice) / currPrice * 100;     // 평단 대비 상승폭 (%)
                var downRatio = (currPrice - prevPrice) / currPrice * 100;  // 현재가 대비 하락폭 (%)
                var result = null as JObject;                               // 거래 결과

                Debug.WriteLine("upRatio : {0}, downRatio {1}", upRatio, downRatio);
                WriteLog("upRatio : {0}, downRatio {1}", upRatio, downRatio);

                try
                {
                    if (upRatio >= TriggerRatio && coinPrice > 5000)
                    {
                        // 올랐을때 코인 금액 절반 팔기
                        var vol = (coinPrice / 2) > 5000 ? coinVol / 2 : coinVol;
                        vol = Math.Truncate(vol * 100000) / 100000;
                        Debug.WriteLine("#### {2} SELL : 금액 {0}, 수량 {1}", vol * currPrice, vol, CoinName);
                        WriteLog("#### {2} SELL : 금액 {0}, 수량 {1}", vol * currPrice, vol, CoinName);
                        result = React.executeDeal(false, false, CoinName, vol, 0, 0);
                    }
                    else if (downRatio <= -(TriggerRatio) && krwBalance > 5000)
                    {
                        // 내렸을때 보유 현금 절반으로 코인 사기
                        var total = (krwBalance / 2) > 5000 ? krwBalance / 2 : krwBalance;
                        total = Math.Truncate(total * 1000) / 1000;
                        Debug.WriteLine("#### {2} BUY : 금액 {0}, 수량 {1}", total, total / currPrice, CoinName);
                        WriteLog("#### {2} BUY : 금액 {0}, 수량 {1}", total, total / currPrice, CoinName);
                        result = React.executeDeal(true, false, CoinName, 0, 0, total);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    WriteLog(ex.StackTrace);
                }

                if (result != null)
                {
                    Debug.WriteLine("#### RESULT : {0}", result.ToString());
                    WriteLog("#### RESULT : {0}", result.ToString());
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
                var total = Convert.ToDouble(txtProfit.Text);
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
