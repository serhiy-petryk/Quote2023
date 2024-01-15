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
    public class Polygon2003_Minute : Data.DataAdapter
    {
        public override bool IsStream => false;
        public override string CheckDataInputs(List<spMain.QData.Data.DataInput> inputs) => null;
        public override spMain.QData.Common.TimeInterval BaseTimeInterval => new spMain.QData.Common.TimeInterval("1m");

        private static List<(DateTime, DateTime, string)> m_ZipDataFiles;
        private static Dictionary<(string, DateTime), Quote> m_Corrections;

        private static void Init()
        {
            if (m_ZipDataFiles == null)
            {
                var files = Directory.GetFiles(Settings.MinutePolygon2003DataFolder, "*_20??????.zip");
                var dateAndFiles = files.Select(a => (GetDateOfFile(a), a)).OrderBy(a=>a.Item1).ToArray();
                m_ZipDataFiles = new List<(DateTime, DateTime, string)>();
                if (dateAndFiles.Length==0) return;

                m_ZipDataFiles.Add((new DateTime(2003, 9, 1), dateAndFiles[0].Item1, dateAndFiles[0].Item2));
                for (var k = 1; k < dateAndFiles.Length; k++)
                    m_ZipDataFiles.Add((dateAndFiles[k - 1].Item1, dateAndFiles[k].Item1, dateAndFiles[k].Item2));

                m_Corrections = new Dictionary<(string, DateTime), Quote>();
                files = Directory.GetFiles(Settings.MinutePolygon2003DataFolder, "Corrections*.csv");
                foreach (var file in files)
                {
                    var lines = File.ReadAllLines(file);
                    for (var k = 1; k < lines.Length; k++)
                    {
                        var ss = lines[k].Split(',');
                        var symbol = ss[0].Trim();
                        var date = DateTime.ParseExact(ss[1].Trim(), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
                        var open = double.Parse(ss[2].Trim(), CultureInfo.InvariantCulture);
                        var high = double.Parse(ss[3].Trim(), CultureInfo.InvariantCulture);
                        var low = double.Parse(ss[4].Trim(), CultureInfo.InvariantCulture);
                        var close = double.Parse(ss[5].Trim(), CultureInfo.InvariantCulture);
                        var volume = double.Parse(ss[6].Trim(), CultureInfo.InvariantCulture);
                        var key = (symbol, date);
                        if (!m_Corrections.ContainsKey(key))
                            m_Corrections.Add(key,
                                new Quote
                                {
                                    Date = date, Open = open, High = high, Low = low, Close = close, Volume = volume
                                });
                    }
                }
                {
                    
                }

            }

            DateTime GetDateOfFile(string filename)
            {
                var ss1 = Path.GetFileNameWithoutExtension(filename).Split('_');
                return DateTime.ParseExact(ss1[ss1.Length - 1], "yyyyMMdd", CultureInfo.InvariantCulture);
            }
        }

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
            Init();
            var startDate = endDate.AddDays(-days + 1);
            var zipFileNames = m_ZipDataFiles.Where(a => a.Item1 < endDate && a.Item2 >= startDate).Select(a => a.Item3)
                .OrderBy(a => a).ToArray();
            var tempData = new Dictionary<DateTime, Quote>();

            foreach (var aa in m_Corrections.Where(a => string.Equals(a.Key.Item1, symbol, StringComparison.InvariantCultureIgnoreCase) &&
                a.Key.Item2 >= startDate && a.Key.Item2.Date <= endDate && (!showOnlyTradingHours || General.IsInMarketTime(a.Value.Date))))
                tempData.Add(aa.Value.Date, aa.Value);

            foreach (var zipFileName in zipFileNames.OrderBy(a=>a))
                using (var zip = ZipFile.Open(zipFileName, ZipArchiveMode.Read))
                {
                    var entry = zip.Entries.FirstOrDefault(a =>
                        a.Name.Contains("_" + symbol + "_", StringComparison.InvariantCultureIgnoreCase));
                    if (entry != null)
                    {
                        var o = Newtonsoft.Json.JsonConvert
                            .DeserializeObject<Quote2023.Models.MinutePolygon.cMinuteRoot>(
                                entry.GetContentOfZipEntry());
                        if (o.resultsCount == 0) continue;

                        foreach (var item in o.results.Where(a => a.DateTime >= startDate && a.DateTime.Date <= endDate &&
                          (!showOnlyTradingHours || General.IsInMarketTime(a.DateTime))).OrderBy(a => a.DateTime))
                        {
                            if (!tempData.ContainsKey(item.DateTime))
                                tempData.Add(item.DateTime,
                                    new Quote
                                    {
                                        Date = item.DateTime, Open = item.Open, High = item.High, Low = item.Low,
                                        Close = item.Close, Volume = item.Volume
                                    });
                        }
                    }
                }

            data.AddRange(tempData.Values.OrderBy(a=>a.Date));
        }
    }
}
