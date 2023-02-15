using System;
using System.Drawing;
using System.Collections.Generic;
using ZedGraph;

namespace spMain.QData.Common {
  class XScale {
    public static List<TimeInterval> _xLabelTimeIntervals;// List of xScale Standard fonts

    static XScale() {
      Init();
    }

    static void Init() {
      if (_xLabelTimeIntervals == null) {
        _xLabelTimeIntervals = new List<TimeInterval>();
        _xLabelTimeIntervals.Add(new TimeInterval(5));
        _xLabelTimeIntervals.Add(new TimeInterval(10));
        _xLabelTimeIntervals.Add(new TimeInterval(15));
        _xLabelTimeIntervals.Add(new TimeInterval(30));
        _xLabelTimeIntervals.Add(new TimeInterval(60));
        _xLabelTimeIntervals.Add(new TimeInterval(120));
        _xLabelTimeIntervals.Add(new TimeInterval(5*60));
        _xLabelTimeIntervals.Add(new TimeInterval(10*60));
        _xLabelTimeIntervals.Add(new TimeInterval(15*60));
        _xLabelTimeIntervals.Add(new TimeInterval(30*60));
        _xLabelTimeIntervals.Add(new TimeInterval(60*60));
        _xLabelTimeIntervals.Add(new TimeInterval(120*60));
        _xLabelTimeIntervals.Add(new TimeInterval(-1));
        _xLabelTimeIntervals.Add(new TimeInterval(-2));
        _xLabelTimeIntervals.Add(new TimeInterval(-3));
        _xLabelTimeIntervals.Add(new TimeInterval(-4));
      }
    }

    public static FontSpec GetZedGraphFontSize(Font font, Color fontColor) {
      int i2 = font.FontFamily.GetCellDescent(FontStyle.Regular);
      int i3 = font.FontFamily.GetEmHeight(FontStyle.Regular);
      float f1 = font.GetHeight() * (i3 - i2/3*2) / i3;
      FontSpec fs = new FontSpec(font.FontFamily.Name, f1,fontColor, font.Bold, font.Italic, font.Underline);
      fs.Border.IsVisible = false;
      return fs;
    }

    static void AddFirstXLabelToPane(GraphPane pane, string text, Font font, Color fontColor) {
      TextObj to = new TextObj(text, 0, 0);
      to.Location.CoordinateFrame = CoordType.PaneFraction;
      to.Location.AlignH = AlignH.Left;
      to.Location.AlignV = AlignV.Bottom;
      to.FontSpec = GetZedGraphFontSize(font, fontColor);
      to.FontSpec.IsBold = true;
      float f1 = to.FontSpec.GetHeight(1.0f);
      to.Location.Y = Convert.ToSingle(1.0f + f1 * 0.1f / pane.Rect.Height);
      to.ZOrder = ZOrder.G_BehindChartFill;
      to.Tag = "_";
      pane.GraphObjList.Add(to);
    }

    static void AddXLabelToPane(GraphPane pane, string text, int xLabelCoordinate, Font font, Color fontColor) {
      Scale xScale = pane.XAxis.Scale;
//      double r1 = ((xLabelCoordinate - 0.5) - xScale.Min) / (xScale.Max - xScale.Min) * pane.Chart.Rect.Width + pane.Chart.Rect.X;
      double r1 = (xLabelCoordinate - xScale.Min) / (xScale.Max - xScale.Min) * pane.Chart.Rect.Width + pane.Chart.Rect.X;
      float r2 = Convert.ToSingle(r1 / pane.Rect.Width);
      TextObj to = new TextObj(text, r2, 0);
      to.Location.CoordinateFrame = CoordType.PaneFraction;
      to.Location.AlignH = AlignH.Center;
      to.Location.AlignV = AlignV.Bottom;
      to.FontSpec = GetZedGraphFontSize(font, fontColor);
      float f1 = to.FontSpec.GetHeight(1.0f);
      to.Location.Y = Convert.ToSingle(1.0f + f1 * 0.1f / pane.Rect.Height);
      to.ZOrder = ZOrder.G_BehindChartFill;
      to.Tag = "_";
      pane.GraphObjList.Add(to);
      AddXLabelTick(pane, xLabelCoordinate);
    }

