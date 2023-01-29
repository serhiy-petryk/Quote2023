using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace spMain.Comp
{

    public partial class UI_StockGraph : UserControl
    {

        public UI_StockGraph()
        {
            InitializeComponent();
            this.UpdateToolStrip();

            // Tip!! Example! Add checkbox to ToolStrip
            // var host = new ToolStripControlHost(_cbAutosizeOnOpen);
            // var k = _toolStrip.Items.IndexOf(_btnAutosize);
            // _toolStrip.Items.Insert(k + 1, host);
        }

        // =====================  Public section =============================
        public void _SetUIGraph(QData.UI.UIGraph uiGraph, bool isSnapshotLayout)
        {
            this._stockGraph._IsSnapshotLayout = isSnapshotLayout;
            this._stockGraph._uiGraph = uiGraph;
            this._stockGraph._UIGraphApply();
            this.UpdateToolStrip();
        }

        public void _Autosize() => this._stockGraph._Autosize();
        public void _CopyToClipboard() => this._stockGraph.Copy(false);

        // =====================  Clicks =================================
        private void _btnDataSelect_Click(object sender, EventArgs e)
        {
            this._stockGraph._UIGraphChange();
            this.UpdateToolStrip();
        }
        private void _btnAutosize_Click(object sender, EventArgs e) => this._stockGraph._Autosize();
        private void _btnSaveAsFile_Click(object sender, EventArgs e) => this._stockGraph.SaveAs();
        private void btnSAveAsImageFile_Click(object sender, EventArgs e) => this._stockGraph.SaveAsBitmap();
        private void _btnPrint_Click(object sender, EventArgs e) => this._stockGraph._DoPrintPreview();
        private void _btnCopyToClipboard_Click(object sender, EventArgs e) => this._stockGraph.Copy(true);
        private void _btnSaveDataToFile_Click(object sender, EventArgs e) => this._stockGraph._DoSaveDataToFile();

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            this._stockGraph.log.Add("Before Message");
            MessageBox.Show(csUtils.MemoryUsedInBytes.ToString("N0"));
            this._stockGraph.log.Add("After Message");
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Dictionary<int, string> x = this._stockGraph._uiGraph.Panes[0].Indicators[0]._timeLog;
            this._stockGraph.log.Clear();
        }


        // =====================  Private section =============================
        private void UpdateToolStrip()
        {
            this._btnDataSelect.Text = (this._stockGraph._uiGraph == null ||
              String.IsNullOrEmpty(this._stockGraph._uiGraph.GraphDescription) ? "Click me to choose data" : this._stockGraph._uiGraph.GraphDescription);

            if (!this.DesignMode)
            {
                if (this._btnAutosize.Enabled != this._stockGraph._IsDataExists) this._btnAutosize.Enabled = this._stockGraph._IsDataExists;
                if (this._btnCopyToClipboard.Enabled != this._stockGraph._IsDataExists) this._btnCopyToClipboard.Enabled = this._stockGraph._IsDataExists;
                if (this._btnPrint.Enabled != this._stockGraph._IsDataExists) this._btnPrint.Enabled = this._stockGraph._IsDataExists;
                if (this._btnSaveAsFile.Enabled != this._stockGraph._IsDataExists) this._btnSaveAsFile.Enabled = this._stockGraph._IsDataExists;
            }
        }

    }
}
