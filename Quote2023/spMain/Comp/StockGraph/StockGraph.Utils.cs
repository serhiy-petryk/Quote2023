using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Printing;
using ZedGraph;

namespace spMain.Comp {
  public partial class StockGraph : ZedGraph.ZedGraphControl {

    public void _DoSaveDataToFile() {
      if (this._IsDataExists) {
        ArrayList[] data = new ArrayList[this._dates.Count];
        for (int i = 0; i < data.Length; i++) data[i] = new ArrayList();
        List<string> indIDs = new List<string>();
        List<string> indHeaders = new List<string>();
        foreach (QData.UI.UIPane pane in this._uiGraph.Panes) {
          foreach (QData.UI.UIIndicator ind in pane.Indicators) {
            this.AddIndDataToArray(ind._dataInd, data, indIDs, indHeaders);
            for (int i = 0; i < ind._dataInd._childInds.Count; i++) {
              this.AddIndDataToArray(ind._dataInd._childInds[i], data, indIDs, indHeaders);
            }
          }
        }
        using (SaveFileDialog x = new SaveFileDialog()) {
          //        x.Filter = "txt files (*.txt)|*.txt|Excel files (*.xls)|*.xls|All files (*.*)|*.*";
          x.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
          x.FilterIndex = 1;
          x.RestoreDirectory = true;
          x.OverwritePrompt = true;

          try {
            if (indIDs.Count > 0 && data[0].Count > 0) {
              if (x.ShowDialog() == DialogResult.OK) {
                string fn = x.FileName;
                if (File.Exists(fn)) File.Delete(fn);
                using (StreamWriter sw = new StreamWriter(fn)) {
                  // Save headers
                  for (int i = 0; i < indIDs.Count; i++) sw.Write((i == 0 ? "" : "\t") + indHeaders[i]);
                  sw.Write(Environment.NewLine);
                  // Save data
                  for (int i = 0; i < data.Length; i++) {
                    for (int i1 = 0; i1 < data[i].Count; i1++) {
                      sw.Write((i1 == 0 ? "" : "\t") + csUtils.StringFromObject(data[i][i1]));
                    }
                    sw.Write(Environment.NewLine);
                  }
                }
              }
            }
          }
          catch (Exception ex) {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
          }
        }
      }
    }

    void AddIndDataToArray(QData.Data.DataIndicator ind, ArrayList[] data, List<string> existingInds, List<string> indHeaders) {
      string indID = ind._uniqueID;
      foreach (string s in existingInds) if (s == indID) return;// Data indicator already exist
      existingInds.Add(indID);
      indHeaders.Add(ind.GetFileHeader());
      for (int i = 0; i < data.Length; i++) {
        if (i < ind._data.Count) data[i].Add(ind._data[i]);
        else data[i].Add(null);
      }
    }

    public void _DoPrint() {
      // Add a try/catch pair since the users of the control can't catch this one
      try {
        bool flag = false;
        if (this.MasterPane.PaneList.Count > 0) {
          this.MasterPane.PaneList[0].Title.Text = "frbhtynhjt  Printed at " + csUtils.StringFromDateTime(DateTime.Now);
          this.MasterPane.PaneList[0].Title.FontSpec = new FontSpec("Arial", 24, Color.Black, true, false, false);
          this.MasterPane.PaneList[0].Title.FontSpec.Border.Width = 0;
          this.MasterPane.PaneList[0].Title.IsVisible = true;
          flag = true;
        }
        PrintDocument pd = PrintDocument;
        pd.DocumentName = "fdbhfgngh";
        if (pd != null) {
          //pd.PrintPage += new PrintPageEventHandler( Graph_PrintPage );
          using (PrintDialog pDlg = new PrintDialog()) {
            pDlg.Document = pd;
            if (pDlg.ShowDialog() == DialogResult.OK)
              pd.Print();
          }
        }
        if (flag) this.MasterPane.PaneList[0].Title.IsVisible = false;
      }
      catch (Exception exception) {
        MessageBox.Show(exception.Message);
      }
    }

    public void _DoPrintPreview() {
      /*   Zedgraph version   try {
              PrintDocument pd = PrintDocument;

              if (pd != null) {
                csCoolPrintPreview.MyPrintPreviewDialog ppd = new spMain.csCoolPrintPreview.MyPrintPreviewDialog();
                //pd.PrintPage += new PrintPageEventHandler( Graph_PrintPage );
                ppd.Document = pd;
                ppd.Show(this);
              }
            }
            catch (Exception exception) {
              MessageBox.Show(exception.Message);
            }*/

      using (csCoolPrintPreview.CoolPrintPreviewDialog dlg = new spMain.csCoolPrintPreview.CoolPrintPreviewDialog()) {
        PageSettings ps = this.PrintDocument.DefaultPageSettings;
        if (!ps.Landscape) {
          int left = ps.Margins.Top;
          int top = ps.Margins.Left;
          int right = ps.Margins.Bottom;
          int bottom = ps.Margins.Right;
          ps.Margins = new Margins(left, right, top, bottom);
          ps.Landscape = true;
        }
        dlg.Document = this.PrintDocument;
        dlg.ShowDialog(this);
      }
    }



  }
}

