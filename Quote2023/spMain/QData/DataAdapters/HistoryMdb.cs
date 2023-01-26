using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using spMain.QData.DataFormat;

namespace spMain.QData.DataAdapters {
  
  [Serializable]
  class HistoryMdb : Data.DataAdapter {
//    public const string filename = csIni.pathData + @"hData.mdb";
    public const string filename = csIni.pathData + @"\DBQ\Day\Day.Yahoo.mdb";
    

    public override bool IsStream {
      get { return false; }
    }

    public override string CheckDataInputs(List<spMain.QData.Data.DataInput> inputs) {
      StringBuilder sb = new StringBuilder();
      if (String.IsNullOrEmpty(inputs[0]._value.ToString())) sb.Append("Invalid symbol" + Environment.NewLine);
      DateTime d1 = (DateTime)inputs[1]._value;
      DateTime d2 = (DateTime)inputs[2]._value;
      if (d1.Year < 1900 || (d1.Year - 1) > DateTime.Now.Year) sb.Append("Invalid Start date" + Environment.NewLine);
      if (d2.Year < 1900 || (d2.Year - 1) > DateTime.Now.Year) sb.Append("Invalid End date" + Environment.NewLine);
      if (d1 > d2) sb.Append("Start Date can not be greater than End Date"+ Environment.NewLine);
      return sb.ToString();
    }

    public override List<Data.DataInput> GetInputs() {
      List<Data.DataInput> x = new List<Data.DataInput>();
      x.Add(new spMain.QData.Data.DataInput("symbol", "Symbol", "AA", null));
      x.Add(new spMain.QData.Data.DataInput("startdate", "Start Date", DateTime.Today.AddYears(-2).AddDays(-1), null));
      x.Add(new spMain.QData.Data.DataInput("enddate", "End Date", DateTime.Today.AddDays(-1), null));
//      x.Add(new spMain.QData.Data.DataInput("startdate", "Start Date", new DateTime(2002, 1, 1), null));
  //    x.Add(new spMain.QData.Data.DataInput("enddate", "End Date", new DateTime(2007, 12, 31), null));
      return x;
    }

    public override spMain.QData.Common.TimeInterval BaseTimeInterval {
      get { return new spMain.QData.Common.TimeInterval(-1); }
    }

    public override IList GetData(List<object> inputs, int lastDataOffset, out int newDataOffset) {
      string symbol = (string)inputs[0];
      DateTime startDate = (DateTime)inputs[1];
      DateTime endDate = (DateTime)inputs[2];
      System.Data.OleDb.OleDbParameter p1 = new System.Data.OleDb.OleDbParameter("symbol", symbol);
      System.Data.OleDb.OleDbParameter p2 = new System.Data.OleDb.OleDbParameter("startdate",startDate);
      System.Data.OleDb.OleDbParameter p3 = new System.Data.OleDb.OleDbParameter("enddate", endDate);
      DataTable dt= csUtilsData.GetDataTable("select * from data where symbol=@symbol and date between @startdate and @enddate order by date",
        filename, new System.Data.OleDb.OleDbParameter[] { p1, p2, p3 });

//      ArrayList data = new ArrayList();
      List<Quote> data = new List<Quote>();

      foreach (DataRow dr in dt.Rows) {
        Quote quote = new Quote(csUtilsData.GetDateFromDataField(dr["date"], DateTime.MinValue), 
          csUtilsData.GetDoubleFromDataField(dr["open"],4),
          csUtilsData.GetDoubleFromDataField(dr["high"],4),
          csUtilsData.GetDoubleFromDataField(dr["low"],4),
          csUtilsData.GetDoubleFromDataField(dr["close"],4),
          csUtilsData.GetDoubleFromDataField(dr["volume"],0));
        data.Add(quote);
      }

      newDataOffset = data.Count;
      data.RemoveRange(0, lastDataOffset);
      return data;
    }


  }
}
