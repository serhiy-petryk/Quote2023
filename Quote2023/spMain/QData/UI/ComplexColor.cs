using System;
using System.Drawing;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ZedGraph;

namespace spMain.QData.UI {

  [Serializable]
  [Editor(typeof(ComplexColorEditor), typeof(System.Drawing.Design.UITypeEditor))]
  public partial class ComplexColor : cs.IPG_AdjustProperties, ISerializable {

    // ======================  Override section ===========================
/*    public static bool operator ==(ComplexColor ind1, ComplexColor ind2) {
      if (ComplexColor.ReferenceEquals(ind1, null) && ComplexColor.ReferenceEquals(ind2, null)) return true;
      if (ComplexColor.ReferenceEquals(ind1, null) || ComplexColor.ReferenceEquals(ind2, null)) return false;
      return (ind1.GetHashCode() == ind2.GetHashCode());
    }
    public static bool operator !=(ComplexColor ind1, ComplexColor ind2) {
      return !(ind1 == ind2);
    }
    public override int GetHashCode() { return (this._dbInd == null ? -1 : this._dbInd.GetHashCode()); }
    public override string ToString() { return (this._dbInd == null ? null : this._dbInd.ToString()); }
    public override bool Equals(object obj) {
      if (obj == null || !(obj is ComplexColor)) return false;
      return this.GetHashCode()== obj.GetHashCode();
    }*/
    public override int GetHashCode() {// Need to select from object list in property grid
      if (this._hashCode == -2) {// HashCode not inited
        DataDB.DBIndicator ind = DataDB.DBIndicator.GetDBIndByID(this._indID);
        if (ind != null) this._hashCode = ind.GetHashCode();
        else this._hashCode = -1;
      }
      return this._hashCode;
    }
    public override bool Equals(object obj) {// Need to select from object list in property grid
      if (obj == null || !(obj is ComplexColor)) return false;
      return this._indID == ((ComplexColor)obj)._indID;
    }

    // ==============================================================
    #region IPG_AdjustProperties Members

    public void AdjustProperties(Dictionary<string, PropertyDescriptor> propertyList, ITypeDescriptorContext context) {
      propertyList.Clear();
      foreach (Data.DataInput di in this._inputs) {
        Data.DataInputPropertyDescriptor diPD = new Data.DataInputPropertyDescriptor(this, di, 
          new Attribute[] { new RefreshPropertiesAttribute(RefreshProperties.All) });
        propertyList.Add(diPD.Name, diPD);
      }
    }

    #endregion


    int _hashCode=-2;
    string _indID;
    string _name;
    List<Data.DataInput> _inputs=new List<spMain.QData.Data.DataInput>();
    public Data.DataIndicator _dataInd;
    ArrayList _tempVars = null;
//    Color[] _colors;

    // =====================  Constructor ===========================
    public ComplexColor(DataDB.DBIndicator baseIndicator) {
//      this._hashCode = baseIndicator.GetHashCode();
      this._indID = baseIndicator._id;
      this._name = baseIndicator._name;
      foreach (Data.DataInput di in baseIndicator._inputs) {
        _inputs.Add((Data.DataInput)di.Clone());
      }
    }

    public ComplexColor(SerializationInfo info, StreamingContext ctxt) {
      object[] x = csFastSerializer.Utils.Deserialize(info);
      if (x != null) {
        this._indID = (string)x[0];
        this._name = (string)x[1];
        this._inputs = (List<Data.DataInput>)x[2];
      }
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context) {
      csFastSerializer.Utils.Serialize(info, new object[] { this._indID, this._name, this._inputs});
    }

    // ========================  Properties =============================
    [Browsable(false)]
    public Color[] ColorList {
      get {
        List<Color> colors = new List<Color>();
        foreach (Data.DataInput di in this._inputs) {
          if (di._dataType == typeof(Color)) colors.Add((Color)di._value);
        }
        return colors.ToArray();
      }
    }

    // ================================  Public section =============================
    public void CreateDataSources(List<Data.DataInput> globalInputs) {
      List<Data.DataInput> localInputs = new List<Data.DataInput>();
      foreach (Data.DataInput input in this._inputs) {
        if (!(input._value is Color)) {
          localInputs.Add(input);
        }
      }
      this._dataInd = Data.DataManager.GetDataIndicator(this._indID, localInputs, globalInputs, this);
      this._tempVars = null;
//      this._colors = ColorList;
    }
    public void ClearDataSources() {
      if (this._dataInd != null) {
        this._dataInd.UnRegister(this);
      }
    }

    public Fill GetDataColorFill() {
      Color[] colors = this.ColorList;
      if (colors.Length == 1) {
        Fill myFill = new Fill(Color.White, colors[0]);
//        myFill.SecondaryValueGradientColor = Color.White;
        return myFill;
      }
      else {
        List<Color> cc = new List<Color>();
        cc.Add(Color.Empty);
        cc.AddRange(colors);
        Fill myFill = new Fill(cc.ToArray());
        myFill.Type = FillType.GradientByColorValue;
        myFill.SecondaryValueGradientColor = Color.White;
        myFill.RangeMin = 0;
        myFill.RangeMax = colors.Length;
        myFill.Color = Color.Empty;
        return myFill;
      }
    }

    public double GetDataColorValue(ArrayList parentIndData, int dataOffset) {
      switch (this._indID) {
        case "twolevelsofline":
          if (_tempVars == null) {
            this._tempVars = new ArrayList();
            this._tempVars.Add(Data.DataInput.GetDataInputByID("minlevel", this._inputs)._value);
            this._tempVars.Add(Data.DataInput.GetDataInputByID("maxlevel", this._inputs)._value);
          }
          double minLevel = (double)this._tempVars[0];
          double maxLevel = (double)this._tempVars[1];
          if (dataOffset > 0 && dataOffset < parentIndData.Count) {
            double prevValue = (double)parentIndData[dataOffset - 1];
            double thisValue = (double)parentIndData[dataOffset];
            if (prevValue < minLevel && thisValue >= minLevel) return 1;
            if (prevValue > maxLevel && thisValue <= maxLevel) return 2;
          }
          return 0;
        case "singlecolor": return 0;
        default:
          if (dataOffset < this._dataInd._data.Count) {
             return Convert.ToDouble(this._dataInd._data[dataOffset]);
          }
          break;
      }
      return 0;
    }

    public override string ToString() {
      return this._name;
    }


  }
}
