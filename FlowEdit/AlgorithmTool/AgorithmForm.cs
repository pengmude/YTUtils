using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace YTUtils.AlgorithmTool
{
    public partial class AgorithmForm : DockContent
    {
        public AgorithmForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 窗口加载时候先隐藏具体工具项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AgorithmForm_Load(object sender, System.EventArgs e)
        {
            this.tableLayoutPanel1.Visible = false;
            this.tableLayoutPanel2.Visible = false;
            this.tableLayoutPanel3.Visible = false;
        }

        private void button5_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Button button = sender as Button;
                // 添加拖放相关的事件的数据（如控件名称）
                DataObject dragData = new DataObject("MyCustomFormat", button.Name); 
                //启动拖放
                button.DoDragDrop(dragData, DragDropEffects.Move);
            }
        }
        /// <summary>
        /// 点击展开或隐藏工具栏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void label3_MouseClick(object sender, MouseEventArgs e)
        {
            if (sender is Label label)
            {
                if(label.Text.Equals("图像采集  »"))
                {
                    label.Text = label.Text.Replace("»", "︾");
                    this.tableLayoutPanel1.Visible = !this.tableLayoutPanel1.Visible;
                }
                else if(label.Text.Equals("图像采集  ︾"))
                {
                    label.Text = label.Text.Replace("︾", "»");
                    this.tableLayoutPanel1.Visible = !this.tableLayoutPanel1.Visible;
                }else if (label.Text.Equals("图像处理  »"))
                {
                    label.Text = label.Text.Replace("»", "︾");
                    this.tableLayoutPanel2.Visible = !this.tableLayoutPanel2.Visible;
                }
                else if (label.Text.Equals("图像处理  ︾"))
                {
                    label.Text = label.Text.Replace("︾", "»");
                    this.tableLayoutPanel2.Visible = !this.tableLayoutPanel2.Visible;
                }
                else if (label.Text.Equals("通信工具  »"))
                {
                    label.Text = label.Text.Replace("»", "︾");
                    this.tableLayoutPanel3.Visible = !this.tableLayoutPanel3.Visible;
                }
                else if (label.Text.Equals("通信工具  ︾"))
                {
                    label.Text = label.Text.Replace("︾", "»");
                    this.tableLayoutPanel3.Visible = !this.tableLayoutPanel3.Visible;
                }
            }
        }
    }
}
