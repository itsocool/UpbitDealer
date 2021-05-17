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
        public Dictionary<string, string> Coins { get; set; } = BotSetting.Coins;
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
        }

        private void Bot_Load(object sender, EventArgs e)
        {
            var accessKey = Program.Accesskey;
            var secretEky = Program.Secretkey;
            Algorithm = cmbAlgorithm.SelectedIndex;
            ApiData = new ApiData(accessKey, secretEky);
            React = new React(accessKey, secretEky);
        }

        //private string GetCandleType()
        //{
        //    var types = new string[]
        //    {
        //        ac.CANDLE_MIN1,
        //        ac.CANDLE_MIN3,
        //        ac.CANDLE_MIN5,
        //        ac.CANDLE_MIN10,
        //        ac.CANDLE_MIN15,
        //    };
        //    var idx = cmbAlgorithm.SelectedIndex;
        //    return types[idx];
        //}

        //private void SaveSettings()
        //{
        //    Settings.Default.algorithm = Algorithm;
        //    Settings.Default.coins = string.Join(",", Coins);
        //    Settings.Default.maxProfit = MaxProfit;
        //    Settings.Default.ratio = Ratio;
        //    Settings.Default.Save();
        //}

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

            var coinName = cmbCoinName.SelectedText;
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

        private void Timer_Tick(object sender, EventArgs e)
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

            if(result != null)
            {
                Debug.WriteLine("#### RESULT : {0}", result.ToString());
                WriteLog("#### RESULT : {0}", result.ToString());
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
                var coinName = cmbCoinName.Text;
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

        /*
        private void setDefaultSetting()
        {
            lock (((Main)Owner).lock_bot)
                setting = ((Main)Owner).bot.setting;

            btn_pause.BackColor = setting.pauseBuy ? Color.Red : Color.DarkGray;

            text_top.Text = setting.top.ToString();
            text_yield.Text = setting.yield.ToString();
            text_krw.Text = setting.krw.ToString();
            text_time.Text = setting.time.ToString();
            text_limit.Text = setting.limit.ToString();
            text_lostCut.Text = setting.lostCut.ToString();

            check_week_bb.Checked = setting.week_bb;
            check_day_bb.Checked = setting.day_bb;
            check_hour4_bb.Checked = setting.hour4_bb;
            check_hour1_bb.Checked = setting.hour1_bb;
            check_min30_bb.Checked = setting.min30_bb;

            check_week_tl.Checked = setting.week_tl;
            check_day_tl.Checked = setting.day_tl;
            check_hour4_tl.Checked = setting.hour4_tl;
            check_hour1_tl.Checked = setting.hour1_tl;
            check_min30_tl.Checked = setting.min30_tl;
        }


        private void btn_save_Click(object sender, EventArgs e)
        {
            if (text_yield.Text == "" || text_krw.Text == "" || text_time.Text == "")
            {
                MessageBox.Show("Check Parameters.");
                return;
            }

            BotSettingData setting = new BotSettingData();

            setting.pauseBuy = this.setting.pauseBuy;

            if (!int.TryParse(text_top.Text, out setting.top))
            {
                if (text_top.Text == "") setting.top = 70;
                else
                {
                    MessageBox.Show("Top value is not number.");
                    return;
                }
            }
            if (!double.TryParse(text_yield.Text, out setting.yield))
            {
                MessageBox.Show("Yield value is not number.");
                return;
            }
            if (!double.TryParse(text_krw.Text, out setting.krw))
            {
                MessageBox.Show("KRW value is not number.");
                return;
            }
            if (!double.TryParse(text_time.Text, out setting.time))
            {
                MessageBox.Show("Time value is not number.");
                return;
            }
            if (!double.TryParse(text_limit.Text, out setting.limit))
            {
                if (text_limit.Text == "") setting.limit = 0;
                else
                {
                    MessageBox.Show("Limit value is not number.");
                    return;
                }
            }
            if (!double.TryParse(text_lostCut.Text, out setting.lostCut))
            {
                if (text_lostCut.Text == "") setting.lostCut = 0;
                else
                {
                    MessageBox.Show("Lost Cut value is not number.");
                    return;
                }
            }

            setting.week_bb = check_week_bb.Checked;
            setting.day_bb = check_day_bb.Checked;
            setting.hour4_bb = check_hour4_bb.Checked;
            setting.hour1_bb = check_hour1_bb.Checked;
            setting.min30_bb = check_min30_bb.Checked;

            setting.week_tl = check_week_tl.Checked;
            setting.day_tl = check_day_tl.Checked;
            setting.hour4_tl = check_hour4_tl.Checked;
            setting.hour1_tl = check_hour1_tl.Checked;
            setting.min30_tl = check_min30_tl.Checked;

            if (setting.top < 0 || setting.yield < 0 || setting.krw < 0 ||
                setting.time < 0 || setting.limit < 0 || setting.lostCut < 0)
            {
                MessageBox.Show("Required setting values can't be negative.");
                return;
            }
            if (!(setting.week_bb || setting.day_bb || setting.hour4_bb || setting.hour1_bb || setting.min30_bb ||
                setting.week_tl || setting.day_tl || setting.hour4_tl || setting.hour1_tl || setting.min30_tl))
            {
                MessageBox.Show("At least one of optional setting values must be checked.");
                return;
            }

            lock (((Main)Owner).lock_bot)
                ((Main)Owner).bot.saveBotSetting(setting);

            setDefaultSetting();
            MessageBox.Show("Save success.");
        }
        private void btn_cancel_Click(object sender, EventArgs e)
        {
            setDefaultSetting();
        }
        private void btn_pause_Click(object sender, EventArgs e)
        {
            setting.pauseBuy = setting.pauseBuy ? false : true;
            lock (((Main)Owner).lock_bot)
                ((Main)Owner).bot.saveBotSetting(setting);
            setDefaultSetting();

            if (setting.pauseBuy)
                MessageBox.Show("Pause Bot.");
            else
                MessageBox.Show("Continue Bot.");
        }
        */
    }
}
