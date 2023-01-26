using System;

namespace spMain
{
    class csUtils
    {
        public static string StringFromObject(object o)
        {
            if (o == null) return "";
            if (o is DateTime) return StringFromDateTime((DateTime)o);
            else if (o is double) return ((double)o).ToString(csIni.fiNumberUS);
            else if (o is decimal) return ((decimal)o).ToString(csIni.fiNumberUS);
            else if (o is float) return ((float)o).ToString(csIni.fiNumberUS);
            else return o.ToString();
        }

        public static string StringFromDateTime(DateTime date)
        {
            if (date.TimeOfDay == TimeSpan.Zero) return date.ToString("yyyy-MM-dd");
            else return date.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string FormattedStringFromObject(object o)
        {
            if (o == null) return "";
            else if (o is DateTime)
            {
                DateTime d = (DateTime)o;
                if (d.TimeOfDay == TimeSpan.Zero) return d.ToString("yyyy-MM-dd");
                else return d.ToString("HH:mm");
            }
            else if (o is double)
            {
                return NormilizeDecimalString(((double)o).ToString("R", csIni.fiNumberUS), csIni.fiNumberUS.NumberDecimalSeparator);
            }
            else return o.ToString();
        }

        static string NormilizeDecimalString(string s, string decimalSeparator)
        {
            // Remove last zeros("0") in decimal parts of number
            int k = s.IndexOf(decimalSeparator);
            if (k >= 0)
            {
                k = 0;
                for (int i = s.Length - 1; i >= 0; i--)
                {
                    if (s.Substring(i, 1) == "0") k++;
                    else if (s.Substring(i, 1) == decimalSeparator)
                    {
                        k++;
                        break;
                    }
                    else break;
                }
                if (k > 0) return s.Substring(0, s.Length - k);
            }
            return s;
        }

        // taken from "C# 4.0 in a Nutshell" (Joseph Albahari and Ben Albahari)
        public static long MemoryUsedInKB
        {
            get
            {
                // clear memory
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                //
                return GC.GetTotalMemory(true) / 1000;
            }
        }
        public static long MemoryUsedInBytes
        {
            get
            {
                // clear memory
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                //
                return GC.GetTotalMemory(true);
            }
        }
    }
}
