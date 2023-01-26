using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using ZedGraph;
using spMain.QData.DataFormat;

namespace spMain.Comp {
  partial class StockGraph {
    class PaneHeader {

      // =============================  Static section ====================================
      static Font _fontAxis = new Font("Arial", 8.25f, FontStyle.Regular);
      static Font _fontLabel = new Font("Arial", 8.25f, FontStyle.Regular);
      static Font _fontValue = new Font("Arial", 8.25f, FontStyle.Regular);

      static Color _colorAxis = Color.Black;
      static Color _colorLabel = Color.Black;
      static Color _colorValue = Color.Blue;

      static FontSpec _graphFontAxis = QData.Common.XScale.GetZedGraphFontSize(_fontAxis, _colorAxis);
      static FontSpec _graphFontLabel = QData.Common.XScale.GetZedGraphFontSize(_fontLabel, _colorLabel);
      static FontSpec _graphFontValue = QData.Common.XScale.GetZedGraphFontSize(_fontValue, _colorValue);

      public static float GetPaneHeaderHeight() {
        return Math.Max(Math.Max(_fontAxis.GetHeight(), _fontLabel.GetHeight()), _fontValue.GetHeight());
//        return Convert.ToSingle(Math.Floor(Math.Max(Math.Max(_fontAxis.GetHeight(), _fontLabel.GetHeight()), _fontValue.GetHeight())));
      }
      public static List<string> GetLabelText(string labelTemplate, PointPair pp, DateTime date) {
        string s = labelTemplate;
        if (String.IsNullOrEmpty(s)) return new List<string>();
        s = s.Replace("{D}", csUtils.StringFromDateTime(date));
        s = s.Replace("{d}", csUtils.StringFromDateTime(date));
        if (pp.Tag is Quote) {
          Quote quote = (Quote)pp.Tag;
          s = s.Replace(@"{O}", csUtils.FormattedStringFromObject(quote.open));
          s = s.Replace(@"{o}", csUtils.FormattedStringFromObject(quote.open));
          s = s.Replace(@"{H}", csUtils.FormattedStringFromObject(quote.high));
          s = s.Replace(@"{h}", csUtils.FormattedStringFromObject(quote.high));
          s = s.Replace(@"{L}", csUtils.FormattedStringFromObject(quote.low));
          s = s.Replace(@"{l}", csUtils.FormattedStringFromObject(quote.low));
          s = s.Replace(@"{C}", csUtils.FormattedStringFromObject(quote.close));
          s = s.Replace(@"{c}", csUtils.FormattedStringFromObject(quote.close));
          s = s.Replace(@"{V}", csUtils.FormattedStringFromObject(quote.volume));
          s = s.Replace(@"{v}", csUtils.FormattedStringFromObject(quote.volume));
        }
        else if (pp is StockPt) {
          StockPt sp = (StockPt)pp;
          s = s.Replace(@"{O}", csUtils.FormattedStringFromObject(sp.Open));
          s = s.Replace(@"{o}", csUtils.FormattedStringFromObject(sp.Open));
          s = s.Replace(@"{H}", csUtils.FormattedStringFromObject(sp.High));
          s = s.Replace(@"{h}", csUtils.FormattedStringFromObject(sp.High));
          s = s.Replace(@"{L}", csUtils.FormattedStringFromObject(sp.Low));
          s = s.Replace(@"{l}", csUtils.FormattedStringFromObject(sp.Low));
          s = s.Replace(@"{C}", csUtils.FormattedStringFromObject(sp.Close));
          s = s.Replace(@"{c}", csUtils.FormattedStringFromObject(sp.Close));
          s = s.Replace(@"{V}", csUtils.FormattedStringFromObject(sp.Vol));
          s = s.Replace(@"{v}", csUtils.FormattedStringFromObject(sp.Vol));
        }
        else if (pp.Tag is double) {
          double x = (double)pp.Tag;
          s = s.Replace(@"{X}", csUtils.FormattedStringFromObject(x));
          s = s.Replace(@"{x}", csUtils.FormattedStringFromObject(x));
        }
        else {
          s = s.Replace(@"{X}", csUtils.FormattedStringFromObject(pp.Y));
          s = s.Replace(@"{x}", csUtils.FormattedStringFromObject(pp.Y));
        }
        return new List<string>(s.Split(new string[] { @"\n" }, StringSplitOptions.None));
      }

