using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using spMain.Helpers;

namespace DGWnd.Quote
{
    public static class Actions
    {
        public static void AddIntradaySnapshoysInDb(Action<string> showStatusAction, string[] zipFiles)
        {
            var liveSymbolsAndDates = new Dictionary<Tuple<string, DateTime>, object>();

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

            foreach (var zipFile in zipFiles)
            {
                showStatusAction($"MinuteYahoo_GetQuotesFromZipFiles is working for {Path.GetFileName(zipFile)}");
                using (var zip = new ZipReader(zipFile))
                    foreach (var item in zip)
                        if (item.Length > 0 && item.FileNameWithoutExtension.ToUpper().StartsWith("YMIN-"))
                        {
                            var symbol = item.FileNameWithoutExtension.Substring(5);

                        }
            }

        }
    }
}
