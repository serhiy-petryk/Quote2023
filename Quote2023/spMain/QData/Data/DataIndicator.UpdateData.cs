using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using spMain.QData.DataFormat;

namespace spMain.QData.Data {
  public partial class DataIndicator {

    int _startItemNo;
    int _endItemNo;
    public double _dataFactor = double.NaN; // factor which is used for Compare

    void AddInputsToTempVars(string[] inputIDs) {
      this._tempVars.Clear();
      foreach (string s in inputIDs) {
        DataInput di = DataInput.GetDataInputByID(s, this._localInputs);
        if (di._dataType == typeof(Quote.ValueProperty)) {
          this._tempVars.Add(StatFunctionsOld.GetValueDelegate(di._value.ToString()));
        }
        else {
          this._tempVars.Add(di._value);
        }
      }
    }

    void ClearToOffset() {// Remove items from this._data based on baseIndicator
      this._startItemNo = -1;
      this._endItemNo = -1;
      if (this._childInds.Count > 0) {
        int lastNo = this._childInds[0]._lastUpdateOffset;
        this._data.RemoveRange(lastNo, this._data.Count - lastNo);
        this._startItemNo = this._childInds[0]._lastUpdateOffset;
        this._endItemNo = this._childInds[0]._data.Count;
        while (this._data.Count < this._endItemNo) {// Add blank data array elements
          object o = (this._valueDataType == typeof(double)? double.NaN :
            this._valueDataType == typeof(float)?  float.NaN :
            Activator.CreateInstance(this._valueDataType));
          this._data.Add(o);
        }
      }
    }

    // Случай Compare и т.п, когда используется несколько источников данных:
    // Шкала времени может быть разная. В этом случае индикатор строится так:
    // - первый индикатор - главный; он определяет шкалу времени
    // - остальные индикаторы нуждаются в согласовании шкалы времени, то есть
    // нужна специальная процедура, которая убирает или добавляет необходимые данные для вторичных индикаторов.

    public int _lastUpdateOffset = 0;

