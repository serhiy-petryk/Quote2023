using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Design;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;

namespace spMain.QData.UI {

  public class ComplexColorEditor : UITypeEditor {
    public override bool GetPaintValueSupported(ITypeDescriptorContext context) {
      return true;
    }

    public override void PaintValue(PaintValueEventArgs e) {
      if (e.Value != null) {
        ComplexColor complexColor = (ComplexColor)e.Value;
        Color[] colors = complexColor.ColorList;
        double iwidth = Convert.ToDouble(e.Bounds.Width) / colors.Length;
        Brush brush = null;
        try {
          GraphicsState state = e.Graphics.Save();
          for (int i = 0; i < colors.Length; i++) {
            Rectangle r = new Rectangle(e.Bounds.X + Convert.ToInt32(iwidth * i), e.Bounds.Y, Convert.ToInt32(iwidth), e.Bounds.Height);
            if (i == colors.Length - 1) {// last color
              r.Width = e.Bounds.Width - r.X;
            }
            brush = new SolidBrush(colors[i]);
            e.Graphics.FillRectangle(brush, r);
            brush.Dispose();
          }
          e.Graphics.Restore(state);
        }
        finally {
          if (brush != null) brush.Dispose();
        }
      }
    }
  }
}


