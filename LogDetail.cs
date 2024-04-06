using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YTUtils.Controls
{
    public partial class LogDetail : Form
    {
        string logMsg = string.Empty;
        public LogDetail(string inputMsg)
        {
            InitializeComponent();
            logMsg = inputMsg;
        }

        private void LogDetail_Load(object sender, EventArgs e)
        {
            try
            {
                string[] itemList = Regex.Split(logMsg, "    ");
                string time = itemList[0].Trim('[').Trim(']');
                string msgLevel = itemList[1].Trim('[').Trim(']');
                string msgDetal = itemList[2].Trim('[').Trim(']');
                txbLogTime.Text = time;
                txbLogLevel.Text = msgLevel;
                Color color = Color.DarkBlue;
                switch (msgLevel)
                {
                    case "Info":
                        color = Color.Black;
                        break;
                    case "Debug":
                        color = Color.DarkGreen;
                        break;
                    case "Warn":
                        color = Color.Orange;
                        break;
                    case "Exception":
                        color = Color.DarkRed;
                        break;
                    case "Fatal":
                        color = Color.Red;
                        break;
                    default:
                        color = Color.DarkBlue;
                        break;
                }
                //txbLogLevel.BackColor = color;
                txbLogLevel.ForeColor = color;
                richDetail.Text = msgDetal;
            }
            catch (Exception)
            {
                richDetail.Text = logMsg;
            }
        }
    }
}
