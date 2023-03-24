using System;
using System.Diagnostics;
using System.Windows.Forms;
using WebScanner.Helpers;

namespace WebScanner
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            dtpRunAt.Value = DateTime.Now;
            dtpEndAt.Value = DateTime.Now.AddHours(12);

            nudInterval_ValueChanged(null, null);

            Logger.MessageAdded += (sender, args) => StatusLabel.Text = args.FullMessage;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (DateTime.Now >= dtpRunAt.Value && DateTime.Now <= dtpEndAt.Value)
            {
                if (cbNasdaqScreener.Checked)
                    Actions.NasdaqScreenerLoader.Start();

                if (cbTradingViewScreener.Checked)
                    Actions.TradingViewScreenerLoader.Start();

                Debug.Print($"Timer Tick {DateTime.Now}");
            }
        }

        private void nudInterval_ValueChanged(object sender, EventArgs e) => timer1.Interval = Convert.ToInt32(nudInterval.Value * 1000 * 60);

        private void checkBox1_CheckedChanged(object sender, EventArgs e) => timer1.Enabled = checkBox1.Checked;

        private void button1_Click(object sender, EventArgs e)
        {
            Actions.NasdaqScreenerLoader.Start();
            Actions.TradingViewScreenerLoader.Start();
        }
    }
}
