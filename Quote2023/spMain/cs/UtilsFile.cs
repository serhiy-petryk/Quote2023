using System;
using System.IO;
using System.Text;

namespace spMain {

  class csUtilsFile {

    public static string GetFileName(string filenameTemplate, string symbol, DateTime localTime) {
      if (String.IsNullOrEmpty(filenameTemplate)) return null;
      DateTime date = (localTime.TimeOfDay == TimeSpan.Zero ? localTime : localTime);
      string fn = filenameTemplate.Replace("{S}", symbol).Replace("{s}", symbol).
        Replace("{D}", date.ToString("yyyy-MM-dd")).Replace("{d}", date.ToString("yyyy-MM-dd")).
        Replace("{Y}", date.ToString("yyyy")).Replace("{y}", date.ToString("yyyy"));
      string folder = Path.GetDirectoryName(fn);
      if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
      return fn;
    }

    public static string GetStringFromStream(Stream stream, Encoding encoding) {
      stream.Position = 0;
      int bufferSize = 1024 * 8;
      byte[] bb = new byte[bufferSize];
      StringBuilder sb = new StringBuilder();
      long bytes = 0;
      int cnt = 0;
      do {
        cnt = stream.Read(bb, 0, bufferSize);
        if (cnt > 0) {
          sb.Append(encoding.GetString(bb, 0, cnt));
          bytes += cnt;
        }
      }
      while (cnt > 0);
      return sb.ToString();
    }

    public static long SaveStreamToFile(Stream stream, string fileName) {
      int bufferSize = 1024 * 8;
      byte[] bb = new byte[bufferSize];
      long bytes = 0;
      int cnt = 0;
      using (FileStream fs = new FileStream(fileName, FileMode.CreateNew)) {
        do {
          cnt = stream.Read(bb, 0, bufferSize);
          if (cnt > 0) {
            fs.Write(bb, 0, cnt);
            bytes += cnt;
          }
        }

        while (cnt > 0);
        fs.Close();
      }
      return bytes;
    }

    public static string RenameFile(string filename, string prefix) {
      string folder = Path.GetDirectoryName(filename);
      string fn = Path.GetFileName(filename);
      string newFN = GetNearestNewFileName(folder, prefix + "." + fn);
      File.Move(filename, newFN);
      return newFN;
    }

    public static string GetNearestNewFileName(string path, string fileName) {
      string sPath;
      sPath = (path.EndsWith(@"\") ? path : path + @"\");
      if (!File.Exists(sPath + fileName)) return (sPath + fileName);
      int t = fileName.LastIndexOf(".");
      string s1;
      if (t > 0)
        s1 = sPath + fileName.Substring(0, t) + "#{0}." + fileName.Substring(t + 1);
      else {
        s1 = sPath + fileName + "#{0}";
      }
      for (int i = 0; i < 1000; i++) {
        string s2 = String.Format(s1, i.ToString());
        if (!File.Exists(s2)) return s2;
      }
      return "";
    }
    public static string GetNearestNewFolder(string path) {
      string sPath;
      sPath = (path.EndsWith(@"\") ? path.Substring(0, path.Length - 1) : path);
      if (!Directory.Exists(sPath)) return (sPath + @"\");
      for (int i = 0; i < 1000; i++) {
        if (!Directory.Exists(sPath + "#" + i.ToString()))
          return sPath + "#" + i.ToString() + @"\";
      }
      return "";
    }

    /*public static string GetFileContext(string filename) {
      return (new EPocalipse.IFilter.FilterReader(filename)).ReadToEnd();
    }*/

    public static string CompareTwoFiles(string fn1, string fn2) {
      FileInfo fi1 = new FileInfo(fn1);
      FileInfo fi2 = new FileInfo(fn2);
      string errLen = (fi1.Length == fi2.Length ? null : "Different length of files: " + fi1.Length + " and " + fi2.Length);
      byte[] bb1 = File.ReadAllBytes(fn1);
      byte[] bb2 = File.ReadAllBytes(fn2);
      int count = Convert.ToInt32(Math.Min(fi1.Length, fi2.Length));
      for (int i = 0; i < count; i++) {
        if (bb1[i] != bb2[i]) {
          if (errLen == null) {
            return "Position: 0x" + i.ToString("X") + ". Byte1: 0x" + bb1[i].ToString("X") + ", byte2: 0x" + bb2[i].ToString("X");
          }
          else {
            return errLen + ". Position: 0x" + i.ToString("X") + ". Byte1: 0x" + bb1[i].ToString("X") + ", byte2: 0x" + bb2[i].ToString("X");
          }
        }
      }
      return errLen;

      int cnt = 0;
      using (FileStream fs1 = new FileStream(fn1, FileMode.Open, FileAccess.Read)) {
        using (BinaryReader br1 = new BinaryReader(fs1)) {
          using (FileStream fs2 = new FileStream(fn2, FileMode.Open, FileAccess.Read)) {
            using (BinaryReader br2 = new BinaryReader(fs2)) {
              while (cnt < fi1.Length) {
                cnt++;
                byte b1 = br1.ReadByte();
                byte b2 = br2.ReadByte();
                if (b1 != b2) {
                  return "Position: 0x" + cnt.ToString("X") + ". Byte1: 0x" + b1.ToString("X") + ", byte2: 0x" + b2.ToString("X");
                }
              }
            }
          }
        }
      }
      return null;
    }


  }
}
