using System;
using System.Drawing;
using System.Windows.Forms;
using ZedGraph;

namespace spMain.Comp {
  public partial class StockGraph : ZedGraphControl {

// not need    bool _contextMenuOpen = false;
    // ============================  Mouse Up/Down/Move ================================
    bool StockGraph_MouseDownEvent(ZedGraphControl sender, MouseEventArgs e) {
      if (Control.MouseButtons == MouseButtons.Left && csWinApi.IsKeyPress(Keys.T)) {// Text
        return DrawText(e);
      }
      if (Control.MouseButtons == MouseButtons.Left && csWinApi.IsKeyPress(Keys.L)) {// Line
        return StartDrawLine(e, false);
      }
      if (Control.MouseButtons == MouseButtons.Left && csWinApi.IsKeyPress(Keys.A)) {// Arrow
        return StartDrawLine(e, true);
      }
      //      return false;

      this._noPaneResizing = this.SetNoPaneResizing(e);
      if (this._noPaneResizing != -1) {
        _fPaneResizing = e.Y;
        this.Capture = true;
        return true;
      }
      else {
        return _MouseDown_Zooming(e);
      }
    }

    bool StockGraph_MouseUpEvent(ZedGraphControl sender, MouseEventArgs e) {
      if (_drawLineObj != null) {
        if (_drawLineObj.Location.Width == 0 && _drawLineObj.Location.Height == 0) {// Remove blank line
          _drawPane.GraphObjList.Remove(_drawLineObj);
        }
        _drawLineObj = null;
        _drawPane = null;
        sender.Invalidate();
        return true;
      }
      if (this._noPaneResizing != -1) {
        this._noPaneResizing = -1;
        this._fPaneResizing = 0f;
        this.Capture = false;
        return true;
      }
      else {
        if (this._isZooming) {
          this._isZooming = false;
          this.Capture = false;
          return true;
        }
        return false;
      }
    }

    void StockGraph_MouseMove(object sender, MouseEventArgs e) {
      Point p = this.PointToClient(Cursor.Position);
      if (_drawLineObj != null) {
        if (Control.MouseButtons == MouseButtons.Left && (csWinApi.IsKeyPress(Keys.L) || csWinApi.IsKeyPress(Keys.A))) {
          DrawLine(e);
          return;
        }
        _drawLineObj = null;
        _drawPane = null;
      }

      if (this._MouseMove_PaneResizing(e)) return;
      if (this._isZooming) {
        this._MouseMove_Zooming(e);
        return;
      }
      if (e.Button != MouseButtons.None) {
        return;
      }
      lock (_cursorObj) {
//        if (this.CursorRestorePicture()) this.CursorDrawCross();
        this.CursorRestorePicture();
        this.CursorDrawCross();
      }
    }

    bool StockGraph_MouseMoveEvent(ZedGraphControl sender, MouseEventArgs e) => e.Button != MouseButtons.Left;

    // ======================  Other Mouse Events ===========================
    void StockGraph_MouseLeave(object sender, EventArgs e) {
      this.CursorRestorePicture();
    }

    // Bug! Mouse wheel does a scroll of graph instead of zooming (after KeyLeft/Right on Hscroll) 
    // To deactivate HScroolBar we need to do 2 activation (vScrollbar(even if control is not visible) & thisControl)
    void sb_MouseWheel(object sender, MouseEventArgs e) {
      if (_vScrollBar != null) {
        this.ParentForm.ActiveControl = _vScrollBar;
        this.ParentForm.ActiveControl = this;
      }
    }

    // ========================  Scroll events =================================
    void StockGraph_ScrollDoneEvent(ZedGraphControl sender, ScrollBar scrollBar, ZoomState oldState, ZoomState newState) {
      if (this._uiGraph != null) {
        this.RoundXScale();// Round to integer X coordinate
        this.AxisChange();// Adjust XScale
        this.Invalidate();
      }
    }

    void StockGraph_ScrollEvent(object sender, ScrollEventArgs e) {
      // Если все доджи, то масштаб сбивается (шкала стает с большим запасом). 
      //Если дальше скролинг, то нет сигнала на скролинг (возврат функции false) и высота свечей - маленькая
//       if (this._labelYRangeLabel
      this.AxisChange();
      this.Invalidate();
    }

    // ========================  Zoom events =================================
    void StockGraph_ZoomEvent(ZedGraphControl sender, ZoomState oldState, ZoomState newState) {
      // Нижняя строка исправила CursorBag: когда при Zoom идет MouseButtonUp и курсор за пределами Zedgraph control, остается след от курсора
      //      if (!this.IsPointInside(this.PointToClient(Cursor.Position))) this._cursorBMX = null;// не отрисовывать курсор
      if (this.CursorGetActivePaneNo(this.PointToClient(Cursor.Position)) < 0) this._cursorBMX = null;// не отрисовывать курсор
      this.AxisChange();// Adjust scale after zoom
      this.Invalidate();
    }

    // ========================  Menu events =================================
    void StockGraph_ContextMenuBuilder(ZedGraphControl sender, ContextMenuStrip menuStrip, Point mousePt, ZedGraphControl.ContextMenuObjectState objState) {
      this.CursorRestorePicture();// Clear cursor before menu open
//      menuStrip.Opened -= new EventHandler(menuStrip_Opened);
  //    menuStrip.Opened += new EventHandler(menuStrip_Opened);

      ToolStripMenuItem item = new ToolStripMenuItem();
      item.Name = "Test";
      item.Text = "Test";
      item.Click += new EventHandler(item_Click);
      menuStrip.Items.Add(item);
      log.Add("ContextMenuBuilder" + ", " + DateTime.Now.TimeOfDay.ToString());
    }

    /*void menuStrip_Opened(object sender, EventArgs e) {
      log.Add("ContextMenuOpened" + ", " + DateTime.Now.TimeOfDay.ToString());
      ((ContextMenuStrip)sender).Opened -= new EventHandler(menuStrip_Opened);
      this._contextMenuOpen = true;
    }*/

    // ==============================  Other events ================================
    void StockGraph_Resize(object sender, EventArgs e) {
      this.AxisChange();
      this.Invalidate();
    }

    void StockGraph_Disposed(object sender, EventArgs e) {
      this.Disposed -= new EventHandler(StockGraph_Disposed);
      this.UIGraphClear();
    }

  }
}
