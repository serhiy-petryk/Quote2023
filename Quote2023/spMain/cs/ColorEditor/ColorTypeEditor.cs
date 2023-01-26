using System;
using System.Drawing;
using System.Drawing.Design;
using System.Collections.Generic;
using System.Windows.Forms.Design;

namespace spMain.csColorEditor {
  // ================================== Class TimeIntervalTypeEditor ================================

  class ColorTypeEditor : System.Drawing.Design.UITypeEditor {
    public ColorTypeEditor() {
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

      if (value != null && (value.GetType() != typeof(Color)))
        return value;

      // Uses the IWindowsFormsEditorService to display a drop-down UI in the Properties window.
      IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
      if (edSvc != null) {
        ColorFormEditor editor = new ColorFormEditor(edSvc, (Color)value);
        edSvc.DropDownControl(editor);

        return editor._value;

      }
      return value;
    }

    // Indicates whether the UITypeEditor supports painting a representation of a property's value.
    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    public override bool GetPaintValueSupported(System.ComponentModel.ITypeDescriptorContext context) {
      return true;
    }

    public override void PaintValue(PaintValueEventArgs e) {
      Color c = (Color)e.Value;
      e.Graphics.FillRectangle(new SolidBrush(c), e.Bounds);
//      base.PaintValue(e);
    }
  }
}