    static void AddXLabelTick(GraphPane pane, int tickX) {
      double yPixelsPerTick = pane.Chart.Rect.Height / (pane.Y2Axis.Scale.Max - pane.Y2Axis.Scale.Min);
      float tickHeight = Convert.ToSingle(3 / yPixelsPerTick);
      Scale xScale = pane.XAxis.Scale;
//      double _x = tickX - 0.5;
      double _x = tickX;
      double r1 = (_x - xScale.Min) / (xScale.Max - xScale.Min) * pane.Chart.Rect.Width + pane.Chart.Rect.X;
      float r2 = Convert.ToSingle(r1 / pane.Rect.Width);
      LineObj lo = new LineObj(_x, pane.Y2Axis.Scale.Min - tickHeight, _x, pane.Y2Axis.Scale.Min + tickHeight);
      lo.Location.CoordinateFrame = CoordType.AxisXY2Scale;
      lo.ZOrder = ZOrder.A_InFront;
      lo.Tag = "_";
      pane.GraphObjList.Add(lo);
    }

    public static void AddXScaleObjectsToPanes(PaneList panes, List<DateTime> dates, Common.TimeInterval ti,
      double graceLeft, double graceRight, Font xLabelFont, List<List<double>> lastYValues, float xMarginRight) {
      if (panes.Count == 0) return; // no panes
      // Clear panes;
      foreach (GraphPane pane1 in panes) {
        int ii = 0;
        while (ii < pane1.GraphObjList.Count) {
          GraphObj go = pane1.GraphObjList[ii];
          if (go is TextObj) {
          }
          if ((go is BoxObj || go is LineObj || go is TextObj) && go.Tag != null && go.Tag.ToString() == "_") {
            pane1.GraphObjList.RemoveAt(ii);
          }
          else ii++;
        }
      }

      GraphPane pane = panes[0];// MasterPane
      double xMin = pane.XAxis.Scale.Min;
      double xMax = pane.XAxis.Scale.Max;
      double chartWidth = Convert.ToDouble(pane.Chart.Rect.Width);

      double pixelsPerTick = chartWidth / (xMax - xMin);
      double pixelsPerDay = pixelsPerTick * 390 *60 / ti.GetSecondsInInterval();
      TimeInterval scaleTI = GetXScaleTimeInterval(pixelsPerTick, ti, xLabelFont);
      TimeInterval scaleTINextLevel = new TimeInterval(1); // blank
      bool flagTINextLevel = true;
      switch (scaleTI._timeInterval) {
        case -1: scaleTINextLevel = new TimeInterval(-2); break;
        case -2: scaleTINextLevel = new TimeInterval(-3); break;
        case -3: scaleTINextLevel = new TimeInterval(-4); break;
        case -4: flagTINextLevel = false; break;// blank
        default: scaleTINextLevel = new TimeInterval(-1); break;
      }

//      int iStart = Math.Max(0, Convert.ToInt32(Math.Ceiling(xMin + 0.5)));
      int iStart = Math.Max(0, Convert.ToInt32(Math.Ceiling(xMin)));
//      int iEnd = Convert.ToInt32(Math.Ceiling(xMax + 0.501));// Number of last divider line
      int iEnd = Convert.ToInt32(Math.Ceiling(xMax + 0.001));// Number of last divider line
      DateTime? lasDT = null;
      if (iStart > 0 && iStart <= dates.Count) lasDT = dates[iStart - 1]; // previous date

      float yPositionOfXScale = 1.0f + 2 / pane.Rect.Width;
      if (ti._timeInterval > 0 && iStart >= 0 && iStart < dates.Count && flagTINextLevel) {// Intraday left label
        String firstLabelText = dates[iStart].ToString(scaleTINextLevel.GetXScaleFormat());
        AddFirstXLabelToPane(pane, firstLabelText, xLabelFont, Color.Black);
      }

//      int iBoxLeft = Convert.ToInt32(Math.Ceiling(xMin + 0.5)) - 1;// Left edge of X
      int iBoxLeft = Convert.ToInt32(Math.Ceiling(xMin)) - 1;// Left edge of X
      Color lastColor = GetBackColor(lasDT);

      for (int i1 = iStart; i1 < iEnd && i1 < dates.Count; i1++) {
        DateTime thisDate = dates[i1];
        if (i1 == dates.Count - 1 && dates.Count > 5) {
          int dd = Convert.ToInt32((dates[dates.Count - 1].TimeOfDay.TotalSeconds));
          if (dd % 30 == 0) {
          }
        }
        bool b1 = (flagTINextLevel ? IsDatesInTheSameTimeInterval(lasDT, thisDate, scaleTINextLevel) : true);
        if (!b1) {
//          AddXLineToPanes(panes, i1 - 0.5f, 5f, 1f);
          AddXLineToPanes(panes, Convert.ToSingle( i1), 5f, 1f);
          string text = dates[i1].ToString(scaleTINextLevel.GetXScaleFormat());
          AddXLabelToPane(pane, text, i1, xLabelFont, Color.Black); //Add label & labeltick
        }
        else if (!IsDatesInTheSameTimeInterval(lasDT, thisDate, scaleTI)) {
          //          AddXLineToPanes(panes, i1 - 0.5f, 1f, 5f);
          AddXLineToPanes(panes, Convert.ToSingle(i1), 1f, 5f);
          string text = dates[i1].ToString(scaleTI.GetXScaleFormat());
          AddXLabelToPane(pane, text, i1, xLabelFont, Color.Black);// Add label & labeltick
        }

        if (ti._timeInterval > 0 && pixelsPerDay > 20) {// Intraday == draw boxes
          Color thisColor = GetBackColor(thisDate);
          if (lastColor != thisColor) {
            AddBoxToPanes(panes, iBoxLeft, i1, lastColor);
            iBoxLeft = i1;
            lastColor = thisColor;
          }
        }
        lasDT = thisDate;
      }
      if ((dates.Count - 0.5) < xMax) {
        //        AddXLineToPanes(panes, dates.Count - 0.5f, 5f, 1f);// not show the last line
      }

      if (ti._timeInterval > 0 && pixelsPerDay > 20) {// Intraday == draw boxes
        int i1 = Math.Min(iEnd, dates.Count);
        if (i1 > iBoxLeft) {
          AddBoxToPanes(panes, iBoxLeft, i1, lastColor);
          iBoxLeft = i1;
        }
        if (i1 == dates.Count) {
          AddBoxToPanes(panes, iBoxLeft, int.MaxValue, GetBackColor(null));
        }
      }

      if (lastYValues!=null) {// LastYValues labels
        for (int i1 = 0; i1 < lastYValues.Count; i1++) {
          for (int i2 = 0; i2 < lastYValues[i1].Count; i2++) {
            if (panes[i1].Y2Axis.Scale.Min < lastYValues[i1][i2] && panes[i1].Y2Axis.Scale.Max > lastYValues[i1][i2]) {
              double magFactor = Math.Pow(10, panes[i1].Y2Axis.Scale.Mag);
              string text = (lastYValues[i1][i2]/magFactor).ToString();
//              double delta = (panes[i1].Rect.Right - panes[i1].Chart.Rect.Right) / pixelsPerTick;
              double delta = xMarginRight / pixelsPerTick;
//              TextObj to = new TextObj(text, panes[i1].XAxis.Scale.Max + delta, lastYValues[i1][i2]);
              TextObj to = new TextObj(text, panes[i1].XAxis.Scale.Max, lastYValues[i1][i2]);
              to.Location.CoordinateFrame = CoordType.AxisXY2Scale;
              to.Location.AlignH = AlignH.Left;
              to.Location.AlignV = AlignV.Center;
              to.FontSpec = GetZedGraphFontSize(xLabelFont, Color.Black);
              to.FontSpec.Fill = new Fill(Color.Pink);
              float f1 = to.FontSpec.GetHeight(1.0f);
              to.ZOrder = ZOrder.C_BehindChartBorder;
              to.Tag = "_";
              panes[i1].GraphObjList.Add(to);
            }
          }
        }
      }
    }

