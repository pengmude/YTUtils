using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace YTUtils.Controls
{
    public partial class UserLogger : UserControl
    {
        bool logFocus = true;
        string logDictory = Environment.CurrentDirectory + @"\Log\";

        public UserLogger()
        {
            InitializeComponent();
        }
        ~UserLogger()
        {
            logFocus = false;
        }
        /// <summary>
        /// 每次在日志窗口新增一行日志都要根据日志等级绘制不同颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            ListBox seelctBox = sender as ListBox;
            if (e.Index < 0)
            { return; }
            else
            {
                e.DrawBackground();
                Brush mybsh = Brushes.Black;
                // 判断是什么类型的标签,在调用时必须信息前边标注信息的类别，分为Info，Warning，Error
                try
                {
                    string level = Regex.Split(seelctBox.Items[e.Index].ToString(), "    ")[1].Trim('[').Trim(']');
                    //MessageBox.Show($"level:{level}");
                    MsgLevel msgLevel = (MsgLevel)Enum.Parse(typeof(MsgLevel), level);
                    switch (msgLevel)
                    {
                        case MsgLevel.Info:
                            mybsh = Brushes.Black; // 黑色
                            break;
                        case MsgLevel.Debug:
                            mybsh = Brushes.DarkGreen; // 亮绿色
                            break;
                        case MsgLevel.Warn:
                            mybsh = Brushes.Orange; // 橙色
                            break;
                        case MsgLevel.Exception:
                            mybsh = Brushes.DarkRed; // 暗红色
                            break;
                        case MsgLevel.Fatal:
                            mybsh = Brushes.Red; // 红色
                            break;
                        default:
                            mybsh = Brushes.Black; // 黑色
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"解析日志等级的规则错误！{ex}");
                    mybsh = Brushes.Blue;  // 蓝色
                }
                e.DrawFocusRectangle();
                e.Graphics.DrawString(seelctBox.Items[e.Index].ToString(), e.Font, mybsh, e.Bounds, StringFormat.GenericDefault);
            }
        }

        /// <summary>
        /// 在日志窗口和日志文件中添加一行不带异常信息的日志
        /// </summary>
        /// <param name="level"></param>
        /// <param name="logInfo"></param>
        /// <param name="fileName"></param>
        /// <param name="funcName"></param>
        /// <param name="lineNumber"></param>
        public void AddLog(MsgLevel level,string logInfo, string fileName, string funcName, int lineNumber)
        {
            try
            {
                // 写入日志文件
                WriteLog($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}]    [{level}]    [{logInfo}]    [{fileName}]    [{funcName}]    [{lineNumber}]");
                
                // 输出日志窗口
                this.Invoke((MethodInvoker)delegate
                {
                    switch (level)
                    {
                        case MsgLevel.Debug:
                            ControlListBox(level, logInfo, listBoxDebug);
                            break;
                        case MsgLevel.Info:
                            ControlListBox(level, logInfo, listBoxInfo);
                            break;
                        case MsgLevel.Warn:
                            ControlListBox(level, logInfo, listBoxWarn);
                            break;
                        case MsgLevel.Exception:
                        case MsgLevel.Fatal:
                            ControlListBox(level, logInfo, listBoxExpection);
                            break;
                        default:
                            break;
                    }
                    listBoxAll.Items.Add($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}]    [{level}]    [{logInfo}]");
                    listBoxAll.SelectedIndex = listBoxAll.Items.Count - 1;
                    if (listBoxAll.Items.Count > 300)
                    {
                        listBoxAll.Items.Clear();
                    }
                    Application.DoEvents();
                });
            }
            catch (Exception)
            {

            }
        }

        private void ControlListBox(MsgLevel level, string logInfo, ListBox myListBox)
        {
            myListBox.Items.Add($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss-fff")}]    [{level}]    [{logInfo}]");
            myListBox.SelectedIndex = myListBox.Items.Count - 1;
            if (myListBox.Items.Count > 300)
            {
                myListBox.Items.Clear();
            }
        }
        /// <summary>
        /// 在日志窗口和日志文件中添加一行带异常信息的日志
        /// </summary>
        /// <param name="level"></param>
        /// <param name="logInfo"></param>
        /// <param name="ex"></param>
        /// <param name="fileName"></param>
        /// <param name="funcName"></param>
        /// <param name="lineNumber"></param>
        public void AddLog(MsgLevel level, string logInfo, string fileName, string funcName, int lineNumber, Exception ex)
        {
            try
            {
                WriteLog($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}]    [{level}]    [{logInfo}]    [{fileName}]    [{funcName}]    [{lineNumber}]    [{ex}]");
                this.Invoke((MethodInvoker)delegate
                {
                    switch (level)
                    {
                        case MsgLevel.Debug:
                            ControlListBox(level, logInfo, listBoxDebug);
                            break;
                        case MsgLevel.Info:
                            ControlListBox(level, logInfo, listBoxInfo);
                            break;
                        case MsgLevel.Warn:
                            ControlListBox(level, logInfo, listBoxWarn);
                            break;
                        case MsgLevel.Exception:
                        case MsgLevel.Fatal:
                            ControlListBox(level, logInfo, listBoxExpection);
                            break;
                        default:
                            break;
                    }
                    listBoxAll.Items.Add($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}]    [{level}]    [{logInfo}]    [{ex}]");
                    listBoxAll.SelectedIndex = listBoxAll.Items.Count - 1;
                    if (listBoxAll.Items.Count > 300)
                    {
                        listBoxAll.Items.Clear();
                    }
                    Application.DoEvents();
                });
            }
            catch (Exception)
            {

            }
        }
        /// <summary>
        /// 写入一行日志到文件中
        /// </summary>
        /// <param name="msg"></param>
        public void WriteLog(string msg)
        {
            string time = DateTime.Now.ToString("HH:mm:ss.fff");
            if (!Directory.Exists(logDictory))
            {
                Directory.CreateDirectory(logDictory);
            }
            string runningLogFileName = logDictory + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
            StreamWriter mySW = new StreamWriter(runningLogFileName, true);
            mySW.WriteLine(msg);
            mySW.Close();
        }

        /// <summary>
        /// 日志窗口初始化时就开启线程监听日志队列中是否不为空，不为空就执行添加日志
        /// </summary>
        object myObject = new object();
        private void UserLogger_Load(object sender, EventArgs e)
        {
            Task startLogFocus = new Task(() =>
            {
                while (logFocus)
                {
                    if (LoggerClass.logQueue.Count > 0)
                    {
                        lock (myObject)
                        {
                            if (LoggerClass.logQueue.Count > 0)
                            {
                                LogInfo log = LoggerClass.logQueue.Dequeue();
                                if (log.ex != null)
                                {
                                    AddLog(log.logLevel, log.message, log.fileName, log.funcName, log.lineNumber, log.ex);
                                }
                                else
                                {
                                    AddLog(log.logLevel, log.message, log.fileName, log.funcName, log.lineNumber);
                                }
                            }
                        }
                    }
                }
            });
            startLogFocus.Start();
        }
        /// <summary>
        /// 点击清除日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                switch (tabControl1.SelectedIndex)
                {
                    case 0:
                        listBoxAll.Items.Clear();
                        break;
                    case 1:
                        listBoxInfo.Items.Clear();
                        break;
                    case 2:
                        listBoxDebug.Items.Clear();
                        break;
                    case 3:
                        listBoxWarn.Items.Clear();
                        break;
                    case 4:
                        listBoxExpection.Items.Clear();
                        break;
                    default:
                        break;
                }

            }
            catch
            {
            }

        }
        /// <summary>
        /// 双击日志显示窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            ListBox mySelectBox = sender as ListBox;
            if (mySelectBox.SelectedItem != null)
            {
                string selectInfo = mySelectBox.SelectedItem.ToString();
                LogDetail myLogDetail = new LogDetail(selectInfo);
                myLogDetail.Show();
            }

        }
    }

    public enum MsgLevel
    {
        /// <summary>
        /// 0.调试信息输出
        /// </summary>
        Debug = 0,
        /// <summary>
        /// 1.业务信息记录
        /// </summary>
        Info = 1,
        /// <summary>
        /// 2.警告提醒（捕获的业务异常）
        /// </summary>
        Warn = 2,
        /// <summary>
        /// 3.发生了异常（捕获的系统异常）
        /// </summary>
        Exception = 3,
        /// <summary>
        /// 4.发生致命异常（未被捕获的异常|捕获的业务逻辑异常）
        /// </summary>
        Fatal = 4
    }
}
