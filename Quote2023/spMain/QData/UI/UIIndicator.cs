using System;
using System.Diagnostics;
using System.Drawing;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using ZedGraph;
using spMain.QData.DataFormat;

namespace spMain.QData.UI {
  [Serializable]
  public class UIIndicator : cs.IPG_AdjustProperties, ISerializable, cs.IPG_ValidateSupport {

        #region IPG_ValidateSupport Members

    public string GetErrorDescription() {
      if (this._dbInd == null) return "Indicator can not be empty." + Environment.NewLine;
      return null;
    }

    #endregion

    #region IPG_AdjustProperties Members
    public void AdjustProperties(Dictionary<string, System.ComponentModel.PropertyDescriptor> propertyList, System.ComponentModel.ITypeDescriptorContext context) {
      cs.PGPropertyDescriptor pd_type = (cs.PGPropertyDescriptor)propertyList["Type"];
      pd_type._attrList = new spMain.cs.PG_ListAttribute(DataDB.DBIndicator._dataIndicatorList, false);
      cs.PGPropertyDescriptor pd_curve = (cs.PGPropertyDescriptor)propertyList["Curve"];
      pd_curve._attrBrowsable = new BrowsableAttribute(this._dbInd!=null);

      // Inputs + reoder propertyList
      propertyList.Clear();
      propertyList.Add("Type", pd_type);
      foreach (Data.DataInput input in this._inputs) {
        Data.DataInputPropertyDescriptor diPD = new Data.DataInputPropertyDescriptor(this, input,
          new Attribute[] { new RefreshPropertiesAttribute(RefreshProperties.All) });
        propertyList.Add(diPD.Name, diPD);
      }
      propertyList.Add("Curve", pd_curve);
    }
    #endregion


    public DataDB.DBIndicator _dbInd;// cloned DBIndicator
    Curve _curveObject;
    List<Data.DataInput> _inputs = new List<Data.DataInput>();
    public Data.DataIndicator _dataInd;
    double _lastValue;
    int _lastTimerID = -1;
    CurveItem _graphCurve = null;// Zedgraph curve

    // ========================  Constructor =============================
    public UIIndicator() { }

    public UIIndicator(SerializationInfo info, StreamingContext ctxt) {
      object[] x = csFastSerializer.Utils.Deserialize(info);
      if (x != null) {
        string s = (string)x[0];
        if (!String.IsNullOrEmpty(s)) {
          this._dbInd = DataDB.DBIndicator.GetDBIndByID(s);
        }
        this._inputs = (List<Data.DataInput>)x[1];
        this._curveObject = (Curve)x[2];
        Data.DataInput.CheckInputs(this._inputs, this._dbInd._inputs);// normalize inputs
        if (this._dbInd != null && this._curveObject == null) this._curveObject = new Curve(this._dbInd);
      }
    }
    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext ctxt) {
      csFastSerializer.Utils.Serialize(info, new object[] { (this._dbInd == null ? null : this._dbInd._id), this._inputs, this._curveObject });
    }

    // ===========================  Properties =================================
    [RefreshProperties(RefreshProperties.All)]
    public DataDB.DBIndicator Type {
      get { return this._dbInd; }
      set {
        if (value != null && (this.Type != value)) {
          this._dbInd = DataDB.DBIndicator.GetDBIndByID(value._id);
          _inputs.Clear();
          foreach (Data.DataInput input in this._dbInd._inputs) {
            _inputs.Add((Data.DataInput)input.Clone());
          }
          this._curveObject = new Curve(this._dbInd);
        }
      }
    }

    public Curve Curve {
      get { return this._curveObject; }
      set {
        if (value != null && value != Curve) {
          this._curveObject = value;
        }
      }
    }

    // ==========================  Public section ==============================
    [Browsable(false)]
    public double LastValue {
      get {
        if (Double.IsNaN(this._dataInd._dataFactor)) return Math.Round(this._lastValue, this._curveObject.DecimalPlaces);
        else return double.NaN;
      }
    }

