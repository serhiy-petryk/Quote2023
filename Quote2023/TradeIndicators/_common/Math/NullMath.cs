using System;

namespace Skender.Stock.Indicators
{

    // NULLABLE SYSTEM.MATH
    // System.Math does not allow or handle null input values.
    // Instead of putting a lot of inline defensive code
    // we're building nullable equivalents here.
    public static class NullMath
    {
        public static double? Abs(this double? value)
        {
            return (!value.HasValue) ? null : new double?((value < 0.0) ? (0.0 - value).Value : value.Value);
        }

        public static decimal? Round(this decimal? value, int digits)
        {
            return (!value.HasValue) ? null : new decimal?(Math.Round(value.Value, digits));
        }

        public static double? Round(this double? value, int digits)
        {
            return (!value.HasValue) ? null : new double?(Math.Round(value.Value, digits));
        }

        public static double Round(this double value, int digits)
        {
            return Math.Round(value, digits);
        }

        public static decimal Round(this decimal value, int digits)
        {
            return Math.Round(value, digits);
        }

        public static double Null2NaN(this double? value)
        {
            return (!value.HasValue) ? double.NaN : value.Value;
        }

        public static double? NaN2Null(this double? value)
        {
            return (value.HasValue && double.IsNaN(value.GetValueOrDefault())) ? null : value;
        }

        public static double? NaN2Null(this double value)
        {
            return double.IsNaN(value) ? null : new double?(value);
        }
    }
}
