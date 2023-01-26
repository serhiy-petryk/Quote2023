using System;
using System.Collections.Generic;
using System.Text;

namespace spMain.Comp {
  public partial class StockGraph : ZedGraph.ZedGraphControl {

    const double initPixelsToOnePoint = 12.0;
//   const string srlFN = @"c:\x1.srl";// serialization file name

    static StockGraph() {// Global Defaults
      ZedGraph.Scale.Default.FontFamily = "Tahoma";
      ZedGraph.Scale.Default.FontSize = 9f;
      ZedGraph.Scale.Default.FormatDayDay = "dd.MM.yy";
      ZedGraph.Scale.Default.FormatMonthMonth = "dd.MM.yy";
      ZedGraph.Scale.Default.MinGrace = 0.0;
      ZedGraph.Scale.Default.MaxGrace = 0.0;

      ZedGraph.Legend.Default.FontFamily = "Tahoma";
      ZedGraph.Legend.Default.FontSize = 10f;
      ZedGraph.Legend.Default.Gap = 0;
      ZedGraph.Legend.Default.IsBorderVisible = false; // ???

      ZedGraph.PaneBase.Default.IsFontsScaled = false; /// It is allow to allign pains (using YAxis.MinSpace) after resizing
      ZedGraph.PaneBase.Default.TitleGap = 0;
      ZedGraph.PaneBase.Default.IsShowTitle = true; ///???
      ZedGraph.PaneBase.Default.IsBorderVisible = false;
      ZedGraph.PaneBase.Default.IsPenWidthScaled = false;
// do not tauch !!! Printing is incorect !!!      ZedGraph.PaneBase.Default.BaseDimension=1.0f;

      ZedGraph.Margin.Default.Left = 0;
      ZedGraph.Margin.Default.Top = 0;
      ZedGraph.Margin.Default.Right = 0;
      ZedGraph.Margin.Default.Bottom = 0;

      ZedGraph.MasterPane.Default.IsShowLegend = false;
      ZedGraph.MasterPane.Default.InnerPaneGap = 0;
      ZedGraph.MasterPane.Default.PaneLayout = ZedGraph.PaneLayout.SingleColumn;

    }
  }
}
