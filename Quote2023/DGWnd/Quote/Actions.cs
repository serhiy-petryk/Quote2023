using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DGWnd.Quote.Helpers;
using DGWnd.Quote.Models;
using DGWnd.UI;
using Newtonsoft.Json;
using spMain.Comp;
using spMain.Helpers;

namespace DGWnd.Quote
{
    public static class Actions
    {
        public static void AddIntradaySnapshoysInDb(Action<string> showStatusAction, string[] zipFiles, frmMDI host)
        {
            var aa1 = System.Windows.Forms.Application.OpenForms;

            var liveSymbolsAndDates = new Dictionary<Tuple<string, DateTime>, object>();
            var liveSymbols = new Dictionary<string, object>();
            var toLoadSymbolsAndDate = new Dictionary<Tuple<string, DateTime>, Models.IntradaySnapshot>();

            showStatusAction($"AddIntradaySnapshoysInDb. Loading data from database ...");
            using (var conn = new SqlConnection(Settings.DbConnectionString))
            {
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "SELECT * from SymbolsAndDatesLive WHERE Date>'2022-09-01'";
                    using (var rdr = cmd.ExecuteReader())
                        while (rdr.Read())
                            liveSymbolsAndDates.Add(new Tuple<string, DateTime>((string)rdr["Symbol"], (DateTime)rdr["Date"]), null);

                    
                    cmd.CommandText = "SELECT * from IntradaySnapshots";
                    using (var rdr = cmd.ExecuteReader())
                        while (rdr.Read())
                        {
                            var key = new Tuple<string, DateTime>((string) rdr["Symbol"], (DateTime) rdr["Date"]);
                            if (liveSymbolsAndDates.ContainsKey(key))
                                liveSymbolsAndDates.Remove(key);
                        }
                }
            }

            liveSymbols = liveSymbolsAndDates.Select(a => a.Key.Item1).Distinct().ToDictionary(a => a, a => (object)null);

            var cnt = 0;
            foreach (var zipFile in zipFiles)
            {
                showStatusAction($"AddIntradaySnapshoysInDb is working for {Path.GetFileName(zipFile)}");
                using (var zip = new ZipReader(zipFile))
                    foreach (var item in zip)
                        if (item.Length > 0 && item.FileNameWithoutExtension.ToUpper().StartsWith("YMIN-"))
                        {
                            var symbol = item.FileNameWithoutExtension.Substring(5).ToUpper();
                            if (!liveSymbols.ContainsKey(symbol))
                                continue;

                            cnt++;
                            if ((cnt % 100) == 0)
                                showStatusAction($"AddIntradaySnapshoysInDb is working for {Path.GetFileName(zipFile)}. Total file processed: {cnt:N0}");

                            var o = JsonConvert.DeserializeObject<spMain.Models.MinuteYahoo>(item.Content);
                            var dates = o.GetQuotes(symbol).Select(a => a.date.Date).Distinct();
                            foreach (var date in dates)
                            {
                                var key = new Tuple<string, DateTime>(symbol, date);
                                if (liveSymbolsAndDates.ContainsKey(key))
                                    toLoadSymbolsAndDate.Add(key, null);
                            }

                            //if (toLoadSymbolsAndDate.Count > 3000)
                              //  break;
                        }
            }

            cnt = 0;
            foreach (var key in toLoadSymbolsAndDate.Keys.ToArray())
            {
                cnt++;
                var graph = spMain.csUtils.GetGraphToSave(key.Item1, key.Item2, 1);
                host.AttachNewChildForm(new frmUIStockGraph(graph, true));
                using (var ms = new MemoryStream())
                {
                    Clipboard.GetImage()?.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    toLoadSymbolsAndDate[key] = new IntradaySnapshot
                        {Symbol = key.Item1, Date = key.Item2, Snapshot = ms.ToArray()};
                }
                if ((cnt % 10) == 0)
                    showStatusAction($"AddIntradaySnapshoysInDb. {cnt:N0} snapshots created");
                Application.DoEvents();
            }

            showStatusAction($"AddIntradaySnapshoysInDb. Save images to database ...");
           DbHelper.SaveToDbTable(toLoadSymbolsAndDate.Values, "IntradaySnapshots", "Symbol", "Date", "Snapshot");
            showStatusAction($"AddIntradaySnapshoysInDb finished!");
        }
    }
}