    static void AddBoxToPanes(List<GraphPane> panes, int iStart, int iEnd, Color color) {

      if (iStart == iEnd && iStart > 0) return;

      foreach (GraphPane pane in panes) {
        double xStart = iStart == 0 ? pane.XAxis.Scale.Min : iStart;
//        double xStart = iStart == 0 ? pane.XAxis.Scale.Min : iStart - 0.5;
//        double xEnd = iEnd == int.MaxValue ? pane.XAxis.Scale.Max : iEnd - 0.5;
        double xEnd = iEnd == int.MaxValue ? pane.XAxis.Scale.Max : iEnd;
        if (xStart >= xEnd) return;
        BoxObj bgBox = new BoxObj(xStart, pane.Y2Axis.Scale.Max, xEnd - xStart, pane.Y2Axis.Scale.Max - pane.Y2Axis.Scale.Min,
          Color.Transparent, color);
        bgBox.ZOrder = ZOrder.F_BehindGrid;
        bgBox.IsClippedToChartRect = true;
        bgBox.Border.IsVisible = false;
        bgBox.Border.Width = 0;
        bgBox.Tag = "_";
        pane.GraphObjList.Add(bgBox);// backgroundDayBox (last Daya is blue ???)
      }
    }

    static Color GetBackColor(DateTime? dt) {
      if (dt == null) return Color.WhiteSmoke;
      int a = 192;
      TimeSpan ts = dt.Value.TimeOfDay;
      if (ts < General.marketStart || ts >= General.marketEnd) return Color.Gainsboro; // not market Time
      switch (dt.Value.DayOfWeek.GetHashCode()) {
        case 1: return Color.FromArgb(a, 255, 192, 192); // monday
        case 2: return Color.FromArgb(a, 255, 224, 192);
        case 3: return Color.FromArgb(a, 255, 255, 192); 
        case 4: return Color.FromArgb(a, 192, 255, 192); 
        case 5: return Color.FromArgb(a, 192, 255, 255); // friday
        default: return Color.WhiteSmoke; 
      }
    }