    public void UpdateData(int timerID) {
      if (timerID > this._lastTimerID) {
        this._lastTimerID = timerID;
        this._lastUpdateOffset = Math.Max(0, this._data.Count - 1);

        foreach (DataIndicator ind in this._childInds) {
          ind.UpdateData(timerID);
        }

        this.ClearToOffset();
        // Update quote

        if (this._indID.StartsWith("ti_"))
        {
          Upd_TradeIndicators();
        }
        else
        {
          switch (this._indID)
          {
            case "dquote":
              Upd_QuoteDifferential();
              break; // 
//          case "value_changes": Upd_ValueChanges(); break; // 
            //        case "value_abschanges": this.Upd_ValueAbsChanges(); break; // 
            case "dj+":
              Upd_DJPlus();
              break; // DJ+ (ToS checked)
            case "dj-":
              Upd_DJMinus();
              break; // DJ- (ToS checked)
            case "adx":
              Upd_ADX();
              break; // ADX (ToS checked)
            case "roc":
              Upd_RateOfChange();
              break; // Rate of Change  (ToS checked)
            case "rsi":
              Upd_RSI();
              break; // RSI (old version)
            case "rsiwilder":
              Upd_RSIWilder();
              break; // RSI Wilder (ToT)
            case "rsi_ema":
              Upd_RSI_EMA();
              break; // RSI with EMA (toT)
            case "macd":
              Upd_MACD();
              break; //(ToS checked)
            case "macdsignal":
              Upd_MACDSignal();
              break; //(ToS checked)
            case "macdhistogram":
              Upd_MACDHistogram();
              break; //(ToS checked)
            case "maenvdown":
              this.Upd_MAEnvelope(-1.0);
              break;
            case "maenvup":
              this.Upd_MAEnvelope(1.0);
              break; // 
            case "momentum":
              this.Upd_Momentum();
              break; // ToS checked = MomentumPercent
//          case "moneyflow": this.Upd_MoneyFlow(); break; // Money Flow
            case "onbalvol":
              this.Upd_OnBalVolume();
              break; // On Balance Volume (ToS checked)
            case "singleline":
              this.Upd_SingleLine();
              break;
            case "%r":
              this.Upd_R();
              break; // Williams, R% (ToS checked)
            case "ma":
              this.Upd_MA();
              break; // moving average of quote (ToS checked)
            case "stdev":
              this.Upd_StDev();
              break; // Standart deviation of quote (ToS checked)
            case "bb":
              this.Upd_BB();
              break; // Middle line of Bollinger bands (ToS checked)
            case "bbu":
              this.Upd_BB_Bands(1);
              break; // Bollinger upper band (ToS checked)
            case "bbl":
              this.Upd_BB_Bands(-1);
              break; // Bollinger lower band (ToS checked)
            case "%d":
              this.Upd_D();
              break; // stochastic (ToS checked)
            case "%k":
              this.Upd_K();
              break; // stochastic (ToS checked)
            case "%kfast":
              this.Upd_KFast();
              break; // stochastic (ToS checked)
            case "framedquote":
              this.Upd_FramedQuote();
              break;
            case "quoteother":
              this.Upd_QuoteOther();
              break;
            case "compare":
              this.Upd_Compare();
              break;
            case "max":
              this.Upd_Max();
              break;
            case "min":
              this.Upd_Min();
              break;
            case "updowndoji":
              this.Upd_UpDownDoji();
              break;
            case "volbuy_ma":
              this.Upd_VolBuySell_MA("volbuy");
              break;
            case "volsell_ma":
              this.Upd_VolBuySell_MA("volsell");
              break;
//          case "volbuy_ma":
            //        case "volsell_ma": this.Upd_VolBuySell_MA(); break;
            case "volsell":
              this.Upd_VolSell();
              break;
            case "volbuy":
              this.Upd_VolBuy();
              break;
            case "volume":
              this.Upd_Volume();
              break;
            case "trades":
              Upd_Trades();
              break;
            case "tradeindicators":
              Upd_TradeIndicators();
              break;
            case "singlecolor": break;
            default:
//            MessageBox.Show("Update procedure does not define for " + this._indID + " indicator");
              break;
          }
        }

      }
    }

/*    void Upd_ValueChanges() {
      DataIndicator indQuote = this._childInds[0];
      if (this._tempVars.Count == 0) {
        AddInputsToTempVars(new string[] { "value" });
      }
      Func<object, double> delValueType = (Func<object, double>)_tempVars[0];
      for (int i1 = this._startItemNo; i1 < this._endItemNo; i1++) {
        if (i1 == 0) this._data[i1] = double.NaN;
        else {
          this._data[i1] = delValueType(indQuote._data[i1]) - delValueType(indQuote._data[i1 - 1]);
        }
      }
    }
    void Upd_ValueAbsChanges() {
      DataIndicator indQuote = this._childInds[0];
      if (this._tempVars.Count == 0) {
        AddInputsToTempVars(new string[] { "value" });
      }
      Func<object, double> delValueType = (Func<object, double>)_tempVars[0];
      for (int i1 = this._startItemNo; i1 < this._endItemNo; i1++) {
        if (i1 == 0) this._data[i1] = double.NaN;
        else {
          this._data[i1] = Math.Abs(delValueType(indQuote._data[i1]) - delValueType(indQuote._data[i1 - 1]));
        }
      }
    }*/

    void Upd_QuoteDifferential() {
      ArrayList quotes = this._childInds[0]._data;
      if (this._tempVars.Count == 0) this._tempVars.Add(double.NaN); // last Close Value

      for (int i1 = this._startItemNo; i1 < this._endItemNo; i1++) {
        double lastClose = (double)this._tempVars[0];
        Quote q = (Quote)quotes[i1];
        if (double.IsNaN(lastClose)) lastClose = q.open;
        this._data[i1] = new Quote(q.date, q.open - lastClose, q.high - lastClose, q.low - lastClose, q.close - lastClose, q.volume);
        if (!double.IsNaN(q.close)) this._tempVars[0] = q.close;// save the last close value
      }
    }

    void Upd_ADX() {
      DataIndicator indMinus = this._childInds[0];
      DataIndicator indPlus = this._childInds[1];
      if (this._tempVars.Count == 0) {
        AddInputsToTempVars(new string[] { "period" });
      }
      int period = (int)this._tempVars[0];
      int recs = 0;
      double ema = double.NaN;
      for (int i = 0; i < this._endItemNo; i++) {
        if (i > 0) {
          double plus = (double)indPlus._data[i];
          double minus = (double)indMinus._data[i];
          double x = (plus + minus == 0 ? 0 : 100 * Math.Abs( (plus - minus)) / (plus + minus));
          recs++;
          ema = StatFunctionsOld.EMA2(x, ema, period, recs);
        }
        if (i >= this._startItemNo) this._data[i] = ema;
      }
    }

