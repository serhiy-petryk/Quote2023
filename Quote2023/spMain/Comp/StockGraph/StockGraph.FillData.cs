using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ZedGraph;

namespace spMain.Comp {
  public partial class StockGraph : ZedGraphControl {

    const int _sparesTicks = 100;
    int _iDataMinOffset = -1;
    int _iDataMaxOffset = -1;
    int _iDataFillCount = 0;

    // ============================   First version ================================
    void FillData(bool forceFlag) {
      if (this._uiGraph != null && this._dates!=null) {
//        this._iDataMinOffset = Math.Max(0, Convert.ToInt32(this.GraphPane.XAxis.Scale.Min - 0.5 - 1));// 1 запасной один тик
  //      this._iDataMaxOffset = Math.Min(this._dates.Count - 1, Convert.ToInt32(this.GraphPane.XAxis.Scale.Max - 0.5 + 1)); // 1 запасной один тик
        int iMin = Math.Max(0, Convert.ToInt32(this.GraphPane.XAxis.Scale.Min - 0.5 - 1));// 1 запасной один тик
        int iMax = Math.Min(this._dates.Count - 1, Convert.ToInt32(this.GraphPane.XAxis.Scale.Max - 0.5+1)); // 1 запасной один тик
        bool flag = forceFlag;

//        this.ScrollGrace = (iMax - iMin) / (Convert.ToInt32(this.GraphPane.Rect.Width / 2));

        // New data range
        if (!flag) flag = iMin < this._iDataMinOffset || iMax > this._iDataMaxOffset;
        // Zoom changed more than 2 times and many datas loaded into Graph
        if (!flag && iMin != iMax) flag = (this._iDataMaxOffset - this._iDataMinOffset) / (iMax - iMin) > 1 && 
          (this._iDataMaxOffset - this._iDataMinOffset) > 400;
        if (flag) {
          this._iDataMinOffset = Math.Max(0, iMin - _sparesTicks);
          this._iDataMaxOffset = Math.Min(this._dates.Count - 1, iMax + _sparesTicks);
          foreach (QData.UI.UIPane pane in this._uiGraph.Panes) {
            foreach (QData.UI.UIIndicator ind in pane.Indicators) {
              ind.CurveFillData(this._iDataMinOffset, this._iDataMaxOffset, Convert.ToInt32(this.GraphPane.Rect.Width));
            }
          }
          _iDataFillCount++;
//          this.ParentForm.Text = "Fill Count: " + this._iDataFillCount;
        }
//        this.ScrollGrace = 2;
      }
    }

    // ============================ Second version ================================
    // ƒл€ быстрого вывода большого количества данных
    // ¬вести параметр  и делать контроль загрузки данных по _iDataMin(Max)Offset/_iDataQuotesPerTick
    // «агрузка данных ind.CurveFillData(_iDataMinOffset, _iDataMaxOffset, _iDataQuotesPerTick);
    // ѕри загрузке делаетс€ DataMerge. ƒл€ одного пиксел€ нужно 2 тика: один-миним.значение, второй-максимальное 
    // ѕри этом на PairPoint цепл€етс€ объект, который нужен при выводе значений на экран 



  }
}
