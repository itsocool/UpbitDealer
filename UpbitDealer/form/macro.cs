﻿using UpbitDealer.src;
using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;

namespace UpbitDealer.form
{
    public partial class Macro : Form
    {
        private MacroSettingData setting;


        public Macro()
        {
            InitializeComponent();
        }
        private void Macro_Load(object sender, EventArgs e)
        {
            setDefaultSetting();
        }
        private void text_focus_disable(object sender, EventArgs e)
        {
            groupBox1.Focus();
        }


        private void setDefaultSetting()
        {
            lock (((Main)Owner).lock_macro)
                setting = ((Main)Owner).macro.setting;

            btn_pause.BackColor = setting.pause ? Color.Red : Color.DarkGray;

            text_top.Text = setting.top.ToString();
            text_yield.Text = setting.yield.ToString();
            text_krw.Text = setting.krw.ToString();
            text_time.Text = setting.time.ToString();
            text_limit.Text = setting.limit.ToString();
            text_lostCut.Text = setting.lostCut.ToString();

            if (setting.week > -90000d) text_week.Text = setting.week.ToString();
            if (setting.day > -90000d) text_day.Text = setting.day.ToString();
            if (setting.hour4 > -90000d) text_hour4.Text = setting.hour4.ToString();
            if (setting.hour1 > -90000d) text_hour1.Text = setting.hour1.ToString();
            if (setting.min30 > -90000d) text_min30.Text = setting.min30.ToString();

            check_week_bias.Checked = setting.week_bias;
            check_day_bias.Checked = setting.day_bias;
            check_hour4_bias.Checked = setting.hour4_bias;
            check_hour1_bias.Checked = setting.hour1_bias;
            check_min30_bias.Checked = setting.min30_bias;

            check_week_auto.Checked = setting.week_auto;
            check_day_auto.Checked = setting.day_auto;
            check_hour4_auto.Checked = setting.hour4_auto;
            check_hour1_auto.Checked = setting.hour1_auto;
            check_min30_auto.Checked = setting.min30_auto;
        }


        private void btn_save_Click(object sender, EventArgs e)
        {
            if (text_yield.Text == "" || text_krw.Text == "" || text_time.Text == "")
            {
                MessageBox.Show("Check Parameters.");
                return;
            }

            MacroSettingData setting = new MacroSettingData();

            setting.pause = this.setting.pause;
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
            if (!double.TryParse(text_week.Text, out setting.week))
            {
                if (text_week.Text == "") setting.week = -100000;
                else
                {
                    MessageBox.Show("Week rate is not number.");
                    return;
                }
            }
            if (!double.TryParse(text_day.Text, out setting.day))
            {
                if (text_day.Text == "") setting.day = -100000;
                else
                {
                    MessageBox.Show("Day rate is not number.");
                    return;
                }
            }
            if (!double.TryParse(text_hour4.Text, out setting.hour4))
            {
                if (text_hour4.Text == "") setting.hour4 = -100000;
                else
                {
                    MessageBox.Show("4 hour rate is not number.");
                    return;
                }
            }
            if (!double.TryParse(text_hour1.Text, out setting.hour1))
            {
                if (text_hour1.Text == "") setting.hour1 = -100000;
                else
                {
                    MessageBox.Show("1 hour rate is not number.");
                    return;
                }
            }
            if (!double.TryParse(text_min30.Text, out setting.min30))
            {
                if (text_min30.Text == "") setting.min30 = -100000;
                else
                {
                    MessageBox.Show("'from' 30 minute rate is not number.");
                    return;
                }
            }
            setting.week_bias = check_week_bias.Checked;
            setting.day_bias = check_day_bias.Checked;
            setting.hour4_bias = check_hour4_bias.Checked;
            setting.hour1_bias = check_hour1_bias.Checked;
            setting.min30_bias = check_min30_bias.Checked;
            setting.week_auto = check_week_auto.Checked;
            setting.day_auto = check_day_auto.Checked;
            setting.hour4_auto = check_hour4_auto.Checked;
            setting.hour1_auto = check_hour1_auto.Checked;
            setting.min30_auto = check_min30_auto.Checked;

            if (setting.top < 0 || setting.yield < 0 || setting.krw < 0 ||
                setting.time < 0 || setting.limit < 0 || setting.lostCut < 0)
            {
                MessageBox.Show("Top, yield, krw, timem limit and lostCut value can't be negative.");
                return;
            }
            if (!(setting.week >= -10000d ||
                setting.day >= -10000d ||
                setting.hour4 >= -10000d ||
                setting.hour1 >= -10000d ||
                setting.min30 >= -10000d ||
                setting.week_auto ||
                setting.day_auto ||
                setting.hour4_auto ||
                setting.hour1_auto ||
                setting.min30_auto
                ))
            {
                MessageBox.Show("One of rate value must be set and the value must be at least -10000.\n" +
                                "Or one of rate auto must be checked.");
                return;
            }

            lock (((Main)Owner).lock_macro)
                ((Main)Owner).macro.saveMacroSetting(setting);

            setDefaultSetting();
            MessageBox.Show("Save success.");
        }
        private void btn_cancel_Click(object sender, EventArgs e)
        {
            setDefaultSetting();
        }
        private void btn_pause_Click(object sender, EventArgs e)
        {
            setting.pause = setting.pause ? false : true;
            lock (((Main)Owner).lock_macro)
                ((Main)Owner).macro.saveMacroSetting(setting);
            setDefaultSetting();

            if (setting.pause)
                MessageBox.Show("Pause Macro.");
            else
                MessageBox.Show("Continue Macro.");
        }
    }
}
