using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace spMain.csColorEditor {
  public partial class ColorFormEditor : UserControl {

    // ======================  Static section(utilities)  =======================
    static Color[] GetColorList() {
      Color[] cc1 = new Color[] { 
				Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF), Color.FromArgb(0xFF, 0xFF, 0xE0, 0xE0), Color.FromArgb(0xFF, 0xFF, 0xE8, 0xE0), Color.FromArgb(0xFF, 0xFF, 0xFF, 0xE0), Color.FromArgb(0xFF, 0xE0, 0xFF, 0xE0), Color.FromArgb(0xFF, 0xE0, 0xFF, 0xFF), Color.FromArgb(0xFF, 0xE0, 0xE0, 0xFF), Color.FromArgb(0xFF, 0xFF, 0xE0, 0xFF),
				Color.FromArgb(0xFF, 0xF0, 0xF0, 0xF0), Color.FromArgb(0xFF, 0xFF, 0xC0, 0xC0), Color.FromArgb(0xFF, 0xFF, 0xE0, 0xC0), Color.FromArgb(0xFF, 0xFF, 0xFF, 0xC0), Color.FromArgb(0xFF, 0xC0, 0xFF, 0xC0), Color.FromArgb(0xFF, 0xC0, 0xFF, 0xFF), Color.FromArgb(0xFF, 0xC0, 0xC0, 0xFF), Color.FromArgb(0xFF, 0xFF, 0xC0, 0xFF),
				Color.FromArgb(0xFF, 0xE0, 0xE0, 0xE0), Color.FromArgb(0xFF, 0xFF, 0x80, 0x80), Color.FromArgb(0xFF, 0xFF, 0xE0, 0x80), Color.FromArgb(0xFF, 0xFF, 0xFF, 0x80), Color.FromArgb(0xFF, 0x80, 0xFF, 0x80), Color.FromArgb(0xFF, 0x80, 0xFF, 0xFF), Color.FromArgb(0xFF, 0x80, 0x80, 0xFF), Color.FromArgb(0xFF, 0xFF, 0x80, 0xFF), 
				Color.FromArgb(0xFF, 0xC0, 0xC0, 0xC0), Color.FromArgb(0xFF, 0xFF, 0x00, 0x00), Color.FromArgb(0xFF, 0xFF, 0x80, 0x00), 				Color.FromArgb(0xFF, 0xFF, 0xFF, 0x00), Color.FromArgb(0xFF, 0x00, 0xFF, 0x00), Color.FromArgb(0xFF, 0x00, 0xFF, 0xFF), Color.FromArgb(0xFF, 0x00, 0x00, 0xFF), Color.FromArgb(0xFF, 0xFF, 0x00, 0xFF),
				Color.FromArgb(0xFF, 0x80, 0x80, 0x80), Color.FromArgb(0xFF, 0xC0, 0x00, 0x00), Color.FromArgb(0xFF, 0xC0, 0x80, 0x00), Color.FromArgb(0xFF, 0xC0, 0xC0, 0x00), Color.FromArgb(0xFF, 0x00, 0xC0, 0x00), Color.FromArgb(0xFF, 0x00, 0xC0, 0xC0), Color.FromArgb(0xFF, 0x00, 0x00, 0xC0), Color.FromArgb(0xFF, 0xC0, 0x00, 0xC0),
				Color.FromArgb(0xFF, 0x40, 0x40, 0x40), Color.FromArgb(0xFF, 0x80, 0x00, 0x00), Color.FromArgb(0xFF, 0x80, 0x40, 0x00), Color.FromArgb(0xFF, 0x80, 0x80, 0x00), Color.FromArgb(0xFF, 0x00, 0x80, 0x00), Color.FromArgb(0xFF, 0x00, 0x80, 0x80), Color.FromArgb(0xFF, 0x00, 0x00, 0x80), Color.FromArgb(0xFF, 0x80, 0x00, 0x80),
				Color.FromArgb(0xFF, 0x00, 0x00, 0x00), Color.FromArgb(0xFF, 0x40, 0x00, 0x00), Color.FromArgb(0xFF, 0x80, 0x40, 0x40), Color.FromArgb(0xFF, 0x40, 0x40, 0x00), Color.FromArgb(0xFF, 0x00, 0x40, 0x00), Color.FromArgb(0xFF, 0x00, 0x40, 0x40), Color.FromArgb(0xFF, 0x00, 0x00, 0x40), Color.FromArgb(0xFF, 0x40, 0x00, 0x40)
			};
      Color[] cc2 = GetStandardColorList();
      List<Color> cc = new List<Color>();
      for (int i = 0; i < cc1.Length; i++) {
        for (int k = 0; k < cc2.Length; k++) {
          if (cc1[i].R == cc2[k].R && cc1[i].G == cc2[k].G && cc1[i].B == cc2[k].B && cc2[k] != Color.Transparent) {
            cc1[i] = cc2[k];
            break;
          }
        }
      }
      return cc1;
    }

    static Color[] GetStandardColorList() {
      object[] oo = GetConstants(typeof(Color), typeof(Color));
      Array.Sort(oo, MyColorComparer);
      Color[] cc = new Color[oo.Length];
      for (int i = 0; i < oo.Length; i++) cc[i] = (Color)oo[i];
      return cc;
    }

    private static int MyColorComparer(object o1, object o2) {
      int i1 = ColorButton._GetK_RGB((Color)o1);
      int i2 = ColorButton._GetK_RGB((Color)o2);
      return (i1 > i2 ? 1 : (i1 == i2 ? 0 : -1));
    }

    public static object[] GetConstants(Type enumType, Type constantType) {
      MethodAttributes attributes = MethodAttributes.Static | MethodAttributes.Public;
      PropertyInfo[] properties = enumType.GetProperties();
      ArrayList list = new ArrayList();
      for (int i = 0; i < properties.Length; i++) {
        PropertyInfo info = properties[i];
        if (info.PropertyType == constantType) {
          MethodInfo getMethod = info.GetGetMethod();
          if ((getMethod != null) && ((getMethod.Attributes & attributes) == attributes)) {
            object[] index = null;
            list.Add(info.GetValue(null, index));
          }
        }
      }
      return list.ToArray();
    }

    //=================================================================
    const int _colorsInRow = 8;
    const int _controlSize = 24;
    const bool _isShowColorName = false;
    const int _labelGap=0;

    public Color _value=Color.Empty;
    public IWindowsFormsEditorService _wfes;

    public ColorFormEditor() {
      InitializeComponent();
      if (!this.DesignMode) this.Init();
    }

    public ColorFormEditor(IWindowsFormsEditorService wfes, Color value) {
      InitializeComponent();
      this._wfes = wfes;
      this._value = value;
      Init();
      SetActiveColor(value);
    }

    void SetActiveColor(Color activeColor) {
      foreach (Control c in this.Controls) {
        if (c is ColorButton) {
          ColorButton cb = (ColorButton)c;
          cb._IsActive = (cb._Value == activeColor);
        }
      }
    }

    void Init() {
      this.BackColor = (this.Parent == null? SystemColors.Control :this.Parent.BackColor);
      int maxX = _controlSize;
      int maxY = _controlSize;
      int xOffset = 0;
      int yOffset = 0;
      Color[] colors = GetColorList();
      int width = _controlSize;
      if (_isShowColorName) {
        for (int i = 0; i < colors.Length; i++) {
          string s = ColorButton._GetColorName(colors[i]);
          width = Math.Max(width, TextRenderer.MeasureText(s, ColorButton._fontActive).Width);
        }
        width += 14;
      }
      int row = 0;
      int col = 0;
      for (int i = 0; i < colors.Length; i++) {
        if (col == _colorsInRow) {
          row++; col = 0;
        }
        ColorButton cb = new ColorButton();
        cb._Value = colors[i];
        cb.Width = width;
        cb.Height = _controlSize;
        cb._IsShowColorName = _isShowColorName;
        int x = col * (width + _labelGap) + xOffset;
        int y = row * (cb.Height + _labelGap) + yOffset;
        cb.Location = new Point(x, y);
        cb.Parent = this;
        cb.Click += new EventHandler(cb_Click);
        this.toolTip1.SetToolTip(cb, ColorButton._GetColorName(cb._Value));
        maxX = Math.Max(cb.Location.X + cb.Size.Width, maxX);
        maxY = Math.Max(cb.Location.Y + cb.Size.Height, maxY);
        col++;
      }
      this.Size = new Size(maxX, maxY);
    }

    void cb_Click(object sender, EventArgs e) {
      ColorButton cb = (ColorButton)sender;
      this._value = cb._Value;
      if (this._wfes != null) this._wfes.CloseDropDown();

    }
  }
}
