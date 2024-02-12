using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Skender.Stock.Indicators
{

    // RESULTS UTILITIES

    public static partial class ResultUtility
    {
        // CONDENSE (REMOVE null and NaN results)
        /// <include file='./info.xml' path='info/type[@name="CondenseT"]/*' />
        ///
        public static IEnumerable<TResult> Condense<TResult>(this IEnumerable<TResult> results) where TResult : IReusableResult
        {
            List<TResult> resultsList = results.ToList();
            resultsList.RemoveAll((TResult x) =>
            {
                double? value = x.Value;
                if (value.HasValue)
                {
                    double valueOrDefault = value.GetValueOrDefault();
                    if (!double.IsNaN(valueOrDefault))
                    {
                        return false;
                    }
                }
                return true;
            });
            return resultsList.ToSortedList();
        }

        // CONVERT TO TUPLE (default with pruning)
        /// <include file='./info.xml' path='info/type[@name="TupleChain"]/*' />
        ///
        public static Collection<(DateTime Date, double Value)> ToTupleChainable(this IEnumerable<IReusableResult> reusable)
            => reusable.ToTuple().ToCollection();

        internal static List<(DateTime Date, double Value)> ToTuple(this IEnumerable<IReusableResult> reusable)
        {
            List<(DateTime date, double value)> prices = new List<(DateTime date, double value)>();
            List<IReusableResult> reList = reusable.ToList();

            // find first non-nulled
            int first = reList.FindIndex(x => x.Value != null);

            for (int i = first; i < reList.Count; i++)
            {
                IReusableResult r = reList[i];
                prices.Add((r.Date, r.Value.Null2NaN()));
            }

            return prices.OrderBy(x => x.date).ToList();
        }

        // CONVERT TO TUPLE with non-nullable NaN value option and no pruning
        /// <include file='./info.xml' path='info/type[@name="TupleNaN"]/*' />
        ///
        public static Collection<(DateTime Date, double Value)> ToTupleNaN(
            this IEnumerable<IReusableResult> reusable)
        {
            List<IReusableResult> reList = reusable.ToSortedList();
            int length = reList.Count;

            Collection<(DateTime Date, double Value)> results = new Collection<(DateTime Date, double Value)>();

            for (int i = 0; i < length; i++)
            {
                IReusableResult r = reList[i];
                results.Add((r.Date, r.Value.Null2NaN()));
            }

            return results;
        }
    }
}
