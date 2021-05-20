using UpbitDealer.form;
using System;
using System.Windows.Forms;
using UpbitDealer.src;

namespace UpbitDealer
{
    static class Program
    {
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

            login login = new login();
            Application.Run(login);
            if (login.isGood)
            {
                Accesskey = login.access_key;
                Secretkey = login.secret_key;
                Application.Run(new Bot());
            }
                //Application.Run(new Main(login.access_key, login.secret_key));
        }
    }
}
