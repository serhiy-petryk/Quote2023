using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;
using DGWnd.Quote.Helpers;
using DGWnd.Quote.Models;
using Newtonsoft.Json;
using spMain.Comp;
using spMain.Helpers;

namespace DGWnd.Quote.Actions
{
    public static class MinuteAlphaVantage_CopySnapshotsToDb
    {
        public static bool StopFlag;

        public static void CopySnapshots(string[] zipFiles, Action<string> showStatus)
        {
            StopFlag = false;

            var liveSymbolsAndDates = new List<Tuple<string, DateTime>>();
            var symbolsXref = new Dictionary<string, string>();

            showStatus($"CopySnapshots. Loading data from database ...");
            using (var conn = new SqlConnection(DGWnd.Settings.DbConnectionString))
            {
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "SELECT a.* from vSymbolsAndDatesLive a left join dbQuote2023..IntradaySnapshots b on a.Symbol=b.Symbol and a.Date=b.Date WHERE b.Symbol is null";
                    using (var rdr = cmd.ExecuteReader())
                        while (rdr.Read())
                            liveSymbolsAndDates.Add(new Tuple<string, DateTime>((string)rdr["Symbol"], (DateTime)rdr["Date"]));

                    cmd.CommandText = "SELECT Symbol, AlphaVantageSymbol from vSymbolsLive WHERE AlphaVantageSymbol IS NOT NULL";
                    using (var rdr = cmd.ExecuteReader())
                        while (rdr.Read())
                            symbolsXref.Add((string) rdr["AlphaVantageSymbol"], (string) rdr["Symbol"]);
                }
            }

            var liveSymbolsByDate = liveSymbolsAndDates.GroupBy(a => a.Item2).ToDictionary(a => a.Key, a => a.Select(a1 => a1.Item1).ToArray());

            var cnt = 0;
            var fileCnt = 0;
            // var dataToSave = new List<Tuple<string, DateTime, byte[]>>();
            var toLoadSymbolsAndDate = new Dictionary<Tuple<string, DateTime>, DGWnd.Quote.Models.IntradaySnapshot>();
            // using (var frm = new frmUIStockGraph(null, true))
            {
                var frm = new frmUIStockGraph(null, true);
                frm.Visible = false;

                foreach (var zipFile in zipFiles)
                {
                    fileCnt++;
                    var dateKey = DateTime.ParseExact(Path.GetFileNameWithoutExtension(zipFile).Split('_')[1],
                        "yyyyMMdd", CultureInfo.InvariantCulture);

                    if (!liveSymbolsByDate.ContainsKey(dateKey)) continue;

                    showStatus($"CopySnapshots. File {Path.GetFileName(zipFile)}. Get symbols and dates to copy.");
                    var toLoadSymbols = liveSymbolsByDate[dateKey]
                        .ToDictionary(a => a, a => (DGWnd.Quote.Models.IntradaySnapshot) null);

                    using (var zip = new ZipReader(zipFile))
                        foreach (var item in zip)
                            if (item.Length > 0 &&
                                item.FullName.EndsWith(".csv", StringComparison.InvariantCultureIgnoreCase))
                            {
                                var ss = item.FileNameWithoutExtension.Split('_');
                                var alphaVantageSymbol = ss[0].ToUpper();
                                var symbol = symbolsXref[alphaVantageSymbol];
                                if (!toLoadSymbols.ContainsKey(symbol))
                                    continue;

                                cnt++;
                                if ((cnt % 10) == 0)
                                    showStatus(
                                        $"CopySnapshots. File {Path.GetFileName(zipFile)}. Total items in zip processed: {cnt:N0}. Processed {fileCnt:N0} zip files from {zipFiles.Length:N0}");

                                var key = new Tuple<string, DateTime>(symbol, dateKey);
                                if (!toLoadSymbolsAndDate.ContainsKey(key))
                                {
                                    var graph = spMain.csUtils.GetAlphaVantageGraphToSave(alphaVantageSymbol, dateKey, 1);
                                    frm._SetUIGraph(graph, true);
                                    using (var image = frm._GetImage())
                                    using (var ms = new MemoryStream())
                                    {
                                        image.Save(ms, ImageFormat.Png);
                                        toLoadSymbolsAndDate.Add(key, new IntradaySnapshot {Symbol = key.Item1, Date = key.Item2, Snapshot = ms.ToArray()});
                                    }
                                }

                                if (toLoadSymbolsAndDate.Count >= 100)
                                {
                                    showStatus($"CopySnapshots. File {Path.GetFileName(zipFile)}. Save snapshots to database ...");
                                    DbHelper.SaveToDbTable(toLoadSymbolsAndDate.Values, "dbQuote2023..IntradaySnapshots", "Symbol", "Date", "Snapshot");
                                    toLoadSymbolsAndDate.Clear();

                                    frm.Dispose();
                                    if (StopFlag)
                                    {
                                        showStatus($"CopySnapshots. Interrupted!");
                                        return;
                                    }

                                    frm = new frmUIStockGraph(null, true);
                                    frm.Visible = false;
                                }
                            }


                    if (toLoadSymbolsAndDate.Count > 0)
                    {
                        showStatus($"CopySnapshots. File {Path.GetFileName(zipFile)}. Save snapshots to database ...");
                        DbHelper.SaveToDbTable(toLoadSymbolsAndDate.Values, "dbQuote2023..IntradaySnapshots", "Symbol",
                            "Date", "Snapshot");
                        toLoadSymbolsAndDate.Clear();
                    }
                }
            }

            showStatus($"CopySnapshots. Finished!");
        }

    }
}
