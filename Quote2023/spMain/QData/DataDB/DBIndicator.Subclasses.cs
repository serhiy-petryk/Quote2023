using System;
using System.Drawing;
using System.ComponentModel;
using System.Collections.Generic;
//using System.Runtime.Serialization;

namespace spMain.QData.DataDB {

  public partial class DBIndicator : cs.IPG_AdjustProperties {
    // ============================= Subcalss DBIndicatorTypeConverter ==============================
    class DBIndicatorTypeConverter : TypeConverter {// need TypeConverter because StandardValuesCollection
      public override bool GetStandardValuesSupported(ITypeDescriptorContext context) {
        return true;
      }
      public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) {
        return new StandardValuesCollection(DBIndicator._dataIndicatorList);
      }
      public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
        if (sourceType == typeof(string)) return true;
        return base.CanConvertFrom(context, sourceType);
      }
      public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value) {
        if (value is string) {
          string s = (string)value;
          if (string.IsNullOrEmpty(s)) return null;
          foreach (DBIndicator ind in DBIndicator._dataIndicatorList) {
            if (ind._name == s) return ind;
          }
        }
        return base.ConvertFrom(context, culture, value);
      }

      public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType) {
        if (destinationType == typeof(string) && value != null) return value.ToString();
        return base.ConvertTo(context, culture, value, destinationType);
      }
      public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) {
        if (destinationType == typeof(string)) return true;
        return base.CanConvertTo(context, destinationType);
      }
      public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes) {
        if (value is DBIndicator) {
          List<PropertyDescriptor> pdList = new List<PropertyDescriptor>();
          DBIndicator ind = (DBIndicator)value;
          foreach (Data.DataInput di in ind._inputs) {
            pdList.Add(new Data.DataInputPropertyDescriptor(value, di, new Attribute[] { new RefreshPropertiesAttribute(RefreshProperties.All) }));
          }
          return new PropertyDescriptorCollection(pdList.ToArray());
        }
        return base.GetProperties(context, value, attributes);
      }
      public override bool GetPropertiesSupported(ITypeDescriptorContext context) {
        return true;
      }
    }


    // ===========================   SubClass DependedIndicator ==============================
    public class DependedIndicator {

      public string _dependedIndID;
      public List<string> _dependedIndInputs = new List<string>();
      public List<string> _baseIndInputs = new List<string>();
      public List<object> _baseIndValues = new List<object>();

      public DependedIndicator(string s) {
        s = s.Trim().ToLower();
        int k = -1;
        if (s.EndsWith(")")) {
          k = s.LastIndexOf('(');
        }
        if (k == -1) {// dependon without parameters (inputs)
          this._dependedIndID = s;
        }
        else {// dependon with parameters (inputs)
          this._dependedIndID = s.Substring(0, k);
          string[] ss1 = s.Substring(k + 1, s.Length - k - 2).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
          for (int i1 = 0; i1 < ss1.Length; i1++) {
            string[] ss2 = ss1[i1].Split(new string[] { "<-" }, StringSplitOptions.RemoveEmptyEntries);
            this._baseIndValues.Add(null);
            switch (ss2.Length) {
              case 0: break;
              case 1: _dependedIndInputs.Add(ss2[0]); _baseIndInputs.Add(ss2[0]); break;
              case 2:
                string s1 = ss2[1];
                if (s1.StartsWith("\"") && s1.EndsWith("\"")) {
                  
                  this._baseIndValues[this._baseIndValues.Count - 1] = s1.Substring(1, s1.Length-2);
  //                _baseIndInputs.Add(null);
                }
                else {
//                  _baseIndInputs.Add(ss2[1]);
                }
                _dependedIndInputs.Add(ss2[0]); _baseIndInputs.Add(ss2[1]); 
                break;
              default: break;
            }
          }
        }
      }

      public void SetDataInputValue(Data.DataInput di, List<Data.DataInput> inputs) {
        int k1 = this._dependedIndInputs.IndexOf(di._id);
        if (k1 >= 0) {
          if (this._baseIndValues[k1]!=null) {
            di._value=this._baseIndValues[k1];
          }
          else {
           di._value = Data.DataInput.GetDataInputByID(this._baseIndInputs[k1], inputs)._value;
          }
        }
      }

      public Exception Check(DBIndicator parentInd) {
        if (!_allIndicators.ContainsKey(this._dependedIndID)) return new Exception("DBIndicator '" +
          parentInd._id + "' has non-existing depended indicator '" + this._dependedIndID + "'");
        DBIndicator depended = _allIndicators[this._dependedIndID];
        int cnt = 0;
        List<string> checkedDI = new List<string>();
        foreach (Data.DataInput di in depended._inputs) {
          if (di._dataType != typeof(Color)) {
            if (!_dependedIndInputs.Contains(di._id)) {
              return new Exception("DBIndicator '" + parentInd._id + "' for depended indicator '" + this._dependedIndID +
                "' does not have link to '" + di._id + "' DataInput of depended indicator");
            }
            int i1 = _dependedIndInputs.IndexOf(di._id);
            if (this._baseIndValues[i1] != null) {// Constant
              TypeConverter tc = TypeDescriptor.GetConverter(di._dataType);
              object o = tc.ConvertFromString(this._baseIndValues[i1].ToString());
              this._baseIndValues[i1] = o;
            }
            else {
              Data.DataInput _baseDI = Data.DataInput.GetDataInputByID(this._baseIndInputs[i1], parentInd._inputs);
              if (_baseDI == null) {
                Type t = di._dataType;
                TypeConverter tc = TypeDescriptor.GetConverter(t);
                object o = tc.ConvertFromString("Close");
                return new Exception("DBIndicator '" + parentInd._id + "' for depended indicator '" + this._dependedIndID +
                  "'. Base indicator does not have DataInput '" + this._baseIndInputs[i1] + "'");
              }
              if (_baseDI._dataType != di._dataType) {
                return new Exception("DBIndicator '" + parentInd._id + "' for depended indicator '" + this._dependedIndID +
                  "'. Different types for DataInput '" + di._id + "'");
              }
            }
            checkedDI.Add(di._id);
            cnt++;
          }
        }
        foreach (string s in this._dependedIndInputs) {
          if (!checkedDI.Contains(s)) {
            if (s != "timeinterval" && s != "symbol") {
              return new Exception("DBIndicator '" + parentInd._id + "' for depended indicator '" + this._dependedIndID +
                "' and DataInput '" + s + "'. DataInput not found for depended indicator.");
            }
          }
        }
        return null;
      }

    }

  }
}
