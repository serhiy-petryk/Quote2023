using System;
using System.Data.OleDb;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using spMain.QData.DataFormat;

namespace spMain.QData.DataAdapters {

  [Serializable]
  class MBT_Minute : Data.DataAdapter {
    public const string dbFileTemplate = csIni.pathData + @"DBQ\mdb.min\{0}.min.mbthttp.mdb";

    public override bool IsStream {
      get { return false; }
    }

    public override string CheckDataInputs(List<spMain.QData.Data.DataInput> inputs) {
      return null;
    }

    public override spMain.QData.Common.TimeInterval BaseTimeInterval {
      get { return new spMain.QData.Common.TimeInterval("1m"); }
    }

    public override List<Data.DataInput> GetInputs() {

      List<Data.DataInput> x = new List<Data.DataInput>();
      x.Add(new spMain.QData.Data.DataInput("symbol", "Symbol", "AA", null));
      x.Add(new spMain.QData.Data.DataInput("date", "Date", new DateTime(2011, 9, 28), "Last date to show"));
      x.Add(new spMain.QData.Data.DataInput("days", "Number of days", 1,
        "Number of working days before last date. This parameter and 'Date' parameter define the start date."));
      return x;
    }

    public override IList GetData(List<object> inputs, int lastDataOffset, out int newDataOffset) {
      string symbol = (string)inputs[0];
      DateTime endDate = (DateTime)inputs[1];
      int days = (int)inputs[2];
      DateTime date = endDate.AddDays(-(days - 1));
      List<Quote> data = new List<Quote>();

      while (date <= endDate) {
        LoadData(symbol, date, data);
        date = date.AddDays(1);
      }
      newDataOffset = data.Count;
      data.RemoveRange(0, lastDataOffset);
      return data;
    }

    void LoadData(string symbol, DateTime date, List<Quote> data) {
      string fn = String.Format(dbFileTemplate, MBT.C.GetDateFileID(date, "m"));
      if (File.Exists(fn)) {
        byte[] bytes = null;
        using (OleDbConnection conn = MBT.C.GetConnection(fn)) {
          using (OleDbCommand cmd = new OleDbCommand("Select [data] from [data] where [symbol]=@symbol and [date]=@date and [version]=1", conn)) {
            OleDbParameter p1 = new OleDbParameter("@symbol", symbol);
            OleDbParameter p2 = new OleDbParameter("@date", date);
            cmd.Parameters.AddRange(new OleDbParameter[] { p1, p2 });
            conn.Open();
            bytes = (byte[])cmd.ExecuteScalar();
            conn.Close();
          }
        }
        if (bytes != null) {
          MBT.QDChunk chunk = new MBT.QDChunk();
          List<DataFormat.Quote> aa = (List<DataFormat.Quote>)chunk.GetData(bytes);
          data.AddRange(aa);
        }
      }
    }

  }
}
