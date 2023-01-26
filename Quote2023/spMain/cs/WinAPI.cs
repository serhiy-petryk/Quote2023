using System;
using System.Net;
using System.Text;
using System.Runtime.InteropServices;

namespace spMain {
  class csWinApi {

    #region ***********************  Moveable control ************************
    //http://www.codeproject.com/KB/miscctrl/DragExtender.aspx
    public const int WM_SYSCOMMAND = 0x112;
    public const int MOUSE_MOVE = 0xF012;
    [DllImport("User32.dll")]
    public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, ref int lParam);

    [DllImport("User32.dll")]
    public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int[] lParam);
    [DllImport("user32")]
    public static extern int ReleaseCapture(IntPtr hwnd);

    /*    using (bug == ZOrder is changing)
        private void UserControl1_MouseDown(object sender, MouseEventArgs e) {
          if (!DesignMode) {
            Control control = sender as Control;
            csWinApi.ReleaseCapture(control.Handle);
            int nul = 0;
            csWinApi.SendMessage(control.Handle, csWinApi.WM_SYSCOMMAND, csWinApi.MOUSE_MOVE, ref nul);
          }
        }*/
    #endregion

    #region ***********   Keyboard Key state **************
    [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
    private static extern short GetKeyState(int keyCode);

    public static bool IsKeyPress(System.Windows.Forms.Keys key) {
      short sh = GetKeyState(Convert.ToInt32(key));
      return sh < 0;
    }
    #endregion

    #region ******************   System Time ***************
    [StructLayout(LayoutKind.Sequential)]
    private struct SYSTEMTIME {
      public short wYear;
      public short wMonth;
      public short wDayOfWeek;
      public short wDay;
      public short wHour;
      public short wMinute;
      public short wSecond;
      public short wMilliseconds;
    }

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool SetSystemTime([In] ref SYSTEMTIME st);

    public static void SetNewSytemTime(DateTime dt) {
      // Change the system time
      SYSTEMTIME SysTime = new SYSTEMTIME();
      SysTime.wYear = (short)dt.Year;
      SysTime.wMonth = (short)dt.Month;
      SysTime.wDay = (short)dt.Day;
      SysTime.wHour = (short)dt.Hour;
      SysTime.wMinute = (short)dt.Minute;
      SysTime.wSecond = (short)dt.Second;
      SysTime.wMilliseconds = (short)dt.Millisecond;
      SetSystemTime(ref SysTime);
    }
    #endregion

    #region *************** Internet Cookies ******************

    [DllImport("wininet.dll", SetLastError = true)]
    private static extern bool InternetGetCookie(string url, string cookieName, StringBuilder cookieData, ref int size);

    public static CookieContainer GetUriCookieContainer(string url) {
      // Taken from www.pinvoke.net
      int datasize = 256;
      StringBuilder cookieData = new StringBuilder(datasize);
      if (!InternetGetCookie(url, null, cookieData, ref datasize)) {
        if (datasize < 0)
          return null;
        // Allocate stringbuilder large enough to hold the cookie
        cookieData = new StringBuilder(datasize);
        if (!InternetGetCookie(url, null, cookieData, ref datasize))
          return null;
      }
      if (cookieData.Length > 0) {
        CookieContainer cookies = new CookieContainer();
        cookies.SetCookies(new Uri(url), cookieData.ToString().Replace(';', ','));
        return cookies;
      }
      else return null;
    }

    #endregion
  }
}
