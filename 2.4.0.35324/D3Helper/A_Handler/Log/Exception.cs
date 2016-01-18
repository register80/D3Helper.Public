using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace D3Helper.A_Handler.Log
{
    public class ExceptionLogEntry
    {
        public ExceptionLogEntry(System.Exception e, DateTime timestamp, A_Enums.ExceptionThread exceptionthread)
        {
            this.E = e;
            this.Timestamp = timestamp;
            this.ExceptionThread = exceptionthread;
        }

        public System.Exception E { get; set; }
        public DateTime Timestamp { get; set; }
        public A_Enums.ExceptionThread ExceptionThread { get; set; }
    }
    public class LogEntry
    {
        public LogEntry(DateTime timestamp, string text)
        {
            
            this.Timestamp = timestamp;
            this.Text = text;
        }
        
        public DateTime Timestamp { get; set; }
        public string Text { get; set; }
    }
    class Exception
    {
        private static string ExceptionsFilePath = AppDomain.CurrentDomain.BaseDirectory + @"logs\exceptions.txt";
        private static string HandlerFilePath = AppDomain.CurrentDomain.BaseDirectory + @"logs\log_handler.txt";
        private static string LogFolderPath = AppDomain.CurrentDomain.BaseDirectory + @"logs";

        public static List<ExceptionLogEntry> ExceptionLog = new List<ExceptionLogEntry>();
        public static List<LogEntry> HandlerLog = new List<LogEntry>();

        public static int ExceptionCount = 0;

        public static void log_Exceptions()
        {
            try
            {
                List<ExceptionLogEntry> ExceptionsToWrite = new List<ExceptionLogEntry>();

                lock(ExceptionLog)
                {
                    foreach(var entry in ExceptionLog.ToList())
                    {
                        ExceptionsToWrite.Add(entry);

                        ExceptionLog.Remove(entry);
                    }
                }

                string WriteToFile = String.Empty;

                foreach(var entry in ExceptionsToWrite)
                {
                    WriteToFile = WriteToFile + entry.Timestamp.ToString("yyyy-MM-dd HH:mm:ss,ffff") + "\t" + entry.ExceptionThread.ToString() + "\t" + entry.E.Message + "\t" + entry.E.StackTrace + Environment.NewLine;

                    ExceptionCount++;
                }

                if (!Directory.Exists(LogFolderPath)) Directory.CreateDirectory(LogFolderPath);

                File.AppendAllText(ExceptionsFilePath, WriteToFile);
            }
            catch (System.Exception e)
            {
                A_Handler.Log.ExceptionLogEntry newEntry = new A_Handler.Log.ExceptionLogEntry(e, DateTime.Now, A_Enums.ExceptionThread.Handler);

                lock (A_Handler.Log.Exception.ExceptionLog) A_Handler.Log.Exception.ExceptionLog.Add(newEntry);
            }
        }
        public static void log_Handler()
        {
            try
            {
                List<LogEntry> HandlerLogsToWrite = new List<LogEntry>();

                lock (HandlerLog)
                {
                    foreach (var entry in HandlerLog.ToList())
                    {
                        HandlerLogsToWrite.Add(entry);

                        HandlerLog.Remove(entry);
                    }
                }

                string WriteToFile = String.Empty;

                foreach (var entry in HandlerLogsToWrite)
                {
                    WriteToFile = WriteToFile + entry.Timestamp.ToString("yyyy-MM-dd HH:mm:ss,ffff") + "\t" + entry.Text + Environment.NewLine;

                    //ExceptionCount++;
                }

                if (!Directory.Exists(LogFolderPath)) Directory.CreateDirectory(LogFolderPath);

                File.AppendAllText(HandlerFilePath, WriteToFile);
            }
            catch (System.Exception e)
            {
                A_Handler.Log.ExceptionLogEntry newEntry = new A_Handler.Log.ExceptionLogEntry(e, DateTime.Now, A_Enums.ExceptionThread.Handler);

                lock (A_Handler.Log.Exception.ExceptionLog) A_Handler.Log.Exception.ExceptionLog.Add(newEntry);
            }
        }
    }
}
