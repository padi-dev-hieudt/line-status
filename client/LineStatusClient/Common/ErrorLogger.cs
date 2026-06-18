using System;
using System.IO;

namespace LineStatusClient.Common
{
    public class ErrorLogger
    {
        private static readonly object _logLock = new object();

        public static void Write(Exception exception)
        {
            string message = $"Error: {exception.Message}\n{exception.StackTrace}";
            WriteLog("Logs", message);
        }

        public static void SaveLog(string title, string error)
        {
            string message = $"{title}:\n{error}";
            WriteLog("SaveLogs", message);
        }

        private static void WriteLog(string folderName, string message)
        {
            try
            {
                DateTime now = DateTime.Now;
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string logsDirectory = Path.Combine(baseDirectory, folderName, now.ToString("yyyy-MM"));
                string logFilePath = Path.Combine(logsDirectory, $"{now:dd}.txt");

                Directory.CreateDirectory(logsDirectory);

                string logEntry = $"[{now:yyyy-MM-dd HH:mm:ss}] {message}\n\n";

                lock (_logLock)
                {
                    File.AppendAllText(logFilePath, logEntry);
                }
            }
            catch
            {
                // Không ghi log lỗi ghi log để tránh vòng lặp
            }
        }

    }
}