      // ================================   Object ==============================
      TextObj _labelLeft = null;
      TextObj _labelRight = null;
      TextObj _labelYRangeLabel = null;
      TextObj _labelYRangeValue = null;
      double _labelGapDraw = -3;// points
      double _labelGapShow = -3;// points
      double _interLabelGapDraw = -8; // Gap between label and value
      double _interLabelGapShow = -5; // Gap between label and value
      double _xCurrent = 0;
      double _xRight;
      GraphPane _pane;
      double _lastXRightDraw = -1;
      public List<string> _labelTemplates = new List<string>();

      public PaneHeader(GraphPane pane) {
        this._pane = pane;
        QData.UI.UIPane uiPane = (QData.UI.UIPane)pane.Tag;
        foreach (QData.UI.UIIndicator ind in uiPane.Indicators) {
          this._labelTemplates.Add(ind.GetLabelTemplateForGraph());
        }
      }

      public void DrawBackground(Graphics g) {
        if (_xCurrent < _xRight) {
          double width = Math.Min(this._xRight, _lastXRightDraw + 8);
          g.FillRectangle(new SolidBrush(this._pane.Fill.Color), Convert.ToSingle(_xCurrent), this._pane.Rect.Y,
//            Convert.ToSingle(width - _xCurrent), GetPaneHeaderHeight() + 1.0f);
            Convert.ToSingle(width - _xCurrent), this._pane.Margin.Top-1f);
        }
        _lastXRightDraw = -1;// Reset value
      }

      public double DrawLabels(Graphics g, string labelTemplate, PointPair pp, DateTime date, QData.UI.Curve curve, double xOffset) {// When cursor moving (Only draw in Graphics)
        int pictureHeight = Convert.ToInt32( Math.Floor(PaneHeader.GetPaneHeaderHeight()));
        int pictureWidth = Math.Max(Convert.ToInt32( pictureHeight * 1.5), 10);
        pictureWidth = QData.UI.CurveEditor.GetPictureWidth(pictureHeight, curve);
        double xCurrent = (xOffset == 0 ? this._xCurrent : xOffset + 4);
        double xRight = this._xRight;
        if (curve.CurveStyle == spMain.QData.Common.General.CurveStyle.Solid) {
        }
        if ((xCurrent + pictureWidth) < (xRight + this._labelGapDraw)) {
          List<string> labels = PaneHeader.GetLabelText(labelTemplate, pp, date);
          if (labels.Count > 0) {
            Rectangle r1 = new Rectangle(Convert.ToInt32(xCurrent), Convert.ToInt32(this._pane.Rect.Y), pictureWidth, pictureHeight);
            QData.UI.CurveEditor.PaintCurve(g, r1, curve);
            xCurrent += pictureWidth;
            int colorCnt = 0;
            for (int i = 0; i < labels.Count && xCurrent < (xRight + this._labelGapDraw); i++) {
              Color c = ((colorCnt % 2) == 0 ? _colorLabel : _colorValue);
              Font font = ((colorCnt % 2) == 0 ? _fontLabel : _fontValue);
              xCurrent += DrawString(g, labels[i], font, xCurrent, xRight, c) + this._interLabelGapDraw;
              this._lastXRightDraw = xCurrent;
              colorCnt++;
            }
          }
        }
        return xCurrent;
      }

      double DrawString(Graphics g, string text, Font font, double xCurrent, double xRight, Color textColor) {
        if (xCurrent < xRight) {
          double width = TextRenderer.MeasureText(text, font).Width + 3;
          if (width > xRight - xCurrent) {
            width = xRight - xCurrent;
            RectangleF r = new RectangleF(Convert.ToSingle(xCurrent + this._pane.Rect.X), this._pane.Rect.Y,
              Convert.ToSingle(width), font.GetHeight());
            g.DrawString(text, font, new SolidBrush(textColor), r);
          }
          else {
            g.DrawString(text, font, new SolidBrush(textColor), Convert.ToSingle(xCurrent + this._pane.Rect.X), this._pane.Rect.Y);
          }
          return width;
        }
        else return 0;
      }

