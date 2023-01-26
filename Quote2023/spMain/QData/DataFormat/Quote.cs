using System;
using System.Collections;
using System.Collections.Generic;

namespace spMain.QData.DataFormat {

  public interface IQuoteExtended: IQuote {
    string Events { get;set;}
  }
  public interface IQuote {
    DateTime Date { get;set;}
    Double Open { get;set;}
    Double High { get;set;}
    Double Low { get;set;}
    Double Close { get;set;}
    Double Volume { get;set;}
  }

  //==============================
  //==============================
  public class YahooQuote : Quote, IQuoteExtended {

    public double adjClose;
    public YahooQuote() : base() { }
    public YahooQuote(DateTime pDate, double pOpen, double pHigh, double pLow, double pClose, double pVolume)
      : base(pDate, pOpen, pHigh, pLow, pClose, pVolume) {
      adjClose = base.close;
    }
    public YahooQuote(DateTime pDate, double pOpen, double pHigh, double pLow, double pClose, double pVolume, QuoteType pType)
      : base(pDate, pOpen, pHigh, pLow, pClose, pVolume, pType ) {
      adjClose = base.close;
    }

    public double AdjClose {
      get { return adjClose; }
      set { adjClose = value; }
    }
    public string Events {
      get {
        if (adjClose == close) return null;
        else return (Math.Round( adjClose * 100,2)).ToString("R", csIni.fiNumberUS);
      }
      set {
        if (String.IsNullOrEmpty(value)) adjClose = close;
        else adjClose = Math.Round( double.Parse( value)/100,2);
      }
    }
    public override string ToString() {
      return date.ToString("yyyy-MM-dd") + "\t" + open.ToString("R", csIni.fiNumberUS) + "\t" +
        high.ToString("R", csIni.fiNumberUS) + "\t" + low.ToString("R", csIni.fiNumberUS) + "\t" +
        close.ToString("R", csIni.fiNumberUS) + "\t" + volume.ToString("R", csIni.fiNumberUS) + "\t" +
        adjClose.ToString("R", csIni.fiNumberUS) + "\t" +base.Type;
    }
  }

  //============================
  //============================
  public class Quote : IQuote {

    public static void SortList(IList data, System.Windows.Forms.SortOrder sortOrder) {
      if (data is List<Quote>) {// Quote List
        if (sortOrder == System.Windows.Forms.SortOrder.Ascending) {
          ((List<Quote>)data).Sort(delegate(Quote q1, Quote q2) { return q1.date.CompareTo(q2.date); });
        }
        else if (sortOrder == System.Windows.Forms.SortOrder.Descending) {
          ((List<Quote>)data).Sort(delegate(Quote q1, Quote q2) { return -q1.date.CompareTo(q2.date); });
        }
        else throw new Exception(sortOrder.ToString() + " is invalid sort order");
      }
      else if (data is List<YahooQuote>) {// YahooQuote List
        if (sortOrder == System.Windows.Forms.SortOrder.Ascending) {
          ((List<YahooQuote>)data).Sort(delegate(YahooQuote q1, YahooQuote q2) { return q1.date.CompareTo(q2.date); });
        }
        else if (sortOrder == System.Windows.Forms.SortOrder.Descending) {
          ((List<YahooQuote>)data).Sort(delegate(YahooQuote q1, YahooQuote q2) { return -q1.date.CompareTo(q2.date); });
        }
        else throw new Exception(sortOrder.ToString() + " is invalid sort order");
      }
      else throw new Exception(data.GetType().ToString() + " type is not supported by Quote.SortList method");
    }

    public static bool CompareTwoCollection(IList<Quote> data1, IList<Quote> data2, out int badRecordNo) {
      badRecordNo = -1;
      if (data1.Count != data2.Count) return false;
      for (int i = 0; i < data1.Count; i++) {
        if (!IsTwoQuotesEqual(data1[i], data2[i])) {
          badRecordNo = i;
          return false;
        }
      }
      return true;
    }

    public static bool IsTwoQuotesEqual(Quote q1, Quote q2) {
      return q1.date == q2.date && q1.open == q2.open && q1.high == q2.high &&
        q1.low == q2.low && q1.close == q2.close && q1.volume == q2.volume;
    }

