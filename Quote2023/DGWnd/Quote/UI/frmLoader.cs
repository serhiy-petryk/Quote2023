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
using spMain.Comp;

namespace DGWnd.Quote.UI
{
    public partial class frmLoader : Form
    {
        private object _lock = new object();
        public frmLoader()
        {
            InitializeComponent();
        }

        private void ShowStatus(string message)
        {
            lock (_lock) statusLabel.Text = message;
            Application.DoEvents();
        }

        private async void btnSaveIntradaySnapshotsToDb_Click(object sender, EventArgs e)
        {
            btnSaveIntradaySnapshotsToDb.Enabled = false;
            if (CsHelper.OpenFileDialogMultiselect(Settings.MinuteAlphaVantageDataFolder,
                    @"MAV_202?????.zip file (*.zip)|MAV_202?????.zip") is string[] files && files.Length > 0)
            {
                await Task.Factory.StartNew(() => Quote.Actions.MinuteAlphaVantage_CopySnapshotsToDb.CopySnapshots(files, ShowStatus));
            }
            // if (CsHelper.OpenFileDialogMultiselect(Settings.MinuteYahooDataFolder, @"YahooMinute_202?????.zip file (*.zip)|YahooMinute_202?????.zip") is string[] files && files.Length > 0) 
            // Quote.Actions.MinuteYahoo_CopySnapshotsToDb.CopySnapshots(files, ShowStatus);
            btnSaveIntradaySnapshotsToDb.Enabled = true;
        }

        private void btnStopSavingIntradaySnapshotsToDb_Click(object sender, EventArgs e)
        {
            Quote.Actions.MinuteAlphaVantage_CopySnapshotsToDb.StopFlag = true;
        }
    }
}
