using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGWnd.Quote.Models
{
    public class IntradaySnapshot
    {
        public string Symbol;
        public DateTime Date;
        public byte[] Snapshot;
    }
}
