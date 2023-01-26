using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace spMain.Comp {

  public partial class UI_StockGraph : UserControl {

    public UI_StockGraph(){
      InitializeComponent();
      this.UpdateToolStrip();
    }

    // =====================  Public section =============================
    public void _SetUIGraph(QData.UI.UIGraph uiGraph) {
      this._stockGraph._uiGraph = uiGraph;
      this._stockGraph._UIGraphApply();
      this.UpdateToolStrip();
    }

    // =====================  Clicks =================================
    private void _btnDataSelect_Click(object sender, EventArgs e) {
      this._stockGraph._UIGraphChange();
      this.UpdateToolStrip();
    }

    private void _btnShowAllPoints_Click(object sender, EventArgs e) {
      this._stockGraph._ShowAllPoints();
    }

    private void _btnSaveAsFile_Click(object sender, EventArgs e) {
      this._stockGraph.SaveAs();
    }

    private void _btnPrint_Click(object sender, EventArgs e) {
      this._stockGraph._DoPrintPreview();
    }

    private void _btnCopyToClipboard_Click(object sender, EventArgs e) {
      this._stockGraph.Copy(true);
    }

    private void _btnDetach_Click(object sender, EventArgs e) {

    }


    // =====================  Private section =============================
    private void UpdateToolStrip() {
      this._btnDataSelect.Text = (this._stockGraph._uiGraph == null ||
        String.IsNullOrEmpty(this._stockGraph._uiGraph.GraphDescription) ? "Click me to choose data" : this._stockGraph._uiGraph.GraphDescription);

      if (!this.DesignMode) {
        if (this._btnShowAllPoints.Enabled != this._stockGraph._IsDataExists) this._btnShowAllPoints.Enabled = this._stockGraph._IsDataExists;
        if (this._btnCopyToClipboard.Enabled != this._stockGraph._IsDataExists) this._btnCopyToClipboard.Enabled = this._stockGraph._IsDataExists;
        if (this._btnPrint.Enabled != this._stockGraph._IsDataExists) this._btnPrint.Enabled = this._stockGraph._IsDataExists;
        if (this._btnSaveAsFile.Enabled != this._stockGraph._IsDataExists) this._btnSaveAsFile.Enabled = this._stockGraph._IsDataExists;
      }
    }

    private void _btnSaveDataToFile_Click(object sender, EventArgs e) {
      this._stockGraph._DoSaveDataToFile();
    }

    private void toolStripButton2_Click(object sender, EventArgs e) {
      this._stockGraph.log.Add("Before Message");
      MessageBox.Show(csUtils.MemoryUsedInBytes.ToString("N0"));
      this._stockGraph.log.Add("After Message");
    }

    private void toolStripButton3_Click(object sender, EventArgs e) {
      Dictionary<int, string> x = this._stockGraph._uiGraph.Panes[0].Indicators[0]._timeLog;
      this._stockGraph.log.Clear();
    }

  }
}