    /*public static Quote GetSummaryQuote(List<Quote> quotes) {
      double open = Double.NaN;
      double high = Double.NaN;
      double low = Double.NaN;
      double close = Double.NaN;
      double volume = 0;
      for (int i = 0; i < quotes.Count; i++) {
        Quote q = quotes[i];
        TimeSpan time = q.date.TimeOfDay;
        int status = (time < DataAdapters.MBT.C.tsSessionStart ? 0 : (time < DataAdapters.MBT.C.tsSessionEnd ? 1 : 2));
        if (status == 1) {
          if (double.IsNaN(open)) open = q.open;
          if (double.IsNaN(high) || high < q.high) high = q.high;
          if (double.IsNaN(low) || low > q.low) low = q.low;
          close = q.close;
        }
        volume += q.volume;
      }
      return new Quote(quotes.Count == 0 ? DateTime.MinValue : quotes[0].date.Date, open, high, low, close, volume);
    }*/

    //======================================================
    //======================================================
    public enum ValueProperty { Open, High, Low, Close, Volume, VolumeBuy, VolumeSell, VolumeBuySell, TrueRange };

    public DateTime date;
    public double open, high, low, close, volume;

    public Quote() {
      this.date = DateTime.MinValue; this.open = Double.NaN; this.high = Double.NaN; this.low = Double.NaN; this.close = Double.NaN;
      this.volume = 0;
    }

    const int valueDP = 6;
    public Quote(DateTime pDate, double pOpen, double pHigh, double pLow, double pClose, double pVolume) {
      this.date = pDate; 
      if (pDate.Millisecond == 0) {// Quote
        this.open = Math.Round(pOpen, valueDP); this.high = Math.Round(pHigh, valueDP);
        this.low = Math.Round(pLow, valueDP); this.close = Math.Round(pClose, valueDP);
        this.volume = Math.Round(pVolume, valueDP);
      }
      else {// other objects
        this.open = pOpen; this.high = pHigh; this.low = pLow; this.close = pClose; this.volume = pVolume;
      }
    }
    public Quote(DateTime pDate, double pOpen, double pHigh, double pLow, double pClose, double pVolume, QuoteType pType) {
      this.date = pDate; this.open = pOpen; this.high = pHigh; this.low = pLow;
      this.close = pClose; this.volume = pVolume; this.Type = pType;
    }

    public TimeSpan Time { get { return this.date.TimeOfDay; } }
    public DateTime Date { get { return this.date; } set { date = value; } }
    public double Open { get { return this.open; } set { open = Math.Round(value, valueDP); } }
    public double High { get { return this.high; } set { high = Math.Round(value, valueDP); } }
    public double Low { get { return this.low; } set { low = Math.Round(value, valueDP); } }
    public double Close { get { return this.close; } set { close = Math.Round(value, valueDP); } }
    public double Volume { get { return this.volume; } set { volume = Math.Round(value, valueDP); } }

    public enum QuoteType { NewSet = 99, Dividend = 11, NotAdustedDividend=12, Split = 21, EndHtmlSet = 31, EndCsvSet = 32 };
    public enum QuoteError { NegativeOHLC, NegativeVolume, BadOHLC, BadVolume, ZeroPrice};

    public void XXSetQuoteType(string sType) {
      switch (sType.ToLower()) {
        case "newset": this.Type = QuoteType.NewSet; return;
        //        case "d"://msn  ??? not used (bad data)
        case "div":// ??? not used
        case "div1":// yahoo html
        case "div2": // yahoo csv
          this.Type = QuoteType.Dividend; return;
        //        case "s":// msn ??? not used (bad data)
        case "split": this.Type = QuoteType.Split; return;
        case "endset": this.Type = QuoteType.EndHtmlSet; return;
        case "html": this.Type = QuoteType.EndHtmlSet; return;// yahoo file type
        case "csv": this.Type = QuoteType.EndCsvSet; return;// yahoo file type
      }
      throw new Exception("Quote type does not defined for " + sType);
    }

    public QuoteType? Type {
      get {
        switch (this.date.Millisecond) {
          case 0: return null;
          default: return  (QuoteType)this.date.Millisecond;
        }
      }
      set {
        if (this.Type != null && value!=this.Type) throw new Exception("Quote already has type");
        this.date = this.date.AddMilliseconds((int)value);
      }
    }