      public void ShowLabels() {// After axis changed (Add labels)
        double xPerPixel = (this._pane.XAxis.Scale.Max - this._pane.XAxis.Scale.Min) / this._pane.Chart.Rect.Width;
        this._xCurrent = 0;
        this._xRight = this._pane.Rect.Width;

        // Left YAxis label
        if (this._pane.YAxis.Scale.IsVisible && this._pane.YAxis.Scale.Mag != 0) {
          if (_labelLeft == null) {
            this._labelLeft = GetLabel(_graphFontAxis);
            this._labelLeft.Location.X = 0;
            this._pane.GraphObjList.Add(this._labelLeft);
          }
          this._labelLeft.Text = "10^" + this._pane.YAxis.Scale.Mag.ToString();
          this._xCurrent = TextRenderer.MeasureText(this._labelLeft.Text, _fontAxis).Width + this._labelGapShow;
        }
        else {
          if (this._labelLeft != null) {
            this.ClearObject(this._labelLeft); 
            this._labelLeft = null;
          }
        }
        // Left Y2Axis label
        if (this._pane.Y2Axis.Scale.IsVisible && this._pane.Y2Axis.Scale.Mag != 0) {
          if (_labelRight == null) {
            this._labelRight = GetLabel(_graphFontAxis);
            this._labelRight.Location.X = 1;
            this._labelRight.Location.AlignH = AlignH.Right;
            this._pane.GraphObjList.Add(this._labelRight);
          }
          this._labelRight.Text = "10^" + this._pane.Y2Axis.Scale.Mag.ToString();
          this._xRight -= TextRenderer.MeasureText(this._labelRight.Text, _fontAxis).Width;
        }
        else {
          if (this._labelRight != null) {
            this.ClearObject(this._labelRight); 
            this._labelRight = null;
          }
        }
        // YRange label
        if (this._pane.Y2Axis.Scale.Mag != 0 && Math.Round(this._xCurrent, 5) == 0) {
        }
        if (this._pane.Y2Axis.Scale.Mag != 0) {
        }
        if (this._labelYRangeLabel == null) {
          this._labelYRangeLabel = GetLabel(_graphFontLabel);
          this._labelYRangeValue = GetLabel(_graphFontValue);
          this._pane.GraphObjList.Add(this._labelYRangeLabel);
          this._pane.GraphObjList.Add(this._labelYRangeValue);
        }
        this._labelYRangeLabel.Location.X = this._xCurrent / this._pane.Rect.Width;
        this._labelYRangeLabel.Text = "Y range:";
        this._xCurrent += TextRenderer.MeasureText(this._labelYRangeLabel.Text, _fontLabel).Width + this._interLabelGapShow;
        double yRange = Math.Round(this._pane.Y2Axis.Scale.Max - this._pane.Y2Axis.Scale.Min,12);
        int mag = this._pane.Y2Axis.Scale.Mag;
        string s;
        if (mag == 0) {
          s = csUtils.FormattedStringFromObject(yRange);//.ToString();
        }
        else {
          s = (yRange / Math.Pow(10, mag)).ToString() + "^" + mag.ToString();
        }
        this._labelYRangeValue.Location.X = this._xCurrent / this._pane.Rect.Width;
        this._labelYRangeValue.Text = s;
        this._xCurrent += TextRenderer.MeasureText(this._labelYRangeValue.Text, _fontValue).Width;

      }

      TextObj GetLabel(FontSpec fontSpec) {
        TextObj label = new TextObj();
        label.IsClippedToChartRect = false;
        label.Location.AlignH = AlignH.Left;
        label.Location.AlignV = AlignV.Top;
        label.Location.Y = 0;
        label.Location.CoordinateFrame = CoordType.PaneFraction;
        label.FontSpec = fontSpec;
        label.FontSpec.Border.IsVisible = false;
        label.FontSpec.Border.Width=0;
        label.ZOrder = ZOrder.A_InFront;
        return label;
      }

      void ClearObject(GraphObj go) {
        if (go != null) {
          if (this._pane.GraphObjList.IndexOf(go) >= 0) {
            this._pane.GraphObjList.Remove(go);
          }
        }
      }

      public void ClearObjects() {
        ClearObject(this._labelLeft); this._labelLeft = null;
        ClearObject(this._labelRight); this._labelRight = null;
        ClearObject(this._labelYRangeLabel); this._labelYRangeLabel = null;
        ClearObject(this._labelYRangeValue); this._labelYRangeValue = null;
      }
    }
  }
}