    void Upd_DJPlus() {
      DataIndicator indQuote = this._childInds[0];
      if (this._tempVars.Count == 0) {
        AddInputsToTempVars(new string[] { "period" });
      }
      int period = (int)this._tempVars[0];
      int recs = 0;
      double emaTR = double.NaN;
      double emaPlus = double.NaN;
      for (int i = 0; i < this._endItemNo; i++) {
        if (i > 0) {
          Quote thisQ = (Quote)indQuote._data[i];
          Quote prevQ = (Quote)indQuote._data[i - 1];
          double trueRange = Math.Max(prevQ.close, thisQ.high) - Math.Min(prevQ.close, thisQ.low);
          double high = thisQ.high - prevQ.high;
          double low = prevQ.low - thisQ.low;
          double plusDM = (high > low && high > 0 ? high : 0);
          double minusDM = (low > high && low > 0 ? low : 0);
          recs++;
          emaTR = StatFunctionsOld.EMA2(trueRange, emaTR, period, recs);
          emaPlus = StatFunctionsOld.EMA2(plusDM, emaPlus, period, recs);
        }
        if (i >= this._startItemNo) {
          if (emaTR != 0 && !double.IsNaN(emaTR)) this._data[i] = 100 * emaPlus / emaTR;
          else this._data[i] = double.NaN;
        }
      }
    }

    void Upd_DJMinus() {
      DataIndicator indQuote = this._childInds[0];
      if (this._tempVars.Count == 0) {
        AddInputsToTempVars(new string[] { "period" });
      }
      int period = (int)this._tempVars[0];
      int recs = 0;
      double emaTR = double.NaN;
      double emaMinus = double.NaN;
      for (int i = 0; i < this._endItemNo; i++) {
        if (i > 0) {
          Quote thisQ = (Quote)indQuote._data[i];
          Quote prevQ = (Quote)indQuote._data[i - 1];
          double trueRange = Math.Max(prevQ.close, thisQ.high) - Math.Min(prevQ.close, thisQ.low);
          double high = thisQ.high - prevQ.high;
          double low = prevQ.low - thisQ.low;
          double plusDM = (high > low && high > 0 ? high : 0);
          double minusDM = (low > high && low > 0 ? low : 0);
          recs++;
          emaTR = StatFunctionsOld.EMA2(trueRange, emaTR, period, recs);
          emaMinus = StatFunctionsOld.EMA2(minusDM, emaMinus, period, recs);
        }
        if (i >= this._startItemNo) {
          if (emaTR != 0 && !double.IsNaN(emaTR)) this._data[i] = 100 * emaMinus / emaTR;
          else this._data[i] = double.NaN;
        }
      }
    }

    void Upd_RateOfChange() {
      DataIndicator indQuote = this._childInds[0];
      if (this._tempVars.Count == 0) {
        AddInputsToTempVars(new string[] { "period" });
      }
      int period = (int)this._tempVars[0];
      for (int i1 = this._startItemNo; i1 < this._endItemNo; i1++) {
        if ((i1 - period) >= 0) {
          double prevClose = ((Quote)indQuote._data[i1 - period]).close;
          double thisClose = ((Quote)indQuote._data[i1]).close;
          this._data[i1] = (thisClose / prevClose - 1.0) * 100;
          continue;
        }
        this._data[i1] = double.NaN;
      }
    }

    void Upd_RSI() {
      DataIndicator indQuote = this._childInds[0];
      if (this._tempVars.Count == 0) {
        AddInputsToTempVars(new string[] { "value", "smoothperiod" });
        //        this._tempVars.Add(StatFunctionsOld.GetValueDelegate(this._tempVars[0].ToString()));
      }
      Func<object, double> delValueType = (Func<object, double>)_tempVars[0];
      int period = (int)this._tempVars[1];
      for (int i1 = this._startItemNo; i1 < this._endItemNo; i1++) {
        if ((i1 - period) > 0) {
          double x1 = 0; double x2 = 0;
          for (int i2 = 0; i2 < period; i2++) {
            double prevClose = ((Quote)indQuote._data[i1 - i2 - 1]).close;
            double thisClose = ((Quote)indQuote._data[i1 - i2]).close;
            if (thisClose > prevClose) x1 += (thisClose - prevClose);
            else if (thisClose < prevClose) x2 += (prevClose - thisClose);
          }
          if ((x1 + x2) == 0) this._data[i1] = 50.0;
          else {
            this._data[i1] = 100.0 - 100.0 * x2 / (x1 + x2);
          }
        }
        else this._data[i1] = double.NaN;
      }
    }

