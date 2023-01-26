using System;
using System.Drawing;
using ZedGraph;

namespace spMain.Comp {
  partial class StockGraph {

    double _oldXPixelsPerTick = -1;

    void AdjustSymbolSize() {
      double range = this.GraphPane.XAxis.Scale.Max - this.GraphPane.XAxis.Scale.Min;
      if (range != 0) {
        double xPixelsPerTick = Math.Round(this.GraphPane.Chart.Rect.Width / range, 1);
        if (xPixelsPerTick != _oldXPixelsPerTick) {
          this._oldXPixelsPerTick = xPixelsPerTick;
          float symbolSize = Math.Min(10f, Convert.ToSingle(this._oldXPixelsPerTick * 2 / 3));
          foreach (GraphPane pane in this.MasterPane.PaneList) {
            foreach (CurveItem ci in pane.CurveList) {
              if (ci is LineItem) {
                LineItem li = (LineItem)ci;
                if (li.Symbol.Fill.Type == FillType.GradientByColorValue) li.Symbol.Size = symbolSize;
              }
            }
          }
        }
      }
    }

    void AdjustPane(GraphPane pane, bool IsFirstPane) {
      pane.YAxis.IsVisible = _yScaleLeftVisible;
      pane.Y2Axis.IsVisible = true;

      pane.Title.IsVisible = false;
      pane.Legend.IsVisible = false;
      pane.Title.FontSpec.Border.IsVisible = false;
      pane.Legend.FontSpec.Border.IsVisible = false;

      pane.Border.Width = 0;
      pane.Border.IsVisible = false;
      pane.TitleGap = 0;

      // Legend
      pane.Legend.FontSpec.Family = "Tahoma";
      pane.Legend.FontSpec.Size = 10f;
      pane.Legend.Gap = 0.05f;

      // Title
      pane.Title.FontSpec.Family = "Tahoma";
      pane.Title.FontSpec.Size = 10f;
      pane.Title.FontSpec.IsBold = false;

      pane.Margin.All = 0;
      pane.YAxis.Title.Gap = 0;
      pane.Y2Axis.Title.Gap = 0;
      pane.BarSettings.MinClusterGap = 0.8F;
      pane.Chart.Fill = new Fill(Color.Empty);// body of pane: для объектов смесь цвета (colors of pane + object)
      pane.Fill = new Fill(Color.White);// border of pane

      pane.XAxis.Type = AxisType.Linear;
      pane.XAxis.Scale.MinorStep = 1;
      pane.XAxis.MinorGrid.IsVisible = false;
      pane.XAxis.MajorGrid.IsVisible = true;
      pane.XAxis.Scale.MajorStep = 10;

      pane.IsBoundedRanges = true;// Влияет на автоматическое изменение шкалы при Zoom/Scroll
      pane.IsFontsScaled = false;

      // Axis
      pane.YAxis.Scale.FontSpec.Family = "Tahoma";
      pane.Y2Axis.Scale.FontSpec.Family = "Tahoma";
      pane.XAxis.Scale.FontSpec.Family = "Tahoma";
      pane.YAxis.Scale.FontSpec.Size = 9f;
      pane.Y2Axis.Scale.FontSpec.Size = 9f;
      pane.XAxis.Scale.FontSpec.Size = 9f;
      pane.YAxis.Title.Gap = 0;
      pane.Y2Axis.Title.Gap = 0;
      pane.XAxis.Title.Gap = 0;
      pane.YAxis.Scale.LabelGap = 0;
      pane.Y2Axis.Scale.LabelGap = 0;
      pane.XAxis.Scale.LabelGap = 0;
      pane.XAxis.Scale.MinAuto = false;
      pane.YAxis.Scale.MinAuto = true;
      pane.Y2Axis.Scale.MinAuto = true;
      pane.XAxis.Scale.MaxAuto = false;
      pane.YAxis.Scale.MaxAuto = true;
      pane.Y2Axis.Scale.MaxAuto = true;

      pane.YAxis.Scale.MinGrace = 0.05;
      pane.YAxis.Scale.MaxGrace = 0.05;
      pane.Y2Axis.Scale.MinGrace = 0.05;
      pane.Y2Axis.Scale.MaxGrace = 0.05;

      // MinorTic
      pane.XAxis.MinorTic.IsInside = false;
      pane.XAxis.MinorTic.IsOutside = false;
      pane.XAxis.MinorTic.IsOpposite = false;
      pane.YAxis.MinorTic.IsInside = false;
      pane.YAxis.MinorTic.IsOutside = false;
      pane.YAxis.MinorTic.IsOpposite = false;
      pane.Y2Axis.MinorTic.IsInside = false;
      pane.Y2Axis.MinorTic.IsOutside = false;
      pane.Y2Axis.MinorTic.IsOpposite = false;

      // MajorTic
      pane.XAxis.MajorTic.IsInside = true;
      pane.XAxis.MajorTic.IsOutside = false;
      pane.XAxis.MajorTic.IsOpposite = true;
      pane.XAxis.MajorTic.Size = pane.XAxis.MinorTic.Size;
      pane.YAxis.MajorTic.IsInside = true;
      pane.YAxis.MajorTic.IsOutside = false;
      pane.YAxis.MajorTic.IsOpposite = true;
      pane.YAxis.MajorTic.Size = pane.YAxis.MinorTic.Size;
      pane.Y2Axis.MajorTic.IsInside = true;
      pane.Y2Axis.MajorTic.IsOutside = false;
      pane.Y2Axis.MajorTic.IsOpposite = true;
      pane.Y2Axis.MajorTic.Size = pane.Y2Axis.MinorTic.Size; // Reduce the size of tic

      //Grid
      pane.YAxis.MajorGrid.Color = Color.Gray;
      pane.Y2Axis.MajorGrid.Color = Color.Gray;
      pane.YAxis.MajorGrid.IsVisible = true;
      pane.Y2Axis.MajorGrid.IsVisible = true;

      pane.XAxis.MajorGrid.Color = Color.Gray;
      pane.XAxis.MajorGrid.IsVisible = true;
      pane.XAxis.IsVisible = false;
      if (IsFirstPane) pane.Margin.Bottom = _xLabelFont.GetHeight() + 2;// space for labels
      else pane.Margin.Bottom = 0.5f;
      //      pane.Margin.Top = _topLabelFont.GetHeight() + 2;// for UpLabel
      //      pane.Margin.Top = PaneHeader.GetHeaderHeight() + 1f;// for UpLabel
      pane.Margin.Top = PaneHeader.GetPaneHeaderHeight() + 2.5f;// for UpLabel

      // Axis Title
      pane.YAxis.Title.IsVisible = false;
      pane.Y2Axis.Title.IsVisible = false;
      pane.XAxis.Title.IsVisible = false;

      pane.XAxis.AxisGap = 0f;
      pane.XAxis.Scale.LabelGap = 0f;

    }

  }
}
