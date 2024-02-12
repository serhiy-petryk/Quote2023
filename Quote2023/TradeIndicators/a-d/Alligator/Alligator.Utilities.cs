using System.Collections.Generic;
using System.Linq;

namespace Skender.Stock.Indicators {

public static partial class Indicator
{
    // CONDENSE (REMOVE null results)
    /// <include file='../../_common/Results/info.xml' path='info/type[@name="Condense"]/*' />
    ///
    public static IEnumerable<AlligatorResult> Condense(this IEnumerable<AlligatorResult> results)
    {
        List<AlligatorResult> resultsList = results.ToList();
        resultsList.RemoveAll((AlligatorResult x) => !x.Jaw.HasValue && !x.Teeth.HasValue && !x.Lips.HasValue);
        return resultsList.ToSortedList();
    }

    // remove recommended periods
    /// <include file='../../_common/Results/info.xml' path='info/type[@name="Prune"]/*' />
    ///
    public static IEnumerable<AlligatorResult> RemoveWarmupPeriods(this IEnumerable<AlligatorResult> results)
    {
        int removePeriods = results.ToList().FindIndex(x => x.Jaw != null) + 251;

        return results.Remove(removePeriods);
    }
}
}
