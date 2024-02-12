using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace Skender.Stock.Indicators
{

    // QUOTE UTILITIES

    public static partial class QuoteUtility
    {
        private static readonly CultureInfo NativeCulture = Thread.CurrentThread.CurrentUICulture;

        /* STANDARD DECIMAL QUOTES */

        // convert TQuotes to basic double tuple list
        /// <include file='./info.xml' path='info/type[@name="UseCandlePart"]/*' />
        ///
        public static IEnumerable<(DateTime Date, double Value)> Use<TQuote>(this IEnumerable<TQuote> quotes, CandlePart candlePart = CandlePart.Close)
            where TQuote : IQuote => quotes.Select(x => x.ToTuple(candlePart));

        // TUPLE QUOTES

        // convert quotes to tuple list
        public static Collection<(DateTime, double)> ToTupleCollection<TQuote>(
            this IEnumerable<TQuote> quotes,
            CandlePart candlePart)
            where TQuote : IQuote
            => quotes
                .ToTuple(candlePart)
                .ToCollection();

        internal static List<(DateTime, double)> ToTuple<TQuote>(
            this IEnumerable<TQuote> quotes,
            CandlePart candlePart)
            where TQuote : IQuote => quotes
                .OrderBy(x => x.Date)
                .Select(x => x.ToTuple(candlePart))
                .ToList();

        // convert tuples to list, with sorting
        public static Collection<(DateTime, double)> ToSortedCollection(
            this IEnumerable<(DateTime date, double value)> tuples)
            => tuples
                .ToSortedList()
                .ToCollection();

        internal static List<(DateTime, double)> ToSortedList(
            this IEnumerable<(DateTime date, double value)> tuples)
            => tuples
                .OrderBy(x => x.date)
                .ToList();

        // DOUBLE QUOTES

        // convert to quotes in double precision
        internal static List<QuoteD> ToQuoteD<TQuote>(
            this IEnumerable<TQuote> quotes)
            where TQuote : IQuote => quotes
                .Select(x => new QuoteD
                {
                    Date = x.Date,
                    Open = (double)x.Open,
                    High = (double)x.High,
                    Low = (double)x.Low,
                    Close = (double)x.Close,
                    Volume = (double)x.Volume
                })
                .OrderBy(x => x.Date)
                .ToList();

        // convert quoteD list to tuples
        internal static List<(DateTime, double)> ToTuple(
            this List<QuoteD> qdList,
            CandlePart candlePart) => qdList
                .OrderBy(x => x.Date)
                .Select(x => x.ToTuple(candlePart))
                .ToList();

        /* ELEMENTS */

        // convert TQuote element to basic tuple
        internal static (DateTime date, double value) ToTuple<TQuote>(this TQuote q, CandlePart candlePart) where TQuote : IQuote
        {
            switch (candlePart)
            {
                case CandlePart.Open: return (q.Date, (double)q.Open);
                case CandlePart.High: return (q.Date, (double)q.High);
                case CandlePart.Low: return (q.Date, (double)q.Low);
                case CandlePart.Close: return (q.Date, (double)q.Close);
                case CandlePart.Volume: return (q.Date, (double)q.Volume);
                case CandlePart.HL2: return (q.Date, (double)(q.High + q.Low) / 2.0);
                case CandlePart.HLC3: return (q.Date, (double)(q.High + q.Low + q.Close) / 3.0);
                case CandlePart.OC2: return (q.Date, (double)(q.Open + q.Close) / 2.0);
                case CandlePart.OHL3: return (q.Date, (double)(q.Open + q.High + q.Low) / 3.0);
                case CandlePart.OHLC4: return (q.Date, (double)(q.Open + q.High + q.Low + q.Close) / 4.0);
                default: throw new ArgumentOutOfRangeException("candlePart", candlePart, "Invalid candlePart provided.");
            }
        }

        // convert TQuote element to basic double class
        internal static BasicData ToBasicData<TQuote>(this TQuote q, CandlePart candlePart) where TQuote : IQuote
        {
            switch (candlePart)
            {
                case CandlePart.Open:
                    return new BasicData
                    {
                        Date = q.Date,
                        Value = (double)q.Open
                    };
                case CandlePart.High:
                    return new BasicData
                    {
                        Date = q.Date,
                        Value = (double)q.High
                    };
                case CandlePart.Low:
                    return new BasicData
                    {
                        Date = q.Date,
                        Value = (double)q.Low
                    };
                case CandlePart.Close:
                    return new BasicData
                    {
                        Date = q.Date,
                        Value = (double)q.Close
                    };
                case CandlePart.Volume:
                    return new BasicData
                    {
                        Date = q.Date,
                        Value = (double)q.Volume
                    };
                case CandlePart.HL2:
                    return new BasicData
                    {
                        Date = q.Date,
                        Value = (double)(q.High + q.Low) / 2.0
                    };
                case CandlePart.HLC3:
                    return new BasicData
                    {
                        Date = q.Date,
                        Value = (double)(q.High + q.Low + q.Close) / 3.0
                    };
                case CandlePart.OC2:
                    return new BasicData
                    {
                        Date = q.Date,
                        Value = (double)(q.Open + q.Close) / 2.0
                    };
                case CandlePart.OHL3:
                    return new BasicData
                    {
                        Date = q.Date,
                        Value = (double)(q.Open + q.High + q.Low) / 3.0
                    };
                case CandlePart.OHLC4:
                    return new BasicData
                    {
                        Date = q.Date,
                        Value = (double)(q.Open + q.High + q.Low + q.Close) / 4.0
                    };
                default: throw new ArgumentOutOfRangeException("candlePart", candlePart, "Invalid candlePart provided.");
            }
        }

        // convert quoteD element to basic tuple
        internal static (DateTime, double) ToTuple(this QuoteD q, CandlePart candlePart)
        {
            switch (candlePart)
            {
                case CandlePart.Open: return (q.Date, q.Open);
                case CandlePart.High: return (q.Date, q.High);
                case CandlePart.Low: return (q.Date, q.Low);
                case CandlePart.Close: return (q.Date, q.Close);
                case CandlePart.Volume: return (q.Date, q.Volume);
                case CandlePart.HL2: return (q.Date, (q.High + q.Low) / 2.0);
                case CandlePart.HLC3: return (q.Date, (q.High + q.Low + q.Close) / 3.0);
                case CandlePart.OC2: return (q.Date, (q.Open + q.Close) / 2.0);
                case CandlePart.OHL3: return (q.Date, (q.Open + q.High + q.Low) / 3.0);
                case CandlePart.OHLC4: return (q.Date, (q.Open + q.High + q.Low + q.Close) / 4.0);
                default: throw new ArgumentOutOfRangeException("candlePart", candlePart, "Invalid candlePart provided.");
            }
        }
    }
}
