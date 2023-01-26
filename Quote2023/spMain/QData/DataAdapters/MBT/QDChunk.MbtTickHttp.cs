using System;
using System.Diagnostics;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace spMain.QData.DataAdapters.MBT {
  public partial class QDChunk {

    List<DataFormat.MbtTickHttp> Read_MbtTickHttp(BinaryReader br) {

      long lastPrice = this._uStartPrice;
      long lastTime = this._uLastTime + C.offsetDateTimeInSecs;
      //      long startPriceFactor = 100000000;
      long kDP = 1;
      for (int i1 = 0; i1 < this._dp; i1++) kDP *= 10;
      lastPrice = lastPrice * (C.startPriceFactor / kDP);
      long priceFactor = C.NOD(lastPrice, C.startPriceFactor);
      lastPrice /= priceFactor;

      List<DataFormat.MbtTickHttp> data = new List<DataFormat.MbtTickHttp>();
      UInt32 lastVolume = 0;
      int lastCondition = 0;
      int lastType = 0;
      //      int lastStatus = 0;
      int timeSign = 1;
      double dPriceFactor = priceFactor * 1.0 / C.startPriceFactor;

      int recs = 0;
      while (true) {

        byte b = br.ReadByte();
        if (b == 0) {
          b = br.ReadByte();
          if (b == 0) {
            timeSign = -1;// 0x0000 - chunk end
          }
          else if (b == 1) {// 0x0001 - chunk end
            return data;
            //            break; 
          }
          else {
            long k = 0;
            if (b < 128) {// 1 byte
              k = b;
            }
            else {// 2 bytes
              k = ((b & 0x7f) << 8) | br.ReadByte();
            }
            priceFactor /= k;
            lastPrice *= k;
            dPriceFactor = priceFactor * 1.0 / C.startPriceFactor;
          }
        }
        else if (b < 0x20) {// repeating of last data
          int cnt = (b & 0x1f);
          for (int i = 0; i < cnt; i++) {
            DataFormat.MbtTickHttp tick = new DataFormat.MbtTickHttp(++recs, new DateTime(lastTime * C.cTicksInSecond),
              lastPrice * dPriceFactor, lastVolume, lastCondition, lastType);
            data.Add(tick);
          }
        }
        else if (b < 0x40) {// only volume changed
          lastVolume = (Convert.ToUInt32(b & 0x1f) + 1) * 100;
          DataFormat.MbtTickHttp tick = new DataFormat.MbtTickHttp(++recs, new DateTime(lastTime * C.cTicksInSecond), lastPrice * dPriceFactor,
            lastVolume, lastCondition, lastType);
          data.Add(tick);
        }
        else {
          int bOther = (b >> 6) & 0x03;
          int bSecs = (b >> 4) & 0x03;
          int bPrice = (b >> 2) & 0x03;
          int bVolume = b & 0x03;
          UInt32 secs = 0;
          long price = 0;
          UInt16 other = 0;

          switch (bSecs) {
            case 0: break;
            case 1: secs = 1; break;
            case 2: secs = br.ReadByte(); break;
            case 3: secs = br.ReadUInt32(); break;
          }

          switch (bPrice) {
            case 0: break;
            case 1: price = br.ReadSByte(); break;
            case 2: price = br.ReadInt16(); break;
            case 3:
              price = br.ReadInt32();
              if (price == int.MaxValue) price = br.ReadInt64();
              break;
          }

          switch (bVolume) {
            case 0: lastVolume = 100; break;
            case 1: lastVolume = br.ReadByte(); break;
            case 2: lastVolume = Convert.ToUInt32(br.ReadByte()) * 100; break;
            case 3: lastVolume = br.ReadUInt32(); break;
          }

          switch (bOther) {
            case 1: break;
            case 2: other = 54; break;
            case 3: other = br.ReadUInt16(); break;
          }

          long newPrice = lastPrice + price;
          long newTime = lastTime + secs * timeSign;
          lastCondition = other & 0xFF;
          //          lastStatus = (other >> 8) & 0x03;
          lastType = (other >> 10) & 0x03;

          DataFormat.MbtTickHttp tick = new DataFormat.MbtTickHttp(++recs, new DateTime(newTime * C.cTicksInSecond), newPrice * dPriceFactor,
            lastVolume, lastCondition, lastType);
          data.Add(tick);

          lastPrice = newPrice;
          lastTime = newTime;
        }

      }

    }
  }
}
