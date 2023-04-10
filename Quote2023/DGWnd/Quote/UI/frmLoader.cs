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

        private async void btnSaveIntradaySnapshotsToDb_Click(object sender, EventArgs e)
        {
            btnSaveIntradaySnapshotsToDb.Enabled = false;
            await Task.Factory.StartNew(Quote.Actions.MinutePolygon_CopySnapshotsToDb.Start);
            btnSaveIntradaySnapshotsToDb.Enabled = true;
        }

        private void btnStopSavingIntradaySnapshotsToDb_Click(object sender, EventArgs e)
        {
            Quote.Actions.MinuteAlphaVantage_CopySnapshotsToDb.StopFlag = true;
            Quote.Actions.MinutePolygon_CopySnapshotsToDb.StopFlag = true;
        }

        private void frmLoader_FormClosed(object sender, FormClosedEventArgs e)
        {
            Logger.MessageAdded -= Logger_MessageAdded;
        }
    }
}
