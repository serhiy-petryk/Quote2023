using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace spMain.QData.DataDB {

  [Serializable]
  [TypeConverter(typeof(DBIndicatorTypeConverter))]
  public partial class DBIndicator : cs.IPG_AdjustProperties, ISerializable {// need ISerializable when _inputs for DBInd are serialized

    // ======================  Override section ===========================
/*    public static bool operator ==(DBIndicator ind1, DBIndicator ind2) {
      if (DBIndicator.ReferenceEquals(ind1, null) && DBIndicator.ReferenceEquals(ind2, null)) return true;
      if (DBIndicator.ReferenceEquals(ind1, null) || DBIndicator.ReferenceEquals(ind2, null)) return false;
      return (ind1.GetHashCode() == ind2.GetHashCode());
    }
    public static bool operator !=(DBIndicator ind1, DBIndicator ind2) {
      return !(ind1 == ind2);
    }
    public override bool Equals(object obj) {
      if (obj == null || !(obj is DBIndicator)) return false;
      return this._hash == ((DBIndicator)obj)._hash;
    }*/
    public override int GetHashCode() { return this._hash; }
    public override string ToString() { return this._name; }

    // ==================================   Static section =================================
    static Dictionary<string, DBIndicator> _allIndicators; // all db inds (id, ind)
    public static List<DBIndicator> _dataIndicatorList = new List<DBIndicator>();
    public static List<UI.ComplexColor> _complexColorList = new List<UI.ComplexColor>();
    public static UI.ComplexColor _defaultComplexColor = null;
    static IList _curveStyles = (IList)Enum.GetValues(typeof(Common.General.CurveStyle));

    public static DBIndicator GetDBIndByID(string indID) {
      if (String.IsNullOrEmpty(indID)) return null;
      string s = indID.Trim().ToLower();
      if (_allIndicators.ContainsKey(s)) return _allIndicators[indID.Trim().ToLower()];
      else return null;
    }

    static DBIndicator() {
      LoadFromMDB();
    }

    static void LoadFromMDB() {
      if (_allIndicators == null) {
        DataTable dt = csUtilsData.GetDataTable("select * from dbindicators where valid<>0", Settings.pathMdbBaseFileName);
        _allIndicators = new Dictionary<string, DBIndicator>();
        for (int i = 0; i < dt.Rows.Count; i++) {
          DBIndicator item = new DBIndicator(dt.Rows[i]);
          _allIndicators.Add(item._id, item);
        }
        // Check dependeds
        foreach (DBIndicator ind in _allIndicators.Values) {
          foreach (DependedIndicator depInd in ind._dependedInds) {
            Exception ex = depInd.Check(ind);
            if (ex != null) 
              ind._errors.Add(ex);
          }
        }
        // Finally
        StringBuilder sb = new StringBuilder();
        foreach (DBIndicator ind in _allIndicators.Values) {
          if (ind._errors.Count == 0) {
            switch (ind._valueDataType.Name.ToLower()) {
              case "double":
              case "quote":
                _dataIndicatorList.Add(ind);
                break;
              case "color":
                UI.ComplexColor cc = new spMain.QData.UI.ComplexColor(ind);
                _complexColorList.Add(cc);
                // Define default color indicator (no input, no depends)
                if (_defaultComplexColor == null && ind._dependedInds.Count == 0) {// no depended ind
                  bool flag = true;
                  for (int i = 0; i < ind._inputs.Count && flag; i++) {
                    flag = (ind._inputs[i]._dataType == typeof(Color));
                  }
                  if (flag) _defaultComplexColor = cc;
                }
                break;
            }
          }
          else {
            foreach (Exception ex in ind._errors) 
              sb.Append("Indicator " + ind._id + ": " + ex.Message + "\n");
          }
        }
        if (sb.Length > 0) MessageBox.Show(sb.ToString(), "DBIndicators errors", MessageBoxButtons.OK, MessageBoxIcon.Error);
        if (_defaultComplexColor == null && _complexColorList.Count > 0) {
          _defaultComplexColor = _complexColorList[0];
        }
      }
    }


    // ==================================== Object =================================
    static int _cntHash = 1;

    public readonly string _id;
    int _hash = -1;
    public readonly string _name;
    public List<Data.DataInput> _inputs = new List<Data.DataInput>();
    public int _dp;
    public readonly Type _valueDataType;
    public readonly List<Exception> _errors = new List<Exception>();
    public List<Common.General.CurveStyle> _possibleCurveStyles = new List<Common.General.CurveStyle>();
    public Common.General.CurveStyle _defCurveStyle = Common.General.CurveStyle.Solid;
    public Color _defColor = Color.Black;
    public List<DependedIndicator> _dependedInds = new List<DependedIndicator>();
    public string _labelTemplate;

    // =======================  Constructor ========================
    public DBIndicator(DataRow dr) {
//      try {
        this._hash = _cntHash++;
        this._id = dr["id"].ToString().ToLower();
        this._name = dr["name"].ToString();
        if (String.IsNullOrEmpty(this._name)) this._name = this._id;// correct the name of indicator
        // Value data type
        this._valueDataType =  Settings.typeXref[dr["ValueDataType"].ToString().ToLower()];
        this._dp = csUtilsData.GetIntFromDataField(dr["dp"], 0);
        this.SetPossibleCurveStyles();
        this.SetDefCurveStyleAndColor(dr["defCurve"].ToString());
        this._labelTemplate = dr["LabelTemplate"].ToString();
        // dependsOn
        string s = dr["dependOn"].ToString();
        if (!String.IsNullOrEmpty(s)) {// get dependOn elements
          string[] ss = s.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
          for (int i = 0; i < ss.Length; i++) {// parse each dependon element
            this._dependedInds.Add(new DependedIndicator(ss[i]));
          }
        }
        // inputs
        OleDbParameter par = new OleDbParameter("@indID", dr["id"]);
        DataTable dt = csUtilsData.GetDataTable("select * from DBIndicatorInputs where indID=@indID order by sortID", Settings.pathMdbBaseFileName, new OleDbParameter[] { par });
        foreach (DataRow dr1 in dt.Rows) {
          string id1 = dr1["inputID"].ToString().ToLower();
          string prompt = dr1["prompt"].ToString();
          string sDefValue = dr1["defValue"].ToString();
          string sDescription = dr1["description"].ToString();
          object defValue = null;
          Type type = Settings.typeXref[dr1["type"].ToString().ToLower()];
          TypeConverter tc = TypeDescriptor.GetConverter(type);
          if (tc == null) {
            defValue = Convert.ChangeType(sDefValue, type);
          }
          else {
            try {
              defValue = tc.ConvertFromInvariantString(sDefValue);
            }
            catch (Exception ex) {// If null for class == not throw
              if (!type.IsClass) throw;
            }
          }
          this._inputs.Add(new Data.DataInput(id1, prompt, type, defValue, sDescription));
        }
      /*}
      catch (Exception ex) {
        this._errors.Add(ex);
      }*/
    }

    public DBIndicator(SerializationInfo info, StreamingContext ctxt) {
      object[] x = csFastSerializer.Utils.Deserialize(info);
      if (x != null) {
        this._id = (string)x[0];
        this._inputs = (List<Data.DataInput>)x[1];
        DBIndicator ind = GetDBIndByID(this._id);
        if (ind != null) {
          this._defColor = ind._defColor;
          this._hash = ind._hash;
          this._defCurveStyle = ind._defCurveStyle;
          this._dependedInds = ind._dependedInds;
          this._dp = ind._dp;
          this._errors = ind._errors;
          this._name = ind._name;
          this._possibleCurveStyles = ind._possibleCurveStyles;
          this._valueDataType = ind._valueDataType;
          this._labelTemplate = ind._labelTemplate;
        }
        Check();
      }
    }

    public void Check()
    {
      var ind = GetDBIndByID(this._id);
      Data.DataInput.CheckInputs(this._inputs, ind._inputs);// normalize inputs
    }
    public void GetObjectData(SerializationInfo info, StreamingContext context) {
      csFastSerializer.Utils.Serialize(info, new object[] { this._id, this._inputs });
    }

    // ===========================  Private section =============================
    void SetDefCurveStyleAndColor(string s) {
      if (String.IsNullOrEmpty(s)) return;
      int i = s.IndexOf("(");
      s = s.Trim();
      string s1, s2;
      if (i > 0) {
        s1 = s.Substring(0, i);
        s2 = s.Substring(i + 1);
        if (s2.EndsWith(")")) s2 = s2.Substring(0, s2.Length - 1);
      }
      else {
        s1 = s;
        s2 = "";
      }
      try {
        if (!String.IsNullOrEmpty(s1)) this._defCurveStyle = (Common.General.CurveStyle)Enum.Parse(typeof(Common.General.CurveStyle), s1);
      }
      catch (Exception ex) {
        this._errors.Add(ex);
      }


      try {
        if (!String.IsNullOrEmpty(s2)) this._defColor = Color.FromName(s2);
      }
      catch (Exception ex) { this._errors.Add(ex); }

    }

    void SetPossibleCurveStyles() {
      switch (this._valueDataType.Name.ToLower()) {
        case "color": break;
        case "quote":
          this._possibleCurveStyles = new List<spMain.QData.Common.General.CurveStyle>((IEnumerable<Common.General.CurveStyle>)_curveStyles);
          break;
        default:
          for (int i = 2; i < _curveStyles.Count; i++) this._possibleCurveStyles.Add((Common.General.CurveStyle)_curveStyles[i]);
          break;
      }
    }

    #region IPG_AdjustProperties Members

    public void AdjustProperties(Dictionary<string, PropertyDescriptor> propertyList, ITypeDescriptorContext context) {
      //      throw new Exception("The method or operation is not implemented.");
    }

    #endregion
  }
}
