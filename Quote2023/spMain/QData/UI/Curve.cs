using System;
using System.Drawing;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ZedGraph;

namespace spMain.QData.UI {

  [Serializable]
  [cs.PG_IsLastStructureNodeAttribute(true)]
  [Editor(typeof(CurveEditor), typeof(System.Drawing.Design.UITypeEditor))]
  public class Curve : cs.IPG_AdjustProperties, ISerializable {

    #region IPG_AdjustProperties Members

    public void AdjustProperties(Dictionary<string, PropertyDescriptor> propertyList, ITypeDescriptorContext context) {
      cs.PGPropertyDescriptor pd_curveStyle = (cs.PGPropertyDescriptor)propertyList["CurveStyle"];
      pd_curveStyle._attrList = new spMain.cs.PG_ListAttribute(this._possibleCurveStyles, false);// not sorting 

      ((cs.PGPropertyDescriptor)propertyList["DecimalPlaces"])._attrBrowsable = new BrowsableAttribute(this.IsDecimalPlacesApplicable());

      cs.PGPropertyDescriptor pd_bar_color = (cs.PGPropertyDescriptor)propertyList["BarColor"];
      pd_bar_color._attrList = new spMain.cs.PG_ListAttribute(DataDB.DBIndicator._complexColorList, false);

      cs.PGPropertyDescriptor pd_symbol_color = (cs.PGPropertyDescriptor)propertyList["SymbolColor"];
      pd_symbol_color._attrList = pd_bar_color._attrList;

      ((cs.PGPropertyDescriptor)propertyList["BarColor"])._attrBrowsable = new BrowsableAttribute(this.IsBarColorApplicable());
      ((cs.PGPropertyDescriptor)propertyList["LineColor"])._attrBrowsable = new BrowsableAttribute(this.IsLineColorApplicable());
      ((cs.PGPropertyDescriptor)propertyList["SymbolType"])._attrBrowsable = new BrowsableAttribute(this.IsSymbolTypeApplicable());
      ((cs.PGPropertyDescriptor)propertyList["SymbolColor"])._attrBrowsable = new BrowsableAttribute(this.IsSymbolColorApplicable());
    }

    #endregion

    List<Common.General.CurveStyle> _possibleCurveStyles;
    string _labelTemplate;
    int _dp; // decimal places
    Common.General.CurveStyle _curveStyle;
    ComplexColor _barColor;
    Color _lineColor;// Color for line (solid, dot..)
    SymbolType _symbolType;
    ComplexColor _symbolColor;
    
    // ========================== Constructor ======================
    public Curve(DataDB.DBIndicator baseIndicator) {
      this._labelTemplate = baseIndicator._labelTemplate;
      this._dp = baseIndicator._dp;
      this._possibleCurveStyles = baseIndicator._possibleCurveStyles;
      this._curveStyle = baseIndicator._defCurveStyle;
      this._barColor = (ComplexColor)csFastSerializer.Utils.GetClone( DataDB.DBIndicator._defaultComplexColor);
      this._lineColor = baseIndicator._defColor;
      this._symbolType = SymbolType.None;
      this._symbolColor = (ComplexColor)csFastSerializer.Utils.GetClone( DataDB.DBIndicator._defaultComplexColor);
    }
    public Curve(SerializationInfo info, StreamingContext ctxt) {
      object[] x = csFastSerializer.Utils.Deserialize(info);
      if (x != null) {
        this._possibleCurveStyles = (List<Common.General.CurveStyle>)x[0];
        this._labelTemplate = (string)x[1];
        this._dp = (int)x[2];
        this._curveStyle = (Common.General.CurveStyle)x[3];
        this._barColor = (ComplexColor)x[4];
        Color c = this._barColor.ColorList[0];
        this._lineColor = (Color)x[5];
        this._symbolType = (SymbolType)x[6];
        this._symbolColor = (ComplexColor)x[7];
      }
    }
    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext ctxt) {
      csFastSerializer.Utils.Serialize(info, new object[] { this._possibleCurveStyles, this._labelTemplate, this._dp, 
        this._curveStyle, this._barColor, this._lineColor, this._symbolType, this._symbolColor});
      Color c = this._barColor.ColorList[0];
    }

    // ==================   Properties ========================================

    public string LabelTemplate {
      get { return this._labelTemplate; }
      set { this._labelTemplate = value; }
    }

    public int DecimalPlaces {
      get { return this._dp; }
      set { this._dp = value; }
    }

    [RefreshProperties( RefreshProperties.All)]
    public Common.General.CurveStyle CurveStyle {
      get { return this._curveStyle; }
      set {
        if (this._curveStyle != value) {
          this._curveStyle = value;
        }
      }
    }

    [RefreshProperties(RefreshProperties.All)]
    public ComplexColor BarColor {
      get { return this._barColor; }
      set {
        if (this._barColor != value) {
          this._barColor = (ComplexColor)csFastSerializer.Utils.GetClone( value);
        }
      }
    }

    public Color LineColor {
      get { return this._lineColor; }
      set { this._lineColor = value; }
    }

    [RefreshProperties(RefreshProperties.All)]
    [Editor(typeof(CurveEditor), typeof(System.Drawing.Design.UITypeEditor))]
    public SymbolType SymbolType {
      get { return this._symbolType; }
      set { this._symbolType = value; }
    }

    public ComplexColor SymbolColor {
      get { return this._symbolColor; }
      set {
        if (this._symbolColor != value) {
          this._symbolColor = (ComplexColor)csFastSerializer.Utils.GetClone(value);
        }
      }
    }

    // =====================   Public section =============================
    public bool IsDecimalPlacesApplicable() {
      return this._curveStyle != spMain.QData.Common.General.CurveStyle.OHLC &&
        this._curveStyle != spMain.QData.Common.General.CurveStyle.Candle;
    }
    public bool IsBarColorApplicable() {
      return this._curveStyle == Common.General.CurveStyle.OHLC || this._curveStyle == Common.General.CurveStyle.Bar;
    }
    public bool IsLineApplicable() {
      return this._curveStyle != Common.General.CurveStyle.Candle && !this.IsBarColorApplicable();
    }
    public bool IsLineColorApplicable() {
      return this.IsLineApplicable() && this.CurveStyle != Common.General.CurveStyle.None;
    }
    public bool IsSymbolTypeApplicable() {
      return this.IsLineApplicable();
    }
    public bool IsSymbolColorApplicable() {
      return this.IsSymbolTypeApplicable() && this.SymbolType != SymbolType.None;
    }

    // =====================   Private section =============================
  }
}