    public void CreateDataSources(List<Data.DataInput> globalInputs) {
      this.ClearDataSources();
      if (this._dbInd == null) {
        this._dataInd = null;
        return;
      }
/*      List<Data.DataInput> localInputs = new List<Data.DataInput>();
      foreach (Data.DataInput input in this._inputs) {
        localInputs.Add(input);
      }*/
      this._dataInd = Data.DataManager.GetDataIndicator(this._dbInd._id, this._inputs, globalInputs, this);

      if (this._curveObject != null) {
        if (this._curveObject.IsBarColorApplicable()) this._curveObject.BarColor.CreateDataSources(globalInputs);;
        if (this._curveObject.IsSymbolColorApplicable()) this._curveObject.SymbolColor.CreateDataSources(globalInputs); 
      }
    }
    public void ClearDataSources() {
      this._lastTimerID = -1;
      this._lastValue = double.NaN;
      if (this._dataInd != null) {
        this._dataInd.UnRegister(this);
      }
      if (this._curveObject != null) {
        if (this._curveObject.IsBarColorApplicable()) this._curveObject.BarColor.ClearDataSources();
        if (this._curveObject.IsSymbolColorApplicable()) this._curveObject.SymbolColor.ClearDataSources();
      }
    }

    // ============================  Public section ===================================
    public void UpdateData(int timerID) {
      if (timerID > this._lastTimerID) {
        this._lastTimerID = timerID;
        if (this._dataInd != null) {
          int iMin = Math.Max(0, this._dataInd._data.Count - 1);
          this._dataInd.UpdateData(timerID); // Upddate data
          for (int i = this._dataInd._data.Count - 1; i >= iMin; i--) {// Set last value
            object o = this._dataInd._data[i];
            double x=double.NaN;
            if (o is Quote) x = ((Quote)o).close;
            else if (o is double) x = (double)o;
            else break;
            if (!double.IsNaN(x)) {
              this._lastValue = x;
              break;
            }
          }
        }
        if (this._curveObject != null) {
          if (this._curveObject.IsBarColorApplicable()) this._curveObject.BarColor._dataInd.UpdateData(timerID);
          if (this._curveObject.IsSymbolColorApplicable()) this._curveObject.SymbolColor._dataInd.UpdateData(timerID);
        }
      }
    }

/*    public void CurveFillData(int iStart, int iEnd) {
      IList colorArray = null;
      if (this._curveObject.IsBarColorApplicable()) colorArray = this._curveObject.BarColor._dataInd._data;
      if (this._curveObject.IsSymbolColorApplicable()) colorArray = this._curveObject.SymbolColor._dataInd._data;
      this._graphCurve.Clear();
      IPointList points = this._graphCurve.Points;
      for (int i = iStart; i <= iEnd; i++) {// Do need check iStart/iEnd
        PointPair pp = this.GetPointPair(i, colorArray);
        this._graphCurve.AddPoint(pp);
      }
    }*/

//    Dictionary<TimeSpan, double> _timeLog = new Dictionary<TimeSpan, double>();
    public Dictionary<int, string> _timeLog = new Dictionary<int,string>();

    public void CurveFillData(int iStart, int iEnd, float rectWidth) {
      Stopwatch sw = new Stopwatch();
      sw.Start();
      ComplexColor cc = null;
      if (this._curveObject.IsBarColorApplicable()) cc = this._curveObject.BarColor;
      if (this._curveObject.IsSymbolColorApplicable()) cc = this._curveObject.SymbolColor;
      int compression = (iEnd - iStart) / (Convert.ToInt32(rectWidth * 1.2));
      this._graphCurve.Clear();
      IPointList points = this._graphCurve.Points;
      for (int i = iStart; i <= iEnd; i++) {// Do need check iStart/iEnd
        PointPair pp = this.GetPointPair(i, cc);
        // Data compression - не увеличивает скорость скролинга
        /*          if (pp is StockPt) {
                    StockPt x1 = (StockPt)pp;
                    if (x == null) x = x1;
                    else {
                      if (x1.High > x.High) x.High = x1.High;
                      if (x1.Low < x.Low) x.Low = x1.Low;
                      x.Close = x1.Close;
                    }
                    if (k1 == compression) {
                      this._graphCurve.AddPoint(x);
                      k1 = 0;
                      x = null;
                    }
                    else k1++;
                  }
                  else {
                    this._graphCurve.AddPoint(pp);
                  }*/
        this._graphCurve.AddPoint(pp);
      }
      sw.Stop();
      _timeLog.Add(_timeLog.Count, DateTime.Now.TimeOfDay.ToString() + "; " + 
        this._graphCurve.Points.Count.ToString("N0") +"; " + sw.Elapsed.TotalMilliseconds.ToString("R"));
    }

