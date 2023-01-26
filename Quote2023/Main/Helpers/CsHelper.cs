using System;

namespace Main.Helpers
{
    public static class CsHelper
    {
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
