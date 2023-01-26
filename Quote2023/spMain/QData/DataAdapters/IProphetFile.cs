using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using spMain.QData.DataFormat;

namespace spMain.QData.DataAdapters {
  
  [Serializable]
  class IProphetFile: Data.DataAdapter {
    public const string pathIntradayData = csIni.pathData + @"Loader\Prophet\IParsed\Y{y}\D{d}\PIP#{s}.txt";

    public override bool IsStream {
      get { return false; }
    }

    public override string CheckDataInputs(List<spMain.QData.Data.DataInput> inputs) {
      return null;
    }

    public override spMain.QData.Common.TimeInterval BaseTimeInterval {
      get { return new spMain.QData.Common.TimeInterval(60); }
    }

    public override List<Data.DataInput> GetInputs() {
      List<Data.DataInput> x = new List<Data.DataInput>();
      x.Add(new spMain.QData.Data.DataInput("symbol", "Symbol", "RNO", null));
      x.Add(new spMain.QData.Data.DataInput("date", "Date", new DateTime(2007, 4, 20), "Last date to show"));
      x.Add(new spMain.QData.Data.DataInput("days", "Number of days", 10, 
        "Number of working days before last date. This parameter and 'Date' parameter define the start date."));
      return x;
    }

    public override IList GetData(List<object> inputs, int lastDataOffset, out int newDataOffset) {
      string symbol = (string)inputs[0];
      DateTime endDate = (DateTime)inputs[1];
      int days = (int)inputs[2];
      List<string> files = GetFileList(symbol, endDate, days);
//      ArrayList data = new ArrayList();
      List<Quote> data = new List<Quote>();

      for (int i = files.Count - 1; i >= 0; i--) {
        LoadFromFile(files[i], data);
      }
      newDataOffset = data.Count;
      data.RemoveRange(0, lastDataOffset);
      return data;
    }

    List<string> GetFileList(string symbol, DateTime endDate, int days) {
      List<string> files = new List<string>();
      DateTime dt = endDate;
      int cnt = 0;
      int missed = 0;
      while (cnt <days && missed<10) {
        string fn = csUtilsFile.GetFileName(pathIntradayData, symbol, dt);
        if (File.Exists(fn)) {
          files.Add(fn);
          cnt++;
          missed = 0;
        }
        else {
          missed++;
        }
        dt = dt.AddDays(-1);
      }
      return files;
    }

        void LoadFromFile(string filename, List<Quote> data) {
      string[] ss = File.ReadAllLines(filename);
      string[] ss1 = ss[0].Split('|');
      if (ss[0].ToLower().StartsWith("intradayprophet") && ss1.Length > 2) {
        string symbol = ss1[1].Trim().ToUpper();
        DateTime date = DateTime.Parse(ss1[2], csIni.fiDateUA);
        if (ss[ss.Length - 1].ToLower() == "end") {
          //        Common.Quote[] quotes = new Common.Quote[ss.Length - 3];
          for (int i = 2; i < ss.Length - 1; i++) {
            ss1 = ss[i].Split('|');
            if (ss1.Length == 6) {
              TimeSpan ts = TimeSpan.Parse(ss1[0]);
              DateTime qDate = date + ts;
              double open = double.Parse(ss1[1], csIni.fiNumberUS);
              double high = double.Parse(ss1[2], csIni.fiNumberUS);
              double low = double.Parse(ss1[3], csIni.fiNumberUS);
              double close = double.Parse(ss1[4], csIni.fiNumberUS);
              double volume = double.Parse(ss1[5], csIni.fiNumberUS);
              Quote quote = new Quote(qDate, open, high, low, close, volume);
              data.Add(quote);
//              data.Data_Change(data.Data_GetCount(), new object[] { quote });
            }
            else {
              throw new Exception("Invalid line in ProphetIntraday file " + filename +
                Environment.NewLine + "Line: " + ss[i]);
            }
          }
          //          return quotes;
        }
        else {
          throw new Exception("Can not file end file marker in ProphetIntraday file " + filename);
        }
      }
      else {
        throw new Exception("Invalid file header in ProphetIntraday file " + filename);
      }
    }


  }
}
