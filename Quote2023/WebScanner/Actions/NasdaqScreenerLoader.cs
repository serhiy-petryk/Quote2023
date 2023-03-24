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
    public class NasdaqScreenerLoader
    {
        private const string Url = @"https://api.nasdaq.com/api/screener/stocks?tableonly=true&download=true";

        public static void Start()
        {
            Logger.AddMessage($"Started");

            var timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            var dateStamp = DateTime.Now.ToString("yyyyMMdd");
            var filename = $@"E:\Quote\WebScanner\Nasdaq\{dateStamp}\NasdaqScanner_{timeStamp}.json";

            // Download data
            Logger.AddMessage($"Download Nasdaq STOCK data to {filename}");
            Helpers.Download.DownloadPage(Url, filename, true);

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
                        var stockItems = new List<cStockRow>();
                        var content = entry.GetContentOfZipEntry();
                        var oo = JsonConvert.DeserializeObject<cStockRoot>(content);
                        stockItems.AddRange(oo.data.rows);

                        foreach (var item in stockItems)
                            item.TimeStamp = entry.LastWriteTime.DateTime;

                        DbUtils.SaveToDbTable(stockItems, "dbQuote2023..WebScannerNasdaq", "symbol", "TimeStamp",
                            "LastSale", "volume");

                        itemCount += stockItems.Count;
                    }

            return itemCount;
        }

        #region =========  Json classes  ============
        //========================
        private static readonly CultureInfo culture = new CultureInfo("en-US");
        public class cStockRoot
        {
            public cStockData data;
            public object message;
            public cStatus status;
        }
        public class cStockData
        {
            public cStockRow[] rows;
        }
        public class cStockRow
        {
            // "symbol", "name", "LastSale", "Volume", "netChange", "Change", "MarketCap", "country", "ipoYear", "sector", "industry", "timeStamp"
            // github: symbol, name, lastsale, volume, netchange, pctchange, marketCap, country, ipoyear, industry, sector, url
            public string symbol;
            public string lastSale;
            public long volume;
            public DateTime TimeStamp;

            public float? LastSale => lastSale == "NA" ? (float?)null : float.Parse(lastSale, NumberStyles.Any, culture);
        }

        private static string NullCheck(string s) => string.IsNullOrEmpty(s) ? null : s.Trim();

        //=====================
        public class cStatus
        {
            public int rCode;
            public object bCodeMessage;
            public string developerMessage;
        }

        #endregion
    }
}
