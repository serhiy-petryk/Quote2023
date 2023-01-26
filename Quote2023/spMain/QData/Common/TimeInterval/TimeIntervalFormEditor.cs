using System;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace spMain.QData.Common {
//  public partial class TimeInterval {
    public partial class TimeIntervalFormEditor : UserControl {

      public TimeInterval _value;
      // При активации формы через WindowsFormsEditorService, возникает событие Click, для кнопки где Checked=true 
      // ClickFlag - служит для разрешения этой проблемы.
      public bool clickFlag = false;
      public IWindowsFormsEditorService _wfes;

      public TimeIntervalFormEditor() {
        InitializeComponent();
        Init();
      }

      public TimeIntervalFormEditor(IWindowsFormsEditorService wfes, TimeInterval value) {
        InitializeComponent();
        this._wfes = wfes;
        _value = value;
        Init();
      }

      void Init() {
        if (this._value == null) this._value = new TimeInterval(5*60);
        this.btnCustom.Checked = false;
        int i = this._value._timeInterval;
        this.btnYear.Checked = (i == -4);
        this.btnMonth.Checked = (i == -3);
        this.btnWeek.Checked = (i == -2);
        this.btnDay.Checked = (i == -1);
        this.btn1s.Checked = (i == 1);
        this.btn2s.Checked = (i == 2);
        this.btn3s.Checked = (i == 3);
        this.btn5s.Checked = (i == 5);
        this.btn10s.Checked = (i == 10);
        this.btn15s.Checked = (i == 15);
        this.btn20s.Checked = (i == 20);
        this.btn30s.Checked = (i == 30);
        this.btn1.Checked = (i == 1*60);
        this.btn2.Checked = (i == 2 * 60);
        this.btn3.Checked = (i == 3 * 60);
        this.btn5.Checked = (i == 5 * 60);
        this.btn10.Checked = (i == 10 * 60);
        this.btn15.Checked = (i == 15 * 60);
        this.btn30.Checked = (i == 30 * 60);
        this.btn60.Checked = (i == 60 * 60);
        this.numCustom.Value = Math.Max(1, i); ;
        if (i == -4 || i == -3 || i == -2 || i == -1 || i == 1 || i == 2 || i == 3 || i == 5 || i == 10 || i == 15 || i == 20 || i == 30
          || i == 1 * 60 || i == 2 * 60 || i == 3 * 60 || i == 5 * 60 || i == 10 * 60 || i == 15 * 60 || i == 30 * 60 || i == 60 * 60) {
          // одна из кнопок имеет checked == приведет к активации onclick, если нет ParentForm
          this.clickFlag = !(this.ParentForm == null);
        }
        else {
          this.clickFlag = true;
        }
      }

      private void btn_Click(object sender, EventArgs e) {
        if (!clickFlag) {// Not click == this is init procedure
          clickFlag = true;
          return;
        }
        if (sender == this.btnYear) this._value._timeInterval = -4;
        else if (sender == this.btnMonth) this._value._timeInterval = -3;
        else if (sender == this.btnWeek) this._value._timeInterval = -2;
        else if (sender == this.btnDay) this._value._timeInterval = -1;
        else if (sender == this.btnCustom) this._value._timeInterval = Convert.ToInt32(this.numCustom.Value);
        else if (sender == this.btn1s) this._value._timeInterval = 1;
        else if (sender == this.btn2s) this._value._timeInterval = 2;
        else if (sender == this.btn3s) this._value._timeInterval = 3;
        else if (sender == this.btn5s) this._value._timeInterval = 5;
        else if (sender == this.btn10s) this._value._timeInterval = 10;
        else if (sender == this.btn15s) this._value._timeInterval = 15;
        else if (sender == this.btn20s) this._value._timeInterval = 20;
        else if (sender == this.btn30s) this._value._timeInterval = 30;
        else if (sender == this.btn1s) this._value._timeInterval = 1 * 60;
        else if (sender == this.btn1) this._value._timeInterval = 1 * 60;
        else if (sender == this.btn2) this._value._timeInterval = 2 * 60;
        else if (sender == this.btn3) this._value._timeInterval = 3 * 60;
        else if (sender == this.btn5) this._value._timeInterval = 5 * 60;
        else if (sender == this.btn10) this._value._timeInterval = 10 * 60;
        else if (sender == this.btn15) this._value._timeInterval = 15 * 60;
        else if (sender == this.btn30) this._value._timeInterval = 30 * 60;
        else if (sender == this.btn60) this._value._timeInterval = 60 * 60;
        if (this._wfes != null) this._wfes.CloseDropDown();
      }
    }

  }
//}
