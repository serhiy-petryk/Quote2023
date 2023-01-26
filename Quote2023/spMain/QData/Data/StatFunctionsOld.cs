using System;
using System.Collections;
using spMain.QData.DataFormat;

namespace spMain.QData.Data {
  class StatFunctionsOld {

    static Double[] dd = new double[] { 116.44, 117.04, 115.57, 117.06, 117.64, 117.63, 117.28, 
			117.26, 117.86, 117.93, 119.92, 119.6, 118.47, 117.38, 119.33, 118.7, 117.79, 119.29, 119.58, 
			118.57, 116.86, 117.63, 118.57, 118.95, 119.9, 119.32, 118.83, 119.47, 119.43, 118.22, 118.05, 116.69, 
			116.09, 116.33, 117.46, 117.16, 116.76, 117.67, 118.05, 118.88, 119.35, 121.82, 121.88, 122.11, 121.57, 121.61, 
			120.82, 120.94, 121.08, 119.33, 118.81, 119.61, 117.9, 119.02, 119.75, 121.35, 122.78, 122.29, 125.93, 127.04, 127.02, 
			128.35, 127.98, 121.64, 123.06, 122.82, 120.87, 122.69, 120.36, 120.11, 120.65, 121.5, 122.87, 120.61, 120.56, 121.16, 121.29, 123.1, 123.49 };

    public static void Test() {
      ArrayList source = new ArrayList(dd);
      ArrayList dest1 = new ArrayList();
      ArrayList dest2 = new ArrayList();
//      SmoothData(spMain.QData.Common.General.SmoothType.EMA, dest1, source, 9, 0, null);
  //    SmoothData(spMain.QData.Common.General.SmoothType.MATriangular, dest2, source, 9, 0, null);
//      SmoothData(spMain.QData.Common.General.SmoothType.EMA, dest2, source, 9, 0, null);
//      SmoothData(spMain.QData.Common.General.SmoothType.EMA2, dest2, source, 9, 0, null);
    }

    public static void SmoothData(Common.General.SmoothType smoothType, ArrayList destData, ArrayList sourceData,
      int period, int startItemNo, Func<object, double> delItemType) {

      double prevValue = GetLastPreviousValue(destData, startItemNo);
      for (int i = startItemNo; i < sourceData.Count; i++) {
        double x = double.NaN;
        double x1;
        switch (smoothType) {
          case spMain.QData.Common.General.SmoothType.Simple:
            x = Average(sourceData, period, i, delItemType);
            break;
          case spMain.QData.Common.General.SmoothType.MAWeighted:
            x = MAWeighted(sourceData, period, i, delItemType); 
            break;
          case spMain.QData.Common.General.SmoothType.EMA:
            x1 = delItemType(sourceData[i]);
            x = EMA(x1, prevValue, period);
            break;
          case spMain.QData.Common.General.SmoothType.EMA2:
            x1 = delItemType(sourceData[i]);
            x = EMA2(x1, prevValue, period, i);
            break;
          case spMain.QData.Common.General.SmoothType.EMAComplex:
                   x1 =delItemType(sourceData[i]);
                  x = EMAComplex(x1, prevValue, period, i);
            break;
          case spMain.QData.Common.General.SmoothType.MATriangular:
            x = AverageTriangular(sourceData, period, i,delItemType);
            //    x = AverageTriangular(sourceData, period, i, itemType);
            break;
        }
        if (i < destData.Count) destData[i] = x;
        else destData.Add(x);
        if (!double.IsNaN(x)) prevValue = x;
      }
    }

    public static void Min(ArrayList destData, ArrayList sourceData, int period, int startItemNo, Func<object, double> delItemType) {
      for (int i = startItemNo; i < sourceData.Count; i++) {
        double x = Min(sourceData, period, i,delItemType);
        if (i < destData.Count) destData[i] = x;
        else destData.Add(x);
      }
    }

