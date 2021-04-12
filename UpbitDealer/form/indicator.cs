﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using UpbitDealer.src;

namespace UpbitDealer.form
{
    public partial class Indicator : Form
    {
        private Main ownerForm;

        private List<string> hotList;
        private List<string> dangerList;
        private DataView btcBollinger;
        private DataView avgBollinger;
        private NameValue bbLowest;


        public Indicator(Main ownerForm)
        {
            InitializeComponent();
            this.ownerForm = ownerForm;
        }
        private void Indicator_Load(object sender, EventArgs e)
        {
            setDefault();
        }
        private void text_focus_disable(object sender, EventArgs e)
        {
            groupBox1.Focus();
        }


        private void setDefault(int index = 0)
        {
            lock (ownerForm.lock_mainUpdater)
            {
                hotList = new List<string>(ownerForm.mainUpdater.hotList);
                dangerList = new List<string>(ownerForm.mainUpdater.dangerList);
            }
            lock (ownerForm.lock_macro)
            {
                btcBollinger = new DataView(ownerForm.macro.indexBollinger[index].Tables[0]);
                avgBollinger = new DataView(ownerForm.macro.indexBollinger[index].Tables[1]);
                bbLowest = new NameValue(ownerForm.macro.bbLowest[index]);
            }

            list_hotList.Items.Clear();
            list_dangerList.Items.Clear();
            for (int i = 0; i < hotList.Count; i++) list_hotList.Items.Add(hotList[i]);
            for (int i = 0; i < dangerList.Count; i++) list_dangerList.Items.Add(dangerList[i]);

            setDefaultButton();
            chart1.Series["btc"].Points.DataBind(btcBollinger, "date", "value", "");
            chart1.Series["avg"].Points.DataBind(avgBollinger, "date", "value", "");
            chart1.Series["dev"].Points.DataBind(avgBollinger, "date", "dev", "");
            if (btcBollinger.Count > 0)
            {
                switch (index)
                {
                    case 0: chart1.ChartAreas["ChartArea"].AxisX.Maximum = ((DateTime)btcBollinger[0][0]).AddMinutes(10).ToOADate(); break;
                    case 1: chart1.ChartAreas["ChartArea"].AxisX.Maximum = ((DateTime)btcBollinger[0][0]).AddMinutes(30).ToOADate(); break;
                    case 2: chart1.ChartAreas["ChartArea"].AxisX.Maximum = ((DateTime)btcBollinger[0][0]).AddHours(1).ToOADate(); break;
                    case 3: chart1.ChartAreas["ChartArea"].AxisX.Maximum = ((DateTime)btcBollinger[0][0]).AddHours(4).ToOADate(); break;
                    case 4: chart1.ChartAreas["ChartArea"].AxisX.Maximum = ((DateTime)btcBollinger[0][0]).AddDays(1).ToOADate(); break;
                    case 5: chart1.ChartAreas["ChartArea"].AxisX.Maximum = ((DateTime)btcBollinger[0][0]).AddDays(7).ToOADate(); break;
                }
                chart1.ChartAreas["ChartArea"].AxisX.Minimum = ((DateTime)btcBollinger[btcBollinger.Count - 1][0]).ToOADate();
                text_btc.Text = ((double)btcBollinger[0]["value"]).ToString("0.##");
                text_avg.Text = ((double)avgBollinger[0]["value"]).ToString("0.##");
                text_dis.Text = ((double)avgBollinger[0]["dev"]).ToString("0.##");
                text_min_name.Text = bbLowest.coinName;
                text_min_value.Text = bbLowest.value.ToString("0.##");
            }
            switch (index)
            {
                case 0:
                    btn_min10.BackColor = Color.Red;
                    chart1.ChartAreas["ChartArea"].AxisX.IntervalType = DateTimeIntervalType.Hours;
                    chart1.ChartAreas["ChartArea"].AxisX.Interval = 1;
                    break;
                case 1:
                    btn_min30.BackColor = Color.Red;
                    chart1.ChartAreas["ChartArea"].AxisX.IntervalType = DateTimeIntervalType.Hours;
                    chart1.ChartAreas["ChartArea"].AxisX.Interval = 3;
                    break;
                case 2:
                    btn_hour1.BackColor = Color.Red;
                    chart1.ChartAreas["ChartArea"].AxisX.IntervalType = DateTimeIntervalType.Hours;
                    chart1.ChartAreas["ChartArea"].AxisX.Interval = 6;
                    break;
                case 3:
                    btn_hour4.BackColor = Color.Red;
                    chart1.ChartAreas["ChartArea"].AxisX.IntervalType = DateTimeIntervalType.Days;
                    chart1.ChartAreas["ChartArea"].AxisX.Interval = 1;
                    break;
                case 4:
                    btn_day.BackColor = Color.Red;
                    chart1.ChartAreas["ChartArea"].AxisX.IntervalType = DateTimeIntervalType.Days;
                    chart1.ChartAreas["ChartArea"].AxisX.Interval = 6;
                    break;
                case 5:
                    btn_week.BackColor = Color.Red;
                    chart1.ChartAreas["ChartArea"].AxisX.IntervalType = DateTimeIntervalType.Weeks;
                    chart1.ChartAreas["ChartArea"].AxisX.Interval = 6;
                    break;
            }
        }
        private void setDefaultButton()
        {
            btn_min10.BackColor = Color.DarkGray;
            btn_min30.BackColor = Color.DarkGray;
            btn_hour1.BackColor = Color.DarkGray;
            btn_hour4.BackColor = Color.DarkGray;
            btn_day.BackColor = Color.DarkGray;
            btn_week.BackColor = Color.DarkGray;
        }


        private void btn_min10_Click(object sender, EventArgs e)
        {
            setDefault(0);
        }
        private void btn_min30_Click(object sender, EventArgs e)
        {
            setDefault(1);
        }
        private void btn_hour1_Click(object sender, EventArgs e)
        {
            setDefault(2);
        }
        private void btn_hour4_Click(object sender, EventArgs e)
        {
            setDefault(3);
        }
        private void btn_day_Click(object sender, EventArgs e)
        {
            setDefault(4);
        }
        private void btn_week_Click(object sender, EventArgs e)
        {
            setDefault(5);
        }
    }
}