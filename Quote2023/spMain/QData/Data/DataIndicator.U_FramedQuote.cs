using System;
using System.Collections;
using System.Collections.Generic;
using spMain.QData.DataFormat;

namespace spMain.QData.Data {
  public partial class DataIndicator {

    void Upd_FramedQuote() {
      DataAdapter adapter=null;
      List<object> adapterInputs=null;
      Common.TimeInterval ti=null;
      int lastDataOffset = 0;

      if (this._tempVars.Count == 0) {
        adapterInputs = new List<object>();
        for (int i = 0; i < this._globalInputs.Count - 2; i++) {
          adapterInputs.Add(this._globalInputs[i]._value);
        }
        adapter = (DataAdapter)this._globalInputs[this._globalInputs.Count - 2]._value;
        ti = (Common.TimeInterval)this._globalInputs[this._globalInputs.Count - 1]._value;
        this._tempVars.AddRange(new object[] {adapter, adapterInputs, ti, 0 });
      }
      else {
        adapter = (DataAdapter) this._tempVars[0];
        adapterInputs = (List<object>)this._tempVars[1];
        ti = (Common.TimeInterval)this._tempVars[2];
        lastDataOffset = (int)this._tempVars[3];
      }

      int newDataOffset;
      IList data = adapter.GetData(adapterInputs, lastDataOffset, out newDataOffset);
      this._tempVars[3] = newDataOffset;

      if (data.Count > 0) {
        Quote lastQuote = (this._data.Count==0? null : (Quote)this._data[this._data.Count-1]);
        DateTime? lastDate = null;
        if (lastQuote != null) lastDate = lastQuote.date;
        switch (data[0].GetType().Name.ToLower()) {
          case "quote":
            for (int i = 0; i < (newDataOffset - lastDataOffset); i++) {
              Quote q = (Quote)data[i];
              int lastDateCount = this._dates.Count;
              Common.XScale.AddDateToDateArray(this._dates, q.date, ti);
//              Common.UtilsFrame.AddDateToDateArray(this._dates, q.date, ti);
              if (this._dates.Count == lastDateCount) {//              SameTimeFrame:
                ((Quote)this._data[this._data.Count - 1]).MergeQuotes(q);
              }
              else {
                for (int i1 = this._data.Count; i1 < this._dates.Count -1; i1++) {
                  this._data.Add(new Quote(this._dates[i1], double.NaN, double.NaN, double.NaN, double.NaN,0));// missing items
                }
                this._data.Add(new Quote(this._dates[this._dates.Count-1], q.open, q.high, q.low, q.close, q.volume));// last quote
              }
            }
            break;
        }
        // Update datetime array
        if (this._dates != null) {
          for (int i = this._dates.Count; i < this._data.Count; i++) {
            this._dates.Add(((Quote)this._data[i]).date);
          }
        }
      }

    }

  }
}
