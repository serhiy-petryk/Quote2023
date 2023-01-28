using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using ZedGraph;

namespace spMain.Comp {
  partial class StockGraph {

    const bool _yScaleLeftVisible = true;
    static Font _topLabelFont = new Font("Arial", 9f);
    static Color _topLabelBackroundColor = Color.Gainsboro;
    float _xMarginRight;
    float _xMarginLeft;
    public const double _xLabelGap = 4.0f;
    List<double> _lastPaneSizes = new List<double>();
    Font _xLabelFont = new Font("Tahoma", 8f);
    List<PaneHeader> _paneHeaders = null;

    public bool _IsSnapshotLayout = false;
    double _graceLeft => _IsSnapshotLayout ? 1.0 : 1.0;
    double _graceRight => _IsSnapshotLayout ? 1.0 : 3.0;

    public override void AxisChange() {
      if (!this.DesignMode) {
        this.AxisChangeBefore();
        FillData(false);
        base.AxisChange();
        this.AxisChangeAfter();
      }
    }

    void HeaderShowLabels() {
      // Init PainHeaders
      if (this._paneHeaders == null) {
        this._paneHeaders = new List<PaneHeader>();
        foreach (GraphPane pane in this.MasterPane.PaneList) {
          this._paneHeaders.Add(new PaneHeader(pane));
        }
      }
      for (int i = 0; i < this.MasterPane.PaneList.Count; i++ ) {
        this._paneHeaders[i].ShowLabels();
      }
    }

    void AxisChangeBefore() {
      // Set ScroolMin/Max_X
      if (this.ScrollMinX != -_graceLeft) this.ScrollMinX = -_graceLeft;
      if (this._IsDataExists) {
        if (this.ScrollMaxX != (this._dates.Count + _graceRight)) this.ScrollMaxX = this._dates.Count + _graceRight;
      }
      else this.ScrollMaxX = _graceRight;
    }

    void AxisChangeAfter() {
      if (this._IsDataExists) {
        if (_IsSnapshotLayout)
          return;

        this.HeaderShowLabels();
        this.AlignXMarginOfPanes();
        this.AdjustSymbolSize();

        List<GraphPane> changedPanes = new List<GraphPane>();
        bool xChanged = GetChangedSizePanes(changedPanes);

        if (xChanged) {
          QData.Common.XScale.AddXScaleObjectsToPanes(this.MasterPane.PaneList, this._dates,
            this._uiGraph.TimeInterval, this._graceLeft, this._graceRight, _xLabelFont, this.GetLastYValuesOfPanes(), this._xMarginRight);
        }
        else {// No XScale changes == Change Y of pane objects (??? Vertical Resize == change Y position of XLabels) 
          foreach (GraphPane pane in changedPanes) {
            foreach (GraphObj o in pane.GraphObjList) {
              if ((o is BoxObj || o is LineObj) && o.Tag != null && o.Tag.ToString() == "_") {
                if (o is BoxObj) o.Location.Y = pane.Y2Axis.Scale.Max;// boxObj
                else o.Location.Y = pane.Y2Axis.Scale.Min;// line obj
                o.Location.Height = pane.Y2Axis.Scale.Max - pane.Y2Axis.Scale.Min;
              }
            }
          }
        }

        this.SavePaneSizes();
      }
    }

    
    // ==========================  Service Procedures ==============================
    public void AlignXMarginOfPanes() {
      PaneList panes = this.MasterPane.PaneList;
      float[] lefts = new float[panes.Count];
      float[] rights = new float[panes.Count];
      float maxLeft = 0;
      float maxRight = 0;
      for (int i = 0; i < panes.Count; i++) {
        float left = panes[i].Chart.Rect.X - panes[i].Margin.Left;
        float right = panes[i].Rect.Width - panes[i].Chart.Rect.Width - panes[i].Chart.Rect.X - panes[i].Margin.Right;
        lefts[i] = left;
        rights[i] = right;
        maxLeft = Math.Max(left, maxLeft);
        maxRight = Math.Max(right, maxRight);
      }

      List<List<double>> lastYValues = this.GetLastYValuesOfPanes();
      if (lastYValues != null) {// LastYValues labels
        for (int i1 = 0; i1 < lastYValues.Count; i1++) {
          GraphPane pane = this.MasterPane.PaneList[i1];
          for (int i2 = 0; i2 < lastYValues[i1].Count; i2++) {
            if (pane.Y2Axis.Scale.Min < lastYValues[i1][i2] && pane.Y2Axis.Scale.Max > lastYValues[i1][i2]) {
              double magFactor = Math.Pow(10, pane.Y2Axis.Scale.Mag);
              string text = (lastYValues[i1][i2] / magFactor).ToString();
              float labelWidth = Convert.ToSingle(TextRenderer.MeasureText(text, _xLabelFont).Width);
              maxRight = Math.Max(maxRight, labelWidth);
            }
          }
        }
      }

      for (int i = 0; i < panes.Count; i++) {
        float newLeft = maxLeft - lefts[i];
        if (newLeft != panes[i].Margin.Left)
          panes[i].Margin.Left = newLeft;
        float newRight = maxRight - rights[i];
        if (newRight != panes[i].Margin.Right)
          panes[i].Margin.Right = newRight;
      }

      this._xMarginLeft = maxLeft;
      this._xMarginRight = maxRight;
    }

