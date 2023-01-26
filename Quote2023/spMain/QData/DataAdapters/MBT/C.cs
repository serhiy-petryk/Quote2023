using System;
using System.Text;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Globalization;
using spMain.QData.DataFormat;

namespace spMain.QData.DataAdapters.MBT {
  public class C {

    public const string pathDBQDefault = @"T:\Data\DBQ\";
    const string connTemplate = @"Provider=Microsoft.Jet.OleDb.4.0;Data Source={0}";

    internal const long maxUInt32 = UInt32.MaxValue;
    internal const long maxUInt16 = UInt16.MaxValue;
    internal const long maxUInt8 = Byte.MaxValue;
    internal const long maxInt32 = Int32.MaxValue;
    internal const long minInt32 = Int32.MinValue;
    internal const long maxInt16 = Int16.MaxValue;
    internal const long minInt16 = Int16.MinValue;
    internal const long maxInt8 = SByte.MaxValue;
    internal const long minInt8 = SByte.MinValue;
    internal const long cStartTimeFactor = 3600 * 24;// 1 day
    internal const long cTicksInSecond = 10000 * 1000;
    internal const long startPriceFactor = 100000000;

    public enum QDataType { Day, Minute, Tick };
    public enum QDataFormat : byte { NotDefined = 0, Quote = 1, MbtTick = 2, MbtTickHttp = 3 };

    public static DateTime minDateTime = new DateTime(1970, 1, 1); // MaxDate: 2038 year-int; 2106 year-uint
    public static long offsetDateTimeInSecs = minDateTime.Ticks / cTicksInSecond;
    public static readonly TimeSpan tsSessionStart = new TimeSpan(9, 30, 0);
    public static readonly TimeSpan tsSessionEnd = new TimeSpan(16, 01, 0);

    public readonly static Encoding encoding = Encoding.GetEncoding(1252);//Western European (Windows)

    public readonly static CultureInfo ciInvariant = CultureInfo.InvariantCulture;
    public readonly static DateTimeFormatInfo fiDateInvariant = CultureInfo.InvariantCulture.DateTimeFormat;
    public readonly static NumberFormatInfo fiNumberInvariant = CultureInfo.InvariantCulture.NumberFormat;

    public static OleDbConnection GetConnection(string filename) {
      return new OleDbConnection(String.Format(connTemplate, filename));
    }

    internal static long NOD(long n1, long n2) {//Наибольший общий делитель
      long k1 = (n1 < n2 ? n1 : n2);
      long k2 = (n1 < n2 ? n2 : n1);
      if (k1 == 0) return Math.Abs(k2);
      long k3 = k2 % k1;
      while (k3 != 0) {
        k2 = k1;
        k1 = k3;
        k3 = k2 % k1;
      }
      return Math.Abs(k1);
    }

    public static string GetShortNameOfDataType(QDataType dataType) {
      switch (dataType) {
        case QDataType.Day: return "day";
        case QDataType.Minute: return "min";
        case QDataType.Tick: return "ts";
        default: return null;
      }
    }
    public static QDataType GetQDataTypeFromShortName(string shortNameOfDataType) {
      switch (shortNameOfDataType) {
        case "day": return QDataType.Day;
        case "min": return QDataType.Minute;
        case "ts": return QDataType.Tick;
        default: throw new Exception("There is not QDataType with short name '" + shortNameOfDataType + "'");
      }
    }

    public static QDataFormat GetDataFormat(object o) {
      if (o is Quote) return QDataFormat.Quote;
      else if (o is MbtTick) return QDataFormat.MbtTick;
      else if (o is MbtTickHttp) return QDataFormat.MbtTickHttp;
      throw new Exception(o.GetType().Name + " is invalid data format for DBQ data");
    }

    public static DateTime GetDateFromObject(object o) {
      if (o is MbtTickHttp) return ((MbtTickHttp)o)._date;
      else if (o is MbtTick) return ((MbtTick)o)._date;
      else if (o is Quote) return ((Quote)o).date;
      else throw new Exception(o.GetType().Name + " type is invalid for GetDateFromObject procedure");
    }

    static internal string GetDateFileID(DateTime date, string type) {
      // type: "y"-year; "m"-month; "w"-week; "d"-day
      switch (type) {
        case "y": return date.ToString("yyyy", C.fiDateInvariant);
        case "m": return date.ToString("yyyy-MM", C.fiDateInvariant);
        case "d": return date.ToString("yyyy-MM-dd", C.fiDateInvariant);
        case "w":
          return date.ToString("yyyy", C.fiDateInvariant) + "-W" + GetWeekNo(date).ToString("00");
        default: throw new Exception("'" + type + "' is invalid Type parameter in GetDateFileID. Possible enries: y, m, w, d");
      }
    }

    static internal int GetWeekNo(DateTime dt) {
      DateTime dt1 = new DateTime(dt.Year, 1, 1);
      DateTime startDay = dt1.AddDays(-(int)dt1.DayOfWeek);
      return Convert.ToInt32((dt.Date - startDay).TotalDays) / 7;
    }

    internal static int GetDP(double x) {
      //      int ko=Decimal.GetBits(Convert.ToDecimal(Convert.ToDouble(100m*0.01m)))[3]>>16;
      int k = (Decimal.GetBits(Convert.ToDecimal(x))[3] >> 16) & 0x3F;
      if (k > 15) {
        throw new Exception("Program can not support " + k + " decimal places for number " + x.ToString() + ". Maximum number of dp is 15.");
      }
      return k;
    }
  }
}
