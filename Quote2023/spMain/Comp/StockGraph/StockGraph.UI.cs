using System;
using System.Collections.Generic;
using System.Drawing;
using ZedGraph;

namespace spMain.Comp {
  public partial class StockGraph : ZedGraphControl {

    const int _timerIntervalInMiliSecs = 1000;
    public QData.UI.UIGraph _uiGraph;
    List<DateTime> _dates;

    public Boolean _IsDataExists {
      get { return this._uiGraph != null && this._dates != null; }
    }

    public void _UIGraphChange() {
      QData.UI.UIGraph x1 = (this._uiGraph == null ? new spMain.QData.UI.UIGraph() : this._uiGraph);
      object x = cs.PGfrmObjectEditor._GetObject(x1, QData.UI.UIGraph._serializationFileName, true);
      if (x != null) {
        if (_uiGraph != null)
          _uiGraph.ClearDataSources();
        this._uiGraph = (QData.UI.UIGraph)x;
        this._UIGraphApply();
      }
    }

    public void _UIGraphApply() {
      if (this._uiGraph==null) {// no data
        this._dates = null;
        this._hScrollBar.Visible = false;
      }
      else {
        this._hScrollBar.Visible = true;
        List<float> heights = new List<float>();
        List<int> countList = new List<int>();

// doesnot work       this.MasterPane = new MasterPane(); // Clear ZedGraph (http://www.zedgraph.org/wiki/index.php?title=Clear_all_the_data_in_a_graph)
        this.UIGraphClear();
        this._uiGraph.CreateDataSources();
        this._dates= this._uiGraph.GetDateArray();
        // Create curves
        foreach (QData.UI.UIPane pane in this._uiGraph.Panes) {
          GraphPane zedPane = new GraphPane();
          zedPane.Tag = pane;
          foreach (QData.UI.UIIndicator ind in pane.Indicators) {
            ind.CreateCurve(zedPane);
          }
          if (zedPane.CurveList.Count > 0) {// not blank pane
            this.MasterPane.PaneList.Add(zedPane);
            heights.Add(this.MasterPane.PaneList.Count == 1 ? 1.0f: 0.5f);
            countList.Add(1);
          }
        }
        this._uiGraph.UpdateData(0);
        this.FillData(true);
        AdjustXGrace();
        GraphPane pane1 = this.MasterPane.PaneList[0];
        double xMax = this._dates.Count + _graceRight;
        double xMin = xMax - this.ClientRectangle.Width / initPixelsToOnePoint;
        if (!this._uiGraph.DataAdapter.IsStream && xMin < -_graceLeft) xMin = -_graceLeft;//Static Graph with small number of data: All data in chart (no scrolling)
        for (int i = 0; i < this.MasterPane.PaneList.Count; i++) {
          this.AdjustPane(this.MasterPane.PaneList[i], i == 0);
          this.MasterPane.PaneList[i].XAxis.Scale.Max =xMax;
          this.MasterPane.PaneList[i].XAxis.Scale.Min = xMin;
        }

        using (Graphics g = this.CreateGraphics()) {
          this.MasterPane.SetLayout(g, true, countList.ToArray(), heights.ToArray());
        }
        this.AxisChange();
        this.Invalidate();
        if (this._uiGraph.DataAdapter.IsStream) {
          csTimerManager.Register(this, _timerIntervalInMiliSecs, UpdateData);
        }
        if (_uiGraph.AutosizeOnOpen)
            _Autosize();
      }
    }

    void UpdateData() {
      double delta=0.5;
      double xMax = this.GraphPane.XAxis.Scale.Max;
      int cntBefore = (this._dates.Count-delta < xMax ? this._dates.Count : -1);
      int timerCnt = csTimerManager.GetTimerCnt();
      this._uiGraph.UpdateData(timerCnt);
      this.FillData(true);
      if (cntBefore > -1 && this._dates.Count!= cntBefore) {
        double xShift = Convert.ToDouble(this._dates.Count - cntBefore);
        foreach (GraphPane pane in this.MasterPane.PaneList) {
          pane.XAxis.Scale.Max += xShift;
          pane.XAxis.Scale.Min += xShift;
        }
      }
      this.AdjustXGrace();
      this.AxisChange();
      this.Invalidate();
    }

    void AdjustXGrace() {// After UpdateData 
      if (this._dates.Count > 0) {
        double x1 = this._graceLeft / this._dates.Count;
        double x2 = this._graceRight / this._dates.Count;
        foreach (GraphPane pane in this.MasterPane.PaneList) {
          pane.XAxis.Scale.MinGrace = x1; // 
          pane.XAxis.Scale.MaxGrace = x2;
        }
      }
    }

    void UIGraphClear() {
      csTimerManager.UnRegister(this);
      this._lastPaneSizes.Clear();// Reset sizes for refresh XScale/Box
      if (this._paneHeaders != null) {
        for (int i = 0; i < this._paneHeaders.Count; i++) {
          this._paneHeaders[i].ClearObjects();
        }
        this._paneHeaders = null;
      }
      foreach (GraphPane pane in this.MasterPane.PaneList) {
        pane.Tag = null;
      }
      if (this._uiGraph != null) {
        this._uiGraph.ClearDataSources();
      }

      _dates?.Clear();

      this.MasterPane.PaneList.Clear();
      this.MasterPane.GraphObjList.Clear();
    }

    public void _Autosize() {
      foreach (GraphPane pane in this.MasterPane.PaneList) {
        pane.XAxis.Scale.Max = this._dates.Count + _graceRight;
        pane.XAxis.Scale.Min = -this._graceLeft;
      }
      this.AxisChange();
      this.Invalidate();
    }

  }
}
