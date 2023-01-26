﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using spMain.Helpers;

namespace spMain
{
    class csIni
    {
        public static Dictionary<string, Type> typeXref;
        public static Dictionary<string, object[]> typeDataSet;// Dictionary<id, values>

        public static readonly bool isDesignMode = (System.Diagnostics.Process.GetCurrentProcess().ProcessName.ToLower() == "devenv" ||
                                                    System.Diagnostics.Process.GetCurrentProcess().ProcessName.ToLower() == "vcsexpress");

        public static string pathExe = GetPathExe();
        public const string pathLog = @"T:\Log\";
        public const string pathTxtDB = @"T:\Data\textDB\";
        public const string pathObjectDB = @"T:\Data\ObjectDB\";
        public const string pathData = @"T:\Data\";
        public const string pathDataZip = @"T:\Data\ZIP\";
        public const string pathTemp = @"T:\Temp\";

        public const string pathMdbBaseFileName = pathData + @"Base.5.0.mdb";
        public const string pathMdbSymbol = pathData + "sp.3.1.Symbols.mdb";
        public readonly static DateTimeFormatInfo fiDateUA = (new CultureInfo("uk-UA", false)).DateTimeFormat;
        public readonly static NumberFormatInfo fiNumberUA = (new CultureInfo("uk-UA", false)).NumberFormat;
        public readonly static CultureInfo ciUS = new CultureInfo("en-US", false);
        public readonly static DateTimeFormatInfo fiDateUS = ciUS.DateTimeFormat;
        public readonly static NumberFormatInfo fiNumberUS = ciUS.NumberFormat;

        static csIni()
        {
            // change Color Editor
            TypeDescriptor.AddAttributes(typeof(Color), new Attribute[] { new EditorAttribute(typeof(csColorEditor.ColorTypeEditor), typeof(UITypeEditor)) });
            LogFolderClear();
            IniHttp();
            GetPathExe();
            UpdateMDB();
        }

        public static string GetPathExe()
        {
            string startPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            DirectoryInfo di = new DirectoryInfo(startPath);
            //      DirectoryInfo di = Directory.GetParent(startPath);
            StringBuilder sb = new StringBuilder();
            while (di != null)
            {
                sb.Insert(0, di.Name + @"\");
                di = di.Parent;
                if (File.Exists(di.FullName + @"\setDiskOn.bat")) break;
            }
            if (di != null)
            {
                CsHelper.SynRunCmd(@"setDiskOn.bat", di.FullName);
                //      MessageBox.Show(@"T:\" + sb.ToString() + "- OK: GetPath");
                return @"T:\" + sb.ToString();
            }
            else
            {
                //        MessageBox.Show("Error: GetPath");
                throw new Exception("Can not set virtual disk T: (batch file 'setDiskOn.bat')");
            }
        }

        static void UpdateMDB()
        {
            if (typeXref == null)
            {
                typeXref = new Dictionary<string, Type>();
                typeDataSet = new Dictionary<string, object[]>();
                DataTable dt = csUtilsData.GetDataTable("select * from TypeXref", csIni.pathMdbBaseFileName);
                foreach (DataRow dr in dt.Rows)
                {
                    string s = dr["ValueList"].ToString();
                    string sType = dr["fullname"].ToString().ToLower();
                    Type t = (sType == "system.drawing.color" ? System.Drawing.Color.Empty.GetType() : Type.GetType(sType, true, true));
                    if (string.IsNullOrEmpty(s))
                    {
                        typeXref.Add(dr["shortname"].ToString().ToLower(), t);
                    }
                    else
                    {
                        string[] ss = s.Split(';');
                        List<object> oo = new List<object>();
                        foreach (string s1 in ss)
                        {
                            if (!String.IsNullOrEmpty(s1))
                            {
                                oo.Add(Convert.ChangeType(s1.Trim(), t));
                            }
                        }
                        typeDataSet.Add(sType, oo.ToArray());
                    }
                }
            }
        }

        private static void LogFolderClear()
        {
            if (!Directory.Exists(csIni.pathLog)) Directory.CreateDirectory(csIni.pathLog);
            string[] files = Directory.GetFiles(csIni.pathLog, "*.log");
            DateTime cutOffDate = DateTime.Now.AddDays(-1);
            for (int i = 0; i < files.Length; i++)
            {
                if (File.GetLastWriteTime(files[i]) < cutOffDate) File.Delete(files[i]);
            }
        }

        private static void IniHttp()
        {
            WebRequest.DefaultWebProxy.Credentials = CredentialCache.DefaultCredentials;
            HttpWebRequest.DefaultWebProxy.Credentials = CredentialCache.DefaultCredentials;
            //      HttpWebRequest.DefaultWebProxy.als = CredentialCache.DefaultCredentials;
            FtpWebRequest.DefaultWebProxy.Credentials = CredentialCache.DefaultCredentials;
            System.Net.ServicePointManager.Expect100Continue = false;
            System.Net.ServicePointManager.DefaultConnectionLimit = 1000;
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