    static void AddXLineToPanes(List<GraphPane> panes, float x, float dashOn, float dashOff) {
      foreach (GraphPane pane in panes) {
        LineObj line = new LineObj(Color.Gray, x, pane.Y2Axis.Scale.Min, x, pane.Y2Axis.Scale.Max);
        line.IsClippedToChartRect = false;
        line.Line.DashOn = dashOn;
        line.Line.DashOff = dashOff;
        line.ZOrder = ZOrder.F_BehindGrid;
        line.Line.Style = System.Drawing.Drawing2D.DashStyle.Custom;
        line.Tag = "_";
//        line.Location.Width = 1;
        pane.GraphObjList.Add(line);
      }
    }

    
    public static TimeInterval GetXScaleTimeInterval(double pixelsPerTick, TimeInterval ti, Font xLabelFont) {
      double totalSeconds = Convert.ToDouble(ti.GetSecondsInInterval());
      // check this Time Interval
      double labelWidth = ti.GetXLabelWidth(xLabelFont);
      if (pixelsPerTick > (labelWidth + Comp.StockGraph._xLabelGap*2)) return ti;// one tick == one grid
      // Check standard time intervals
      for (int i = 0; i < _xLabelTimeIntervals.Count; i++) {
        labelWidth = _xLabelTimeIntervals[i].GetXLabelWidth(xLabelFont);
        double thisTotalSeconds = Convert.ToDouble(_xLabelTimeIntervals[i].GetSecondsInInterval());
        if (thisTotalSeconds >= totalSeconds) {
          double pixels = thisTotalSeconds / totalSeconds * pixelsPerTick;
          if (pixels > (labelWidth + Comp.StockGraph._xLabelGap)) {
            return _xLabelTimeIntervals[i];
          }
        }
      }
      return _xLabelTimeIntervals[_xLabelTimeIntervals.Count - 1];// return year TimeInterval
    }

    static bool IsDatesInTheSameTimeInterval(DateTime? previousDate, DateTime thisDate, TimeInterval ti) {
      if (previousDate == null) return false;
      switch (ti._timeInterval) {
        case -1: return previousDate.Value.Date == thisDate.Date;
        case -2: return GetWeekNo(previousDate.Value) == GetWeekNo(thisDate);
        case -3: return previousDate.Value.Month == thisDate.Date.Month && previousDate.Value.Year == thisDate.Year;
        case -4: return previousDate.Value.Year == thisDate.Year;
        default: // intraday
          if (previousDate.Value.Date != thisDate.Date) return false;// different days
//          int prevMins = Convert.ToInt32(previousDate.Value.TimeOfDay.TotalMinutes);
  //        int thisMins = Convert.ToInt32(thisDate.TimeOfDay.TotalMinutes);
          int prevMins = Convert.ToInt32(previousDate.Value.TimeOfDay.TotalSeconds);
          int thisMins = Convert.ToInt32(thisDate.TimeOfDay.TotalSeconds);
          int prevNo = prevMins / ti._timeInterval;
          int thisNo = thisMins / ti._timeInterval;
          return (prevNo == thisNo);
      }
    }




















    public static void Test() {
      List<DateTime> dates = new List<DateTime>();
      TimeInterval ti = new TimeInterval(7);
      AddDateToDateArray(dates, new DateTime(2010, 01, 04, 18, 54, 59), ti);
      AddDateToDateArray(dates, new DateTime(2010, 01, 05, 9, 42, 00), ti);
      AddDateToDateArray(dates, new DateTime(2010, 01, 05, 9, 47, 01), ti);
      AddDateToDateArray(dates, new DateTime(2010, 01, 05, 16, 00, 00), ti);
      AddDateToDateArray(dates, new DateTime(2010, 01, 05, 17, 30, 12), ti);
      AddDateToDateArray(dates, new DateTime(2010, 01, 07, 10, 30, 12), ti);
    }


