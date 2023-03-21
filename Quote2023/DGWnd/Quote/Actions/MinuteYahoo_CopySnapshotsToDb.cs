using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using DGWnd.Quote.Helpers;
using DGWnd.Quote.Models;
using Newtonsoft.Json;
using spMain.Comp;
using spMain.Quote2023.Helpers;
using spMain.Quote2023.Models;

namespace DGWnd.Quote.Actions
{
    public static class MinuteYahoo_CopySnapshotsToDb
    {
        public static void CopySnapshots(string[] zipFiles, Action<string> showStatus)
        {
            var liveSymbolsAndDates = new Dictionary<Tuple<string, DateTime>, object>();

            showStatus($"CopySnapshots. Loading data from database ...");
            using (var conn = new SqlConnection(DGWnd.Settings.DbConnectionString))
            {
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "SELECT a.* from vSymbolAndDateLive a left join dbQuote2023..IntradaySnapshots b on a.Symbol=b.Symbol and a.Date=b.Date WHERE b.Symbol is null AND a.Date>'2022-10-01'";
                    using (var rdr = cmd.ExecuteReader())
                        while (rdr.Read())
                            liveSymbolsAndDates.Add(new Tuple<string, DateTime>((string)rdr["Symbol"], (DateTime)rdr["Date"]), null);
                }
            }

            var liveSymbols = liveSymbolsAndDates.Select(a => a.Key.Item1).Distinct().ToDictionary(a => a, a => (object)null);

            var cnt = 0;
            foreach (var zipFile in zipFiles)
            {
                showStatus($"CopySnapshots. File {Path.GetFileName(zipFile)}. Get symbols & date to copy.");
                var toLoadSymbolsAndDate = new Dictionary<Tuple<string, DateTime>, DGWnd.Quote.Models.IntradaySnapshot>();
                using (var zip = ZipFile.Open(zipFile, ZipArchiveMode.Read))
                    foreach (var item in zip.Entries)
                        if (item.Length > 0 && item.Name.ToUpper().StartsWith("YMIN-"))
                        {
                            var symbol = Path.GetFileNameWithoutExtension(item.Name).Substring(5).ToUpper();
                            if (!liveSymbols.ContainsKey(symbol))
                                continue;

                            cnt++;
                            if ((cnt % 100) == 0)
                                showStatus($"CopySnapshots. File {Path.GetFileName(zipFile)}. Total files in zip processed: {cnt:N0}");

                            var o = JsonConvert.DeserializeObject<MinuteYahoo>(item.GetContentOfZipEntry());
                            var dates = o.GetQuotes(symbol).Select(a => a.date.Date).Distinct();
                            foreach (var date in dates)
                            {
                                var key = new Tuple<string, DateTime>(symbol, date);
                                if (liveSymbolsAndDates.ContainsKey(key))
                                    toLoadSymbolsAndDate.Add(key, null);
                            }
                        }

                if (toLoadSymbolsAndDate.Count > 0)
                {
                    Debug.Print($"CopySnapshots. File {Path.GetFileName(zipFile)}. {toLoadSymbolsAndDate.Count} items to save");
                    showStatus($"CopySnapshots. File {Path.GetFileName(zipFile)}. Found {toLoadSymbolsAndDate.Count} quotes to save snapshots");
                    using (var frm = new frmUIStockGraph(null, true))
                    {
                        frm.Visible = false;
                        cnt = 0;
                        var keys = toLoadSymbolsAndDate.Keys.ToArray();
                        toLoadSymbolsAndDate.Clear();
                        foreach (var key in keys)
                        {
                            cnt++;
                            showStatus($"CopySnapshots. File {Path.GetFileName(zipFile)}. {cnt:N0} from {keys.Length:N0} snapshots created");

                            var graph = spMain.csUtils.GetGraphToSave(key.Item1, key.Item2, 1);
                            frm._SetUIGraph(graph, true);
                            var image = frm._GetImage();

                            using (var ms = new MemoryStream())
                            {
                                image.Save(ms, ImageFormat.Png);
                                toLoadSymbolsAndDate.Add(key,
                                    new IntradaySnapshot { Symbol = key.Item1, Date = key.Item2, Snapshot = ms.ToArray() });
                            }

                            if (cnt % 100 == 0)
                            {
                                showStatus($"CopySnapshots. File {Path.GetFileName(zipFile)}. Save snapshots to database ...");
                                DbHelper.SaveToDbTable(toLoadSymbolsAndDate.Values, "dbQuote2023..IntradaySnapshots", "Symbol", "Date", "Snapshot");
                                toLoadSymbolsAndDate.Clear();
                            }

                        }
                    }
                }

                showStatus($"CopySnapshots. File {Path.GetFileName(zipFile)}. Save snapshots to database ...");
                DbHelper.SaveToDbTable(toLoadSymbolsAndDate.Values, "dbQuote2023..IntradaySnapshots", "Symbol", "Date", "Snapshot");
                toLoadSymbolsAndDate.Clear();
            }

            showStatus($"CopySnapshots. Finished!");
        }

    }
}
