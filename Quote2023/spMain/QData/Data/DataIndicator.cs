using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using spMain.QData.DataFormat;

namespace spMain.QData.Data {

  public partial class DataIndicator {

    public string _indID;
    List<Data.DataInput> _localInputs = new List<Data.DataInput>();
    public List<DataInput> _globalInputs = new List<DataInput>();//Only for framedQuoteInd: DataAdapter, TimeInterval, Symbol and other
    public readonly string _uniqueID;
    public List<DataIndicator> _childInds = new List<DataIndicator>();
    List<object> _dataSinks = new List<object>();
    int _lastTimerID = -1;
    ArrayList _tempVars = new ArrayList();
    public ArrayList _data = new ArrayList();
    public List<DateTime> _dates = null;
    public readonly Type _valueDataType;

    // ======================  Constructor ==========================
    public DataIndicator(string indID, List<DataInput> localInputs, List<DataInput> globalInputs) {
      this._indID = indID;
//      DataDB.IndicatorDB indDB = DataDB.IndicatorDB.GetDBIndByID(indID);
      DataDB.DBIndicator dbInd = DataDB.DBIndicator.GetDBIndByID(indID);

      this._valueDataType = dbInd._valueDataType;
      // Prepare localInputs
      foreach (DataInput input in dbInd._inputs) {
        if (!(input._value is Color)) {
          DataInput x = (DataInput)input.Clone();
          DataInput di = DataInput.GetDataInputByID(x._id, localInputs);
          if (di != null) x._value = di._value;
          else {
            di = DataInput.GetDataInputByID(x._id, globalInputs);
            if (di == null) {
              throw new Exception("Can not find DataInput with id '" + x._id + "' for indicator with id '" + this._indID + "'");
            }
            else
              x._value = di._value;
          }
          this._localInputs.Add(x);
        }
      }

      // Set uniqueID
      this._uniqueID = DataManager.GetIndicatorUniqueID(indID, localInputs, globalInputs);

      // Prepare child indicators
      foreach (DataDB.DBIndicator.DependedIndicator depInd in dbInd._dependedInds) {
        DataDB.DBIndicator depDBInd = DataDB.DBIndicator.GetDBIndByID(depInd._dependedIndID);
        List<DataInput> childLocals = new List<DataInput>();
        List<DataInput> childGlobals = new List<DataInput>();
        foreach (DataInput input in globalInputs) {
          DataInput x1 = (DataInput)(input.Clone());
          depInd.SetDataInputValue(x1, this._localInputs);
/*          int k1 = depInd._dependedIndInputs.IndexOf(x1._id);
          if (k1 >= 0) {
            x1._value = DataInput.GetDataInputByID(depInd._baseIndInputs[k1], this._localInputs)._value;
          }*/
          childGlobals.Add(x1);
        }
        foreach (DataInput di in depDBInd._inputs) {
          DataInput x1 = (DataInput)di.Clone();
          depInd.SetDataInputValue(x1, this._localInputs);
/*          int k1 = depInd._dependedIndInputs.IndexOf(x1._id);
          x1._value = DataInput.GetDataInputByID(depInd._baseIndInputs[k1], this._localInputs)._value;*/
          childLocals.Add(x1);
        }
        this._childInds.Add(DataManager.GetDataIndicator(depInd._dependedIndID, childLocals, childGlobals, this));
      }

      /*      foreach (string childIndID in indDB._dependsOnInds) {
        DataDB.IndicatorDB dependedInd = DataDB.IndicatorDB.GetDBIndByID(childIndID);
        List<DataInput> childLocals = new List<DataInput>();
        List<string> inputXref = new List<string>(indDB._dependsOnInputs[indcnt]);
        List<DataInput> childGlobals = new List<DataInput>();
        foreach (DataInput input in globalInputs) {
          DataInput x1 = (DataInput)(input.Clone());
          if (inputXref.Contains(x1._id)) {// (parameter=global Input) == change global input value : indicator-Symbol to compare
            x1._value = DataInput.GetDataInputByID(x1._id, this._localInputs)._value;
            inputXref.Remove(x1._id);
          }
          childGlobals.Add(x1);
        }
        int i = 0;
        foreach (DataInput di in dependedInd._inputs) {
          DataInput input = (DataInput)di.Clone();
          input._value = DataInput.GetDataInputByID(inputXref[i], this._localInputs)._value;
          childLocals.Add(input);
          i++;
        }
        this._childInds.Add(DataManager.GetDataIndicator(childIndID, childLocals, childGlobals, this));
        indcnt++;
      }*/
      // Prepare Global inputs
      if (dbInd._dependedInds.Count == 0) {// framedquote or framedBidAsk == need DataAdapter parameters
        this._dates = new List<DateTime>();
      }
      foreach (DataInput di in globalInputs) this._globalInputs.Add((DataInput)di.Clone());
    }

    // ========================   Public Section  ============================

    public void Register(object o) {
      if (!this._dataSinks.Contains(o)) this._dataSinks.Add(o);
    }
    public void UnRegister(object o) {
      if (this._dataSinks.Contains(o)) this._dataSinks.Remove(o);
      if (this._dataSinks.Count == 0) {
        foreach (DataIndicator ind in this._childInds) {
          ind.UnRegister(this);
        }
        DataManager.RemoveDataIndicator(this);
      }

      _dates?.Clear();
      _data.Clear();
    }

    public List<DateTime> GetDateArray() {
      if (this._dates == null) {
        foreach (DataIndicator ind in this._childInds) {
          List<DateTime> x = ind.GetDateArray();
          if (x != null) return x;
        }
      }
      return this._dates;
    }

    public string GetFileHeader() {
      string template = (this._valueDataType == typeof(Quote) ?
        "Date{0}" + "\t" + "Open{0}" + "\t" + "High{0}" + "\t" + "Low{0}" + "\t" + "Close{0}" + "\t" + "Volume{0}" : this._indID + "{0}");
      string sInputs = DataInput.GetDataInputListDescription(this._localInputs, "_", "_", "");
      return string.Format( template, sInputs);
    }

    public override string ToString() {
      return this._uniqueID;
    }


  }
}
