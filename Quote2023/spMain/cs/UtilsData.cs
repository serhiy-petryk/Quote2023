
using System;
using System.Data;
using System.Data.OleDb;

namespace spMain {
  class csUtilsData {

    public const string mdbProvider = @"Provider=Microsoft.Jet.OleDb.4.0;Data Source={0}";

    /*public static void FromOneTableToAnother(csJob job, string tblSource, string tblTarget, string mdbFullFileName) {
      int messID = csJob.ShowMessage(job, "Start copy data from " + tblSource + " table to " +
        tblTarget + " table. DB filename: " + mdbFullFileName, csJob.MessageType.start);
      OleDbConnection conn = new OleDbConnection(String.Format(mdbProvider, mdbFullFileName));
      try {
        OleDbCommand cmd = new OleDbCommand(String.Format("DELETE * from [{0}]", tblTarget), conn);
        conn.Open();
        cmd.ExecuteNonQuery();
        cmd.CommandText = String.Format(@"INSERT into [{0}] SELECT * from [{1}]", tblTarget, tblSource);
        cmd.ExecuteNonQuery();
        csJob.ShowMessage(job, "Data copied from " + tblSource + " table to " +
           tblTarget + " table. DB filename: " + mdbFullFileName, csJob.MessageType.start, messID);
      }
      catch (Exception ex) { csJob.ShowError(job, ex); }
      finally {
        if (conn.State != ConnectionState.Closed) conn.Close();
      }
    }*/

    public static DataTable GetDataTable(string selectStr, string mdbFullFileName) {
      OleDbConnection conn = new OleDbConnection(String.Format(mdbProvider, mdbFullFileName));
      return GetDataTable(selectStr, conn, null);
    }

    public static DataTable GetDataTable(string selectStr, string mdbFullFileName, OleDbParameter[] pars) {
      using (OleDbConnection conn = new OleDbConnection(String.Format(mdbProvider, mdbFullFileName))) {
        DataTable dt = GetDataTable(selectStr, conn, pars);
        conn.Close();
        return dt;
      }
    }

    public static DataTable GetDataTable(string selectStr, OleDbConnection conn, OleDbParameter[] pars) {
      if (conn.State != ConnectionState.Open) conn.Open();
      if (selectStr.IndexOf(" ") == -1) selectStr = "SELECT * from " + selectStr;
      OleDbCommand cmd = new OleDbCommand(selectStr, conn);
      if (pars != null)
        for (int i = 0; i < pars.Length; i++)
          cmd.Parameters.Add(pars[i]);
      OleDbDataAdapter da = new OleDbDataAdapter(cmd);
      DataTable dt = new DataTable();
      da.Fill(dt);
      return dt;
    }
    public static void ExecuteNonQuery(string sql, OleDbConnection conn, OleDbParameter[] pars) {
      if (conn.State != ConnectionState.Open) conn.Open();
      OleDbCommand cmd = new OleDbCommand(sql, conn);
      if (sql.IndexOf(" ") == -1) cmd.CommandType = CommandType.StoredProcedure;
      if (pars != null)
        for (int i = 0; i < pars.Length; i++)
          cmd.Parameters.Add(pars[i]);
      cmd.ExecuteNonQuery();
    }

    public static Byte GetByteFromDataField(object x, Byte defValue) {
      return (x == System.DBNull.Value ? defValue : Convert.ToByte(x));
    }
    public static DateTime GetDateFromDataField(object x, DateTime defValue) {
      return (x.GetType().ToString() == "System.DBNull" ? defValue : Convert.ToDateTime(x));
    }
    public static double GetDoubleFromDataField(object x, int dp) {
      return (x.GetType().ToString() == "System.DBNull" ? Double.NaN : Math.Round(Convert.ToDouble(x), dp));
    }
    public static double GetDoubleFromString(string s) {
      return (String.IsNullOrEmpty(s) ? Double.NaN : Double.Parse(s));
    }
    public static int GetIntFromDataField(object x, int defaultValue) {
      return (x.GetType().ToString() == "System.DBNull" ? defaultValue : int.Parse(x.ToString()));
    }
    public static string GetStringFromDataField(object field, bool flagNormalize) {
      if (String.IsNullOrEmpty(field.ToString())) return null;
      else {
        if (flagNormalize) return field.ToString().Trim().ToUpper();
        else return field.ToString().Trim();
      }
    }
    public static bool GetBoolFromDataField(object field) {
      switch (field.GetType().Name) {
        case "Boolean": return (Boolean)field;
      }
      throw new Exception("GetBoolFromDataField function. Can not convert " + (field == null ? "null" : field.GetType().Name) + " into boolean.");
    }

    public static OleDbDataReader GetDataReader(string selectStr, string mdbFullFileName) {
      OleDbConnection conn = new OleDbConnection(String.Format(mdbProvider, mdbFullFileName)); ;
      try {
        conn.Open();
        OleDbCommand cmd = new OleDbCommand(selectStr, conn);
        OleDbDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
        return rdr;
      }
      catch (Exception ex) { }
      return null;
    }


  }
}
