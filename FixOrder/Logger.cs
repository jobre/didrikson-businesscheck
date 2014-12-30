using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace GCS
{
  public static class Logger
  {
    private static StreamWriter sw;
    private static string mPath = "";

    private static void openLoggFile(string user)
    {
      string directory = AppDomain.CurrentDomain.BaseDirectory + @"\LoggFiles\" + user + @"\";
      mPath = directory + DateTime.Now.ToString("yyyy-MM-dd") + "_" + user + ".gcslogg";

      try
      {
        if (!Directory.Exists(directory))
          Directory.CreateDirectory(directory);

        sw = new StreamWriter(mPath, true, Encoding.Default);
      }
      catch { }
    }

    private static void closeLoggFile()
    {
      try
      {
        sw.Close();
        sw.Dispose();
        sw = null;

        GC.Collect();
      }
      catch { }
    }

    public static void loggError(Exception e, string own_err_desc, string who, string ext_id)
    {
      openLoggFile(who);

      try
      {
        sw.WriteLine(DateTime.Now + " | " + who + " | " + e.TargetSite.ToString() + " | " + @e.Message.Replace("\n", "").Replace("\r", "") + " | " + @e.StackTrace.Replace("\n", "").Replace("\r", "") + " | " + own_err_desc + " | " + ext_id + " | " + "ERROR");
      }
      catch
      {
        closeLoggFile();
      }

      closeLoggFile();
    }

    public static void loggInfo(string info, string who, string ext_id)
    {
      openLoggFile(who);

      try
      {
        sw.WriteLine(DateTime.Now + " | " + who + " | " + "--" + " | " + "--" + " | " + "--" + " | " + info + " | " + ext_id + " | " + "INFO");
      }
      catch
      {
        closeLoggFile();
      }

      closeLoggFile();
    }

    public static void loggWarning(string info, string who, string ext_id)
    {
      openLoggFile(who);

      try
      {
        sw.WriteLine(DateTime.Now + " | " + who + " | " + "--" + " | " + "--" + " | " + "--" + " | " + info + " | " + ext_id + " | " + "WARNING");
      }
      catch
      {
        closeLoggFile();
      }

      closeLoggFile();
    }

    public static void loggCritical(string info, string who, string ext_id)
    {
      openLoggFile(who);

      try
      {
        sw.WriteLine(DateTime.Now + " | " + who + " | " + "--" + " | " + "--" + " | " + "--" + " | " + info + " | " + ext_id + " | " + "CRITICAL");
      }
      catch
      {
        closeLoggFile();
      }

      closeLoggFile();
    }

    public static string getFileName()
    {
      return mPath;
    }
  }
}