    PointPair GetPointPair(int dataIndex, ComplexColor cc) {
      object data = this._dataInd._data[dataIndex];
      switch (this._curveObject.CurveStyle) {
        case Common.General.CurveStyle.Candle:
        case Common.General.CurveStyle.OHLC: // StockPt
          Quote q = (Quote)data;
          StockPt sp;
          if (q.volume >= 0) {
            sp = new StockPt(dataIndex + 0.5, q.high, q.low, q.open, q.close, q.volume);
          }
          else {
            sp = new StockPt(dataIndex + 0.5, double.NaN, double.NaN, double.NaN, double.NaN, 0);
          }
          if (cc != null) {
            sp.ColorValue = cc.GetDataColorValue(this._dataInd._data, dataIndex);
          }
          //          if (!Double.IsNaN(this._dataInd._dataFactor)) sp.Tag= this._dataInd._dataFactor;
          return sp;
        default: // PointPair
          PointPair pp = null;
          if (Double.IsNaN(this._dataInd._dataFactor)) {
            if (data is double) pp = new PointPair(dataIndex + 0.5, Math.Round((double)data, this._curveObject.DecimalPlaces));
            else if (data is Quote) pp = new PointPair(dataIndex + 0.5, ((Quote)data).close);
            else throw new Exception("UIIndicator.GetPointPair procedure. Can not convert '" + data.ToString() + "' into PointPair");
          }
          else {
            double k = this._dataInd._dataFactor;
            if (data is double) {
              pp = new PointPair(dataIndex + 0.5, (double)data * k);
              pp.Tag = (double)data;
            }
            else if (data is Quote) {
              pp = new PointPair(dataIndex + 0.5, ((Quote)data).close * k);
              pp.Tag = ((Quote)data).close;
            }
            else throw new Exception("UIIndicator.GetPointPair procedure. Can not convert '" + data.ToString() + "' into PointPair");

          }
          if (cc != null) {
            pp.ColorValue = cc.GetDataColorValue(this._dataInd._data, dataIndex);
          }
          //          if (!Double.IsNaN(this._dataInd._dataFactor)) pp.Tag = this._dataInd._dataFactor;
          return pp;
      }
    }

    PointPair GetPointPair(int dataIndex, IList colorArray) {
      object data = this._dataInd._data[dataIndex];
      switch (this._curveObject.CurveStyle) {
        case Common.General.CurveStyle.Candle:
        case Common.General.CurveStyle.OHLC: // StockPt
          Quote q = (Quote)data;
          StockPt sp;
          if (q.volume > 0) {
            sp = new StockPt(dataIndex + 0.5, q.high, q.low, q.open, q.close, q.volume);
          }
          else {
            sp = new StockPt(dataIndex + 0.5, double.NaN, double.NaN, double.NaN, double.NaN, 0);
          }
          if (colorArray != null) {
            sp.ColorValue = Convert.ToDouble(colorArray[dataIndex]);
          }
          //          if (!Double.IsNaN(this._dataInd._dataFactor)) sp.Tag= this._dataInd._dataFactor;
          return sp;
        default: // PointPair
          PointPair pp = null;
          if (Double.IsNaN(this._dataInd._dataFactor)) {
            if (data is double) pp = new PointPair(dataIndex + 0.5, Math.Round((double)data, this._curveObject.DecimalPlaces));
            else if (data is Quote) pp = new PointPair(dataIndex + 0.5, ((Quote)data).close);
            else throw new Exception("UIIndicator.GetPointPair procedure. Can not convert '" + data.ToString() + "' into PointPair");
          }
          else {
            double k = this._dataInd._dataFactor;
            if (data is double) {
              pp = new PointPair(dataIndex + 0.5, (double)data * k);
              pp.Tag = (double)data;
            }
            else if (data is Quote) {
              pp = new PointPair(dataIndex + 0.5, ((Quote)data).close * k);
              pp.Tag = ((Quote)data).close;
            }
            else throw new Exception("UIIndicator.GetPointPair procedure. Can not convert '" + data.ToString() + "' into PointPair");

          }
          if (colorArray != null) {
            pp.ColorValue = Convert.ToDouble(colorArray[dataIndex]);
          }
          //          if (!Double.IsNaN(this._dataInd._dataFactor)) pp.Tag = this._dataInd._dataFactor;
          return pp;
      }
    }

