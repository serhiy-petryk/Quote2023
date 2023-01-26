using System;
using System.Globalization;
using System.Collections.Generic;

namespace spMain.QData.DataFormat {
  public class MbtTick {

    public static Quote GetSummaryQuote(List<MbtTick> ticks) {
      double open = Double.NaN;
      double high = Double.NaN;
      double low = Double.NaN;
      double close = Double.NaN;
      long volume = 0;
      for (int i = 0; i < ticks.Count; i++) {
        MbtTick t = ticks[i];
        TimeSpan time = t._date.TimeOfDay;
        int status = (time < DataAdapters.MBT.C.tsSessionStart ? 0 : (time < DataAdapters.MBT.C.tsSessionEnd ? 1 : 2));
        if (status == 1) {
          if (double.IsNaN(open)) open = t._price;
          if (double.IsNaN(high) || high < t._price) high = t._price;
          if (double.IsNaN(low) || low > t._price) low = t._price;
          close = t._price;
          volume += t._volume;
        }
        //        volume += t._volume;
      }
      return new Quote(ticks.Count == 0 ? DateTime.MinValue : ticks[0]._date.Date, open, high, low, close, volume);
    }

    // ==================================   Class   ==================================
    public readonly int _no;
    public readonly DateTime _date;
    public readonly double _price;
    public readonly long _volume;
    public readonly int _condition;
    public readonly int _type;
    public readonly int _status;

    /*		public DateTime DateAndTime {
          get { return this._date; }
        }
        public double Price {
          get { return this._price; }
        }
        public long Volume {
          get { return this._volume; }
        }
        public int No {
          get { return this._no; }
        }*/
    public bool IsInInterval(bool isFirstQuote) {
      return this._condition == 0 || this._condition == 45 || (this._condition == 10 && isFirstQuote);
    }
    public bool IsEqual(MbtTick t) {
      return (this._date == t._date) && (this._price == t._price) && (this._volume == t._volume) &&
        (this._condition == t._condition) && (this._status == t._status) && (this._type == t._type);
    }

    public MbtTick(int no, DateTime date, double price, long volume, int condition, int status, int type) {
      this._no = no; this._date = date; this._price = Math.Round(price, 10); this._volume = volume;
      this._condition = condition; this._type = type; this._status = status;
    }

    public string ToTimeString(CultureInfo ci) {
      return this._date.ToString("HH:mm:ss") + "\t" + _price.ToString(ci.NumberFormat) + "\t" +
        this._volume.ToString() + "\t" + this._condition + "\t" + this._status + "\t" + this._type;
    }
    public string ToString(CultureInfo ci) {
      return this._date.ToString("yyyy-MM-dd HH:mm:ss") + "\t" + _price.ToString(ci.NumberFormat) + "\t" +
        this._volume.ToString() + "\t" + this._condition + "\t" + this._status + "\t" + this._type;
    }
    public override string ToString() {
      return this._date.ToString("yyyy-MM-dd HH:mm:ss") + "\t" + _price.ToString(DataAdapters.MBT.C.fiNumberInvariant) + "\t" +
        this._volume.ToString() + "\t" + this._condition + "\t" + this._status + "\t" + this._type;
    }
  }
}
