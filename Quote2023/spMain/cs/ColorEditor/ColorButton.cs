using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace spMain.csColorEditor {
  public partial class ColorButton : UserControl {
    // ==========================  Static section ==========================
    const int _margin = 2;
    public static readonly Font _fontActive = new Font("Microsoft Sans Serif", 8, FontStyle.Bold);
    public static readonly Font _fontNormal = new Font("Microsoft Sans Serif", 8, FontStyle.Regular);
    static TypeConverter _tc = TypeDescriptor.GetConverter(typeof(Color));

    public static string _GetColorName(Color color) {
      return (string)_tc.ConvertTo(color, typeof(string));
    }

    public static int _GetK_RGB(Color color) {
      double kR = 1.5;
      double kG = 2.5;
      double kB = 0.5;
      return Convert.ToInt32((kR * color.R + kG * color.G + kB * color.B) / (kR + kB + kG));

      //      return Convert.ToInt32((1.5 * Convert.ToDouble(color.R) + 2.5 * Convert.ToDouble(color.G) + 0.5 * Convert.ToDouble(color.B)) / (1.5 + 2.5 + 0.5));
    }

    // =============================================================
    bool _isActive = false;
    bool _isShowColorName = true;
    Color _value;

    public ColorButton() {
      InitializeComponent();
    }

    // ====================  Properties ===========================
    public Color _Value {
      get { return this._value; }
      set { 
        this._value = value;
        this.UpdateProperties();
      }
    }

    public bool _IsActive {
      get { return this._isActive; }
      set {
        this._isActive = value;
        this.UpdateProperties();
      }
    }

    public bool _IsShowColorName {
      get { return this._isShowColorName; }
      set {
        this._isShowColorName = value;
        this.UpdateProperties();
      }
    }

    // ======================== Private section  ===================================
    protected override void OnPaint(PaintEventArgs e) {
      if (this._isActive) {
      }
      string text = _GetColorName(this._value);
      if (this._isShowColorName) {
        Rectangle r = e.ClipRectangle;
        Rectangle r1 = new Rectangle(r.X + _margin, r.Y + _margin, r.Width - 2 * _margin, r.Height - 2 * _margin);
        Font font = this._isActive ? _fontActive : _fontNormal;
        Size size = TextRenderer.MeasureText(text, font);
        Rectangle r2 = new Rectangle((r.X + r.Width + 1) / 2 - size.Width / 2, (r.Y + r.Height + 1) / 2 - size.Height / 2, size.Width, size.Height);
        e.Graphics.DrawString(text, font, new SolidBrush(this.ForeColor), r2);
      }
    }

    protected override void OnPaintBackground(PaintEventArgs e) {
//      base.OnPaintBackground(e);
  //    return;
      Rectangle r = e.ClipRectangle;
      e.Graphics.FillRectangle(new SolidBrush(this.Parent.BackColor), r);
      Rectangle r1 = new Rectangle(r.X + _margin, r.Y + _margin, r.Width - 2 * _margin, r.Height - 2 * _margin);
      e.Graphics.FillRectangle(new SolidBrush(this.BackColor),r1);
      if (this._isActive) {
        using (Pen pen=new Pen(new SolidBrush(Color.Black),1f)) {
          pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
          e.Graphics.DrawRectangle(pen, r.X, r.Y, r.Width-1, r.Height-1);
        }
      }
    }

/*    private void ColorButton_Click(object sender, EventArgs e) {
      this._isActive = !this._isActive;
//      this.SetBorder();
      this.Invalidate();
    }*/

    void UpdateProperties() {
      /*      if (this._value != Color.Transparent) {
              this.BackColor = this._value;
            }*/
      this.BackColor = this._value;
      this.Font = this._isActive ? _fontActive : _fontNormal;

      if (this._isShowColorName) {
        this.Text = _GetColorName(this._Value);
        int k = _GetK_RGB(this._value);
        this.ForeColor = k <= 140 ? Color.White : Color.Black;
      }
      else {
        this.Text = "";
      }
    }

    private void ColorButton_Click(object sender, EventArgs e) {
      this._IsActive = !this._IsActive;
      this.Invalidate();
    }

  }
}
