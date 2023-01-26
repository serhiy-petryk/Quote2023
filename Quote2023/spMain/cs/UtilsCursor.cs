using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace spMain {
  class csUtilsCursor {
    //================================= Form codeproject(Image Capture) ================================
    public const int SRCCOPY = 0x00CC0020;// 13369376;

    [DllImport("user32.dll", EntryPoint = "GetDC")]
    public static extern IntPtr GetDC(IntPtr ptr);

    [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleBitmap")]
    public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc,
      int nWidth, int nHeight);

    [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleDC")]
    public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

    [DllImport("user32.dll", EntryPoint = "ReleaseDC")]
    public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDc);

    [DllImport("gdi32.dll", EntryPoint = "DeleteObject", CharSet = CharSet.Auto)]
    internal static extern bool DeleteObject(IntPtr handle);

    [DllImport("gdi32.dll", EntryPoint = "DeleteDC")]
    public static extern IntPtr DeleteDC(IntPtr hDc);

    [DllImport("gdi32.dll", EntryPoint = "SelectObject")]
    public static extern IntPtr SelectObject(IntPtr hdc, IntPtr bmp);

    [DllImport("gdi32.dll", EntryPoint = "BitBlt")]
    public static extern bool BitBlt(IntPtr hdcDest, int xDest, int yDest, int wDest, int hDest,
      IntPtr hdcSource, int xSrc, int ySrc, int RasterOp);

    [DllImport("gdi32.dll")]
    static extern bool GdiFlush();

    public static void FromBitmapToControly(Control targetControl, Bitmap sourceBmp, int destX, int destY) {
      /*      IntPtr pTarget = e.Graphics.GetHdc();
            IntPtr pSource = CreateCompatibleDC(pTarget);
            IntPtr pOrig = SelectObject(pSource, bmp.GetHbitmap());
            BitBlt(pTarget, 0, 0, bmp.Width, bmp.Height, pSource, 0, 0, TernaryRasterOperations.SRCCOPY);
            IntPtr pNew = SelectObject(pSource, pOrig);
            DeleteObject(pNew);
            DeleteDC(pSource);
            e.Graphics.ReleaseHdc(pTarget);*/
      using (Graphics g = targetControl.CreateGraphics()) {
        //          g.DrawLine(new Pen(new SolidBrush(Color.Blue), 2), 100, 100, 200, 200);
        IntPtr targetDC = g.GetHdc();
        //        IntPtr targetDC = GetDC(targetControl.Handle);
        IntPtr sourceDC = CreateCompatibleDC(targetDC);
        //IntPtr sourceDC = CreateCompatibleDC(IntPtr.Zero);
        //        IntPtr bmObj = sourceBmp.GetHbitmap();
        //        IntPtr oldObj = SelectObject(sourceDC, bmObj);
        IntPtr oldObj = SelectObject(sourceDC, sourceBmp.GetHbitmap());
        BitBlt(targetDC, destX, destY, sourceBmp.Width, sourceBmp.Height, sourceDC, 0, 0, SRCCOPY);
        //        SelectObject(sourceDC, oldObj);
        IntPtr newObj = SelectObject(sourceDC, oldObj);
        DeleteObject(newObj);
        DeleteObject(oldObj);
        //      DeleteObject(bmObj);
        DeleteDC(sourceDC);
        //        DeleteObject(bmObj);
        g.ReleaseHdc(targetDC);
        //        ReleaseDC(targetControl.Handle, targetDC);
      }
    }

    public static Bitmap FromControlToBitmapy(Control sourceControl, int sourceX, int sourceY, int sourceWidth, int sourceHeight) {
      using (Graphics g = sourceControl.CreateGraphics()) {
        IntPtr sourceDC = g.GetHdc();
        //        IntPtr sourceDC = GetDC(sourceControl.Handle);
        //      IntPtr targetDC = CreateCompatibleDC(sourceDC);
        IntPtr targetDC = CreateCompatibleDC(IntPtr.Zero);
        IntPtr bmObj = csUtilsCursor.CreateCompatibleBitmap(sourceDC, sourceWidth, sourceHeight);
        /*      if (bmObj == IntPtr.Zero) {
                      DeleteDC(targetDC);
                      ReleaseDC(sourceControl.Handle, sourceDC);
                      return null;
                    }*/

        IntPtr oldObj = SelectObject(targetDC, bmObj);
        BitBlt(targetDC, 0, 0, sourceWidth, sourceHeight, sourceDC, sourceX, sourceY, SRCCOPY);
        SelectObject(targetDC, oldObj);

        Bitmap bm = System.Drawing.Image.FromHbitmap(bmObj);
        //          System.Drawing.Imaging.BitmapData data= bm.LockBits(new Rectangle(0,0, sourceWidth, sourceHeight), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb); 
        IntPtr newObj = SelectObject(targetDC, oldObj);
        DeleteObject(newObj);
        DeleteObject(oldObj);

        DeleteDC(targetDC);
        DeleteObject(bmObj);
        //        ReleaseDC(sourceControl.Handle, sourceDC);
        g.ReleaseHdc(sourceDC);
        //      bm.UnlockBits(data);
        return bm;
      }
    }



    public static void FromBitmapToControl(Control targetControl, Bitmap sourceBmp, int destX, int destY) {
      /*   IntPtr pTarget = e.Graphics.GetHdc();
    IntPtr pSource = CreateCompatibleDC(pTarget);
    IntPtr pOrig = SelectObject(pSource, bmp.GetHbitmap());
    BitBlt(pTarget, 0,0, bmp.Width, bmp.Height, pSource,0,0,TernaryRasterOperations.SRCCOPY);
    IntPtr pNew = SelectObject(pSource, pOrig);
    DeleteObject(pNew);
    DeleteDC(pSource);
    e.Graphics.ReleaseHdc(pTarget);*/

      IntPtr targetDC = GetDC(targetControl.Handle);
      //      IntPtr sourceDC = CreateCompatibleDC(targetDC);
      IntPtr sourceDC = CreateCompatibleDC(IntPtr.Zero);
      IntPtr bmObj = sourceBmp.GetHbitmap();
      IntPtr oldObj = SelectObject(sourceDC, bmObj);
      BitBlt(targetDC, destX, destY, sourceBmp.Width, sourceBmp.Height, sourceDC, 0, 0, SRCCOPY);
      //      SelectObject(sourceDC, oldObj);
      IntPtr newObj = SelectObject(sourceDC, oldObj);
      DeleteObject(newObj);
      DeleteObject(oldObj);
      DeleteObject(bmObj);
      DeleteDC(sourceDC);
      DeleteObject(bmObj);
      ReleaseDC(targetControl.Handle, targetDC);
    }

    public static Bitmap FromControlToBitmap(Control sourceControl, int sourceX, int sourceY, int sourceWidth, int sourceHeight) {
      IntPtr sourceDC = GetDC(sourceControl.Handle);
      //      IntPtr targetDC = CreateCompatibleDC(sourceDC);
      IntPtr targetDC = CreateCompatibleDC(IntPtr.Zero);
      IntPtr bmObj = csUtilsCursor.CreateCompatibleBitmap(sourceDC, sourceWidth, sourceHeight);
      /*      if (bmObj == IntPtr.Zero) {
                    DeleteDC(targetDC);
                    ReleaseDC(sourceControl.Handle, sourceDC);
                    return null;
                  }*/

      IntPtr oldObj = SelectObject(targetDC, bmObj);
      BitBlt(targetDC, 0, 0, sourceWidth, sourceHeight, sourceDC, sourceX, sourceY, SRCCOPY);
      //      SelectObject(targetDC, oldObj);
      Bitmap bm = System.Drawing.Image.FromHbitmap(bmObj);

      IntPtr newObj = SelectObject(targetDC, oldObj);
      DeleteObject(newObj);
      DeleteObject(oldObj);
      DeleteDC(targetDC);
      DeleteObject(bmObj);
      ReleaseDC(sourceControl.Handle, sourceDC);
      return bm;
    }
    //==================================================================================================

  }
}