    public void CreateCurve(ZedGraph.GraphPane pane) {
      switch (this._curveObject.CurveStyle) {
        case Common.General.CurveStyle.Candle:
          StockPointList spl = new StockPointList();
          JapaneseCandleStickItem candleCurve = pane.AddJapaneseCandleStick(this._dbInd._id, spl);
          candleCurve.Stick.RisingFill = new Fill(Common.General.candleUpColor);
          candleCurve.Stick.FallingFill = new Fill(Common.General.candleDownColor);
          candleCurve.Stick.FallingColor = Common.General.candleBorderColor; // 
          candleCurve.Stick.RisingBorder.Color = Common.General.candleBorderColor;
          candleCurve.Stick.FallingBorder.Color = Common.General.candleBorderColor;
          pane.BarSettings.MinBarGap = 0.2f;
          pane.BarSettings.MinClusterGap = 0.8f;
          pane.BarSettings.ClusterScaleWidth = 1.0f;
          this._graphCurve = candleCurve;
          break;
        case Common.General.CurveStyle.OHLC:
          StockPointList spl1 = new StockPointList();
          OHLCBarItem ohlcCurve = pane.AddOHLCBar(this._dbInd._id, spl1, Color.Empty);
          pane.BarSettings.MinBarGap = 0.2f;
          pane.BarSettings.MinClusterGap = 0.8f;
          pane.BarSettings.ClusterScaleWidth = 1.0f;
          ohlcCurve.Bar.GradientFill = this._curveObject.BarColor.GetDataColorFill();
          this._graphCurve = ohlcCurve;
          break;
        case Common.General.CurveStyle.Bar:
          PointPairList spl2 = new PointPairList();
          BarItem barCurve = pane.AddBar(this._dbInd._id, spl2, Color.Empty);
          pane.BarSettings.MinBarGap = 0.2f;
          pane.BarSettings.MinClusterGap = 0.8f;
          pane.BarSettings.ClusterScaleWidth = 1.0f;
          barCurve.Bar.Fill = this._curveObject.BarColor.GetDataColorFill();
//          barCurve.Bar.Border.Color = barCurve.Bar.Fill.Color;
//          barCurve.Bar.Border.GradientFill = this._curveObject.BarColor.GetDataColorFill();
          //          pane.BarSettings.Type = BarType.Stack;
          this._graphCurve = barCurve;
          break;
        case Common.General.CurveStyle.Solid:
        case Common.General.CurveStyle.Dash:
        case Common.General.CurveStyle.DashDot:
        case Common.General.CurveStyle.DashDotDot:
        case Common.General.CurveStyle.Dot:
        case Common.General.CurveStyle.None:
          PointPairList ppl2 = new PointPairList();
          LineItem lineCurve = pane.AddCurve(this._dbInd._id, ppl2, this._curveObject.LineColor);
          if (this._curveObject.CurveStyle == Common.General.CurveStyle.None) {// No line
            lineCurve.Line.IsVisible = false;
          }
          else {// line exists
              lineCurve.Line.Style = (System.Drawing.Drawing2D.DashStyle)Enum.Parse(typeof(System.Drawing.Drawing2D.DashStyle),
                this._curveObject.CurveStyle.ToString());
          }
          lineCurve.Symbol.IsVisible = this._curveObject.IsSymbolColorApplicable();
          lineCurve.Symbol.Type = this._curveObject.SymbolType;
          lineCurve.Symbol.Fill = this._curveObject.SymbolColor.GetDataColorFill();
          lineCurve.Symbol.Border.GradientFill = this._curveObject.SymbolColor.GetDataColorFill();
          lineCurve.Symbol.Size=10f;
          this._graphCurve = lineCurve;
          break;
      }
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder();
      foreach (Data.DataInput input in this._inputs) {
        sb.Append("," + input._value.ToString());
      }
      string s = (this._dbInd == null ? null : this._dbInd._name);
      if (sb.Length > 0) {
        return s + " (" + sb.Remove(0, 1).ToString() + ")";
      }
      else {
        return s;
      }
    }

    public string GetLabelTemplateForGraph() {
      string s = this.Curve.LabelTemplate;
      if (this._curveObject != null && !string.IsNullOrEmpty(s)) {
        foreach (Data.DataInput di in this._inputs) {
          string s1 = "{" + di._id + "}";
          s = s.Replace(s1, csUtils.FormattedStringFromObject(di._value));
        }
        if (this._dataInd != null) {
          foreach (Data.DataInput di in this._dataInd._globalInputs) {
            string s1 = "{" + di._id + "}";
            s = s.Replace(s1, csUtils.FormattedStringFromObject(di._value));
          }
        }
        string s2 = Data.DataInput.GetDataInputListDescription(this._inputs, "; ", "", "");
          s=s.Replace("{I}", s2);
          s=s.Replace("{i}", s2);
          return s;
      }
      return "";
    }

    // ==============================  Private section =============================
  }
}
