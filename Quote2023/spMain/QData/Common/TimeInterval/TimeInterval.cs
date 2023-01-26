using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Collections.Generic;

namespace spMain.QData.Common {

  [Serializable]
  [Editor(typeof(TimeIntervalTypeEditor), typeof(UITypeEditor))]
  [TypeConverter(typeof(TimeIntervalConverter))]
  public partial class TimeInterval {
    public int _timeInterval;

    public TimeInterval(int timeInterval) {
      this._timeInterval = timeInterval;
    }

    public TimeInterval(string timeInterval) {
      if (String.IsNullOrEmpty(timeInterval)) {
        throw new ArgumentException("TimeInterval constructor. Parameter 'TimeInterval' can not be blank");
      }
      string str = timeInterval.Trim().ToLower();
      switch (str) {
        case "year":
          this._timeInterval = -4; return;
        case "month":
          this._timeInterval = -3; return;
        case "week": this._timeInterval = -2; return;
        case "day": this._timeInterval = -1; return;
        default:
          if (str.EndsWith("s")) {
            int i;
            if (Int32.TryParse(str.Substring(0, str.Length - 1), out i)) {
              if (i > 0) {
                this._timeInterval = i;
                return;
              }
            }
          }
          else if (str.EndsWith("m")) {// minutes
            int i;
            if (Int32.TryParse(str.Substring(0, str.Length - 1), out i)) {
              if (i > 0) {
                this._timeInterval = i * 60;
                return;
              }
            }
          }
          else {
            int i;
            if (Int32.TryParse(str, out i)) {
              if (i > 0) {
                this._timeInterval = i;
                return;
              }
            }
          }
          break;
      }
      throw new ArgumentException("TimeInterval constructor. Invalid TimeInterval: " + str);
    }

    public int GetSecondsInInterval() {
      switch (this._timeInterval) {
        case -1: return 390*60;// day
        case -2: return 390 * 5*60;// week
        case -3: return 390 * 21*60;// month (251 or 252) // checked by hdata.mdb (2002-2007) 
        case -4: return 390 * 21 * 12*60;// year
        default: return this._timeInterval;// Intraday
      }
    }

    public string GetXScaleFormat() {
      switch (this._timeInterval) {
        case -1: return "yyyy-MM-dd";// day
        case -2: return "yyyy-MM-dd";// week
        case -3: return "yyyy-MM";// month
        case -4: return "yyyy";// year
        default :// Intraday
          int reminder1 = this._timeInterval % 3600;// Hours
          int reminder2 = this._timeInterval % 60;// Minutes
          return (reminder1 == 0 ? "HH" :(reminder2 == 0? "HH:mm": "HH:mm:ss"));
      }
    }

    public Size GetXLabelSize(Font xLabelFont) {
      return TextRenderer.MeasureText(GetXScaleFormat(), xLabelFont);
    }

    public double GetXLabelWidth(Font xLabelFont) {
      return Convert.ToDouble(TextRenderer.MeasureText(GetXScaleFormat(), xLabelFont).Width);
    }

    public override string ToString() {
      if (_timeInterval == -1) return "Day";
      else if (_timeInterval == -2) return "Week";
      else if (_timeInterval == -3) return "Month";
      else if (_timeInterval == -4) return "Year";
      else {
        int reminder = this._timeInterval % 60;
        if (reminder == 0) return (this._timeInterval / 60).ToString() + "m";
        else return this._timeInterval.ToString()+"s";
//        return this._timeInterval.ToString();
      }
    }


    // ================================== Class TimeIntervalTypeEditor ================================

    class TimeIntervalTypeEditor : System.Drawing.Design.UITypeEditor {
      public TimeIntervalTypeEditor() {
      }

      // Indicates whether the UITypeEditor provides a form-based (modal) dialog, 
      // drop down dialog, or no UI outside of the properties window.
      [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
      public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context) {
        return UITypeEditorEditStyle.DropDown;
      }

      // Displays the UI for value selection.
      [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
      public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value) {

        if (value !=null && (value.GetType() != typeof(TimeInterval)))
          return value;

        // Uses the IWindowsFormsEditorService to display a drop-down UI in the Properties window.
        IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
        if (edSvc != null) {
          TimeIntervalFormEditor editor = new TimeIntervalFormEditor(edSvc, (TimeInterval)value);
          edSvc.DropDownControl(editor);

          return editor._value;

        }
        return value;
      }

      // Indicates whether the UITypeEditor supports painting a representation of a property's value.
      [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
      public override bool GetPaintValueSupported(System.ComponentModel.ITypeDescriptorContext context) {
        return false;
      }
    }
    //================================================================================================

  }
}
