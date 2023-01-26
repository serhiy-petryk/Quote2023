using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ZedGraph;

namespace spMain.Comp {
  public partial class StockGraph : ZedGraphControl {

    const int WM_PAINT = 0x000F;
    Bitmap _cursorBMX = null;
    Bitmap _cursorBMY = null;
    Bitmap _scaleXBM = null;
    Bitmap _scaleYBM = null;
    //    List<List<Bitmap>> _scaleValueBM = new List<List<Bitmap>>();
    int _cursorLastX;
    int _cursorLastY;
    int _scaleXLastX;
    int _scaleXLastY;
    int _scaleYLastX;
    int _scaleYLastY;
    List<List<int>> _scaleValueX = new List<List<int>>();
    List<List<int>> _scaleValueY = new List<List<int>>();
    object _cursorObj = new object();
    bool _lastContainsFocus = false;
    bool _drewCursorFlag = false;

    protected override void WndProc(ref Message m) {
      base.WndProc(ref m);

      if (m.Msg == WM_PAINT && this.ParentForm != null && this.ParentForm.ContainsFocus) {
        // this.ParentForm.ContainsFocus: запрет на отрисовку курсора, если поверх находится другое окно
        lock (_cursorObj) {
          log.Add("WN_Paint_before" + ", " + DateTime.Now.TimeOfDay.ToString());
          //            if (this.cursorBMX != null) this.CursorDrawCross();
          this.CursorDrawCross();
          log.Add("WN_Paint_after" + ", " + DateTime.Now.TimeOfDay.ToString());
        }// lock
      }
    }

    void CursorRestorePicture() {
      _drewCursorFlag = false;
      if (_cursorBMX != null) {// restore control image
        if (_lastContainsFocus != this.ContainsFocus) {
          this._lastContainsFocus = this.ContainsFocus;
          this.Invalidate();
        }
        log.Add("Restore" + ", " + DateTime.Now.TimeOfDay.ToString());
        csUtilsCursor.FromBitmapToControl(this, _cursorBMX, _cursorLastX, 0);
        csUtilsCursor.FromBitmapToControl(this, _cursorBMY, 0, _cursorLastY);
        //        scaleBMX = csUtilsCursor.FromControlToBitmap(this, p.X - size.Width / 2, y, size.Width, size.Height);
        csUtilsCursor.FromBitmapToControl(this, _scaleXBM, _scaleXLastX, _scaleXLastY);
        csUtilsCursor.FromBitmapToControl(this, _scaleYBM, _scaleYLastX, _scaleYLastY);
        _cursorBMX.Dispose();
        _cursorBMY.Dispose();
        _scaleXBM.Dispose();
        _scaleYBM.Dispose();
        _cursorBMX = null;
        _cursorBMY = null;
        _scaleXBM = null;
        _scaleYBM = null;
      }
    }


    public List<string> log = new List<string>();

    int CursorGetActivePaneNo(Point p) {
      PaneList panes = this.MasterPane.PaneList;
      for (int i = 0; i < panes.Count; i++) {
        //        if (panes[i].Chart.Rect.Contains(p)) return i;// show cursor in pane areas
        //        if (panes[i].Rect.Contains(p)) return i;// show cursor in Graph area
        if (panes[i].Rect.Contains(p) && panes[i].Chart.Rect.Left <= p.X && (panes[i].Chart.Rect.Left + panes[i].Chart.Rect.Width) >= p.X) return i;// show cursor in Graph area
      }
      return -1;
    }

