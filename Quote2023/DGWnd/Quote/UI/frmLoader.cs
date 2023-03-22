using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using DGWnd.Quote.Helpers;

namespace DGWnd.Quote.UI
{
    public partial class frmLoader : Form
    {
        private object _lock = new object();
        public frmLoader()
        {
            InitializeComponent();

            Logger.MessageAdded -= Logger_MessageAdded;
            Logger.MessageAdded += Logger_MessageAdded;
        }

        private void Logger_MessageAdded(object sender, Logger.MessageAddedEventArgs e) => statusLabel.Text = e.FullMessage;

        private void ShowStatus(string message)
        {
            lock (_lock) statusLabel.Text = message;
            Application.DoEvents();
        }

        private async void btnSaveIntradaySnapshotsToDb_Click(object sender, EventArgs e)
        {
            btnSaveIntradaySnapshotsToDb.Enabled = false;

            await Task.Factory.StartNew(Quote.Actions.MinuteAlphaVantage_CopySnapshotsToDb.Start);

            /*if (CsHelper.OpenFileDialogMultiselect(Settings.MinuteAlphaVantageDataFolder,
                    @"MAV_202?????.zip file (*.zip)|MAV_202?????.zip") is string[] files && files.Length > 0)
            {
                await Task.Factory.StartNew(() => Quote.Actions.MinuteAlphaVantage_CopySnapshotsToDb.CopySnapshots(files, ShowStatus));
            }
            // if (CsHelper.OpenFileDialogMultiselect(Settings.MinuteYahooDataFolder, @"YahooMinute_202?????.zip file (*.zip)|YahooMinute_202?????.zip") is string[] files && files.Length > 0) 
            // Quote.Actions.MinuteYahoo_CopySnapshotsToDb.CopySnapshots(files, ShowStatus);*/

            btnSaveIntradaySnapshotsToDb.Enabled = true;
        }

        private void btnStopSavingIntradaySnapshotsToDb_Click(object sender, EventArgs e)
        {
            Quote.Actions.MinuteAlphaVantage_CopySnapshotsToDb.StopFlag = true;
        }

        private void frmLoader_FormClosed(object sender, FormClosedEventArgs e)
        {
            Logger.MessageAdded -= Logger_MessageAdded;
        }
    }
}
