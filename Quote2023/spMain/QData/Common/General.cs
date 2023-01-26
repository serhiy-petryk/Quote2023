using System;
using System.Drawing;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;

namespace spMain.QData.Common {

  public partial class General {

    public const double doji_k = 1000.0; // doji definition
    public static Color candleUpColor = Color.White;
    public static Color candleDownColor = Color.Black;
    public static Color candleBorderColor = Color.Black;

    [Editor(typeof(QData.UI.CurveEditor), typeof(System.Drawing.Design.UITypeEditor))]
    public enum CurveStyle { Candle, OHLC, Bar, Solid, Dot, Dash, DashDot, DashDotDot, None };

    public enum SmoothType {
      Simple /*Average*/ , EMAComplex /* ExpAverage*/, EMA2, EMA/* ExpAverage*/, MAWeighted /*wma*/,
      MATriangular /* Average(Average()) - значения не совпадают with ThinkOrSwim (в ThinkOrSwim - линии с параметрами k=2 и k=3 идентичны (??? bug)*/
    };

    //    public enum TimeFrameKind { SameTimeFrame = 1, NewTimeFrame = 2, NewDate = 4 };
    public enum TimeFrameKind { SameTimeFrame = 1, NewTimeFrame = 2, NewDayPeriod = 3, NewDate = 4 };

    public static TimeSpan marketStart = new TimeSpan(9, 30, 0);//time=marketStart: inside market
    public static TimeSpan marketEnd = new TimeSpan(16, 0, 0);// time=marketEnd: inside market
    public static TimeSpan preMarketStart = new TimeSpan(7, 00, 0);// from http://www.nasdaq.com/about/schedule.stm
    public static TimeSpan afterMarketEnd = new TimeSpan(20, 0, 0);// from http://www.nasdaq.com/about/schedule.stm

  }
}