    void RoundXScale() {// After scrolling done
      GraphPane p = this.MasterPane.PaneList[0];
      double x2 = p.XAxis.Scale.Max;
      double x1 = p.XAxis.Scale.Min;
      foreach (GraphPane pane in this.MasterPane.PaneList) {
        pane.XAxis.Scale.Max = Math.Round(x2);
        pane.XAxis.Scale.Min = Math.Round(x1);
      }
      double step = (this._hScrollBar.Maximum - this._hScrollBar.Minimum) / (this._graceLeft + this._dates.Count + this._graceRight);
      double xCurrent = this._hScrollBar.Value / step - this._graceLeft;
      int newValue = Convert.ToInt32((p.XAxis.Scale.Min + this._graceLeft) * step);
      double diff = (this._hScrollBar.Value - newValue) / step;
      this._hScrollBar.Value = newValue;
    }

    void SavePaneSizes() {
      //      _lastXCount = (this._dates == null ? 0 : this._dates.Count);
      _lastPaneSizes.Clear();
      _lastPaneSizes.Add((this._dates == null ? 0.0 : this._dates.Count));
      _lastPaneSizes.Add(this.MasterPane.PaneList[0].Rect.Width);
      _lastPaneSizes.Add(this.MasterPane.PaneList[0].XAxis.Scale.Min);
      _lastPaneSizes.Add(this.MasterPane.PaneList[0].XAxis.Scale.Max);
      foreach (GraphPane pane in this.MasterPane.PaneList) {
        _lastPaneSizes.Add(pane.Y2Axis.Scale.Min);
        _lastPaneSizes.Add(pane.Y2Axis.Scale.Max);
      }
    }

    bool GetChangedSizePanes(List<GraphPane> changedPanes) {
      List<bool> changes = new List<bool>();
      bool xChanged = true;
      if (_lastPaneSizes.Count == (this.MasterPane.PaneList.Count * 2 + 4)) {
        xChanged = this._dates.Count != this._lastPaneSizes[0] || // changes X scale
          this.MasterPane.PaneList[0].Rect.Width != this._lastPaneSizes[1] ||
          this.MasterPane.PaneList[0].XAxis.Scale.Min != this._lastPaneSizes[2] ||
          this.MasterPane.PaneList[0].XAxis.Scale.Max != this._lastPaneSizes[3];
        if (xChanged) {
        }
        for (int i = 0; i < this.MasterPane.PaneList.Count; i++) {
          GraphPane pane = this.MasterPane.PaneList[i];
          if (xChanged || pane.Y2Axis.Scale.Min != _lastPaneSizes[2 * i + 4] || pane.Y2Axis.Scale.Max != _lastPaneSizes[2 * i + 5]) {
            changedPanes.Add(pane);
          }
        }
      }
      else {
        for (int i = 0; i < this.MasterPane.PaneList.Count; i++) changedPanes.Add(this.MasterPane.PaneList[i]);
      }
      return xChanged;
    }


    // ======================== OnPaint =================================
    protected override void OnPaint(PaintEventArgs e) {
      if (this._IsDataExists) {
        base.OnPaint(e);
        // Set SmallChange Of HScrollBar: Zedgraph change SmaalChange = we need to have our SmallChange
        if (this.ScrollMaxX != this.ScrollMinX && this._hScrollBar != null && this._dates != null) {
          int smallChange = Convert.ToInt32((this._hScrollBar.Maximum - this._hScrollBar.Minimum)
            / (this._graceLeft + this._dates.Count + this._graceRight));
          if (smallChange < 1) smallChange = 1;
          if (this._hScrollBar.SmallChange != smallChange) this._hScrollBar.SmallChange = smallChange;
        }
      }
      else {
        e.Graphics.FillRectangle(new SolidBrush(Color.White), e.ClipRectangle);
        if (this._hScrollBar.Visible) this._hScrollBar.Visible = false;
      }
    }

    List<List<double>> GetLastYValuesOfPanes() {
      if (!this._IsDataExists || !this._uiGraph.DataAdapter.IsStream) return null;
      List<List<double>> values = new List<List<double>>();
      foreach (GraphPane pane in this.MasterPane.PaneList) {
        QData.UI.UIPane x = (QData.UI.UIPane)pane.Tag;
        values.Add(x.LastValuesOfIndicators);
      }
      return values;
    }


  }

}
