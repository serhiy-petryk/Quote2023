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

        private void btnSaveIntradaySnapshotsToDb_Click(object sender, EventArgs e)
        {
            if (CsHelper.OpenFileDialogMultiselect(Settings.MinuteYahooDataFolder, @"YahooMinute_202?????.zip file (*.zip)|YahooMinute_202?????.zip", true) is string[] files && files.Length > 0)
                Quote.Actions.CopyYahooIntradaySnapshotsToDb.CopySnapshots(files, ShowStatus);
        }
    }
}
