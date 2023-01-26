using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Design;
using System.ComponentModel;
using System.Collections.Generic;
using ZedGraph;

namespace spMain.QData.UI {

  public class CurveEditor : UITypeEditor {// Editor for Curve, CurveStyle and Zedgraph.SymbolType
    // =====================  Override section =========================
    public override bool GetPaintValueSupported(ITypeDescriptorContext context) {
      return true;
    }

    public override void PaintValue(PaintValueEventArgs e) {
      if (e.Value is Curve) {
        Rectangle r = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width - e.Bounds.X * 2, e.Bounds.Height - e.Bounds.Y * 2);
        PaintCurve(e.Graphics, r, (Curve)e.Value);
      }
      else if (e.Value is Common.General.CurveStyle) {
        e.Graphics.FillRectangle(new SolidBrush(SystemColors.Window), e.Bounds);// Draw background
        Rectangle r = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 2, e.Bounds.Height - 2);
        PaintCurveStyle(e.Graphics, r, (Common.General.CurveStyle)e.Value);
      }
      else if (e.Value is SymbolType) {
        Color symbolColor = ((Curve)e.Context.Instance).SymbolColor.ColorList[0];
        Rectangle r = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1);
        e.Graphics.FillRectangle(new SolidBrush(SystemColors.Window), e.Bounds);// Draw background
        DrawSymbol(e.Graphics, r, (SymbolType)e.Value, symbolColor);
      }
    }

    // ========================  Public static section ==========================
    public static void PaintCurve(Graphics g, Rectangle bounds, Curve curve) {
      switch (curve.CurveStyle) {
        case spMain.QData.Common.General.CurveStyle.Candle:
          DrawCandle(g, bounds, SystemColors.WindowText);
          break;
        case spMain.QData.Common.General.CurveStyle.OHLC:
          DrawOHLC(g, bounds, curve.BarColor.ColorList.Length == 0 ? Color.Black : curve.BarColor.ColorList[0]);
          break;
        case spMain.QData.Common.General.CurveStyle.Bar:
          DrawBar(g, bounds, curve.BarColor.ColorList);
          break;
        default:
          if (curve.CurveStyle != Common.General.CurveStyle.None) {
            DashStyle dashStyle = (DashStyle)Enum.Parse(typeof(DashStyle), curve.CurveStyle.ToString());
            DrawLine(g, bounds, dashStyle, curve.LineColor, 1);
          }
          if (curve.SymbolType != SymbolType.None) {
            DrawSymbol(g, bounds, curve.SymbolType, curve.SymbolColor.ColorList[0]);
          }
          break;
      }
    }

    public static void PaintCurveStyle(Graphics g, Rectangle bounds, Common.General.CurveStyle curveStyle) {
      switch (curveStyle) {
        case Common.General.CurveStyle.Candle:
          DrawCandle(g, bounds, SystemColors.WindowText);
          break;
        case Common.General.CurveStyle.OHLC:
          DrawOHLC(g, bounds, SystemColors.WindowText);
          break;
        case Common.General.CurveStyle.Bar:
          DrawBar(g, bounds, new Color[] { SystemColors.WindowText });
          break;
        default:
          if (curveStyle != Common.General.CurveStyle.None) {
            DashStyle dashStyle = (DashStyle)Enum.Parse(typeof(DashStyle), curveStyle.ToString());
            DrawLine(g, bounds, dashStyle, SystemColors.WindowText, 1);
          }
          break;
      }
    }

    public static int GetPictureWidth(int pictureHeight, Curve curve) {
      switch (curve.CurveStyle) {
        case spMain.QData.Common.General.CurveStyle.Candle: return pictureHeight + 3;
        case spMain.QData.Common.General.CurveStyle.OHLC: return 11;
        case spMain.QData.Common.General.CurveStyle.Bar: return 20;
        default:
          int i = (curve.CurveStyle == Common.General.CurveStyle.None ? 0 : 8);
          if (curve.CurveStyle == Common.General.CurveStyle.None) {
            return (curve.SymbolType == SymbolType.None ? 0 : pictureHeight);
          }
          else {
            return (curve.SymbolType == SymbolType.None ? pictureHeight : pictureHeight * 2);
          }
      }
    }

    // =======================  Private section ============================
    static void DrawCandle(Graphics g, Rectangle bounds, Color color) {
      using (Pen pen = new Pen(color)) {
        pen.Width = 1;
        int x1 = bounds.X + bounds.Width / 2;
        int height = bounds.Height - 2;
        int y1 = (bounds.Y + 2) + height / 3 - 1;
        int y2 = (bounds.Y + 2) + height / 3 * 2;
        int width = (y2 - y1) / 2;

        g.DrawLine(pen, x1, bounds.Y + 2, x1, y1);
        g.DrawLine(pen, x1, y2 + 1, x1, bounds.Y + bounds.Height - 1);
        Rectangle r2 = new Rectangle(x1 - width, y1, width * 2, y2 - y1 + 1);
        g.DrawRectangle(pen, r2);
        pen.Dispose();
      }
    }

    static void DrawOHLC(Graphics g, Rectangle bounds, Color color) {
      using (Pen pen = new Pen(color)) {
        pen.Width = 1;
        int x1 = bounds.X + bounds.Width / 2;
        int height = bounds.Height - 2;
        int y1 = (bounds.Y + 2) + height / 3;
        int y2 = (bounds.Y + 2) + height / 3 * 2;
        g.DrawLine(pen, x1, bounds.Y + 2, x1, bounds.Y + bounds.Height - 1);
        g.DrawLine(pen, x1 - 4, y1, x1, y1);
        g.DrawLine(pen, x1 + 4, y2, x1, y2);
        pen.Dispose();
      }
    }

    static void DrawLine(Graphics g, Rectangle bounds, DashStyle dashStyle, Color color, int penWidth) {
      using (Pen pen = new Pen(color)) {
        pen.Width = penWidth;
        pen.DashStyle = dashStyle;
        g.DrawLine(pen, bounds.X, bounds.Y + bounds.Height / 2, bounds.X + bounds.Width, bounds.Y + bounds.Height / 2);
        pen.Dispose();
      }
    }

    static void DrawSymbol(Graphics g, Rectangle bounds, SymbolType symbolType, Color color) {
      Symbol x = new Symbol(symbolType, color);
      x.Size = bounds.Height - 2 - 1;
      x.Fill = new Fill(Color.White, color);
      x.DrawSymbol(g, new GraphPane(), bounds.X + bounds.Width / 2, bounds.Y + bounds.Height / 2, 0.99f, false, null);
    }

    static void DrawBar(Graphics g, Rectangle bounds, Color[] colors) {
      List<Fill> fills = new List<Fill>();
      foreach (Color c in colors) {
        fills.Add(new Fill(Color.White, c));
      }

      int width = 4;
      int x1 = bounds.X + width / 2;
      int colorCnt = 0;
      int barHeight = bounds.Height * 3 / 4;
      while (x1 < (bounds.X + bounds.Width)) {
        Rectangle r = new Rectangle(x1, bounds.Y + (bounds.Height - barHeight), width, bounds.Height - 1);
        Fill fill = new Fill(Color.White, colors[colorCnt]);
        Brush brush = fills[colorCnt].MakeBrush(r);
        Pen pen = new Pen(colors[colorCnt]);
        g.FillRectangle(brush, r);
        //   Draw border (3 sides)
        g.DrawLine(pen, r.X, r.Y, r.X, r.Y + r.Height - 1);
        g.DrawLine(pen, r.X + r.Width, r.Y, r.X + r.Width, r.Y + r.Height - 1);
        g.DrawLine(pen, r.X, r.Y, r.X + r.Width, r.Y);
        brush.Dispose(); pen.Dispose();
        x1 += width + width / 2;
        colorCnt++;
        if (colorCnt == colors.Length) colorCnt = 0;
      }
    }
  }
}


