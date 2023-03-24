using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebScanner.Helpers;

namespace WebScanner.Actions
{
    public class TradingViewScreenerLoader
    {
        private const string parameters = @"{""filter"":[{""left"":""exchange"",""operation"":""in_range"",""right"":[""AMEX"",""NASDAQ"",""NYSE""]}],""options"":{""lang"":""en""},""markets"":[""america""],""symbols"":{""query"":{""types"":[]},""tickers"":[]},""columns"":[""close"",""Recommend.All"",""volume""],""sort"":{""sortBy"":""name"",""sortOrder"":""asc""},""range"":[0,20000]}";

        private const string Url = @"https://scanner.tradingview.com/america/scan";

        public static void Start()
        {
            Logger.AddMessage($"Started");

            var timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            var dateStamp = DateTime.Now.ToString("yyyyMMdd");
            var filename = $@"E:\Quote\WebScanner\TradingView\{dateStamp}\TradingViewScanner_{timeStamp}.json";

            // Download data
            Logger.AddMessage($"Download TradingView STOCK data to {filename}");
            Helpers.Download.DownloadPage_POST(Url, filename, parameters);

            // Zip data
            var zipFileName = Helpers.ZipUtils.ZipFile(filename);
            if (string.IsNullOrEmpty(zipFileName))
            {
                Logger.AddMessage($"!Finished with ERROR. No zip file: {zipFileName}");
                return;
            }

            // Parse and save data to database
            Logger.AddMessage($"Parse and save files to database");
            var itemCount = ParseAndSaveToDb(zipFileName);

            // Remove json files
            File.Delete(filename);

            Logger.AddMessage($"!Finished. Items: {itemCount:N0}. Zip file size: {CsUtils.GetFileSizeInKB(zipFileName):N0}KB. Filename: {zipFileName}");
        }

        public static int ParseAndSaveToDb(string zipFileName)
        {
            var itemCount = 0;
            using (var zip = ZipFile.Open(zipFileName, ZipArchiveMode.Read))
                foreach (var entry in zip.Entries)
                    if (entry.Length > 0)
                    {
                        var o = JsonConvert.DeserializeObject<cRoot>(entry.GetContentOfZipEntry());
                        var items = o.data.Select(a => a.GetDbItem(entry.LastWriteTime.DateTime)).ToArray();

                        DbUtils.SaveToDbTable(items, "dbQuote2023..WebScannerTradingView", "Symbol", "Exchange",
                            "TimeStamp", "Close", "Volume", "Recommend");

                        itemCount += items.Length;
                    }

            return itemCount;
        }

        #region =========  Json classes  ============
        private static readonly CultureInfo culture = new CultureInfo("en-US");
        public class cRoot
        {
            public int totalCount;
            public Item[] data;
        }

        public class Item
        {
            public string s;
            public object[] d;

            public DbItem GetDbItem(DateTime timeStamp)
            {
                var ss = s.Split(':');
                var item = new DbItem
                {
                    Exchange = ss[0],
                    Symbol = ss[1],
                    Close = Convert.ToSingle(d[0]),
                    Volume = Convert.ToInt64(d[2]),
                    Recommend = Convert.ToSingle(d[1]),
                    TimeStamp = timeStamp
                };
                return item;
            }
        }

        public class DbItem
        {
            public string Exchange;
            public string Symbol;
            public float Close;
            public long Volume;
            public float Recommend;
            public DateTime TimeStamp;
        }


        #endregion
    }
}