    void Upd_RSIWilder() { // ThinkOrSwim
      DataIndicator indQuote = this._childInds[0];
      if (this._tempVars.Count == 0) {
        AddInputsToTempVars(new string[] { "value", "smoothperiod" });
      }
      Func<object, double> delValueType = (Func<object, double>)_tempVars[0];
      int period = (int)this._tempVars[1];
      double ema = double.NaN;
      double emaAbs = double.NaN;
      int recs = 0;
      for (int i = 0; i < this._endItemNo; i++) {
        if (i > 0) {
          double diff = delValueType(indQuote._data[i]) - delValueType(indQuote._data[i - 1]);
          if (!double.IsNaN(diff)) {
            recs++;
            ema = StatFunctionsOld.EMA2(diff, ema, period, recs);
            emaAbs = StatFunctionsOld.EMA2(Math.Abs(diff), emaAbs, period, recs);
          }
        }
        if (i >= this._startItemNo) {// need to save value
          if (double.IsNaN(ema) || double.IsNaN(emaAbs)) this._data[i] = double.NaN;
          else {
            double k = emaAbs == 0 ? 0 : ema / emaAbs;
            this._data[i] = 50 * (k + 1);
          }
        }
      }
    }

    void Upd_RSI_EMA() { // ThinkOrSwim
      DataIndicator indQuote = this._childInds[0];
      if (this._tempVars.Count == 0) {
        AddInputsToTempVars(new string[] { "value", "smoothperiod" });
      }
      Func<object, double> delValueType = (Func<object, double>)_tempVars[0];
      int period = (int)this._tempVars[1];
      double ema = double.NaN;
      double emaAbs = double.NaN;
      int recs = 0;
      for (int i = 0; i < this._endItemNo; i++) {
        if (i > 0) {
          double diff = delValueType(indQuote._data[i]) - delValueType(indQuote._data[i - 1]);
          if (!double.IsNaN(diff)) {
            recs++;
            ema = StatFunctionsOld.EMAComplex(diff, ema, period, recs);
            emaAbs = StatFunctionsOld.EMAComplex(Math.Abs(diff), emaAbs, period, recs);
          }
        }
        if (i >= this._startItemNo) {// need to save value
          if (double.IsNaN(ema) || double.IsNaN(emaAbs)) this._data[i] = double.NaN;
          else {
            double k = emaAbs == 0 ? 0 : ema / emaAbs;
            this._data[i] = 50 * (k + 1);
          }
        }
      }
    }

    void Upd_RSIWilder1() {
/*      DataIndicator indChanges = this._childInds[0];
      DataIndicator indAbsChanges = this._childInds[0];
      if (this._tempVars.Count == 0) {
        AddInputsToTempVars(new string[] { "value", "smoothperiod" });
        //        this._tempVars.Add(StatFunctionsOld.GetValueDelegate(this._tempVars[0].ToString()));
      }
      Func<object, double> delValueType = (Func<object, double>)_tempVars[0];
      int period = (int)this._tempVars[1];
      for (int i1 = this._startItemNo; i1 < this._endItemNo; i1++) {
        if ((i1 - period) > 0) {
          double x1 = 0; double x2 = 0;
          for (int i2 = 0; i2 < period; i2++) {
            double prevClose = ((Quote)indQuote._data[i1 - i2 - 1]).close;
            double thisClose = ((Quote)indQuote._data[i1 - i2]).close;
            if (thisClose > prevClose) x1 += (thisClose - prevClose);
            else if (thisClose < prevClose) x2 += (prevClose - thisClose);
          }
          if ((x1 + x2) == 0) this._data[i1] = 50.0;
          else {
            this._data[i1] = 100.0 - 100.0 * x2 / (x1 + x2);
          }
        }
        else this._data[i1] = double.NaN;
      }*/
    }

    void Upd_MAEnvelope(double sign) {
      DataIndicator maInd = this._childInds[0];
      if (this._tempVars.Count == 0) {
        AddInputsToTempVars(new string[] { "percent" });
      }
      double percent = (double)this._tempVars[0];
      for (int i1 = this._startItemNo; i1 < this._endItemNo; i1++) {
        double x = (double)maInd._data[i1];
        this._data[i1] = x * (1.0 + sign * percent / 100);
      }
    }

    void Upd_Momentum() {
      DataIndicator indQuote = this._childInds[0];
      if (this._tempVars.Count == 0) {
        AddInputsToTempVars(new string[] { "period" });
      }
      int period = (int)this._tempVars[0];
      for (int i1 = this._startItemNo; i1 < this._endItemNo; i1++) {
        if ((i1 - period) >= 0) {
          double prevClose = ((Quote)indQuote._data[i1 - period]).close;
          double thisClose = ((Quote)indQuote._data[i1]).close;
          this._data[i1] = thisClose / prevClose * 100;
          continue;
        }
        this._data[i1] = double.NaN;
      }
    }

/*    void Upd_MoneyFlow() {
      DataIndicator indQuote = this._childInds[0];
      if (this._tempVars.Count == 0) this._tempVars.Add((double)0.0);
      for (int i1 = this._startItemNo; i1 < this._endItemNo; i1++) {
        if (i1 > 0) {
          double prevTP = ((Quote)indQuote._data[i1 - 1]).TrueRange;
          double thisTP = ((Quote)indQuote._data[i1]).TrueRange;
          if (!double.IsNaN(prevTP) && !double.IsNaN(thisTP)) {
            double x = (double)this._tempVars[0];
            if (thisTP > prevTP) x += ((Quote)indQuote._data[i1]).volume;
            else if (thisTP < prevTP) x -= ((Quote)indQuote._data[i1]).volume;
            this._data[i1] = x;
            this._tempVars[0] = x;
            continue;
          }
        }
        this._data[i1] = double.NaN;
      }
    }*/

