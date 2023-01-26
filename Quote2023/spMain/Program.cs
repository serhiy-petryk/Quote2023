using System;
using System.Windows.Forms;

namespace spMain
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var s = csIni.GetPathExe();

            Application.Run(new Comp.frmUIStockGraph());
        }
    }
}
