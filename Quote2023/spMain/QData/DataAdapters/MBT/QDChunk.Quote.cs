using System;
using System.IO;
using System.Collections.Generic;
using spMain.QData.DataFormat;

namespace spMain.QData.DataAdapters.MBT {
  public partial class QDChunk {

    List<Quote> Read_Quote(BinaryReader br) {

      long lastClose = this._uStartPrice;
      long lastTime = this._uLastTime + C.offsetDateTimeInSecs;
      long kDP = 1;
      for (int i1 = 0; i1 < this._dp; i1++) kDP *= 10;
      lastClose = lastClose * (C.startPriceFactor / kDP);
      long priceFactor = C.NOD(lastClose, C.startPriceFactor);
      lastClose /= priceFactor;

      List<Quote> data = new List<Quote>();
      int timeSign = 1;
      double dPriceFactor = priceFactor * 1.0 / C.startPriceFactor;
      long timeFactor = C.NOD(lastTime, C.cStartTimeFactor);
      lastTime = lastTime / timeFactor;


      while (true) {
        Byte b1 = br.ReadByte();
        int bSecs, bOpen, bHigh, bLow, bClose, bVolume;
        if (b1 < 128) {// short format
          //            byte b1 = Convert.ToByte((bVolume << 5) | (bSecs << 4) | (bOpen << 3) | (bHigh << 2) | (bLow << 1) | bClose);
          bVolume = (b1 & 0x60) >> 5;
          bSecs = (b1 & 0x10) >> 4;
          bOpen = (b1 & 0x08) >> 3;
          bHigh = (b1 & 0x04) >> 2;
          bLow = (b1 & 0x02) >> 1;
          bClose = b1 & 0x01;
        }
        else if (b1 == 0xAF) {// chunk end
          return data;
          //          break; 
        }
        else if (b1 == 0xBF) {// time sign
          timeSign = -1;
          continue;
        }
        else if (b1 < 0xc0) {// full format
          bVolume = (b1 & 0x30) >> 4;
          int i1 = (b1 & 0x0F) * 256 + br.ReadByte();
          bSecs = i1 / 625; i1 = i1 - bSecs * 625;
          bOpen = i1 / 125; i1 = i1 - bOpen * 125;
          bHigh = i1 / 25; i1 = i1 - bHigh * 25;
          bLow = i1 / 5; i1 = i1 - bLow * 5;
          bClose = i1;
        }
        else {// factors
          long k = 0;
          if (b1 < 0xe0) {// 1 byte 
            k = ((b1 & 0x0f) << 8) + br.ReadByte();
          }
          else {// 2 bytes
            k = ((b1 & 0x0f) << 16) + br.ReadUInt16();
          }
          if ((b1 & 0x10) == 0) {// time
            timeFactor /= k;
            lastTime *= k;
          }
          else {// price
            priceFactor /= k;
            lastClose *= k;
            dPriceFactor = priceFactor * 1.0 / C.startPriceFactor;
          }
          continue;
        }

        long secs = 0, open = 0, high = 0, low = 0, close = 0;
        ulong volume = 0;

        switch (bSecs) {
          case 0: break;
          case 1: secs = br.ReadByte(); break;
          case 2: secs = br.ReadByte() + 256; break;
          case 3: secs = br.ReadUInt16(); break;
          case 4: secs = br.ReadUInt32(); break;
        }
        secs += (data.Count == 0 ? 0 : 1);
        if (timeSign == -1) secs = -secs;

        switch (bOpen) {
          case 0: break;
          case 1: open = br.ReadSByte(); break;
          case 2:
            sbyte sb = br.ReadSByte();
            if (sb >= 0) open = sb + 128;
            else open = sb - 128;
            break;
          case 3: open = br.ReadInt16(); break;
          case 4: open = br.ReadInt32(); break;
        }

        switch (bHigh) {
          case 0: break;
          case 1: high = br.ReadByte(); break;
          case 2: high = Convert.ToInt64(br.ReadByte()) + 256; break;
          case 3: high = br.ReadUInt16(); break;
          case 4: high = br.ReadUInt32(); break;
        }

        switch (bLow) {
          case 0: break;
          case 1: low = br.ReadByte(); break;
          case 2: low = Convert.ToInt64(br.ReadByte()) + 256; break;
          case 3: low = br.ReadUInt16(); break;
          case 4: low = br.ReadUInt32(); break;
        }

        switch (bClose) {
          case 0: break;
          case 1: close = br.ReadByte(); break;
          case 2: close = Convert.ToInt64(br.ReadByte()) + 256; break;
          case 3: close = br.ReadUInt16(); break;
          case 4: close = br.ReadUInt32(); break;
        }

        switch (bVolume) {
          case 0: volume = br.ReadByte(); break;
          case 1: volume = br.ReadUInt16(); break;
          case 2: volume = br.ReadUInt32(); break;
          case 3: volume = br.ReadUInt64(); break;
        }
        open += lastClose;
        high += open;
        low = open - low;
        close += low;

        lastTime += secs;
        lastClose = close;

        Quote q = new Quote(new DateTime(lastTime * timeFactor * C.cTicksInSecond), open * dPriceFactor,
          high * dPriceFactor, low * dPriceFactor, close * dPriceFactor, Convert.ToInt64(volume));
        data.Add(q);

      }



    }
  }
}
