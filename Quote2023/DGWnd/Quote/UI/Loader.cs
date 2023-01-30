using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DGWnd.Quote.Helpers;
using DGWnd.UI;

namespace DGWnd.Quote.UI
{
    public partial class Loader : Form
    {
        private object _lock = new object();
        public Loader()
        {
            InitializeComponent();
        }

        private void ShowStatus(string message)
        {
            lock (_lock) statusLabel.Text = message;
            Application.DoEvents();
        }

        private void btnAddIntradaySnapshots_Click(object sender, EventArgs e)
        {
            var mainForm = this.TopLevelControl as frmMDI;
            if (CsHelper.OpenFileDialogMultiselect(Settings.MinuteYahooDataFolder, @"YahooMinute_202?????.zip file (*.zip)|YahooMinute_202?????.zip", true) is string[] files && files.Length > 0)
                Actions.AddIntradaySnapshoysInDb(ShowStatus, files, mainForm);

        }
    }
}