    public static void Max(ArrayList destData, ArrayList sourceData, int period, int startItemNo, Func<object, double> delItemType) {
      for (int i = startItemNo; i < sourceData.Count; i++) {
        double x = Max(sourceData, period, i, delItemType);
        if (i < destData.Count) destData[i] = x;
        else destData.Add(x);
      }
    }

    public static void StDev(ArrayList destData, ArrayList sourceData, int period, int startItemNo,Func<object,double> delItemType) {
      for (int i = startItemNo; i < sourceData.Count; i++) {
        double x = StDev(sourceData, period, i, delItemType);
        if (i < destData.Count) destData[i] = x;
        else destData.Add(x);
      }
    }

    // ============================= Value stat functions ==============================
    public static double EMAComplex(double newValue, double prevValue, int period, int recs) {
      //  If n >= recNo Then
      //  aEmaComplex = pi / recNo + prevEma * (1 - 1 / recNo)
      //  Else
      //  aEmaComplex = pi * 2 / (n + 1) + prevEma * (1 - 2 / (n + 1))
      //  End If
      if (recs <= 1) return newValue;
      if (recs <= period) return newValue / recs + prevValue * (1 - 1 / (double)recs);
      return newValue * 2 / Convert.ToDouble(period + 1) + prevValue * (1 - 2 / Convert.ToDouble(period + 1));
    }

    public static double EMA2(double newValue, double prevValue, int period, int recs) {
      // Expotential Moving average ' Second method by formula: EMA= pi/n + prevEma*(1-1/n)
      //      if (recs <= 1) return newValue; // changed at 2010-05-06
      if (double.IsNaN(prevValue)) return newValue;
      double n = Convert.ToDouble( Math.Min(recs, period));
      return newValue / n + prevValue * (1 - 1 /n);
    }

    public static double EMA(double newValue, double prevValue, int period) {
      // Expotential Moving average
      // EMA = EMA(i-1) + 2/(n+1) * (pi - EMA(i-1))
      // Start point: == average (iQChart)
      //aEMA = prevEma + 2 / (n + 1) * (pi - prevEma)
      if (double.IsNaN(prevValue)) return newValue;
      return prevValue + 2 / Convert.ToDouble(period + 1) * (newValue - prevValue);
    }


// =============================  Array stat functions ==============================
    static double AverageTriangular(IList data, int period, int startNo, Func<object,double> delItemType) {
      // Average of Average
      int cnt = 0;
      double[] aver = new double[period];
      for (int i = startNo; i >= 0; i--) {
        double x = delItemType(data[i]);
        if (!double.IsNaN(x)) {
          int cnt1 = 1;
          double sum1 = x;
          for (int i1 = i - 1; i1 >= 0; i1--) {
            double x1 = delItemType(data[i1]);
            if (!double.IsNaN(x1)) {
              sum1 += x1;
              cnt1++;
              if (cnt1 == period) break;
            }
          }
          aver[cnt++] = sum1 / Convert.ToDouble(cnt1);
        }
        if (cnt == period) break;
      }
      if (cnt == 0) return double.NaN;
      else {
        double sum = 0;
        for (int i = 0; i < cnt; i++) sum += aver[i];
        return sum / Convert.ToDouble(cnt);
      }
    }

    static double MAWeighted(IList data, int period, int startNo,Func<object,double> delItemType) {
      double rez = 0;
      double rezK = 0;
      int cnt = 0;
      for (int i = startNo; (i >= 0) && (cnt < period); i--) {
        double x = delItemType(data[i]);
        if (!double.IsNaN(x)) {
          rez += x * Convert.ToDouble(period - cnt);
          rezK += Convert.ToDouble(period - cnt);
        }
        cnt++;
      }
      return (cnt == 0 ? double.NaN : rez / rezK);
    }

