using System;

namespace DGWnd.Quote.Models
{
    public class IntradaySnapshot
    {
        public string Symbol;
        public DateTime Date;
        public byte[] Snapshot;
    }
}
