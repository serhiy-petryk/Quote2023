using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spMain.Helpers
{
    public static class CsHelper
    {
        public static int SynRunCmd(string cmdLine, string workingDirectory)
        {
            ProcessStartInfo psi = new ProcessStartInfo("cmd.exe", "/C " + cmdLine);
            psi.WorkingDirectory = workingDirectory;
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            Process pr = Process.Start(psi);
            pr.WaitForExit();
            int i = pr.ExitCode;
            pr.Close();
            return i;
        }

    }
}
