using System.Windows.Forms;

namespace DGWnd.Quote.Helpers
{
    public static class CsHelper
    {
        public static string OpenFileDialogGeneric(string folder, string filter)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.InitialDirectory = folder;
                ofd.RestoreDirectory = true;
                ofd.Multiselect = false;
                ofd.Filter = filter;
                if (ofd.ShowDialog() == DialogResult.OK)
                    return ofd.FileName;
                return null;
            }
        }

        public static string[] OpenFileDialogMultiselect(string folder, string filter)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.InitialDirectory = folder;
                ofd.RestoreDirectory = true;
                ofd.Multiselect = true;
                ofd.Filter = filter;
                if (ofd.ShowDialog() == DialogResult.OK)
                    return ofd.FileNames;
                return null;
            }
        }

    }
}
