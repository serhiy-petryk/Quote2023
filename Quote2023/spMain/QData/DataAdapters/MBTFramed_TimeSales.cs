using System;
using System.Diagnostics;
using System.Linq;
using System.Data.OleDb;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using spMain.QData.DataFormat;

namespace spMain.QData.DataAdapters {

  [Serializable]
  class MBTFramed_TimeSales : Data.DataAdapter {
    public const string dbFileTemplate = csIni.pathData + @"DBQ\mdb.ts\{0}.ts.mbthttp.mdb";

    public override bool IsStream {
      get { return false; }
    }

    public override string CheckDataInputs(List<spMain.QData.Data.DataInput> inputs) {
      return null;
    }

    public override spMain.QData.Common.TimeInterval BaseTimeInterval {
      get { return new spMain.QData.Common.TimeInterval(1); }
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
      //      ArrayList data = new ArrayList();
      List<Quote> data = new List<Quote>();

      //      TestData(date);

      while (date <= endDate) {
        LoadData(symbol, date, data);
        date = date.AddDays(1);
      }
      newDataOffset = data.Count;
      data.RemoveRange(0, lastDataOffset);
      return data;
    }

    void LoadData(string symbol, DateTime date, List<Quote> data) {
      string fn = String.Format(dbFileTemplate, MBT.C.GetDateFileID(date, "w"));
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
          List<DataFormat.MbtTickHttp> aa = (List<DataFormat.MbtTickHttp>)chunk.GetData(bytes);

          DateTime lastDT = DateTime.MinValue;
          double open = double.NaN;
          double high = double.NaN;
          double low = double.NaN;
          double close = double.NaN;
          long volume = 0;

          foreach (DataFormat.MbtTickHttp tick in aa) {
            if (!(tick._condition == 0 || tick._condition == 10 || tick._condition == 29 || tick._condition == 54)) continue;
            if (tick._date != lastDT) {
              if (lastDT != DateTime.MinValue) data.Add(new Quote(lastDT, open, high, low, close, volume));
              lastDT = tick._date;
              open = tick._price;
              high = tick._price;
              low = tick._price;
              close = tick._price;
              volume = tick._volume;
            }
            else {
              if (high < tick._price) high = tick._price;
              if (low > tick._price) low = tick._price;
              close = tick._price;
              volume += tick._volume;
            }
          }
          if (lastDT != DateTime.MinValue) data.Add(new Quote(lastDT, open, high, low, close, volume));
        }
      }
    }


    void TestData(DateTime date) {
      Stopwatch sw = new Stopwatch();
      sw.Start();
      int cnt = 0;
      int recs = 0;
      string fn = String.Format(dbFileTemplate, MBT.C.GetDateFileID(date, "w"));
      if (File.Exists(fn)) {
        byte[] bytes = null;
        using (OleDbConnection conn = MBT.C.GetConnection(fn)) {
          using (OleDbCommand cmd = new OleDbCommand("Select symbol, date, [data] from [data] where [version]=1", conn)) {
            conn.Open();
            using (OleDbDataReader reader = cmd.ExecuteReader()) {
              while (reader.Read()) {
                string symbol = reader.GetString(0);
                DateTime dt1 = reader.GetDateTime(1);
                cnt++;
                bytes = (byte[])reader.GetValue(2);
                MBT.QDChunk chunk = new MBT.QDChunk();
                List<DataFormat.MbtTickHttp> aa = (List<DataFormat.MbtTickHttp>)chunk.GetData(bytes);
                recs += aa.Count;

                /*                DateTime lastDT = DateTime.MinValue;
                                Data.Quote quote = null;
                                List<Data.Quote> qq = new List<spMain.QData.Data.Quote>();
                                double open = double.NaN;
                                double high = double.NaN;
                                double low = double.NaN;
                                double close = double.NaN;
                                long volume = 0;

                                foreach (DataFormat.MbtTickHttp tick in aa) {
                                  if (tick._date != lastDT) {
                                    if (lastDT != DateTime.MinValue) qq.Add(new Data.Quote(lastDT, open, high, low, close, volume));
                                    lastDT = tick._date;
                                    open = tick._price;
                                    high = tick._price;
                                    low = tick._price;
                                    close = tick._price;
                                    volume = tick._volume;
                                  }
                                  else {
                                    if (high < tick._price) high = tick._price;
                                    if (low > tick._price) low = tick._price;
                                    close = tick._price;
                                    volume += tick._volume;
                                  }
                                }
                                if (lastDT != DateTime.MinValue) qq.Add(new Data.Quote(lastDT, open, high, low, close, volume));*/

                /*                IEnumerable<IGrouping<long, DataFormat.MbtTickHttp>> aa1 = Enumerable.GroupBy<DataFormat.MbtTickHttp, long>(aa, delegate(DataFormat.MbtTickHttp o) { return o._date.Ticks / 10000 / 1000; });
                                IEnumerable<IGrouping<long, DataFormat.MbtTickHttp>> aa2 = Enumerable.OrderBy<IGrouping<long, DataFormat.MbtTickHttp>, long>(aa1, delegate(IGrouping<long, DataFormat.MbtTickHttp> o) { return o.Key; });
                                IGrouping<long, DataFormat.MbtTickHttp>[] aa3 = Enumerable.ToArray(aa2);
                /*                foreach (IEnumerable<DataFormat.MbtTickHttp> oo in aa3) {
                                  IEnumerable<DataFormat.MbtTickHttp> aa4 = Enumerable.OrderBy<DataFormat.MbtTickHttp, int>(oo, delegate(DataFormat.MbtTickHttp o) { return o._no; });
                                  DateTime dt = DateTime.MinValue;
                                  double open = double.NaN;
                                  double high = double.NaN;
                                  double low = double.NaN;
                                  double close = double.NaN;
                                  long volume = 0;
                                  foreach (DataFormat.MbtTickHttp tick in aa4) {
                                    if (dt == DateTime.MinValue) {
                                      dt = tick._date;
                                    }
                                    if (!Double.IsNaN(tick._price)) {
                                      if (double.IsNaN(open)) open = tick._price;
                                      if (double.IsNaN(high) || high < tick._price) high = tick._price;
                                      if (double.IsNaN(low) || low > tick._price) low = tick._price;
                                      close = tick._price;
                                      volume += tick._volume;
                                    }
                                  }
                                }*/



              }
            }
            conn.Close();
          }
        }
      }
      sw.Stop();
      double d1 = sw.Elapsed.TotalMilliseconds;
    }


  }
}
