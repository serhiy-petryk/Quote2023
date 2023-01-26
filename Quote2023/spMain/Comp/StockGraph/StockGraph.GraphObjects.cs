using System;
using System.Drawing;
using System.Windows.Forms;
//using System.Runtime.Serialization;
using ZedGraph;

namespace spMain.Comp {

  public partial class StockGraph : ZedGraphControl{

    GraphPane _drawPane = null;
    LineObj _drawLineObj = null;

    private bool StartDrawLine(MouseEventArgs e, bool IsArrow) {
      Point mousePoint = new Point(e.X, e.Y);
      Point p = this.PointToClient(Cursor.Position);
      int paneNo= this.CursorGetActivePaneNo(p);
      if (paneNo >= 0) {
        using (Graphics g = this.CreateGraphics()) {
          GraphPane pane = this.MasterPane.PaneList[paneNo];
            double x, y;
            // Convert the mouse location to X, Y
            pane.ReverseTransform(mousePoint, out x, out y);
            _drawPane = pane;
            if (IsArrow) {
              _drawLineObj = new ArrowObj(Color.Blue, 10, x, y, x, y);
            }
            else {
              _drawLineObj = new LineObj(Color.Blue, x, y, x, y);
            }
            _drawLineObj.IsClippedToChartRect = true;
            //						drawLineObj.ZOrder = ZOrder.E_BehindCurves;
            pane.GraphObjList.Add(_drawLineObj);
        }
        this.Invalidate();
      }
      return true;
    }

    private bool DrawLine(MouseEventArgs e) {
      Point mousePoint = new Point(e.X, e.Y);
      double x, y;
      _drawPane.ReverseTransform(mousePoint, out x, out y);
      _drawLineObj.Location.Height = y - _drawLineObj.Location.Y;
      _drawLineObj.Location.Width = x - _drawLineObj.Location.X;
      this.Invalidate();
      return true;
    }

    private bool DrawText(MouseEventArgs e) {
      using (Graphics g = this.CreateGraphics()) {
        Point p= this.PointToClient(Cursor.Position);
        Point labelPoint = new Point(e.X, e.Y);
        int paneNo = CursorGetActivePaneNo(p);
        if (paneNo>=0) {
          string s = Microsoft.VisualBasic.Interaction.InputBox("Please enter text", "Text", "", 100, 100);
          if (!String.IsNullOrEmpty(s)) {
            GraphPane pane = this.MasterPane.PaneList[paneNo];
            double x, y;
            // Convert the mouse location to X, Y
            pane.ReverseTransform(labelPoint, out x, out y);
            TextObj o = new TextObj(s, x, y);
            o.IsClippedToChartRect = true;
            o.Location.AlignH = AlignH.Left;
            o.Location.AlignV = AlignV.Center;
            o.FontSpec.Size = 14;
            o.FontSpec.Border.IsVisible = false;
            pane.GraphObjList.Add(o);
          }
          this.Invalidate();
          return true;
        }
      }
      return false;
    }



  }
}
