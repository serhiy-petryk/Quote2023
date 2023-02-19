using System;
using System.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using spMain.QData.DataFormat;
using spMain.Quote2023.Helpers;
using spMain.Quote2023.Models;

namespace spMain.QData.DataAdapters
{

    [Serializable]
    class Yahoo_Minute : Data.DataAdapter
    {
        private const string zipFileEntryNameTemplate = @"yMin-{0}";

        public override bool IsStream => false;

        public override string CheckDataInputs(List<spMain.QData.Data.DataInput> inputs) => null;

        public override spMain.QData.Common.TimeInterval BaseTimeInterval => new spMain.QData.Common.TimeInterval("1m");

        public override List<Data.DataInput> GetInputs() =>
            new List<Data.DataInput>
            {
                new spMain.QData.Data.DataInput("symbol", "Symbol", "AA", null),
                new spMain.QData.Data.DataInput("date", "Date", new DateTime(2022, 12, 1), "Last date to show"),
                new spMain.QData.Data.DataInput("days", "Number of days", 1,
                    "Number of working days before last date. This parameter and 'Date' parameter define the start date.")
            };

        public override IList GetData(List<object> inputs, int lastDataOffset, out int newDataOffset)
        {
            var symbol = ((string)inputs[0]).Trim().ToUpper();
            var endDate = (DateTime)inputs[1];
            var days = (int)inputs[2];
            var date = endDate.AddDays(-(days - 1));
            var data = new List<Quote>();
            LoadData(symbol, date, endDate, data);

            newDataOffset = data.Count;
            data.RemoveRange(0, lastDataOffset);
            return data;
        }

        void LoadData(string symbol, DateTime startDate, DateTime endDate, List<Quote> data)
        {
            // Get valid file names
            var files = Directory.GetFiles(csIni.MinuteYahooDataFolder, "*_20??????.zip");
            if (files.Length == 0) return;

            var fileKeys = new List<Tuple<DateTime, string>>();
            foreach (var file in files)
            {
                var aa = Path.GetFileNameWithoutExtension(file).Split('_');
                var timestamp = DateTime.ParseExact(aa[aa.Length - 1], "yyyyMMdd", CultureInfo.InvariantCulture);
                fileKeys.Add(new Tuple<DateTime, string>(timestamp, file));
            }

            var keys = fileKeys.OrderBy(a => a.Item1).ToArray();
            if (keys.Length == 0) return;

            var validFiles = new List<string>();
            if (startDate <= keys[0].Item1 || endDate <= keys[0].Item1)
                validFiles.Add(keys[0].Item2);
            for (var k = 1; k < keys.Length; k++)
            {
                var minDate = keys[k - 1].Item1;
                if (minDate.DayOfWeek == DayOfWeek.Saturday) minDate = minDate.AddDays(2);
                else if (minDate.DayOfWeek == DayOfWeek.Sunday) minDate = minDate.AddDays(1);

                var maxDate = keys[k].Item1;
                if (maxDate.DayOfWeek == DayOfWeek.Saturday) maxDate = maxDate.AddDays(-1);
                else if (maxDate.DayOfWeek == DayOfWeek.Sunday) maxDate = maxDate.AddDays(-2);

                if (!((startDate < minDate && endDate < minDate) || (startDate > maxDate && endDate > maxDate)))
                    validFiles.Add(keys[k].Item2);
            }

            // Get data
            var entryName = string.Format(zipFileEntryNameTemplate, symbol);
            foreach (var zipFileName in validFiles)
            {
                using (var zip = new ZipReader(zipFileName))
                    foreach (var item in zip.Where(a=> a.Length >0 && string.Equals(a.FileNameWithoutExtension, entryName, StringComparison.InvariantCultureIgnoreCase) ))
                    {
                        var o = JsonConvert.DeserializeObject<MinuteYahoo>(item.Content);
                        data.AddRange(o.GetQuotes(symbol).Where(a => a.date.Date >= startDate && a.date.Date <= endDate));
                    }
            }
        }

    }
}
