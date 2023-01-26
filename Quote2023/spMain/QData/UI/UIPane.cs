using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace spMain.QData.UI {
  [Serializable]
  public class UIPane : ISerializable, cs.IPG_ValidateSupport {

    #region IPG_ValidateSupport Members

    public string GetErrorDescription() {
      StringBuilder sb = new StringBuilder();
      if (this.Indicators.Count == 0) sb.Append("Pane should have an indicator" + Environment.NewLine);
      foreach (UIIndicator ind in this.Indicators) {
        string s = ((cs.IPG_ValidateSupport)ind).GetErrorDescription();
        if (!String.IsNullOrEmpty(s)) sb.Append(s);
      }
      return sb.ToString();
    }

    #endregion

    List<UIIndicator> _inds = new List<UIIndicator>();
    int _lastTimerID = -1;

    // ========================  Constructor =============================
    public UIPane() { }

    public UIPane(SerializationInfo info, StreamingContext ctxt) {
      object[] x = csFastSerializer.Utils.Deserialize(info);
      if (x != null) {
        this._inds = (List<UIIndicator>)x[0];
      }
    }
    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext ctxt) {
      csFastSerializer.Utils.Serialize(info, new object[] { this._inds });
    }

    [cs.PG_IsShowNameOfArrayElement(true)]
    public List<UIIndicator> Indicators {
      get { return this._inds; }
    }

    [Browsable(false)]
    public List<double> LastValuesOfIndicators {
      get {
        List<double> x = new List<double>();
        foreach (UIIndicator ind in this.Indicators) {
          double value = ind.LastValue;
          if (!Double.IsNaN(value)) x.Add(value);// do not add invalid value (in case of CompareIndicator)
        }
        return x;
      }
    }

    public void CreateDataSources(List<Data.DataInput> globalInputs) {
      this.ClearDataSources();
      foreach (UIIndicator ind in this._inds) {
        ind.CreateDataSources(globalInputs);
      }
    }
    public void ClearDataSources() {
      this._lastTimerID = -1;
      foreach (UIIndicator ind in this._inds) {
        ind.ClearDataSources();
      }
    }

    public void UpdateData(int timerID) {
      if (this._lastTimerID < timerID) {
        this._lastTimerID = timerID;
        foreach (UIIndicator ind in this._inds) {
          ind.UpdateData(timerID);
        }
      }
    }
  }
}