    void Upd_OnBalVolume() {
      DataIndicator indQuote = this._childInds[0];
      if (this._tempVars.Count == 0) this._tempVars.Add((double)0.0);
      for (int i1 = this._startItemNo; i1 < this._endItemNo; i1++) {
        if (i1 > 0) {
          double prevClose = ((Quote)indQuote._data[i1 - 1]).close;
          double thisClose = ((Quote)indQuote._data[i1]).close;
          if (!double.IsNaN(prevClose) && !double.IsNaN(thisClose)) {
            double x = (double)this._tempVars[0];
            if (thisClose > prevClose) x += ((Quote)indQuote._data[i1]).volume;
            else if (thisClose < prevClose) x -= ((Quote)indQuote._data[i1]).volume;
            this._data[i1] = x;
            this._tempVars[0] = x;
            continue;
          }
        }
        this._data[i1] = double.NaN;
      }
    }

    void Upd_QuoteOther() {
      int i2Cnt = this._tempVars.Count == 0 ? 0 : (int)this._tempVars[0];
      for (int i1 = this._startItemNo; i1 < this._endItemNo; i1++) {
        DateTime date1 = this._childInds[0]._dates[i1];
        for (int i2 = i2Cnt; i2 < this._childInds[1]._dates.Count; i2++) {
          if (this._childInds[1]._dates[i2] == date1) {
            this._data[i1] = this._childInds[1]._data[i2];
            i2Cnt = i2 + 1;
            break;
          }
          else if (this._childInds[1]._dates[i2] > date1) {
            this._data[i1] = double.NaN;
            i2Cnt = i2;
            break;
          }
        }
      }

      // Save the i2cnt
      if (this._tempVars.Count == 0) this._tempVars.Add(i2Cnt-1);
      else this._tempVars[0] = i2Cnt-1;
    }

    void Upd_SingleLine() {
      if (this._tempVars.Count == 0) {
        AddInputsToTempVars(new string[] { "value"});
      }
      double value = (double)this._tempVars[0];
      for (int i = this._startItemNo; i < this._endItemNo; i++) {
        this._data[i] = value;
      }
    }

    void Upd_R() {
      DataIndicator indMax = this._childInds[0];
      DataIndicator indMin = this._childInds[1];
      DataIndicator indQuote = this._childInds[2];
      for (int i = this._startItemNo; i < this._endItemNo; i++) {
        double max = (double)indMax._data[i];
        double min = (double)indMin._data[i];
        this._data[i] = -(max == min ? -50.0 : (max- ((Quote)indQuote._data[i]).close) / (max - min) * 100);
      }
    }

    /*
         double iR(Indicator userInd) {
          if (this.quoteCount >= userInd.k1) {
            if (userInd.refInd1 == null) {
              userInd.refInd1 = Utils.GetUniqueIndicatorID("MAX", userInd.k1, 0, 0);
              userInd.refInd2 = Utils.GetUniqueIndicatorID("MIN", userInd.k1, 0, 0);
            }
            double high = this.inds[userInd.refInd1].data[0];
            double low = this.inds[userInd.refInd2].data[0];
    //				double high = this.GetIndicatorById("MAX", userInd.k1, 0, 0).data[0];
      //			double low = this.GetIndicatorById("MIN", userInd.k1, 0, 0).data[0];
            if (high == low) return -50.0;
            else return - (high - this.lastQuote.close) / (high - low) * 100;
          }
          else return double.NaN;
        }*/

    void Upd_MACD() {
      DataIndicator indMA12 = this._childInds[0];
      DataIndicator indMA26 = this._childInds[1];
      for (int i = this._startItemNo; i < this._endItemNo; i++) {
        this._data[i] = (double)indMA12._data[i] - (double)indMA26._data[i];
      }
    }

