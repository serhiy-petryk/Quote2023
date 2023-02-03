﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DGWnd.Quote.Helpers;
using DGWnd.Quote.Models;
using Newtonsoft.Json;
using spMain.Comp;
using spMain.Helpers;

namespace DGWnd.Quote.UI
{
    public partial class frmCopySnapshotsToDb : Form
    {
        private object _lock = new object();

        public frmCopySnapshotsToDb()
        {
            InitializeComponent();
        }

        private void ShowStatus(string message)
        {
            lock (_lock) statusLabel.Text = message;
            Application.DoEvents();
        }

        private void btnSelectFiles_Click(object sender, EventArgs e)
        {
            if (CsHelper.OpenFileDialogMultiselect(Settings.MinuteYahooDataFolder, @"YahooMinute_202?????.zip file (*.zip)|YahooMinute_202?????.zip", true) is string[] files && files.Length > 0)
                CopySnapshots(files);
        }

        private void CopySnapshots(string[] zipFiles)
        {
            var liveSymbolsAndDates = new Dictionary<Tuple<string, DateTime>, object>();
            var toLoadSymbolsAndDate = new Dictionary<Tuple<string, DateTime>, Models.IntradaySnapshot>();

            ShowStatus($"CopySnapshots. Loading data from database ...");
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
                            var key = new Tuple<string, DateTime>((string)rdr["Symbol"], (DateTime)rdr["Date"]);
                            if (liveSymbolsAndDates.ContainsKey(key))
                                liveSymbolsAndDates.Remove(key);
                        }
                }
            }

            var liveSymbols = liveSymbolsAndDates.Select(a => a.Key.Item1).Distinct().ToDictionary(a => a, a => (object)null);

            var cnt = 0;
            foreach (var zipFile in zipFiles)
            {
                ShowStatus($"CopySnapshots is working for {Path.GetFileName(zipFile)}");
                using (var zip = new ZipReader(zipFile))
                    foreach (var item in zip)
                        if (item.Length > 0 && item.FileNameWithoutExtension.ToUpper().StartsWith("YMIN-"))
                        {
                            var symbol = item.FileNameWithoutExtension.Substring(5).ToUpper();
                            if (!liveSymbols.ContainsKey(symbol))
                                continue;

                            cnt++;
                            if ((cnt % 100) == 0)
                                ShowStatus($"CopySnapshots is working for {Path.GetFileName(zipFile)}. Total file processed: {cnt:N0}");

                            var o = JsonConvert.DeserializeObject<spMain.Models.MinuteYahoo>(item.Content);
                            var dates = o.GetQuotes(symbol).Select(a => a.date.Date).Distinct();
                            foreach (var date in dates)
                            {
                                var key = new Tuple<string, DateTime>(symbol, date);
                                if (liveSymbolsAndDates.ContainsKey(key))
                                    toLoadSymbolsAndDate.Add(key, null);
                            }
                        }
            }

            if (toLoadSymbolsAndDate.Count > 0)
            {
                Debug.Print($"CopySnapshots. {toLoadSymbolsAndDate.Count} items to save");
                ShowStatus($"CopySnapshots. Found {toLoadSymbolsAndDate.Count} quotes to save snapshots");
                using (var frm = new frmUIStockGraph(null, true))
                {
                    frm.Visible = false;
                    UI_StockGraph uiStockGraph = null;

                    foreach (var c in frm.Controls)
                        if (c is UI_StockGraph c1)
                        {
                            uiStockGraph = c1;
                            break;
                        }
                    if (uiStockGraph != null)
                    {
                        foreach (var c in uiStockGraph.Controls)
                            if (c is StockGraph stockGraph)
                            {
                                var stockGraphControl = (Control)c;
                                stockGraphControl.Dock = DockStyle.None;
                                stockGraphControl.Size = new Size(100, 60);
                                break;
                            }
                    }

                    cnt = 0;
                    var keys = toLoadSymbolsAndDate.Keys.ToArray();
                    toLoadSymbolsAndDate.Clear();
                    foreach (var key in keys)
                    {
                        cnt++;
                        ShowStatus($"CopySnapshots. {cnt:N0} from {keys.Length:N0} snapshots created");

                        var graph = spMain.csUtils.GetGraphToSave(key.Item1, key.Item2, 1);
                        uiStockGraph._SetUIGraph(graph, true);
                        var image = uiStockGraph._GetImage();

                        using (var ms = new MemoryStream())
                        {
                            image.Save(ms, ImageFormat.Png);
                            toLoadSymbolsAndDate.Add(key,
                                new IntradaySnapshot {Symbol = key.Item1, Date = key.Item2, Snapshot = ms.ToArray()});
                        }

                        if (cnt % 100 == 0)
                        {
                            ShowStatus($"CopySnapshots. Save snapshots to database ...");
                            DbHelper.SaveToDbTable(toLoadSymbolsAndDate.Values, "IntradaySnapshots", "Symbol", "Date", "Snapshot");
                            toLoadSymbolsAndDate.Clear();
                        }

                    }
                }
            }

            ShowStatus($"CopySnapshots. Save snapshots to database ...");
            DbHelper.SaveToDbTable(toLoadSymbolsAndDate.Values, "IntradaySnapshots", "Symbol", "Date", "Snapshot");

            ShowStatus($"CopySnapshots. Finished!");
        }
    }
}
