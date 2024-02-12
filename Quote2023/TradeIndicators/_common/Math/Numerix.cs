using System;

namespace Skender.Stock.Indicators
{
    public static class Numerix
    {
        // STANDARD DEVIATION
        public static double StdDev(this double[] values)
        {
            // validate parameters
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values), "StdDev values cannot be null.");
            }

            double sd = 0;
            int n = values.Length;
            if (n > 1)
            {
                double sum = 0;
                for (int i = 0; i < n; i++)
                {
                    sum += values[i];
                }

                double avg = sum / n;

                double sumSq = 0;
                for (int i = 0; i < n; i++)
                {
                    double v = values[i];
                    sumSq += (v - avg) * (v - avg);
                }

                sd = Math.Sqrt(sumSq / n);
            }

            return sd;
        }

        // SLOPE of BEST FIT LINE
        public static double Slope(double[] x, double[] y)
        {
            // validate parameters
            if (x is null)
            {
                throw new ArgumentNullException(nameof(x), "Slope X values cannot be null.");
            }

            if (y is null)
            {
                throw new ArgumentNullException(nameof(y), "Slope Y values cannot be null.");
            }

            if (x.Length != y.Length)
            {
                throw new ArgumentException("Slope X and Y arrays must be the same size.");
            }

            int length = x.Length;

            // get averages for period
            double sumX = 0;
            double sumY = 0;

            for (int i = 0; i < length; i++)
            {
                sumX += x[i];
                sumY += y[i];
            }

            double avgX = sumX / length;
            double avgY = sumY / length;

            // least squares method
            double sumSqX = 0;
            double sumSqY = 0;
            double sumSqXY = 0;

            for (int i = 0; i < length; i++)
            {
                double devX = x[i] - avgX;
                double devY = y[i] - avgY;

                sumSqX += devX * devX;
                sumSqY += devY * devY;
                sumSqXY += devX * devY;
            }

            double slope = sumSqXY / sumSqX;

            return slope;
        }

        // DATE ROUNDING
        internal static DateTime RoundDown(this DateTime dateTime, TimeSpan interval)
            => interval == TimeSpan.Zero
            ? dateTime
            : dateTime
                .AddTicks(-(dateTime.Ticks % interval.Ticks));

        // PERIOD-SIZE to TIMESPAN CONVERSION
        internal static TimeSpan ToTimeSpan(this PeriodSize periodSize)
        {
            switch (periodSize)
            {
                case PeriodSize.OneMinute: return TimeSpan.FromMinutes(1.0);
                case PeriodSize.TwoMinutes: return TimeSpan.FromMinutes(2.0);
                case PeriodSize.ThreeMinutes: return TimeSpan.FromMinutes(3.0);
                case PeriodSize.FiveMinutes: return TimeSpan.FromMinutes(5.0);
                case PeriodSize.FifteenMinutes: return TimeSpan.FromMinutes(15.0);
                case PeriodSize.ThirtyMinutes: return TimeSpan.FromMinutes(30.0);
                case PeriodSize.OneHour: return TimeSpan.FromHours(1.0);
                case PeriodSize.TwoHours: return TimeSpan.FromHours(2.0);
                case PeriodSize.FourHours: return TimeSpan.FromHours(4.0);
                case PeriodSize.Day: return TimeSpan.FromDays(1.0);
                case PeriodSize.Week: return TimeSpan.FromDays(7.0);
                default: return TimeSpan.Zero;
            }
        }


        // DETERMINE DECIMAL PLACES
        internal static int GetDecimalPlaces(this decimal n)
        {
            // source: https://stackoverflow.com/a/30205131/4496145

            n = Math.Abs(n); // make sure it is positive.
            n -= (int)n;     // remove the integer part of the number.
            int decimalPlaces = 0;
            while (n > 0)
            {
                decimalPlaces++;
                n *= 10;
                n -= (int)n;
            }

            return decimalPlaces;
        }
    }
}