    void Upd_MACDSignal() {
      DataIndicator indMACD = this._childInds[0];
      if (this._tempVars.Count == 0) {
        AddInputsToTempVars(new string[] { "macdperiod", "smoothtype" });
        this._tempVars.Add(StatFunctionsOld.GetValueDelegate(null));
      }
      int macdPeriod = (int)this._tempVars[0];
      Common.General.SmoothType smoothType = (Common.General.SmoothType)this._tempVars[1];
      Func<object, double> delItemType = (Func<object, double>)this._tempVars[2];

      StatFunctionsOld.SmoothData(smoothType, this._data, indMACD._data, macdPeriod, this._startItemNo, delItemType);
    }

    void Upd_MACDHistogram() {
      DataIndicator indMACD = this._childInds[0];
      DataIndicator indMACD_Signal = this._childInds[1];
      for (int i = this._startItemNo; i < this._endItemNo; i++) {
        this._data[i] = (double)indMACD._data[i] - (double)indMACD_Signal._data[i];
      }
    }

    void Upd_MA() {// moving average of quote
      DataIndicator indQuote = this._childInds[0];
      if (this._tempVars.Count == 0) {
        AddInputsToTempVars(new string[] { "value", "smoothperiod", "smoothtype" });
      }
      Func<object, double> delValueType = (Func<object, double>)this._tempVars[0];
      int smoothPeriod = (int)this._tempVars[1];
      Common.General.SmoothType smoothType = (Common.General.SmoothType)this._tempVars[2];

      StatFunctionsOld.SmoothData(smoothType, this._data, indQuote._data, smoothPeriod, this._startItemNo, delValueType);
    }

    void Upd_StDev() {// Standart deviation of quote
      DataIndicator indQuote = this._childInds[0];
      if (this._tempVars.Count == 0) {
        AddInputsToTempVars(new string[] { "value", "smoothperiod" });
      }
      //      Quote.ValueProperty quoteValueType = (Quote.ValueProperty)this._tempVars[0];
      //      Quote.ValueProperty
      Func<object, double> delItemType = (Func<object, double>)this._tempVars[0];
      int smoothPeriod = (int)this._tempVars[1];
      StatFunctionsOld.StDev(this._data, indQuote._data, smoothPeriod, this._startItemNo, delItemType);
    }

    void Upd_BB() {// Middle line of Bollinger bands
      DataIndicator indMA = this._childInds[0];
      this._data = indMA._data;
    }

    void Upd_BB_Bands(double sign) {// Bollinger upper(sign=1)/lower(sign=-1) band 
      if (this._tempVars.Count == 0) {
        AddInputsToTempVars(new string[] { "deviationvalue" });
      }
      double deviationValue = (double)this._tempVars[0];
      DataIndicator indMA = this._childInds[0];
      DataIndicator indStDev = this._childInds[1];
      for (int i = this._startItemNo; i < this._endItemNo; i++) {
        this._data[i] = (double)indMA._data[i] + sign* deviationValue * (double)indStDev._data[i];
      }
    }

    void Upd_D()
    {
      DataIndicator indK = this._childInds[0];
      if (this._tempVars.Count == 0)
      {
        AddInputsToTempVars(new string[] { "smoothtype", "smoothslow" });
        this._tempVars.Add(StatFunctionsOld.GetValueDelegate(null));
      }
      Common.General.SmoothType smoothType = (Common.General.SmoothType)this._tempVars[0];
      int smooth = (int)this._tempVars[1];// smooth period
      Func<object, double> delItemType = (Func<object, double>)this._tempVars[2];
      StatFunctionsOld.SmoothData(smoothType, this._data, indK._data, smooth, this._startItemNo, delItemType);
    }

    void Upd_K()
    {
      DataIndicator indKFast = this._childInds[0];
      if (this._tempVars.Count == 0)
      {
        AddInputsToTempVars(new string[] { "smoothtype", "smooth" });
        this._tempVars.Add(StatFunctionsOld.GetValueDelegate(null));
      }
      Common.General.SmoothType smoothType = (Common.General.SmoothType)this._tempVars[0];
      int smooth = (int)this._tempVars[1];//// smooth period
      Func<object, double> delItemType = (Func<object, double>)this._tempVars[2];
      StatFunctionsOld.SmoothData(smoothType, this._data, indKFast._data, smooth, this._startItemNo, delItemType);
    }

    /*    double iK(Indicator userInd) {
              if (this.quoteCount >= userInd.k1) {
                if (userInd.refInd1 == null) userInd.refInd1 = Utils.GetUniqueIndicatorID("%KFAST", userInd.k1, 0, 0);
                double kFast = inds[userInd.refInd1].data[0];
                //				double kFast = this.GetIndicatorById("%KFAST", userInd.k1, 0, 0).data[0];
                userInd.temp0 = StatFunctions.EMAComplex(kFast, userInd.temp0, userInd.k2, this.quoteCount - userInd.k1 + 1);  // start K
                if (this.quoteCount == (userInd.k1 + userInd.k2)) return userInd.temp0;
                if (this.quoteCount > (userInd.k1 + userInd.k2))
                  return StatFunctions.EMA(kFast, userInd.temp0, userInd.k2); // ??? Need to check: this.tmpData[0]
              }
              return double.NaN;
            }*/



