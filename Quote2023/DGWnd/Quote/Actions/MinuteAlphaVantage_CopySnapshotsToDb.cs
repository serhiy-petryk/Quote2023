using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using DGWnd.Quote.Helpers;
using spMain.Comp;

namespace DGWnd.Quote.Actions
{
    public static class MinuteAlphaVantage_CopySnapshotsToDb
    {
        public static bool StopFlag;

        private class CopyItem
        {
            public string Symbol;
            public string AlphaVantageSymbol;
            public DateTime Date;
            public byte[] Snapshot;
        }

        public static void Start()
        {
            StopFlag = false;

            var items = new List<CopyItem>();
            var symbolsXref = new Dictionary<string, string>();

            Logger.AddMessage("Loading data from database ...");
            using (var conn = new SqlConnection(DGWnd.Settings.DbConnectionString))
            {
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandTimeout = 150;
                    cmd.CommandText = "select distinct a.Symbol, b.AlphaVantageSymbol, a.date from vSymbolAndDateLive a "+
                                      "inner join SymbolsEoddata b on a.Symbol = b.Symbol and a.Exchange = b.Exchange "+
                                      "left join dbQ2023..IntradaySnapshots c on a.Symbol = c.Symbol and a.Date = c.Date "+
                                      "where a.date is not null and b.AlphaVantageSymbol is not null and c.Symbol is null";
                    using (var rdr = cmd.ExecuteReader())
                        while (rdr.Read())
                            items.Add(new CopyItem{Symbol= (string)rdr["Symbol"], AlphaVantageSymbol = (string)rdr["AlphaVantageSymbol"], Date = (DateTime)rdr["Date"] });
                }
            }

            var groupedItems = items.GroupBy(a=>a.Date).ToDictionary(a => a.Key, a => a.ToArray());
            items.Clear();

            var frm = new frmUIStockGraph(null, true) {Visible = false};
            var savedToDbCount = 0;
            var dateCnt = 0;
            foreach (var kvp in groupedItems)
            {
                dateCnt++;
                Logger.AddMessage($"Process data for {kvp.Key:d}. {dateCnt} from {groupedItems.Count} dates processed");

                var zipFile = $@"E:\Quote\WebData\Minute\AlphaVantage\Data\MAV_{kvp.Key:yyyyMMdd}.zip";
                if (File.Exists(zipFile))
                    using (var zip = ZipFile.Open(zipFile, ZipArchiveMode.Read))
                        foreach (var item in kvp.Value)
                        {
                            var fileKey = $"{item.AlphaVantageSymbol}_{kvp.Key:yyyyMMdd}.csv";
                            var entry = zip.Entries.FirstOrDefault(a =>
                                string.Equals(a.Name, fileKey, StringComparison.InvariantCultureIgnoreCase));
                            if (entry != null)
                            {
                                var graph = spMain.csUtils.GetAlphaVantageGraphToSave(item.AlphaVantageSymbol,
                                    item.Date, 1);
                                frm._SetUIGraph(graph, true);

                                using (var image = frm._GetImage())
                                using (var ms = new MemoryStream())
                                {
                                    image.Save(ms, ImageFormat.Png);
                                    item.Snapshot = ms.ToArray();
                                    items.Add(item);
                                }

                                if (items.Count >= 100)
                                {
                                    DbHelper.SaveToDbTable(items, "dbQ2023..IntradaySnapshots", "Symbol", "Date",
                                        "Snapshot");

                                    savedToDbCount += items.Count;
                                    foreach (var a in items) a.Snapshot = null;
                                    items.Clear();

                                    Logger.AddMessage($"Process data for {kvp.Key:d}. {dateCnt} from {groupedItems.Count} dates processed. Saved {savedToDbCount} snapshots to database");

                                    frm.Dispose();
                                    if (StopFlag)
                                    {
                                        Logger.AddMessage($"CopySnapshots. Interrupted!");
                                        return;
                                    }

                                    frm = new frmUIStockGraph(null, true) {Visible = false};
                                }
                            }
                        }
            }

            frm.Dispose();

            if (items.Count > 0)
            {
                DbHelper.SaveToDbTable(items, "dbQ2023..IntradaySnapshots", "Symbol", "Date", "Snapshot");

                savedToDbCount += items.Count;
                foreach (var a in items) a.Snapshot = null;
                items.Clear();
            }

            Logger.AddMessage($"!Finished. {savedToDbCount} snapshots saved to database");
        }
    }
}