    static DateTime gStartDate = new DateTime(1900, 1, 1);// Monday
    // SameTimeFrame: dates.count not change
    // NewTimeFrame: milisec in date == 0
    // NotMarketTime: milisec in date == 1 (only for Intraday)
    // NewDate: milisec in date == 2 or 3(for not market time) (only for Intraday)
    // NewTimeFrameCluster: milisec in date == 4(for not market time) (only for Intraday)

    static int GetWeekNo(DateTime dt) {
      return Convert.ToInt32((dt - gStartDate).TotalDays) / 7;
    }
    static DateTime GetFirstDateOfWeek(DateTime dt) {
      if (dt.DayOfWeek == DayOfWeek.Sunday) return dt.AddDays(-6).Date;// Sunday: return value == previous Monday
      else return dt.AddDays(-(dt.DayOfWeek.GetHashCode() - 1)).Date;// Sunday.GetHashCode==0
    }

    public static void AddDateToDateArray(List<DateTime> dates, DateTime newDate, TimeInterval timeInterval) {
      DateTime? lastDT = null;
      if (dates.Count > 0) {
        lastDT = dates[dates.Count - 1];
        if (lastDT > newDate) {
          throw new Exception("XScale.AddDateToDateArray procedure. New date can not be lees than the last date of array");
        }
      }

      switch (timeInterval._timeInterval) {
        case -1: // Day
          if (lastDT == null || lastDT.Value != newDate.Date) dates.Add(newDate.Date);
          return;
        case -2: // Week
          if (lastDT == null || GetWeekNo(lastDT.Value) != GetWeekNo(newDate)) dates.Add(GetFirstDateOfWeek(newDate.Date));
          return;
        case -3: // Month
          if (lastDT == null || (lastDT.Value.Month + lastDT.Value.Year * 100 != newDate.Month + newDate.Year * 100))
            dates.Add(newDate.Date.AddDays(-newDate.Date.Day + 1));
          return;
        case -4: // Year
          if (lastDT == null || (newDate.Year != lastDT.Value.Year)) dates.Add(newDate.Date.AddDays(-newDate.Date.DayOfYear + 1));
          return;
        // =========================== intraday =============================
        default:
          if (timeInterval._timeInterval <= 0) {
            throw new Exception("Invalid TimeInterval in XScale.AddDateToDateArray!");
          }

          TimeSpan startTS = General.marketStart;
          TimeSpan endTS = General.marketEnd;
          TimeSpan tsDelta = new TimeSpan(0, 0, timeInterval._timeInterval);

          bool flagNewDate = (lastDT == null);
          if (lastDT != null && lastDT.Value.Date != newDate.Date) {// missing previous date
            flagNewDate = true;
            TimeSpan ts = lastDT.Value.TimeOfDay + tsDelta;
            while (ts < endTS) {
              dates.Add(lastDT.Value.Date + ts);
              ts += tsDelta;
            }
          }
          if (flagNewDate) {
            if (newDate.TimeOfDay >= startTS && newDate.TimeOfDay <= endTS) {// market time
              DateTime tmp2 = newDate.Date + startTS;
              while (tmp2 <= newDate) {// fill blank dates
                dates.Add(tmp2);
                tmp2 += tsDelta;
              }
            }
            else if (newDate.TimeOfDay < startTS) {// Premarket
              TimeSpan x1 = startTS;
              while (newDate.TimeOfDay < x1) x1 = x1.Subtract(tsDelta);
              dates.Add(newDate.Date + x1);
            }
            else {// After market time == rounding date (no seconds)
              dates.Add(newDate.Date + RemoveSecondsFromTimeSpan(newDate.TimeOfDay));
            }
          }
          else {// the same date
            DateTime tmp3 = lastDT.Value;
            while (tmp3 < newDate) {
              DateTime nextDate = tmp3.AddSeconds(timeInterval._timeInterval);
              if (tmp3.TimeOfDay < startTS && nextDate.TimeOfDay > startTS) {
                nextDate = tmp3.Date + startTS;
              }
              if (tmp3.TimeOfDay < endTS && nextDate.TimeOfDay > endTS) {
                nextDate = tmp3.Date + endTS;
              }
              // if ((nextDate > newDate) || (nextDate == newDate && nextDate.TimeOfDay == endTS)) break;
              if (nextDate > newDate) break;
              dates.Add(nextDate);
              tmp3 = nextDate;
            }
          }//if (flagNewDate) {
          return; // default of switch
      }//switch (timeInterval._timeInterval) {
    }// procedure

    static TimeSpan RemoveSecondsFromTimeSpan(TimeSpan ts) {
      return new TimeSpan(ts.Hours, ts.Minutes, 0);
    }
  }
}
