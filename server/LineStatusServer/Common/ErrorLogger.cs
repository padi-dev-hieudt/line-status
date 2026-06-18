using System;
using System.IO;

namespace LineStatusServer.Common
{
    public class ErrorLogger
    {
        public static void Write(Exception exception)
        {
            try
            {
                DateTime now = DateTime.Now;

                string logsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
                string monthDirectory = Path.Combine(logsDirectory, now.ToString("yyyy-MM"));

                Directory.CreateDirectory(monthDirectory);

                string logFilePath = Path.Combine(monthDirectory, now.ToString("dd") + ".txt");

                string logMessage = $"[{now}] Error: {exception.Message}\n{exception.StackTrace}\n\n";

                using (var stream = new FileStream(logFilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                using (var writer = new StreamWriter(stream))
                {
                    writer.WriteLine(logMessage);
                }
            }
            catch (Exception)
            {
            }
        }


        public static void Write(string t_error)
        {
            try
            {
                DateTime now = DateTime.Now;

                string logsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SaveLogs");
                string monthDirectory = Path.Combine(logsDirectory, now.ToString("yyyy-MM"));

                Directory.CreateDirectory(monthDirectory);

                string logFilePath = Path.Combine(monthDirectory, now.ToString("dd") + ".txt");

                string logMessage = $"[{now}] {t_error}\n\n";

                using (var stream = new FileStream(logFilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                using (var writer = new StreamWriter(stream))
                {
                    writer.WriteLine(logMessage);
                }
            }
            catch (Exception)
            {
            }
        }

    }
}