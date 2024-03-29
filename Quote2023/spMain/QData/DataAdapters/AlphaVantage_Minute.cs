using System;
using System.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Compression;
using spMain.QData.Common;
using spMain.QData.DataFormat;
using spMain.Quote2023.Helpers;

namespace spMain.QData.DataAdapters
{

    [Serializable]
    public class AlphaVantage_Minute : Data.DataAdapter
    {
        public override bool IsStream => false;

        public override string CheckDataInputs(List<spMain.QData.Data.DataInput> inputs) => null;

        public override spMain.QData.Common.TimeInterval BaseTimeInterval => new spMain.QData.Common.TimeInterval("1m");

        public override List<Data.DataInput> GetInputs() =>
            new List<Data.DataInput>
            {
                new spMain.QData.Data.DataInput("symbol", "Symbol", "AA", null),
                new spMain.QData.Data.DataInput("date", "Date", new DateTime(2022, 12, 1), "Last date to show"),
                new spMain.QData.Data.DataInput("days", "Number of days", 1,
                    "Number of working days before last date. This parameter and 'Date' parameter define the start date."),
                new spMain.QData.Data.DataInput("showOnlyTradingHours", "Show only trading hours", true, null)
            };

        public override IList GetData(List<object> inputs, int lastDataOffset, out int newDataOffset)
        {
            var symbol = ((string)inputs[0]).Trim().ToUpper();
            var endDate = (DateTime)inputs[1];
            var days = (int)inputs[2];
            // var date = endDate.AddDays(-(days - 1));
            var showOnlyTradingHours = inputs.Count < 4 || (bool) inputs[3];
            var data = new List<Quote>();
            LoadData(symbol, endDate, days, showOnlyTradingHours, data);

            newDataOffset = data.Count;
            data.RemoveRange(0, lastDataOffset);
            return data;
        }

        public void LoadData(string symbol, DateTime endDate, int days, bool showOnlyTradingHours, List<Quote> data)
        {
            // Get valid file names
            var files = new List<string>();
            for (var k = 0; k < days; k++)
            {
                var filename = Settings.MinuteAlphaVantageDataFolder + "MAV_" + endDate.AddDays(-k).ToString("yyyyMMdd") + ".zip";
                if (File.Exists(filename))
                    files.Add(filename);
            }
            if (files.Count == 0) return;

            var alphaSymbol = Quote2023.Models.SymbolsXref.GetSymbolsXref(symbol)?.AlphaVantageSymbol ?? symbol;
            var key = alphaSymbol + "_";
            for (var k = files.Count - 1; k >= 0; k--)
                using (var zip = ZipFile.Open(files[k], ZipArchiveMode.Read))
                    foreach (var item in zip.Entries.Where(a => a.Length > 0 && a.Name.StartsWith(key, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        foreach (var line in item.GetLinesOfZipEntry().Where(a => !a.StartsWith("#")))
                        {
                            var ss = line.Split(',');
                            var date = DateTime.ParseExact(ss[0], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
                                .AddMinutes(-1);
                            var include = !showOnlyTradingHours || General.IsInMarketTime(date);
                            if (include)
                            {
                                var open = Math.Round(double.Parse(ss[1], CultureInfo.InvariantCulture), 4);
                                var high = Math.Round(double.Parse(ss[2], CultureInfo.InvariantCulture), 4);
                                var low = Math.Round(double.Parse(ss[3], CultureInfo.InvariantCulture), 4);
                                var close = Math.Round(double.Parse(ss[4], CultureInfo.InvariantCulture), 4);
                                var volume = Math.Round(double.Parse(ss[5], CultureInfo.InvariantCulture), 4);
                                data.Add(new Quote
                                    {Date = date, Open = open, High = high, Low = low, Close = close, Volume = volume});
                            }
                        }

                        break;
                    }
        }
    }
}
