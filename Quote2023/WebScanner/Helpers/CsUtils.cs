﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScanner.Helpers
{
    public static class CsUtils
    {
        public static string GetString(object o)
        {
            if (o is DateTime dt)
                return dt.ToString(dt.TimeOfDay == TimeSpan.Zero ? "yyyy-MM-dd" : "yyyy-MM-dd HH:mm");
            else if (o is TimeSpan ts)
                return ts.ToString("hh\\:mm");
            else if (Equals(o, null)) return null;
            return o.ToString();
        }

        public static DateTime GetNextWeekday(DateTime start, DayOfWeek day)
        {
            // From https://stackoverflow.com/questions/6346119/compute-the-datetime-of-an-upcoming-weekday
            // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
            var daysToAdd = ((int)day - (int)start.DayOfWeek + 7) % 7;
            return start.AddDays(daysToAdd).Date;
        }

        public static DateTime GetPreviousWeekday(DateTime start, DayOfWeek day) => GetNextWeekday(start, day).AddDays(-7);

        public static int GetFileSizeInKB(string filename) => Convert.ToInt32(new FileInfo(filename).Length / 1024.0);

        public static Tuple<DateTime, string, string> GetTimeStamp(int hourOffset = -9) =>
            new Tuple<DateTime, string, string>(DateTime.Now.AddHours(hourOffset),
                DateTime.Now.AddHours(hourOffset).ToString("yyyyMMdd"),
                DateTime.Now.AddHours(0).ToString("yyyyMMddHHmmss"));

        public static long MemoryUsedInBytes
        {
            get
            {
                // clear memory
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                return GC.GetTotalMemory(true);
            }
        }
    }
}
