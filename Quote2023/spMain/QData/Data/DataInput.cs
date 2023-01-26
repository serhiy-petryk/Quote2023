using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace spMain.QData.Data {

  [Serializable]
  public class DataInput : ICloneable, ISerializable {

    static int _cnt = 0;
    // ===========================  Static section ==========================
    public static DataInput GetDataInputByID(string id, List<DataInput> inputs) {
      string s = id.ToLower();
      foreach (DataInput di in inputs) {
        if (di._id == s) return di;
      }
      return null;
    }
    public static void CheckInputs(List<DataInput> secondaryInputs, List<DataInput> sampleInputs) {
      int i = 0;
      while (i < secondaryInputs.Count) {
        if (Data.DataInput.GetDataInputByID(secondaryInputs[i]._id, sampleInputs) == null) {
          secondaryInputs.RemoveAt(i);
        }
        else i++;
      }
      foreach (Data.DataInput di in sampleInputs) {
        if (Data.DataInput.GetDataInputByID(di._id, secondaryInputs) == null) {
          secondaryInputs.Add((Data.DataInput)di.Clone());
        }
      }
    }
    public static string GetDataInputListDescription(List<DataInput> inputs, string delimeter, string prefix, string suffix) {
      if (inputs == null || inputs.Count == 0) return "";
      StringBuilder sb = new StringBuilder(prefix + csUtils.StringFromObject(inputs[0]._value));
      for (int i = 1; i < inputs.Count; i++) {
        sb.Append(delimeter + csUtils.StringFromObject(inputs[i]._value));
      }
      return sb.ToString() + suffix;
    }

    public int _tmpCnt = _cnt++;
    public readonly Type _dataType;
    public readonly string _id;
    public readonly string _prompt;
    public object _value;
    public readonly string _description;

    public DataInput(string id, string prompt, object defValue, string description) {
      this._dataType = defValue.GetType();
      this._id = id; this._prompt = prompt; this._description = description;
      this._value = defValue;
    }

    public DataInput(string id, string prompt, Type dataType, object defValue, string description) {
      this._dataType = dataType;
      this._id = id; this._prompt = prompt; this._description = description;
      if (defValue != null && dataType != defValue.GetType())// different datatype
        if (dataType.IsClass && String.IsNullOrEmpty(defValue.ToString())) {// blank string for object: value=null
          this._value = null;
        }
        else {
          throw new Exception("DataInput constructor. Type of default value does not equals to type of DataInput");
        }
      else this._value = defValue;
    }

    // ===========================   Constructor ============================
    public DataInput() { }

    public DataInput(SerializationInfo info, StreamingContext ctxt) {
      object[] x = csFastSerializer.Utils.Deserialize(info);
      if (x != null) {
        this._dataType = (Type)x[0];
        this._id = (string)x[1];
        this._prompt = (string)x[2];
        int offset = (x.Length == 6 ? 0 : -1);
        this._value = x[4+offset];
        this._description = (string)x[5+offset];
      }
    }
    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext ctxt) {
      csFastSerializer.Utils.Serialize(info, new object[] { this._dataType, this._id, this._prompt, this._value, this._description });
    }

    // =============================  Public section =======================================
    public override string ToString() {
      return _tmpCnt+"\t"+ _id + "\t" + this._prompt + "\t" +  (this._value == null ? "" : this._value.ToString());
    }

    public object Clone() {
      return new DataInput(this._id, this._prompt, this._dataType, this._value, this._description);
/* 2010-09-02     if (this._value != null && this._value is UI.ComplexColor) {
      }
      if (this._dataType.IsSerializable && this._dataType.IsClass && this._value != null && this._dataType != typeof(string)) {
        return new DataInput(this._id, this._propmpt, this._dataType, csFastSerializer.Utils.GetClone(this._value), this._description);
      }
      if (this._value != null && this._value is ICloneable) {
        return new DataInput(this._id, this._propmpt, this._dataType, ((ICloneable)this._value).Clone(), this._description);
      }
      else {
        return new DataInput(this._id, this._propmpt, this._dataType, this._value, this._description);
      }*/
    }
  }
}