    void CursorDrawCross() {
      if (_drewCursorFlag) {
        _drewCursorFlag = false;
        this.Invalidate();
        return;
      }

      Point cursorPoint = this.PointToClient(Cursor.Position);
      int activePaneNo = this.CursorGetActivePaneNo(cursorPoint);
      if (activePaneNo >= 0 && this._dates != null) {
        _drewCursorFlag = true;
        log.Add("DrawCross" + ", " + DateTime.Now.TimeOfDay.ToString());
        // ===================== Set X scale data
        GraphPane pane = this.MasterPane.PaneList[activePaneNo];
        double xCurrent, yCurrent;
        pane.ReverseTransform(cursorPoint, out xCurrent, out yCurrent);
        int iCurrent = Convert.ToInt32(xCurrent - 0.5);
        int iDataCurrent = iCurrent - this._iDataMinOffset;
        string textX = "";
        if (iCurrent >= 0 && iCurrent < this._dates.Count) {
          DateTime dt = this._dates[iCurrent];
          textX = dt.ToString(this._uiGraph.TimeInterval.GetXScaleFormat());
        }
        Size sizeX = TextRenderer.MeasureText(textX, this._xLabelFont);
        _scaleXLastY = Convert.ToInt32(this.MasterPane.PaneList[0].Rect.Height - sizeX.Height); // !! first pane
        _scaleXLastX = cursorPoint.X - sizeX.Width / 2;

        // ================= Set Y scale data
        /* Плохая чувствительность       double yStep = pane.Y2Axis.Scale.MinorStep;
                double y2 = Convert.ToDouble(Convert.ToInt32(y1 / yStep)) * yStep;*/

        double yStep = pane.Y2Axis.Scale.MinorStep / 2.0;// чувствительность уменьшена в 2 раза
        double y2 = Convert.ToDouble(Convert.ToInt32(yCurrent / yStep)) * yStep;
        y2 = y2 / Math.Pow(10, pane.Y2Axis.Scale.Mag);// thousand, millions, ..

        string textY = y2.ToString();
        Size sizeY = TextRenderer.MeasureText(textY, this._xLabelFont);
        _scaleYLastX = Convert.ToInt32(pane.Rect.Width - sizeY.Width);
        _scaleYLastY = cursorPoint.Y - sizeY.Height / 2;

        // ================== Save control image to bitmap
        _scaleXBM = csUtilsCursor.FromControlToBitmap(this, _scaleXLastX, _scaleXLastY, sizeX.Width, sizeX.Height);
        _scaleYBM = csUtilsCursor.FromControlToBitmap(this, _scaleYLastX, _scaleYLastY, sizeY.Width, sizeY.Height);
        _cursorBMX = csUtilsCursor.FromControlToBitmap(this, cursorPoint.X, 0, 1, this.Height - 17);// ??? 17
        _cursorBMY = csUtilsCursor.FromControlToBitmap(this, 0, cursorPoint.Y, _scaleYLastX, 1);

        using (Graphics g = this.CreateGraphics()) {
          // Header Labels
          if (this._paneHeaders != null && this._paneHeaders.Count == this.MasterPane.PaneList.Count) {
            for (int i = 0; i < this.MasterPane.PaneList.Count; i++) {
              GraphPane pane1 = this.MasterPane.PaneList[i];
              QData.UI.UIPane uiPane = (QData.UI.UIPane)pane1.Tag;
              this._paneHeaders[i].DrawBackground(g);
              if (iCurrent >= 0 && iCurrent < this._dates.Count) {
                double xOffset = 0;
                for (int i1 = 0; i1 < pane1.CurveList.Count; i1++) {
                  if (iDataCurrent >= 0 && iDataCurrent < pane1.CurveList[i1].Points.Count) {// added at 2010-08-05
                    QData.UI.Curve curve = uiPane.Indicators[i1].Curve;
                    PointPair pp = pane1.CurveList[i1].Points[iDataCurrent];
                    xOffset = this._paneHeaders[i].DrawLabels(g, this._paneHeaders[i]._labelTemplates[i1], pp, this._dates[iCurrent], curve,  xOffset);
                  }
                  else {// lovushka (cursor is in margin of X Axis)
                  }
                }//for (int i1 = 0; i1 < pane1.CurveList.Count; i1++) {
              }// if (iCurrent >= 0 && iCurrent < this._dates.Count) {
            }
          }

          // ================ Draw cursor on control ============================
          g.DrawLine(new Pen(Color.Red, 1), cursorPoint.X, 0, cursorPoint.X, this.Height - 17);// Vertical
          g.DrawLine(new Pen(Color.Red, 1), 0, cursorPoint.Y, _scaleYLastX, cursorPoint.Y);// horisontal
          _cursorLastX = cursorPoint.X;// Save last positions
          _cursorLastY = cursorPoint.Y;


          // Draw X label
          g.FillRectangle(new SolidBrush(Color.Yellow), _scaleXLastX, _scaleXLastY, sizeX.Width, sizeX.Height);
          g.DrawString(textX, this._xLabelFont, new SolidBrush(Color.Blue), _scaleXLastX, _scaleXLastY);
          // Draw Y label
          g.FillRectangle(new SolidBrush(Color.Yellow), _scaleYLastX, _scaleYLastY, sizeY.Width, sizeY.Height);
          g.DrawString(textY, this._xLabelFont, new SolidBrush(Color.Blue), _scaleYLastX, _scaleYLastY);
        }

      }
    }


  }
}
