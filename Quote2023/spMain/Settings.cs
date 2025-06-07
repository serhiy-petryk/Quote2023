using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.IO;
using System.Net;
using System.Threading;
using spMain.csColorEditor;

namespace spMain
{
    class Settings
    {
        public const string DbConnectionString = "Data Source=localhost;Initial Catalog=dbQuote2022;Integrated Security=True;Connect Timeout=150;Encrypt=false;";

        public const string MinuteYahooDataFolder = @"E:\Quote\WebData\Minute\Yahoo\Data\";
        public const string MinuteAlphaVantageDataFolder = @"D:\Quote\WebData\Minute\AlphaVantage\Data\";
        public const string MinutePolygonDataFolder = @"E:\Quote\WebData\Minute\Polygon\Data\";
        public const string MinutePolygon2003DataFolder = @"E:\Quote\WebData\Minute\Polygon2003\Data\";

        public static Dictionary<string, Type> typeXref = new Dictionary<string, Type>
        {
            {"integer", typeof(int)}, {"smoothtype", typeof(QData.Common.General.SmoothType)},
            {"string", typeof(string)}, {"timeinterval", typeof(QData.Common.TimeInterval)}, {"double", typeof(double)},
            {"color", typeof(System.Drawing.Color)}, {"quote", typeof(QData.DataFormat.Quote)},
            {"quotevalue", typeof(QData.DataFormat.Quote.ValueProperty)}, {"boolean", typeof(bool)},
            {"dbindicator", typeof(QData.DataDB.DBIndicator)}
        };

        public static readonly bool isDesignMode = (Process.GetCurrentProcess().ProcessName.ToLower() == "devenv" ||
                                                    Process.GetCurrentProcess().ProcessName.ToLower() == "vcsexpress");

        public const string pathRoot = @"D:\Old\vs\";
        public const string pathLog = pathRoot + @"Log\";
        public const string pathData = pathRoot + @"Data\";
        public const string pathSerialization = pathData + @"Serialization\";
        public const string pathTxtDB = pathData + @"textDB\";
        public const string pathObjectDB = pathData + @"ObjectDB\";
        public const string pathDataZip = pathData + @"ZIP\";
        public const string pathTemp = pathRoot + @"Temp\";

        public const string pathMdbBaseFileName = pathData + @"Base.5.0.mdb";
        // public const string pathMdbSymbol = pathData + "sp.3.1.Symbols.mdb";

        public static readonly DateTimeFormatInfo fiDateUA = (new CultureInfo("uk-UA", false)).DateTimeFormat;
        // public static readonly NumberFormatInfo fiNumberUA = (new CultureInfo("uk-UA", false)).NumberFormat;
        public static readonly CultureInfo ciUS = new CultureInfo("en-US", false);
        // public static readonly DateTimeFormatInfo fiDateUS = ciUS.DateTimeFormat;
        public static readonly NumberFormatInfo fiNumberUS = ciUS.NumberFormat;

        static Settings()
        {
            // change Color Editor
            TypeDescriptor.AddAttributes(typeof(Color), new Attribute[] { new EditorAttribute(typeof(ColorTypeEditor), typeof(UITypeEditor)) });
            LogFolderClear();
            IniHttp();
        }

        private static void LogFolderClear()
        {
            if (!Directory.Exists(Settings.pathLog)) Directory.CreateDirectory(Settings.pathLog);
            var files = Directory.GetFiles(Settings.pathLog, "*.log");
            var cutOffDate = DateTime.Now.AddDays(-1);
            for (int i = 0; i < files.Length; i++)
                if (File.GetLastWriteTime(files[i]) < cutOffDate)
                    File.Delete(files[i]);
        }

        private static void IniHttp()
        {
            WebRequest.DefaultWebProxy.Credentials = CredentialCache.DefaultCredentials;
            HttpWebRequest.DefaultWebProxy.Credentials = CredentialCache.DefaultCredentials;
            //      HttpWebRequest.DefaultWebProxy.als = CredentialCache.DefaultCredentials;
            FtpWebRequest.DefaultWebProxy.Credentials = CredentialCache.DefaultCredentials;
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.DefaultConnectionLimit = 1000;
            int t1; int t2;
            ThreadPool.GetAvailableThreads(out t1, out t2);
            ThreadPool.GetMaxThreads(out t1, out t2);
            ThreadPool.SetMaxThreads(1000, 1500);
            ThreadPool.GetAvailableThreads(out t1, out t2);
            ThreadPool.GetMaxThreads(out t1, out t2);
            ThreadPool.GetMinThreads(out t1, out t2);
        }
    }
}
