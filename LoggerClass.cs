using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YTUtils.Controls
{
    public class LoggerClass
    {
        /// <summary>
        /// Log队列
        /// </summary>
        public static Queue<LogInfo> logQueue { get; set; } = new Queue<LogInfo>() { };

        public static void WriteLog(string info, bool ShowMsgBox = false, 
            [CallerFilePath] string fileName = "",
            [CallerMemberName] string funcName = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            logQueue.Enqueue(new LogInfo{ message = info, ex = null, logLevel = MsgLevel.Info, fileName = fileName, funcName = funcName, lineNumber = lineNumber});
            if(ShowMsgBox)
            {
                MessageBox.Show(info);
            }
        }
        public static void WriteLog(string info, MsgLevel msgLevel, bool ShowMsgBox = false,
            [CallerFilePath] string fileName = "",
            [CallerMemberName] string funcName = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            logQueue.Enqueue(new LogInfo { message = info, ex = null, logLevel = msgLevel, fileName = fileName, funcName = funcName, lineNumber = lineNumber });
            if (ShowMsgBox)
            {
                MessageBox.Show(info);
            }
        }
        public static void WriteLog(string info, MsgLevel msgLevel, Exception ex, bool ShowMsgBox = false,
            [CallerFilePath] string fileName = "",
            [CallerMemberName] string funcName = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            logQueue.Enqueue(new LogInfo { message = info, ex = ex, logLevel = msgLevel, fileName = fileName, funcName = funcName, lineNumber = lineNumber });
            if (ShowMsgBox)
            {
                MessageBox.Show(info);
            }
        }
        public static void WriteLog(string info, Exception ex, bool ShowMsgBox = false,
            [CallerFilePath] string fileName = "",
            [CallerMemberName] string funcName = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            logQueue.Enqueue(new LogInfo {message = info, ex = ex, logLevel = MsgLevel.Exception, fileName = fileName, funcName = funcName, lineNumber = lineNumber });
            if (ShowMsgBox)
            {
                MessageBox.Show(info);
            }
        }
    }

    public class LogInfo
    {
        public string message { get; set; }
        public MsgLevel logLevel { get; set; }
        public Exception ex { get; set; }
        public string fileName { get; set; }
        public string funcName { get; set; }
        public int lineNumber { get; set; }
    }
}
