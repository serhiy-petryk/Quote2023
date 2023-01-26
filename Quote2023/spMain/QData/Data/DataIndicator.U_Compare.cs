using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using spMain.QData.DataFormat;

namespace spMain.QData.Data {
  public partial class DataIndicator {
    // !!! Сделать Compare индикатор плавающим, то есть его можно выделить и перемещать по оси Y(при этом будет изменяться _dataFactor)
    void Upd_Compare() {
      //    this._childInds[0] == данные для базового символа (FramedQuote)
      //    this._childInds[1] == данные для символа, который сравнивают (FramedQuote)

      if (Double.IsNaN(this._dataFactor)) {
        // Define k;
        for (int i1 = 0; i1 < this._childInds[0]._dates.Count && Double.IsNaN(this._dataFactor); i1++) {
          double close1 = ((Quote)this._childInds[0]._data[i1]).close;
          DateTime date1 = this._childInds[0]._dates[i1];
          if (!double.IsNaN(close1)) {
            for (int i2 = 0; i2 < this._childInds[1]._dates.Count; i2++) {
              if (this._childInds[1]._dates[i2] == date1) {
                double close2 = ((Quote)this._childInds[1]._data[i2]).close;
                if (Double.IsNaN(close2)) break;
                else {
                  this._dataFactor = close1 / close2;
                  break;
                }
              }
              if (this._childInds[1]._dates[i1] > date1) break;
            }//for (int i2 = 0; 
          }//if (!double.IsNaN(close1))
        }//for (int i1 = 0;
      }


      int i2Cnt = this._tempVars.Count == 0 ? 0 : (int)this._tempVars[0];
      for (int i1 = this._startItemNo; i1 < this._endItemNo; i1++) {
        DateTime date1 = this._childInds[0]._dates[i1];
        for (int i2 = i2Cnt; i2 < this._childInds[1]._dates.Count; i2++) {
          if (this._childInds[1]._dates[i2] == date1) {
            double lastClose = ((Quote)this._childInds[1]._data[i2]).close;
            this._data[i1] = lastClose;
            i2Cnt = i2 + 1;
            break;
          }
          else if (this._childInds[1]._dates[i2] > date1) {
            this._data[i1] = double.NaN;
            i2Cnt = i2;
            break;
          }
        }
      }

      // Save the i2cnt
      if (this._tempVars.Count == 0) this._tempVars.Add(i2Cnt-1);
      else this._tempVars[0] = i2Cnt-1;

    }

    void Upd_CompareOld() {
      //    this._childInds[0] == данные для базового символа (FramedQuote)
      //    this._childInds[1] == данные для символа, который сравнивают (FramedQuote)

      // Define base close
      double firstClose = Double.NaN;
      if (this._tempVars.Count == 0) {
        this._tempVars.Add(firstClose);
      }
      else {
        firstClose = (double)this._tempVars[0];
      }

      double baseClose = double.NaN;
      if (this._childInds[1]._data.Count > 0) {
        for (int i1 = 0; i1 < this._childInds[0]._data.Count; i1++) {
          baseClose = ((Quote)this._childInds[0]._data[i1]).close;
          if (!double.IsNaN(baseClose)) {
            break;
          }
        }
      }

      int compareCnt;
      if (baseClose != firstClose) {// baseClose changed
        //2010-08-07        this._myVars[0] = baseClose; // Save last baseClose
        this._data.Clear();// clear my data
        compareCnt = 0;// reset count
        this._lastUpdateOffset = 0; // clear _lastUpdateOffset
      }
      else {// baseClose is the same
        compareCnt = this._childInds[1]._lastUpdateOffset; // set count
      }

      while (this._data.Count < this._childInds[0]._data.Count) {// Add blank data array elements
        this._data.Add(double.NaN);
      }

      for (int i = compareCnt; i < this._childInds[1]._data.Count; i++) {
        Quote q1 = (Quote)this._childInds[1]._data[i];// данные для символа, который сравнивают (FramedQuote)
        for (int j = 0; j < this._childInds[0]._data.Count; j++) {// поиск одинаковых дат
          if (q1.date == ((Quote)this._childInds[0]._data[j]).date) {// даты совпадают 
            this._data[j] = q1.close / baseClose;//установить значение индикатора
            if (this._lastUpdateOffset > j) this._lastUpdateOffset = j;// set _lastUpdateOffset == минимальный индекс измененного значения
            break;
          }
        }
      }

    }

  }
}