    static double Average(IList data, int period, int startNo, Func<object,double> delItemType) {
      double rez = 0;
      int dCnt = 0;
      int cnt = 0;
      for (int i = startNo; (i >= 0) && (cnt < period); i--) {
        double x = delItemType(data[i]);
        if (!double.IsNaN(x)) {
          rez += x;
          dCnt++;
        }
        cnt++;
      }
      return (dCnt == 0 ? double.NaN : rez / Convert.ToDouble(dCnt));
    }

    static double StDev(IList data, int period, int startNo, Func<object, double> delItemType) {
      double rez = 0;
      double[] dd = new double[period];
      int dCnt = 0;
      int cnt = 0;
      for (int i = startNo; (i >= 0) && (cnt < period); i--) {
        double x = delItemType(data[i]);
        if (!double.IsNaN(x)) {
          rez += x;
          dd[dCnt++] = x;
        }
        cnt++;
      }
      if (dCnt > 0) {
        double aver = rez / dCnt;
        double sum = 0;
        for (int i = 0; i < dCnt; i++) {
          double delta = (aver - dd[i]);
          sum += delta * delta;
        }
        return Math.Sqrt(sum / dCnt);
      }
      if (dCnt == period) {
        double aver = rez / period;
        double sum = 0;
        for (int i = 0; i < dd.Length; i++) {
          double delta = (aver - dd[i]);
          sum += delta * delta;
        }
        return Math.Sqrt(sum / period);
      }
      else return double.NaN;
    }

    static double Min(IList data, int period, int startNo, Func<object,double> delItemType) {
      double rez = double.NaN;
      int cnt = 0;
      for (int i = startNo; (i >= 0) && (cnt < period); i--) {
        double x =delItemType(data[i]);
        if (double.IsNaN(rez) || (!double.IsNaN(x) && (x < rez))) rez = x;
        cnt++;
      }
      return rez;
    }

    static double Max(IList data, int period, int startNo, Func<object,double> delItemType) {
      double rez = double.NaN;
      int cnt = 0;
      for (int i = startNo; (i >= 0) && (cnt < period); i--) {
        double x = delItemType(data[i]);
        if (double.IsNaN(rez) || (!double.IsNaN(x) && (x > rez))) rez = x;
        cnt++;
      }
      return rez;
    }


//========================   Utils ==============================
    static double GetLastPreviousValue(ArrayList data, int startItemNo) {
      for (int i = (startItemNo - 1); i >= 0; i--) {
        double x = (double)data[i];
        if (!double.IsNaN(x)) return x;
      }
      return double.NaN;
    }

    public static Func<object, double> GetValueDelegate(string itemType) {
      if (String.IsNullOrEmpty(itemType)) return delegate(object o) { return (double)o; };
      switch (itemType.ToLower()) {
        case "open": return delegate(object o) { return ((Quote)o).open; };
        case "high": return delegate(object o) { return ((Quote)o).high; };
        case "low": return delegate(object o) { return ((Quote)o).low; };
        case "close": return delegate(object o) { return ((Quote)o).close; };
        case "volume": return delegate(object o) { return ((Quote)o).volume; };
        case "volumebuy": return delegate(object o) { return ((Quote)o).VolumeBuy; };
        case "volumesell": return delegate(object o) { return ((Quote)o).VolumeSell; };
        case "volumebuysell": return delegate(object o) { return ((Quote)o).VolumeBuySell; };
        case "hl2": return delegate(object o) { return ((Quote)o).HL2; };
        case "hlc3": return delegate(object o) { return ((Quote)o).HLC3; };
        case "ohlc4": return delegate(object o) { return ((Quote)o).OHLC4; };
        case "middlebodyvalue": return delegate(object o) { return ((Quote)o).MiddleBodyValue; };
        case "bodyheight": return delegate(object o) { return ((Quote)o).BodyHeight; };
//        default: return delegate(object o) { return (double)o; };
      }
      throw new Exception("Delegate for '" + itemType + "' item type does not defined");
    }

  }

}
