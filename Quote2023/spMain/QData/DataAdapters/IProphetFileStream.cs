using System;
using System.Collections;
using System.Collections.Generic;

namespace spMain.QData.DataAdapters {

  [Serializable]
  class IProphetFileStream : Data.DataAdapter {

    // ============================  SubClass DataElement ==============================
    class DataElement {
      string _dataID;
      DateTime _lastUpdated = DateTime.Now;
      double _tickIntervalInMiliSec;
      int _dataOffset = 0;
      int _dataRecords;

      internal DataElement(string dataID, double tickIntervalInMiliSec, int dataRecords) {
        this._dataID = dataID; this._tickIntervalInMiliSec = tickIntervalInMiliSec; this._dataRecords = dataRecords;
      }

      internal int GetDataOffset() { return this._dataOffset; }

      internal void TimerTick() {
        TimeSpan ts = DateTime.Now - this._lastUpdated;
        if (this._dataRecords > this._dataOffset) {
          if (ts.TotalMilliseconds > this._tickIntervalInMiliSec) {
            this._dataOffset++;
            this._lastUpdated = DateTime.Now;
          }
        }
        if (!(this._dataRecords > this._dataOffset)) csTimerManager.UnRegister(this); 
      }

    }

    // =======================================================================
    Dictionary<string, IList> _dictData = new Dictionary<string, IList>();
    Dictionary<string, DataElement> _dictElement = new Dictionary<string, DataElement>();

    public override bool IsStream {
      get { return true; }
    }

    public override string CheckDataInputs(List<spMain.QData.Data.DataInput> inputs) {
      return null;
    }

    public override spMain.QData.Common.TimeInterval BaseTimeInterval {
      get { return new spMain.QData.Common.TimeInterval(60); }
    }

    public override List<Data.DataInput> GetInputs() {
      List<Data.DataInput> x = new List<Data.DataInput>();
      x.Add(new spMain.QData.Data.DataInput("symbol", "Symbol", "RNO", null));
      x.Add(new spMain.QData.Data.DataInput("date", "Date", new DateTime(2007, 4, 20), "Last date to show"));
      x.Add(new spMain.QData.Data.DataInput("days", "Number of days", 10,
        "Number of working days before last date. This parameter and 'Date' parameter define the start date."));
      x.Add(new spMain.QData.Data.DataInput("tickinterval", "Tick interval in seconds", 1.5,
        "Tick interval in seconds"));
      return x;
    }

    public override IList GetData(List<object> inputs, int lastDataOffset, out int newDataOffset) {
      newDataOffset = 0;
      return null;
      string symbol = (string)inputs[0];
      DateTime endDate = (DateTime)inputs[1];
      int days = (int)inputs[2];
      double tickIntervalInMiliSec = ((double)inputs[3])*1000;
      string dataID = this.GetDataID(symbol, endDate, days);
      if (!_dictData.ContainsKey(dataID)) {
        IProphetFile adapter = (IProphetFile)Data.DataManager.dataProviders[typeof(IProphetFile)];
        int itmp;
        List<object> xinputs = new List<object>();
        xinputs.Add(inputs[0]); 
        xinputs.Add(inputs[1]); 
        xinputs.Add(inputs[2]);
        IList data = adapter.GetData(xinputs, 0, out itmp);
//        data.RemoveRange(10, data.Count - 10);
        this._dictData.Add(dataID, data);
      }

      IList data1 = this._dictData[dataID];
      string fullID = this.GetFullID(symbol, endDate, days, tickIntervalInMiliSec);
      DataElement elem = null;
      if (!this._dictElement.ContainsKey(fullID)) {
        elem = new DataElement(dataID, tickIntervalInMiliSec, data1.Count);
        this._dictElement.Add(fullID, elem);
        csTimerManager.Register(elem, Convert.ToInt32(tickIntervalInMiliSec), elem.TimerTick);
      }
      else {
        elem = this._dictElement[fullID];
      }
      newDataOffset = elem.GetDataOffset();
      int elements = newDataOffset- lastDataOffset;
      object[] x = new object[elements];
///????      data1.CopyTo(lastDataOffset, x, 0, elements);
      return new ArrayList(x);
    }

    // ==============================  Private section  =======================================
    string GetDataID(string symbol, DateTime date, int days) {
      return (symbol + "\t" + date.ToString("yyyy-mm-dd") + "\t" + days.ToString()).ToLower();
    }

    string GetFullID(string symbol, DateTime date, int days, double tickInterval) {
      return (symbol + "\t" + date.ToString("yyyy-mm-dd") + "\t" + days.ToString() + "\t" + tickInterval.ToString()).ToLower();
    }


  }
}
