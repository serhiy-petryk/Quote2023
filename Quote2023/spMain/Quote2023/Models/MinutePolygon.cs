using System;

namespace spMain.Quote2023.Models
{
    public class MinutePolygon
    {
        public class cMinuteRoot
        {
            public string ticker;
            public int queryCount;
            public int resultsCount;
            public int count;
            public bool adjusted;
            public string status;
            public string next_url;
            public string request_id;

            public cMinuteItem[] results;

            // public string Symbol => PolygonCommon.GetMyTicker(ticker);
            public string Symbol => ticker;
        }

        public class cMinuteItem
        {
            public long t;
            public float o;
            public float h;
            public float l;
            public float c;
            public long v;
            public float vw;
            public int n;

            public DateTime DateTime => GetEstDateTimeFromUnixSeconds(t / 1000);
            public float Open => o;
            public float High => h;
            public float Low => l;
            public float Close => c;
            public long Volume => v;
            public float WeightedVolume => vw;
            public int TradeCount => n;

            private static readonly TimeZoneInfo EstTimeZone =
                TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

            public static DateTime GetEstDateTimeFromUnixSeconds(long unixTimeInSeconds)
            {
                var aa2 = DateTimeOffset.FromUnixTimeSeconds(unixTimeInSeconds);
                var aa3 = aa2 + EstTimeZone.GetUtcOffset(aa2);
                return aa3.DateTime;
            }
        }
    }
}