    void Upd_KFast()
    {
      DataIndicator indMax = this._childInds[0];
      DataIndicator indMin = this._childInds[1];
      DataIndicator indQuote = this._childInds[2];
      for (int i = this._startItemNo; i < this._endItemNo; i++)
      {
        double max = (double)indMax._data[i];
        double min = (double)indMin._data[i];
        this._data[i] = (max == min ? 50.0 : (((Quote)indQuote._data[i]).close - min) / (max - min) * 100);
      }
    }

    void Upd_Volume()
    {
      for (int i = this._startItemNo; i < this._endItemNo; i++)
      {
        this._data[i] = ((Quote)this._childInds[0]._data[i]).volume;
      }
    }

    void Upd_Trades()
    {
      for (int i = this._startItemNo; i < this._endItemNo; i++)
      {
        if (this._childInds[0]._data[i] is QuotePolygon qp)
          this._data[i] = Convert.ToDouble(qp.TradeCount);
        else
          this._data[i] = double.NaN;
      }
    }

    void Upd_VolBuySell_MA()
    {
      if (this._tempVars.Count == 0)
      {
        AddInputsToTempVars(new string[] { "smoothperiod", "smoothtype" });
        this._tempVars.Add(StatFunctionsOld.GetValueDelegate(null));
      }
      int smoothPeriod = (int)this._tempVars[0];
      Common.General.SmoothType smoothType = (Common.General.SmoothType)this._tempVars[1];
      Func<object, double> delItemType = (Func<object, double>)_tempVars[2];
      StatFunctionsOld.SmoothData(smoothType, this._data, this._childInds[0]._data, smoothPeriod, this._startItemNo, delItemType);
    }

    void Upd_VolBuySell_MA(string itemType)
    {
      if (this._tempVars.Count == 0)
      {
        AddInputsToTempVars(new string[] { "smoothperiod", "smoothtype" });
        this._tempVars.Add(StatFunctionsOld.GetValueDelegate(null));
      }
      int smoothPeriod = (int)this._tempVars[0];
      Common.General.SmoothType smoothType = (Common.General.SmoothType)this._tempVars[1];
      Func<object, double> delItemType = (Func<object, double>)_tempVars[2];
      StatFunctionsOld.SmoothData(smoothType, this._data, this._childInds[0]._data, smoothPeriod, this._startItemNo, delItemType);
    }

    void Upd_VolBuy()
    {
      ArrayList data = this._childInds[0]._data;
      for (int i = this._startItemNo; i < this._endItemNo; i++)
      {
        this._data[i] = ((Quote)data[i]).VolumeBuy;
      }
    }

    void Upd_VolSell()
    {
      ArrayList data = this._childInds[0]._data;
      for (int i = this._startItemNo; i < this._endItemNo; i++)
      {
        this._data[i] = ((Quote)data[i]).VolumeSell;
      }
    }

    void Upd_UpDownDoji()
    {
      //      DataInput di = (DataInput)this._inputs["doji_k"];
      double doji_k = Common.General.doji_k;// (double)di._value;
      for (int i = this._startItemNo; i < this._endItemNo; i++)
      {
        Quote q = (Quote)this._childInds[0]._data[i];
        if (Double.IsNaN(q.close) || Double.IsNaN(q.open)) this._data[i] = 3;
        if (q.open < q.close * (1 - 1 / doji_k))
        {// uptick
          this._data[i] = 1;
        }
        else if (q.open > q.close * (1 + 1 / doji_k))
        {// downtick
          this._data[i] = 2;
        }
        else this._data[i] = 3;
      }
    }

    void Upd_Min()
    {
      int period = (int)DataInput.GetDataInputByID("period", this._localInputs)._value;
      Func<object, double> delItemType = StatFunctionsOld.GetValueDelegate("low");
      StatFunctionsOld.Min(this._data, this._childInds[0]._data, period, this._startItemNo, delItemType);
    }

    void Upd_Max()
    {
      int period = (int)DataInput.GetDataInputByID("period", this._localInputs)._value;
      Func<object, double> delItemType = StatFunctionsOld.GetValueDelegate("high");
      StatFunctionsOld.Max(this._data, this._childInds[0]._data, period, this._startItemNo, delItemType);
    }

