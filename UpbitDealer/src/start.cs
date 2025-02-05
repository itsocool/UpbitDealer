﻿using UpbitDealer.form;
using System;
using System.Windows.Forms;
using UpbitDealer.src;
using log4net;

namespace UpbitDealer
{
    static class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));

        public static string Accesskey { get; internal set; }
        public static string Secretkey { get; internal set; }

        [STAThread]
        static void Main()
        {
            System.Diagnostics.Process[] processes = null;
            string strCurrentProcess = System.Diagnostics.Process.GetCurrentProcess().ProcessName.ToUpper();
            processes = System.Diagnostics.Process.GetProcessesByName(strCurrentProcess);
            if (processes.Length > 1)
            {
                MessageBox.Show("Already program executed.");
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += (sender, e) =>
            {
                log.Error(e.Exception);
            };

            login login = new login();
            Application.Run(login);
            if (login.isGood)
            {
                Accesskey = login.access_key;
                Secretkey = login.secret_key;
                Application.Run(new Bot());
            }
        }
    }
}
