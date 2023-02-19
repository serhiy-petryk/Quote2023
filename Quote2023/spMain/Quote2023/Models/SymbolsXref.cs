using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace spMain.Quote2023.Models
{
    public class SymbolsXref
    {
        private static Dictionary<string, SymbolsXref> _symbolsXrefs;

        public static SymbolsXref GetSymbolsXref(string symbol)
        {
            if (_symbolsXrefs == null)
            {
                _symbolsXrefs=new Dictionary<string, SymbolsXref>();
                using (var conn = new SqlConnection(Settings.DbConnectionString))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "SELECT * from vSymbolsLive";
                    using (var rdr = cmd.ExecuteReader())
                        while (rdr.Read())
                        {
                            var dbSymbol = (string) rdr["Symbol"];
                            var xref = new SymbolsXref
                            {
                                YahooSymbol = DbGetString(rdr["YahooSymbol"]),
                                NasdaqSymbol = DbGetString(rdr["NasdaqSymbol"]),
                                AlphaVantageSymbol = DbGetString(rdr["AlphaVantageSymbol"]),
                                TradingViewSymbol = DbGetString(rdr["TradingViewSymbol"])
                            };
                            _symbolsXrefs.Add(dbSymbol, xref);
                        }

                }
            }

            if (_symbolsXrefs.ContainsKey(symbol)) return _symbolsXrefs[symbol];
            return null;

            string DbGetString(object o)
            {
                if (Equals(o, DBNull.Value)) return null;
                return (string)o;
            }
        }

        public string Symbol;
        public string YahooSymbol;
        public string NasdaqSymbol;
        public string AlphaVantageSymbol;
        public string TradingViewSymbol;
    }
}
