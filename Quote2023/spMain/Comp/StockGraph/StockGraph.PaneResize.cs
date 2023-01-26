using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ZedGraph;

namespace spMain.Comp {
  public partial class StockGraph : ZedGraphControl {

    float _fPaneResizing;
    int _noPaneResizing = -1;
    bool _isZooming = false;
    float _zoomingX;
    float _zoomingStartX;

    bool _MouseMove_PaneResizing(MouseEventArgs e) {
      if (this._noPaneResizing != -1) {
        PaneList panes = this.MasterPane.PaneList;
        float del = e.Y - this._fPaneResizing;
        float[] props = new float[panes.Count];
        for (int i = 0; i < panes.Count; i++) {
          props[i] = panes[i].Rect.Height;
        }
        if ((props[this._noPaneResizing - 1] + del) < 5 || (props[this._noPaneResizing] - del < 5)) del = 0;

        float s1 = 0f;
        float s2 = 0f;

        for (int i = 0; i < this._noPaneResizing; i++) s1 += panes[i].Rect.Height;
        for (int i = this._noPaneResizing; i < panes.Count; i++) s2 += panes[i].Rect.Height;

        int[] _dummy = new int[panes.Count];
        for (int i = 0; i < _dummy.Length; i++) _dummy[i] = 1; 
        for (int i = 0; i < this._noPaneResizing; i++) props[i] = props[i] * (s1 + del) / s1;
        for (int i = this._noPaneResizing; i < panes.Count; i++) props[i] = props[i] * (s2 - del) / s2;

        using (Graphics g = this.CreateGraphics()) {
          this.MasterPane.SetLayout(g, true, _dummy, props);
          this.AxisChange();
          this.Invalidate();
        }
        this._fPaneResizing += del;
        this.Cursor = Cursors.HSplit;
        return true;
      }
      else {
        if (this.SetNoPaneResizing(e) != -1) {
          this.Cursor = Cursors.HSplit;
          return true;
        }
        else {
          this.Cursor = Cursors.Default;
          return false;
        }
      }
    }

    int SetNoPaneResizing(MouseEventArgs e) {
      float delta = 1.5F;
      PaneList panes = this.MasterPane.PaneList;
      for (int i = 1; i < panes.Count; i++) {
        if (e.Y > (panes[i].Rect.Y - delta) && e.Y < (panes[i].Rect.Y + delta)) {
          return i;
        }
      }
      return -1;
    }

    // ===================================================
    bool _MouseDown_Zooming(MouseEventArgs e) {
      RectangleF r1 = this.MasterPane.PaneList[0].Rect;
      RectangleF r2 = this.MasterPane.PaneList[0].Chart.Rect;
      RectangleF r = new RectangleF(r2.X, r2.Y + r2.Height, r2.Width, (r1.Height + r1.Y) - (r2.Height + r2.Y));
      if (r.Contains(new PointF(e.X, e.Y))) {
        this._isZooming = true;
        this._zoomingX = e.X;
        this._zoomingStartX = e.X;
        this.Capture = true;
        return true;
      }
      return false;
    }

    void _MouseMove_Zooming(MouseEventArgs e) {
      float delta = e.X - this._zoomingX;

      this._zoomingX = e.X;

      double x1 = this.GraphPane.XAxis.Scale.Min;
      double x2 = this.GraphPane.XAxis.Scale.Max;
      double xx1 = Math.Round(x1 * (1 - delta / this._zoomingStartX), 1);
      double xx2 = Math.Round(x2 * (1 + delta / this._zoomingStartX), 1);
      if (xx1 > xx2) xx1 = xx2 - 1;
      xx1 = Math.Max(this.ScrollMinX, xx1);
      xx2 = Math.Min(this.ScrollMaxX, xx2);

      PaneList panes = this.MasterPane.PaneList;
      for (int i = 0; i < panes.Count; i++) {
        panes[i].XAxis.Scale.Min = xx1;
        panes[i].XAxis.Scale.Max = xx2;
      }
      this.AxisChange();
      this.Invalidate();
    }

  }
}
