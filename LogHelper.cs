// -----------------------------------------------------------------------------
// 文件名称：LogHelper.cs
// 作者：彭木德
// 版本：1.0
// 创建日期：2024-3-27
// 描述：这是一个为YTUtils项目提供统一日志管理的助手类，基于log4net库实现。
// 它包含了Info、Debug、Error和Fatal四种级别的日志记录方法，并利用Caller Information特性自动填充日志输出的源文件路径、成员名称及行号。
// 注意：此类在构造时默认读取当前程序的"Config\log4net.config"作为日志配置文件，
// 并提供了ReleaseLogerConfigure方法删除临时生成的配置文件（如果有的话）。
// -----------------------------------------------------------------------------

using log4net;
using log4net.Config;
using System.IO;
using System.Runtime.CompilerServices;

namespace YTUtils.Logger
{
    public class LogHelper
    {
        private LogHelper()
        {
            // 不传参数时默认去读取App.config或者Web.config
            XmlConfigurator.Configure(new FileInfo(@"Config\log4net.config"));
        }

        //创建log对象
        static ILog ilog = LogManager.GetLogger("[null]");
        /// <summary>
        /// 写入一行Info级别的日志
        /// </summary>
        /// <param name="info"></param>
        /// <param name="filePath"></param>
        /// <param name="memberName"></param>
        /// <param name="lineNumber"></param>
        public static void Info(
            string info,
            [CallerFilePath] string filePath = "",
            [CallerMemberName] string memberName = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            
            ilog.Info($"[{filePath}] [{memberName}] [{lineNumber}] - {info}");
        }
        /// <summary>
        /// 写入一行Debug级别的日志
        /// </summary>
        /// <param name="info"></param>
        /// <param name="filePath"></param>
        /// <param name="memberName"></param>
        /// <param name="lineNumber"></param>
        public static void Debug(
        string info,
        [CallerFilePath] string filePath = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0)
        {

            ilog.Debug($"[{filePath}] [{memberName}] [{lineNumber}] - {info}");
        }
        /// <summary>
        /// 写入一行Error级别的日志
        /// </summary>
        /// <param name="info"></param>
        /// <param name="filePath"></param>
        /// <param name="memberName"></param>
        /// <param name="lineNumber"></param>
        public static void Error(
        string info,
        [CallerFilePath] string filePath = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0)
        {

            ilog.Error($"[{filePath}] [{memberName}] [{lineNumber}] - {info}");
        }
        /// <summary>
        /// 写入一行Fatal级别的日志
        /// </summary>
        /// <param name="info"></param>
        /// <param name="filePath"></param>
        /// <param name="memberName"></param>
        /// <param name="lineNumber"></param>
        public static void Fatal(
        string info,
        [CallerFilePath] string filePath = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0)
        {

            ilog.Fatal($"[{filePath}] [{memberName}] [{lineNumber}] - {info}");
        }
    }
}
