using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace TestPropertyGrid.cs.Support {
  class UIObjectEditor : System.Drawing.Design.UITypeEditor {
    public UIObjectEditor() {
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

      MessageBox.Show("Object editor does not ready.");
      return value;

      if (value.GetType() != typeof(Color))
        return value;

      // Uses the IWindowsFormsEditorService to display a 
      // drop-down UI in the Properties window.
      IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
      if (edSvc != null) {
        //          ColorEditorControl editor = new ColorEditorControl((Color)value);
        //        edSvc.DropDownControl(editor);

        // Return the value in the appropraite data format.
        if (value.GetType() == typeof(Color))
          //            return editor.color;
          return null;
      }
      return value;
    }

    // Draws a representation of the property's value.
    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    public override void PaintValue(System.Drawing.Design.PaintValueEventArgs e) {
      if (e.Value != null) {
        Font f = new Font("Arial", 8);
        e.Graphics.DrawString(e.Value.ToString(), f, new SolidBrush(Color.Blue), new PointF(1f, 1f));
      }
    }

    // Indicates whether the UITypeEditor supports painting a 
    // representation of a property's value.
    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    public override bool GetPaintValueSupported(System.ComponentModel.ITypeDescriptorContext context) {
      return false;
    }

  }
}
