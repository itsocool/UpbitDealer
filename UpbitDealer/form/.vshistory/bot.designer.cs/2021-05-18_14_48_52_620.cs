
namespace UpbitDealer.form
{
    partial class Bot
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Bot));
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.cmbCandle = new System.Windows.Forms.ComboBox();
            this.txtCandleCount = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.cmbCoin = new System.Windows.Forms.ComboBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.txtTriggerRate = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.txtInterval = new System.Windows.Forms.TextBox();
            this.textBox10 = new System.Windows.Forms.TextBox();
            this.lblmin = new System.Windows.Forms.TextBox();
            this.txtTradeRate = new System.Windows.Forms.TextBox();
            this.textBox14 = new System.Windows.Forms.TextBox();
            this.cmbAlgorithm = new System.Windows.Forms.ComboBox();
            this.textBox16 = new System.Windows.Forms.TextBox();
            this.txtFee = new System.Windows.Forms.TextBox();
            this.textBox23 = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnFinish = new System.Windows.Forms.Button();
            this.txtKRW = new System.Windows.Forms.TextBox();
            this.textBox18 = new System.Windows.Forms.TextBox();
            this.txtCoinBalance = new System.Windows.Forms.TextBox();
            this.txtCoin = new System.Windows.Forms.TextBox();
            this.btnApply = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkAutoScroll = new System.Windows.Forms.CheckBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.txtSellRate = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.txtpp = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.txtpr = new System.Windows.Forms.TextBox();
            this.groupBox6.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.BackColor = System.Drawing.Color.Black;
            this.button2.Location = new System.Drawing.Point(849, 25);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 45;
            this.button2.Text = "Clear";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.butClear_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.txtSellRate);
            this.groupBox6.Controls.Add(this.textBox2);
            this.groupBox6.Controls.Add(this.cmbCandle);
            this.groupBox6.Controls.Add(this.txtCandleCount);
            this.groupBox6.Controls.Add(this.textBox4);
            this.groupBox6.Controls.Add(this.cmbCoin);
            this.groupBox6.Controls.Add(this.textBox3);
            this.groupBox6.Controls.Add(this.txtTriggerRate);
            this.groupBox6.Controls.Add(this.textBox6);
            this.groupBox6.Controls.Add(this.txtInterval);
            this.groupBox6.Controls.Add(this.textBox10);
            this.groupBox6.Controls.Add(this.lblmin);
            this.groupBox6.Controls.Add(this.txtTradeRate);
            this.groupBox6.Controls.Add(this.textBox14);
            this.groupBox6.Controls.Add(this.cmbAlgorithm);
            this.groupBox6.Controls.Add(this.textBox16);
            this.groupBox6.Controls.Add(this.txtFee);
            this.groupBox6.Controls.Add(this.textBox23);
            this.groupBox6.ForeColor = System.Drawing.Color.White;
            this.groupBox6.Location = new System.Drawing.Point(12, 12);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(402, 404);
            this.groupBox6.TabIndex = 42;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "설정";
            // 
            // cmbCandle
            // 
            this.cmbCandle.BackColor = System.Drawing.Color.LightGray;
            this.cmbCandle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCandle.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold);
            this.cmbCandle.FormattingEnabled = true;
            this.cmbCandle.Location = new System.Drawing.Point(140, 61);
            this.cmbCandle.Name = "cmbCandle";
            this.cmbCandle.Size = new System.Drawing.Size(256, 38);
            this.cmbCandle.TabIndex = 55;
            // 
            // txtCandleCount
            // 
            this.txtCandleCount.BackColor = System.Drawing.Color.LightGray;
            this.txtCandleCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCandleCount.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtCandleCount.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCandleCount.ForeColor = System.Drawing.Color.Black;
            this.txtCandleCount.Location = new System.Drawing.Point(140, 301);
            this.txtCandleCount.Name = "txtCandleCount";
            this.txtCandleCount.Size = new System.Drawing.Size(256, 38);
            this.txtCandleCount.TabIndex = 54;
            this.txtCandleCount.Text = "3";
            this.txtCandleCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox4
            // 
            this.textBox4.BackColor = System.Drawing.Color.Black;
            this.textBox4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox4.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.textBox4.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox4.ForeColor = System.Drawing.Color.White;
            this.textBox4.Location = new System.Drawing.Point(6, 304);
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(128, 28);
            this.textBox4.TabIndex = 53;
            this.textBox4.Text = "캔들 개수";
            this.textBox4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // cmbCoin
            // 
            this.cmbCoin.BackColor = System.Drawing.Color.LightGray;
            this.cmbCoin.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCoin.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold);
            this.cmbCoin.FormattingEnabled = true;
            this.cmbCoin.Location = new System.Drawing.Point(140, 181);
            this.cmbCoin.Name = "cmbCoin";
            this.cmbCoin.Size = new System.Drawing.Size(256, 38);
            this.cmbCoin.TabIndex = 52;
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.Color.Black;
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.textBox3.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox3.ForeColor = System.Drawing.Color.Cyan;
            this.textBox3.Location = new System.Drawing.Point(6, 184);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(128, 28);
            this.textBox3.TabIndex = 51;
            this.textBox3.Text = "코인";
            this.textBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtTriggerRate
            // 
            this.txtTriggerRate.BackColor = System.Drawing.Color.LightGray;
            this.txtTriggerRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTriggerRate.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTriggerRate.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTriggerRate.ForeColor = System.Drawing.Color.Black;
            this.txtTriggerRate.Location = new System.Drawing.Point(140, 261);
            this.txtTriggerRate.Name = "txtTriggerRate";
            this.txtTriggerRate.Size = new System.Drawing.Size(256, 38);
            this.txtTriggerRate.TabIndex = 50;
            this.txtTriggerRate.Text = "1";
            this.txtTriggerRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox6
            // 
            this.textBox6.BackColor = System.Drawing.Color.Black;
            this.textBox6.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox6.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.textBox6.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox6.ForeColor = System.Drawing.Color.White;
            this.textBox6.Location = new System.Drawing.Point(6, 264);
            this.textBox6.Name = "textBox6";
            this.textBox6.ReadOnly = true;
            this.textBox6.Size = new System.Drawing.Size(128, 28);
            this.textBox6.TabIndex = 49;
            this.textBox6.Text = "촉발 등락폭(%)";
            this.textBox6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtInterval
            // 
            this.txtInterval.BackColor = System.Drawing.Color.LightGray;
            this.txtInterval.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtInterval.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtInterval.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInterval.ForeColor = System.Drawing.Color.Black;
            this.txtInterval.Location = new System.Drawing.Point(140, 221);
            this.txtInterval.Name = "txtInterval";
            this.txtInterval.Size = new System.Drawing.Size(256, 38);
            this.txtInterval.TabIndex = 48;
            this.txtInterval.Text = "1";
            this.txtInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox10
            // 
            this.textBox10.BackColor = System.Drawing.Color.Black;
            this.textBox10.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox10.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.textBox10.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox10.ForeColor = System.Drawing.Color.White;
            this.textBox10.Location = new System.Drawing.Point(6, 224);
            this.textBox10.Name = "textBox10";
            this.textBox10.ReadOnly = true;
            this.textBox10.Size = new System.Drawing.Size(128, 28);
            this.textBox10.TabIndex = 46;
            this.textBox10.Text = "호출주기 (초)";
            this.textBox10.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblmin
            // 
            this.lblmin.BackColor = System.Drawing.Color.Black;
            this.lblmin.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblmin.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.lblmin.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblmin.ForeColor = System.Drawing.Color.White;
            this.lblmin.Location = new System.Drawing.Point(6, 64);
            this.lblmin.Name = "lblmin";
            this.lblmin.ReadOnly = true;
            this.lblmin.Size = new System.Drawing.Size(128, 28);
            this.lblmin.TabIndex = 45;
            this.lblmin.Text = "캔들 유형";
            this.lblmin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtTradeRate
            // 
            this.txtTradeRate.BackColor = System.Drawing.Color.LightGray;
            this.txtTradeRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTradeRate.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTradeRate.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTradeRate.ForeColor = System.Drawing.Color.Black;
            this.txtTradeRate.Location = new System.Drawing.Point(140, 141);
            this.txtTradeRate.Name = "txtTradeRate";
            this.txtTradeRate.Size = new System.Drawing.Size(256, 38);
            this.txtTradeRate.TabIndex = 44;
            this.txtTradeRate.Text = "100";
            this.txtTradeRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox14
            // 
            this.textBox14.BackColor = System.Drawing.Color.Black;
            this.textBox14.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox14.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.textBox14.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox14.ForeColor = System.Drawing.Color.White;
            this.textBox14.Location = new System.Drawing.Point(6, 144);
            this.textBox14.Name = "textBox14";
            this.textBox14.ReadOnly = true;
            this.textBox14.Size = new System.Drawing.Size(128, 28);
            this.textBox14.TabIndex = 43;
            this.textBox14.Text = "거래 비율 (%)";
            this.textBox14.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // cmbAlgorithm
            // 
            this.cmbAlgorithm.BackColor = System.Drawing.Color.LightGray;
            this.cmbAlgorithm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAlgorithm.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold);
            this.cmbAlgorithm.FormattingEnabled = true;
            this.cmbAlgorithm.Location = new System.Drawing.Point(140, 21);
            this.cmbAlgorithm.Name = "cmbAlgorithm";
            this.cmbAlgorithm.Size = new System.Drawing.Size(256, 38);
            this.cmbAlgorithm.TabIndex = 1;
            // 
            // textBox16
            // 
            this.textBox16.BackColor = System.Drawing.Color.Black;
            this.textBox16.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox16.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.textBox16.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox16.ForeColor = System.Drawing.Color.White;
            this.textBox16.Location = new System.Drawing.Point(6, 24);
            this.textBox16.Name = "textBox16";
            this.textBox16.ReadOnly = true;
            this.textBox16.Size = new System.Drawing.Size(128, 28);
            this.textBox16.TabIndex = 38;
            this.textBox16.Text = "알고리즘";
            this.textBox16.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtFee
            // 
            this.txtFee.BackColor = System.Drawing.Color.LightGray;
            this.txtFee.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFee.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtFee.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFee.ForeColor = System.Drawing.Color.Black;
            this.txtFee.Location = new System.Drawing.Point(140, 101);
            this.txtFee.Name = "txtFee";
            this.txtFee.ReadOnly = true;
            this.txtFee.Size = new System.Drawing.Size(256, 38);
            this.txtFee.TabIndex = 10;
            this.txtFee.Text = "0.05";
            this.txtFee.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox23
            // 
            this.textBox23.BackColor = System.Drawing.Color.Black;
            this.textBox23.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox23.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.textBox23.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox23.ForeColor = System.Drawing.Color.White;
            this.textBox23.Location = new System.Drawing.Point(6, 104);
            this.textBox23.Name = "textBox23";
            this.textBox23.ReadOnly = true;
            this.textBox23.Size = new System.Drawing.Size(128, 28);
            this.textBox23.TabIndex = 5;
            this.textBox23.Text = "수수료 (%)";
            this.textBox23.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.Green;
            this.btnStart.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.ForeColor = System.Drawing.Color.Black;
            this.btnStart.Location = new System.Drawing.Point(6, 21);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(128, 34);
            this.btnStart.TabIndex = 43;
            this.btnStart.Text = "시 작";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnFinish
            // 
            this.btnFinish.BackColor = System.Drawing.Color.Red;
            this.btnFinish.Enabled = false;
            this.btnFinish.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFinish.ForeColor = System.Drawing.Color.Black;
            this.btnFinish.Location = new System.Drawing.Point(137, 21);
            this.btnFinish.Name = "btnFinish";
            this.btnFinish.Size = new System.Drawing.Size(128, 34);
            this.btnFinish.TabIndex = 44;
            this.btnFinish.Text = "중 지";
            this.btnFinish.UseVisualStyleBackColor = false;
            this.btnFinish.Click += new System.EventHandler(this.btnFinish_Click);
            // 
            // txtKRW
            // 
            this.txtKRW.BackColor = System.Drawing.Color.LightGray;
            this.txtKRW.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtKRW.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtKRW.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtKRW.ForeColor = System.Drawing.Color.Black;
            this.txtKRW.Location = new System.Drawing.Point(142, 62);
            this.txtKRW.Name = "txtKRW";
            this.txtKRW.Size = new System.Drawing.Size(254, 38);
            this.txtKRW.TabIndex = 45;
            this.txtKRW.Text = "0";
            this.txtKRW.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox18
            // 
            this.textBox18.BackColor = System.Drawing.Color.Black;
            this.textBox18.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox18.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.textBox18.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox18.ForeColor = System.Drawing.Color.White;
            this.textBox18.Location = new System.Drawing.Point(8, 65);
            this.textBox18.Name = "textBox18";
            this.textBox18.ReadOnly = true;
            this.textBox18.Size = new System.Drawing.Size(128, 28);
            this.textBox18.TabIndex = 46;
            this.textBox18.Text = "보유 KRW";
            this.textBox18.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtCoinBalance
            // 
            this.txtCoinBalance.BackColor = System.Drawing.Color.LightGray;
            this.txtCoinBalance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCoinBalance.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtCoinBalance.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCoinBalance.ForeColor = System.Drawing.Color.Black;
            this.txtCoinBalance.Location = new System.Drawing.Point(142, 107);
            this.txtCoinBalance.Name = "txtCoinBalance";
            this.txtCoinBalance.Size = new System.Drawing.Size(254, 38);
            this.txtCoinBalance.TabIndex = 47;
            this.txtCoinBalance.Text = "0";
            this.txtCoinBalance.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtCoin
            // 
            this.txtCoin.BackColor = System.Drawing.Color.Black;
            this.txtCoin.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCoin.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.txtCoin.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCoin.ForeColor = System.Drawing.Color.White;
            this.txtCoin.Location = new System.Drawing.Point(8, 110);
            this.txtCoin.Name = "txtCoin";
            this.txtCoin.ReadOnly = true;
            this.txtCoin.Size = new System.Drawing.Size(128, 28);
            this.txtCoin.TabIndex = 48;
            this.txtCoin.Text = "평가금 (KRW)";
            this.txtCoin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnApply
            // 
            this.btnApply.BackColor = System.Drawing.Color.DarkGray;
            this.btnApply.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApply.ForeColor = System.Drawing.Color.Black;
            this.btnApply.Location = new System.Drawing.Point(268, 21);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(128, 34);
            this.btnApply.TabIndex = 49;
            this.btnApply.Text = "적 용";
            this.btnApply.UseVisualStyleBackColor = false;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtpp);
            this.groupBox1.Controls.Add(this.textBox5);
            this.groupBox1.Controls.Add(this.textBox7);
            this.groupBox1.Controls.Add(this.txtpr);
            this.groupBox1.Controls.Add(this.btnStart);
            this.groupBox1.Controls.Add(this.btnApply);
            this.groupBox1.Controls.Add(this.btnFinish);
            this.groupBox1.Controls.Add(this.txtCoinBalance);
            this.groupBox1.Controls.Add(this.textBox18);
            this.groupBox1.Controls.Add(this.txtCoin);
            this.groupBox1.Controls.Add(this.txtKRW);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(12, 422);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(402, 246);
            this.groupBox1.TabIndex = 50;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "실행";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.BackColor = System.Drawing.Color.Black;
            this.groupBox2.Controls.Add(this.chkAutoScroll);
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.txtLog);
            this.groupBox2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(420, 13);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Size = new System.Drawing.Size(930, 655);
            this.groupBox2.TabIndex = 51;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Log";
            // 
            // chkAutoScroll
            // 
            this.chkAutoScroll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkAutoScroll.AutoSize = true;
            this.chkAutoScroll.Checked = true;
            this.chkAutoScroll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoScroll.Location = new System.Drawing.Point(747, 29);
            this.chkAutoScroll.Name = "chkAutoScroll";
            this.chkAutoScroll.Size = new System.Drawing.Size(96, 21);
            this.chkAutoScroll.TabIndex = 37;
            this.chkAutoScroll.Text = "AutoScroll";
            this.chkAutoScroll.UseVisualStyleBackColor = true;
            // 
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.BackColor = System.Drawing.SystemColors.WindowText;
            this.txtLog.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.txtLog.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLog.ForeColor = System.Drawing.SystemColors.Window;
            this.txtLog.Location = new System.Drawing.Point(7, 24);
            this.txtLog.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(915, 619);
            this.txtLog.TabIndex = 2;
            // 
            // txtSellRate
            // 
            this.txtSellRate.BackColor = System.Drawing.Color.LightGray;
            this.txtSellRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSellRate.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSellRate.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSellRate.ForeColor = System.Drawing.Color.Black;
            this.txtSellRate.Location = new System.Drawing.Point(140, 341);
            this.txtSellRate.Name = "txtSellRate";
            this.txtSellRate.Size = new System.Drawing.Size(256, 38);
            this.txtSellRate.TabIndex = 57;
            this.txtSellRate.Text = "1";
            this.txtSellRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtSellRate.Visible = false;
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.Black;
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.textBox2.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.ForeColor = System.Drawing.Color.White;
            this.textBox2.Location = new System.Drawing.Point(6, 344);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(128, 28);
            this.textBox2.TabIndex = 56;
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtpp
            // 
            this.txtpp.BackColor = System.Drawing.Color.LightGray;
            this.txtpp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtpp.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtpp.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtpp.ForeColor = System.Drawing.Color.Black;
            this.txtpp.Location = new System.Drawing.Point(141, 197);
            this.txtpp.Name = "txtpp";
            this.txtpp.Size = new System.Drawing.Size(254, 38);
            this.txtpp.TabIndex = 52;
            this.txtpp.Text = "0";
            this.txtpp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox5
            // 
            this.textBox5.BackColor = System.Drawing.Color.Black;
            this.textBox5.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox5.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.textBox5.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox5.ForeColor = System.Drawing.Color.White;
            this.textBox5.Location = new System.Drawing.Point(7, 155);
            this.textBox5.Name = "textBox5";
            this.textBox5.ReadOnly = true;
            this.textBox5.Size = new System.Drawing.Size(128, 28);
            this.textBox5.TabIndex = 51;
            this.textBox5.Text = "수익율 (%)";
            this.textBox5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox7
            // 
            this.textBox7.BackColor = System.Drawing.Color.Black;
            this.textBox7.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox7.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.textBox7.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox7.ForeColor = System.Drawing.Color.White;
            this.textBox7.Location = new System.Drawing.Point(7, 200);
            this.textBox7.Name = "textBox7";
            this.textBox7.ReadOnly = true;
            this.textBox7.Size = new System.Drawing.Size(128, 28);
            this.textBox7.TabIndex = 53;
            this.textBox7.Text = "수익금";
            this.textBox7.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtpr
            // 
            this.txtpr.BackColor = System.Drawing.Color.LightGray;
            this.txtpr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtpr.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtpr.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtpr.ForeColor = System.Drawing.Color.Black;
            this.txtpr.Location = new System.Drawing.Point(141, 152);
            this.txtpr.Name = "txtpr";
            this.txtpr.Size = new System.Drawing.Size(254, 38);
            this.txtpr.TabIndex = 50;
            this.txtpr.Text = "0";
            this.txtpr.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Bot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1362, 681);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox6);
            this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Bot";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bot";
            this.Load += new System.EventHandler(this.Bot_Load);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.ComboBox cmbAlgorithm;
        private System.Windows.Forms.TextBox textBox16;
        private System.Windows.Forms.TextBox txtFee;
        private System.Windows.Forms.TextBox textBox23;
        private System.Windows.Forms.TextBox txtTradeRate;
        private System.Windows.Forms.TextBox textBox14;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnFinish;
        private System.Windows.Forms.TextBox txtKRW;
        private System.Windows.Forms.TextBox textBox18;
        private System.Windows.Forms.TextBox txtCoinBalance;
        private System.Windows.Forms.TextBox txtCoin;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox lblmin;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox txtTriggerRate;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.TextBox txtInterval;
        private System.Windows.Forms.TextBox textBox10;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.ComboBox cmbCoin;
        private System.Windows.Forms.CheckBox chkAutoScroll;
        private System.Windows.Forms.TextBox txtCandleCount;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.ComboBox cmbCandle;
        private System.Windows.Forms.TextBox txtSellRate;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox txtpp;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.TextBox txtpr;
    }
}