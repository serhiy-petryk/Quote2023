using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using spMain.QData.Data;

namespace spMain.QData.UI {

  [Serializable]
  public class UIGraph : cs.IPG_AdjustProperties, ISerializable, cs.IPG_ValidateSupport {

    public static readonly string _serializationFileName = csIni.pathExe + @"srl\uiGraph.srl";

    #region IPG_ValidateSupport Members

    string spMain.cs.IPG_ValidateSupport.GetErrorDescription() {
      StringBuilder sb = new StringBuilder();
      if (this._dataAdapter == null) sb.Append("Specify data adapter" + Environment.NewLine);
      if (this._timeInterval == null) sb.Append("Specify time interval" + Environment.NewLine);
      if (this._dataAdapter != null) {
        string s1 = this._dataAdapter.CheckDataInputs(this._adapterInputs);
        if (!String.IsNullOrEmpty(s1)) sb.Append(s1);
      }
      if (this.Panes.Count == 0) sb.Append("Graph should have an pane" + Environment.NewLine);
      if (this._dataAdapter != null && this._timeInterval.GetSecondsInInterval() < this._dataAdapter.BaseTimeInterval.GetSecondsInInterval())
        sb.Append("Time Interval can not be less than BaseTimeInterval of Data Adapter" + Environment.NewLine);
      foreach (UIPane pane in this.Panes) {
        string s = ((cs.IPG_ValidateSupport)pane).GetErrorDescription();
        if (!String.IsNullOrEmpty(s)) sb.Append(s);
      }
      return sb.ToString();
    }

    #endregion
    //    public List<DateTime> _dates = new List<DateTime>();
    private Data.DataAdapter _dataAdapter;
    private Common.TimeInterval _timeInterval = new spMain.QData.Common.TimeInterval(60);
    private List<UIPane> _panes = new List<UIPane>();
    private List<Data.DataInput> _adapterInputs = new List<spMain.QData.Data.DataInput>();
    int _lastTimerID = -1;
    string _description = "Enter description";

    #region IPG_AdjustProperties Members

    void spMain.cs.IPG_AdjustProperties.AdjustProperties(Dictionary<string, PropertyDescriptor> propertyList, ITypeDescriptorContext context) {
      cs.PGPropertyDescriptor pd = (cs.PGPropertyDescriptor)propertyList["DataAdapter"];
      pd._attrList = new spMain.cs.PG_ListAttribute(Data.DataManager.dataProviders.Values, false);

      if (this._adapterInputs != null) {
        foreach (Data.DataInput input in this._adapterInputs) {
          Data.DataInputPropertyDescriptor diPD = new spMain.QData.Data.DataInputPropertyDescriptor(this, input, null);
          propertyList.Add(diPD.Name, diPD);
        }
      }

    }

    #endregion

    // ========================  Constructor =============================
    public UIGraph() { }

    public UIGraph(SerializationInfo info, StreamingContext ctxt) {
      object[] x = csFastSerializer.Utils.Deserialize(info);
      if (x != null) {
        if (x[0] != null) {
          this._dataAdapter = Data.DataManager.dataProviders[(Type)x[0]];
        }
        this._timeInterval = (Common.TimeInterval)x[1];
        this._panes = (List<UIPane>)x[2];
        this._adapterInputs = (List<Data.DataInput>)x[3];
        this._description = x[4].ToString();
        if (x.Length > 5) AutosizeOnOpen = (bool)x[5];
      }
    }
    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext ctxt) {
      csFastSerializer.Utils.Serialize(info, new object[] { 
        (this._dataAdapter==null? null : this._dataAdapter.GetType()), 
        this._timeInterval, this._panes, this._adapterInputs, this._description, AutosizeOnOpen});
    }

    // =======================  Properties ===========================
    [Browsable(false)]
    public string GraphDescription {
      get { return Data.DataInput.GetDataInputListDescription(this._adapterInputs, "; ", "", ""); }
    }

    public string Description {
      get { return this._description; }
      set { this._description = value; }
    }

    [RefreshProperties(RefreshProperties.All)]
    public Data.DataAdapter DataAdapter {
      get { return this._dataAdapter; }
      set {
        if (value != null && value != this._dataAdapter) {
          this._dataAdapter = value;
          this._timeInterval = this._dataAdapter.BaseTimeInterval;
          List<Data.DataInput> inputs = this._dataAdapter.GetInputs();
          this._adapterInputs.Clear();
          foreach (Data.DataInput input in inputs) {
            this._adapterInputs.Add((Data.DataInput)input.Clone());
          }
        }
      }
    }

    [RefreshProperties(RefreshProperties.All)]
    public Common.TimeInterval TimeInterval {
      get { 
        return this._timeInterval; 
      }
      set {
        if (value != null && value != this._timeInterval) {
          this._timeInterval = value;
        }
      }
    }

    // [RefreshProperties(RefreshProperties.All)]
    public bool AutosizeOnOpen { get; set; } = true;

    [RefreshProperties(RefreshProperties.All)]
    public List<UIPane> Panes {
      get { return this._panes; }
    }

    // ======================== Public section ==================================

    public DataInput GetDataInputById(string id) => _adapterInputs.FirstOrDefault(a => string.Equals(id, a._id, StringComparison.CurrentCultureIgnoreCase));

    public void CreateDataSources() {
      this.ClearDataSources();
      List<Data.DataInput> globalInputs = new List<Data.DataInput>();
      foreach (Data.DataInput input in this._adapterInputs) {
        globalInputs.Add(input);
      }
      globalInputs.Add(new Data.DataInput("adapter", null, this._dataAdapter, null));
      globalInputs.Add(new Data.DataInput("timeinterval", null, this._timeInterval, null));
      foreach (UIPane pane in this._panes) {
        pane.CreateDataSources(globalInputs);
      }
    }
    public void ClearDataSources() {
      this._lastTimerID = -1;
      foreach (UIPane pane in this._panes) {
        pane.ClearDataSources();
      }
    }

    public void UpdateData(int timerID) {
      if (this._lastTimerID < timerID) {
        this._lastTimerID = timerID;
        foreach (UIPane pane in this._panes) {
          pane.UpdateData(timerID);
        }
      }
    }

    public List<DateTime> GetDateArray() {
      foreach (UIPane pane in this._panes) {
        foreach (UIIndicator ind in pane.Indicators) {
          List<DateTime> x = ind._dataInd.GetDateArray();
          if (x != null) {
            //            this._dates = x;
            return x;
          }
        }
      }
      return null;
    }

    public override string ToString() {
      return this._description;
    }
    // ============================== Private section ==============================


  }
}
