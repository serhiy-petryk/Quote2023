using System;
using System.IO;
using System.Collections;
using System.Text;

namespace spMain.QData.DataAdapters.MBT {

  public partial class QDChunk {

    long _position;
    public C.QDataFormat _dataFormat = C.QDataFormat.NotDefined;
    uint _length;
    int _dp;
    uint _records;
    long _uStartPrice;
    long _uLastTime;
    long _dataOffset;

    public string _comment;
    public DateTime _timeStamp;
    public bool _deleted;
    public string _error;

    public static double _dd2 = 0;

    /*		public IList GetData(QDElement element) {
          OleDbConnection conn = C.GetConnection(element._filename);
          try {
            OleDbCommand cmd = new OleDbCommand("Select [data] from [data] where [symbol]=@symbol and [date]=@date and [version]=@version", conn);
            OleDbParameter p1 = new OleDbParameter("@symbol", typeof(string));
            OleDbParameter p2 = new OleDbParameter("@date", typeof(DateTime));
            OleDbParameter p3 = new OleDbParameter("@version", typeof(byte));
            p1.Value = element._symbol; p2.Value = element._date; p3.Value = element._version;
            cmd.Parameters.AddRange(new OleDbParameter[] { p1, p2, p3 });
            conn.Open();
            byte[] bytes = (byte[])cmd.ExecuteScalar();
            conn.Close();
            return this.GetData(bytes);
          }
          catch (Exception ex) {
            this._error = ex.ToString();
            return null;
          }
          finally {
            if (conn.State == System.Data.ConnectionState.Open) conn.Close();
          }
        }*/

    public IList GetData(byte[] bytes) {
      try {
        using (MemoryStream ms = new MemoryStream(bytes)) {
          using (BinaryReader br = new BinaryReader(ms, Encoding.UTF8)) {
            this.ReadHeader(br);
            DateTime dt1 = DateTime.Now;
            IList data = null;
            switch (this._dataFormat) {
              case C.QDataFormat.MbtTickHttp:
                //              data = this.Read_MbtTickHttp(br); ; break;
                return this.Read_MbtTickHttp(br);
              case C.QDataFormat.MbtTick:
                //                data = this.Read_MbtTick(br); break;
                return this.Read_MbtTick(br);
              case C.QDataFormat.Quote:
                //              data = this.Read_Quote(br); break;
                return this.Read_Quote(br);
              default: throw new Exception(this._dataFormat + " format does not support while data reading");
            }
            DateTime dt2 = DateTime.Now;
            TimeSpan ts2 = dt2 - dt1;
            _dd2 += ts2.TotalMilliseconds;
            return data;
          }
        }
      }
      catch (Exception ex) {
        this._error = ex.ToString();
        return null;
      }
    }

    void ReadHeader(BinaryReader br) {
      this._error = null;
      this._position = br.BaseStream.Position;
      this._length = br.ReadUInt32();
      this._dataFormat = (C.QDataFormat)br.ReadByte();
      byte b1 = br.ReadByte();
      this._deleted = ((b1 >> 7) & 0x01) == 1;
      int bRecs = (b1 >> 6) & 0x01;
      int bPrice = (b1 >> 5) & 0x01;
      this._dp = b1 & 0x0F;
      if (bRecs == 0) this._records = br.ReadUInt16();
      else this._records = br.ReadUInt32();
      if (bPrice == 0) this._uStartPrice = br.ReadUInt16();
      else this._uStartPrice = br.ReadUInt32();
      this._timeStamp = new DateTime((br.ReadUInt32() + C.offsetDateTimeInSecs) * C.cTicksInSecond);
      this._uLastTime = br.ReadUInt32();
      this._comment = br.ReadString();
      this._dataOffset = br.BaseStream.Position - this._position;
    }
  }
}
