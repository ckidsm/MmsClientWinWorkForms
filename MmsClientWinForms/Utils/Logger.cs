// Utils/Logger.cs
using System;
using System.IO;

namespace MmsClientWinForms.Utils
{
   public static class Logger
   {
      private static readonly string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "app.log");

      public static void Info(string message)
      {
         WriteLog("INFO", message);
      }

      public static void Error(string message)
      {
         WriteLog("ERROR", message);
      }

      private static void WriteLog(string level, string message)
      {
         try
         {
            var logLine = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}";
            Directory.CreateDirectory(Path.GetDirectoryName(logFilePath)!);
            File.AppendAllText(logFilePath, logLine + Environment.NewLine);
         }
         catch
         {
            // 로그 기록 실패 시 무시
         }
      }
   }
}