    void Upd_TradeIndicators()
    {
      var quotes = this._childInds[0]._data;
      var indicator = DataDB.DBIndicator.GetDBIndByID(_indID);
      var seriousDataType = indicator.GetSeriesDataType();
      // IList data;
      var pp = new List<object>();
      if (seriousDataType == typeof((DateTime, double)))
      {
        var quotePart = (Quote.ValueProperty)this._localInputs[0]._value;
        var fnQuotePart = (Func<object, double>)StatFunctionsOld.GetValueDelegate(quotePart.ToString());
        var data = new List<(DateTime, double)>();
        foreach (Quote q in quotes)
        {
          var value = fnQuotePart(q);
          data.Add((q.Date, value));
        }

        pp.Add(data);
        for (var k = 1; k < _localInputs.Count; k++)
          pp.Add(_localInputs[k]._value);
      }
      else if (seriousDataType ==  typeof(IQuote))
      {
        var data = new List<Skender.Stock.Indicators.Quote>();
        foreach (Quote q in quotes)
        {
          data.Add(new Skender.Stock.Indicators.Quote
          {
            Date = q.Date,
            Open = Convert.ToDecimal(q.Open),
            High = Convert.ToDecimal(q.High),
            Low = Convert.ToDecimal(q.Low),
            Close = Convert.ToDecimal(q.Close),
            Volume = Convert.ToDecimal(q.Volume)
          });
        }
        pp.Add(data);
        for (var k = 0; k < _localInputs.Count; k++)
          pp.Add(_localInputs[k]._value);
      }
      else
        throw new Exception($"Need to check for {seriousDataType.Name} serious data type");

      var method = indicator.GetMethod();
      try
      {
        var oo = method.Invoke(null, pp.ToArray()) as IList;
        for (var k = 0; k < oo.Count; k++)
        {
          var result = oo[k] as Skender.Stock.Indicators.IReusableResult;
          _data[k] = result.Value ?? double.NaN;
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show($@"ERROR!{Environment.NewLine}{ex}");
      }

      return;

      // var quotePart = (Quote.ValueProperty)this._localInputs[0]._value;
      // var fnQuotePart = (Func<object, double>)StatFunctionsOld.GetValueDelegate(quotePart.ToString());

      // var smoothPeriod = (int)this._localInputs[1]._value;

      /*var data = new List<Ti_Value>();
      foreach(Quote q in quotes)
      {
        var value = fnQuotePart(q);
        data.Add(new Ti_Value { Date = q.Date, Value = double.IsNaN(value) ? null : value });
      }*/

      // calculate
      /*var method = indicator.GetMethod();
      var oo = method.Invoke(null, new object[] { data, smoothPeriod }) as IList;
      for (var k = 0; k < oo.Count; k++)
      {
        var result = oo[k] as IReusableResult;
        _data[k] = result.Value ?? double.NaN;
      }
      return;

      List<Skender.Stock.Indicators.Quote> iQuotes = new List<Skender.Stock.Indicators.Quote>();
      foreach (Quote q in quotes)
      {
        iQuotes.Add(new Skender.Stock.Indicators.Quote
        {
          Date = q.Date,
          Open = Convert.ToDecimal(q.Open),
          High = Convert.ToDecimal(q.High),
          Low = Convert.ToDecimal(q.Low),
          Close = Convert.ToDecimal(q.Close),
          Volume = Convert.ToDecimal(q.Volume)
        });
      }*/

      // calculate
      // var indicator = DataDB.DBIndicator.GetDBIndByID(_indID);
      /*var results = ((IEnumerable< IReusableResult >) indicator._method.Invoke(this, new object[] {iQuotes, smoothPeriod})).ToArray();
      // var results1 = iQuotes.GetWma(smoothPeriod).ToArray();
      for (var k = 0; k < results.Length; k++)
      {
        _data[k] = results[k].Value ?? double.NaN;
      }
      */

      /*DataIndicator indQuote = this._childInds[0];
      if (this._tempVars.Count == 0)
      {
        AddInputsToTempVars(new string[] { "value", "smoothperiod", "smoothtype" });
      }
      Func<object, double> delValueType = (Func<object, double>)this._tempVars[0];
      int smoothPeriod = (int)this._tempVars[1];
      Common.General.SmoothType smoothType = (Common.General.SmoothType)this._tempVars[2];

      StatFunctionsOld.SmoothData(smoothType, this._data, indQuote._data, smoothPeriod, this._startItemNo, delValueType);*/
    }

    private class Ti_Value : Skender.Stock.Indicators.IReusableResult
    {
      public double? Value { get; set; }
      public DateTime Date { get; set; }
    }
  }
}