    public double HL2 {
      get { return (this.high + this.low) / 2; }
    }
    public double HLC3 {
      get { return (this.high + this.low + this.close) / 3; }
    }
    public double OHLC4 {
      get { return (this.open + this.high + this.low + this.close) / 4; }
    }
    public double MiddleBodyValue {
      get { return (this.open + this.close) / 2; }
    }
    public double BodyHeight {
      get { return (Math.Abs(this.open - this.close)) / 2; }
    }
    public double VolumeBuy {
      get {
        if (this.high == this.low) return Math.Round(this.volume / 2, 0);
        return Math.Round(((this.high - this.open + this.close - this.low) / (this.high - this.low) / 2 * this.volume), 0);
      }
    }
    public double VolumeSell {
      get {
        if (this.high == this.low) return Math.Round(this.volume / 2, 0);
        return Math.Round(((this.open - this.low + this.high - this.close) / (this.high - this.low) / 2 * this.volume), 0);
      }
    }
    public double VolumeBuySell {
      get { return this.VolumeBuy - this.VolumeSell; }
    }

    public override string ToString() {
      return csUtils.StringFromDateTime(this.date) + '\t' + GetValueString();
    }
    string GetValueString() {
      return open.ToString(csIni.fiNumberUS) + '\t' + high.ToString(csIni.fiNumberUS) + '\t' + this.low.ToString(csIni.fiNumberUS) + '\t' +
        this.close.ToString(csIni.fiNumberUS) + '\t' + this.volume.ToString();
    }

    public static bool operator ==(Quote q1, Quote q2) {
      //      if (q1.symbol != q2.symbol) return false;
      if (Quote.ReferenceEquals(q1, null) && Quote.ReferenceEquals(q2, null)) return true;
      if (Quote.ReferenceEquals(q1, null) || Quote.ReferenceEquals(q2, null)) return false;
      return (q1.open == q2.open) && (q1.high == q2.high) && (q1.low == q2.low) && (q1.close == q2.close) &&
        (q1.volume == q2.volume) && (q1.date == q2.date);
    }
    public static bool operator !=(Quote q1, Quote q2) {
      return !(q1 == q2);
    }

    public void MergeQuotes(Quote otherQuote) {
      if (Double.IsNaN(this.open) && !Double.IsNaN(otherQuote.open)) this.open = otherQuote.open;
      if (!Double.IsNaN(otherQuote.high)) {
        if (double.IsNaN(this.high) || this.high < otherQuote.high) this.high = otherQuote.high;
      }
      if (!Double.IsNaN(otherQuote.low)) {
        if (double.IsNaN(this.low) || this.low > otherQuote.low) this.low = otherQuote.low;
      }
      if (!Double.IsNaN(otherQuote.close)) this.close = otherQuote.close;
      this.volume += otherQuote.volume;
    }

    public static void MergeQuotes(object[] thisQuote, object[] otherQuote) {
      // object[] members: 0-Date, 1-Open, 2-High, 3-Low, 4-Close, 5-Volume;
      if (Double.IsNaN((double)thisQuote[1]) && !Double.IsNaN((double)otherQuote[1])) thisQuote[1] = otherQuote[1]; // open
      if (!Double.IsNaN((double)otherQuote[2])) {// High
        if (double.IsNaN((double)thisQuote[2]) || (double)thisQuote[2] < (double)otherQuote[2]) thisQuote[2] = otherQuote[2];
      }
      if (!Double.IsNaN((double)otherQuote[3])) {// Low
        if (double.IsNaN((double)thisQuote[3]) || (double)thisQuote[3] > (double)otherQuote[3]) thisQuote[3] = otherQuote[3];
      }
      if (!Double.IsNaN((double)otherQuote[4])) thisQuote[4] = otherQuote[4];// Close
      thisQuote[5] = (double)thisQuote[5] + (double)otherQuote[5];// volume
    }

    public QuoteError? Error {
      get {
        if (Type == null) {
          if (open < 0 || high < 0 || low < 0 || close < 0 ) return QuoteError.NegativeOHLC;
          if (volume < 0) return QuoteError.NegativeVolume;
          if (open > high || open < low || close > high || close < low) return QuoteError.BadOHLC;
          if (volume == 0 && (open != high || open != low || close != open)) return QuoteError.BadVolume;
          if (volume != 0 && high==0 && low==0) return QuoteError.ZeroPrice;
        }
        return null;
      }
    }
/*    public string CheckQuote(bool isIndex) {
      if (Type == null) {
        if (open < 0 || high < 0 || low < 0 || close < 0 || volume < 0) return "NegativeValue";
        if (open > high || open < low || close > high || close < low) return "BadOHLC";
        if ((!isIndex) && volume == 0 && (open != high || open != low || close != open)) return "BadVolume";
      }
      return null;
    }*/

  }

}