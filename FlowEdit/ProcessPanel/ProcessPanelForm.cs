using GroupBoxMove;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;
using WeifenLuo.WinFormsUI.Docking;
using YTUtils.FlowEdit.ProcessPanel;
using YTUtils.Properties;
using static YTUtils.FlowEdit.MouseEventHelper;

namespace YTUtils.FlowEdit
{
    public partial class ProcessPanelForm : DockContent
    {
        #region 节点移动功能
        private bool IsMoving;
        private Point StartPoint;
        #endregion

        #region 鼠标连线功能
        public static DrawStatus DrawStatus = DrawStatus.Normal;
        private int NodeClickedNum = 0;   // 连线状态下有几个节点被点击
        private FlowNode Node1;
        private FlowNode Node2;
        #endregion

        #region 鼠标框选功能
        private List<LineInfo> _selectedLines; // 存储当前选中的线段集合
        private Point _dragStartPoint; // 记录鼠标按下时的位置，用于计算框选矩形
        #endregion

        // 添加流程节点时，触发以下事件，让流程节点类对象得到节点ID，
        // 也就是流程编辑区域每拖入一个节点，节点ID加1
        public static event EventHandler<int> CreateNodeIdEvent;

        public ProcessPanelForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 工具拖拽完成执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProcessPanelForm_DragDrop(object sender, DragEventArgs e)
        {
            FlowNode node = new FlowNode();
            int pX = (e.X - node.Width / 2);
            int pY = (e.Y - node.Height / 2);
            // 处理流程节点在流程图中的移动事件
            MouseEventHelper.RegistryMouseEvent(node, NodeMouseDown, MouseEventName.MouseDown);
            MouseEventHelper.RegistryMouseEvent(node, NodeMouseMove, MouseEventName.MouseMove);
            MouseEventHelper.RegistryMouseEvent(node, NodeMouseUp, MouseEventName.MouseUp);
            // 处理流程节点的连线事件
            MouseEventHelper.RegistryMouseEvent(node, NodeClick, MouseEventName.MouseDown);
            // 绑定对应的参数设置窗口
            node.ParamSettingsForm = new Form();
            node.Location = PointToClient(new Point(pX, pY));
            this.panel1.Controls.Add(node);

            // 告诉要创建的节点当前应该得到的NodeId值
            int Count = 0;
            foreach (Control item in panel1.Controls)
            {
                if (item is FlowNode)
                    Count++;
            }
            CreateNodeIdEvent?.Invoke(this, Count);
        }
        /// <summary>
        /// 连线状态下对节点的点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NodeClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                // 判断是否是连线状态
                if(DrawStatus == DrawStatus.DrawLine)
                {
                    var control = sender as Control;
                    if(NodeClickedNum == 0)
                    {
                        if (control.Parent != null && control.Parent.Parent != null)
                        {
                            // 把第一个节点先记录下来
                            Node1 = control.Parent.Parent as FlowNode;
                            NodeClickedNum = 1;
                        }
                    }
                    else if(NodeClickedNum == 1)
                    {
                        // 此时已经点击了2个节点，开始执行绘制连线操作
                        if (control.Parent != null && control.Parent.Parent != null)
                        {
                            Node2 = control.Parent.Parent as FlowNode;
                            // 点击到的节点不是同一个才符合连线要求
                            if (!Node1.Equals(Node2))
                            {
                                // 设置节点的上下级关系
                                if(FlowNode.SetUpDownOfNode(Node1, Node2))
                                {
                                    // 绘制连线
                                    DrawLine(Node1, Node2);
                                }
                                else
                                {
                                    MessageBox.Show("当前两个节点已存在连接！");
                                }
                                // 找到了第二个节点，NodeClickedNum 重置为0，为下一次连线做准备
                                NodeClickedNum = 0;
                                // 绘制完成后要将当前绘制状态设为普通状态
                                DrawStatus = DrawStatus.Normal;
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 绘制连线逻辑
        /// </summary>
        /// <param name="node1"></param>
        /// <param name="node2"></param>
        private void DrawLine(FlowNode node1, FlowNode node2)
        {
            Point point1, point2;
            //从左到右
            if(Math.Abs(node2.Location.X - node1.Location.X) > Math.Abs(node2.Location.Y - node1.Location.Y) && node2.Location.X >= node1.Location.X)
            {
                point1 = new Point(node1.Location.X + node1.Width + 1, node1.Location.Y + node1.Height / 2);
                point2 = new Point(node2.Location.X - 1, node2.Location.Y + node2.Height / 2);
                AddLine(point1, point2, LineForward.LEFT2RIGHT);
            }
            // 从右到左
            else if (Math.Abs(node2.Location.X - node1.Location.X) > Math.Abs(node2.Location.Y - node1.Location.Y) && node2.Location.X < node1.Location.X)
            {
                point1 = new Point(node1.Location.X - 1, node1.Location.Y + node1.Height / 2);
                point2 = new Point(node2.Location.X + node2.Width + 1, node2.Location.Y + node2.Height / 2);
                AddLine(point1 , point2, LineForward.RIGHT2LEFT);
            }
            // 从上到下
            else if (Math.Abs(node2.Location.X - node1.Location.X) <= Math.Abs(node2.Location.Y - node1.Location.Y) && node2.Location.Y >= node1.Location.Y)
            {
                point1 = new Point(node1.Location.X + node1.Width / 2, node1.Location.Y + node1.Height + 1);
                point2 = new Point(node2.Location.X + node2.Width / 2, node2.Location.Y - 1);
                AddLine(point1, point2, LineForward.UP2DOWN);
            }
            // 从下到上
            else if (Math.Abs(node2.Location.X - node1.Location.X) <= Math.Abs(node2.Location.Y - node1.Location.Y) && node2.Location.Y < node1.Location.Y)
            {
                point1 = new Point(node1.Location.X + node1.Width / 2, node1.Location.Y - 1);
                point2 = new Point(node2.Location.X + node2.Width / 2, node2.Location.Y + node2.Height + 1);
                AddLine(point1 , point2, LineForward.DOWN2UP);
            }
        }
        /// <summary>
        /// 绘制两个点的连线
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <param name="lineForward"></param>
        private void AddLine(Point point1, Point point2, LineForward lineForward)
        {
            Graphics g = this.panel1.CreateGraphics();
            g.SmoothingMode = SmoothingMode.HighQuality;
            Color color = Color.DarkGoldenrod;
            Pen p = new Pen(color, 6);
            p.DashStyle = DashStyle.Solid;
            p.StartCap = LineCap.Round;
            p.EndCap = LineCap.ArrowAnchor;
            p.LineJoin = LineJoin.Round;

            Point inflectPoint1; 
            Point inflectPoint2;
            if(lineForward == LineForward.LEFT2RIGHT || lineForward == LineForward.RIGHT2LEFT)
            {
                inflectPoint1 = new Point((point1.X + point2.X) / 2, point1.Y);
                inflectPoint2 = new Point((point1.X + point2.X) / 2, point2.Y);
            }
            else
            {
                inflectPoint1 = new Point(point1.X, (point1.Y + point2.Y) / 2); 
                inflectPoint2 = new Point(point2.X, (point1.Y + point2.Y) / 2);
            }
            Point[] points = new Point[] { point1, inflectPoint1, inflectPoint2, point2 };
            g.DrawLines(p, points);
        }

        /// <summary>
        /// 在流程节点上弹起鼠标左键处理事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NodeMouseUp(object sender, MouseEventArgs e)
        {
            if (IsMoving)
            {
                var con = sender as Control;
                // 自定义节点控件的最上层是Label,Label的父亲是Panel，Panel的父亲才是自定义控件的底
                if (con.Parent.Parent is FlowNode node)
                {
                    node.Cursor = Cursors.Arrow;
                    IsMoving = false;
                }
            }
        }
        /// <summary>
        /// 在流程节点上鼠标移动处理事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NodeMouseMove(object sender, MouseEventArgs e)
        {
            if (IsMoving)
            {
                if(e.Button == MouseButtons.Left)
                {
                    // 自定义控件的最上层是Label,Label的父亲是Panel，Panel的父亲才是自定义控件的底
                    Control label = sender as Control;
                    FlowNode node = label.Parent.Parent as FlowNode;
                    // 获取控件的位置
                    int left = node.Left + (e.X - StartPoint.X);
                    int top = node.Top + (e.Y - StartPoint.Y);
                    // 获取控件的宽和高
                    int width = node.Width;
                    int height = node.Height;
                    // 动态获取流程区域的范围
                    var rect = node.Parent.ClientRectangle;
                    // 判断拖拽过程不能超过编辑区域
                    left = left < 0 ? 0 : (left + width > rect.Width) ? rect.Width - width: left;
                    top = top < 0 ? 0 : (top + height > rect.Height) ? rect.Height - height: top;
                    // 设置控件实际只能到达的位置
                    node.Location = new Point(left, top);
                    node.Cursor = Cursors.SizeAll;
                    // 强制刷新流程编辑区域(为了重绘时，连线不出现重影)
                    this.panel1.Invalidate();
                }
            }
        }
        /// <summary>
        /// 在流程节点上按下鼠标左键处理事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NodeMouseDown(object sender, MouseEventArgs e)
        {
            // 标记鼠标移动
            IsMoving = true;
            // 记录当前鼠标位置
            StartPoint = new Point(e.X, e.Y);
        }

        /// <summary>
        /// 工具拖拽进入执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProcessPanelForm_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }
        /// <summary>
        /// 鼠标右击选中标记为连线状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chuaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DrawStatus = DrawStatus.DrawLine;
        }
        /// <summary>
        /// 流程区域面板的重绘事件，用于刷新连线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            var control = sender as Control;
            DrawAllLines(control);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        private void DrawAllLines(Control control)
        {
            // 找出流程编辑器中所有的流程控件
            foreach (Control con1 in control.Controls)
            {
                if(con1 is FlowNode node1)
                {
                    // 找到一个流程节点继续找下一个
                    foreach (Control con2 in control.Controls)
                    {
                        if (con2 is FlowNode node2)
                        {
                            if (node1.NextNodeIds.Contains(node2.NodeId))
                                DrawLine(node1, node2);
                        }
                    }
                }

            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (DrawStatus == DrawStatus.DrawLine)
            {
                MessageBox.Show("请通过先后点击2个节点进行连接！");
                // 只要连线状态下点击不到节点，都将NodeClickedNum 重置为0，为下一次连线做准备
                NodeClickedNum = 0;
                // 绘制完成后要将当前绘制状态设为普通状态
                DrawStatus = DrawStatus.Normal;
            }
        }
    }
    /// <summary>
    /// 流程绘制状态（常态、绘制连线状态）
    /// </summary>
    public enum DrawStatus
    {
        Normal,
        DrawLine
    }
    /// <summary>
    /// 给两个节点绘制线段时的方向
    /// </summary>
    public enum LineForward 
    {
        LEFT2RIGHT,//左到右
        UP2DOWN,    // 上到下
        RIGHT2LEFT,//右到左
        DOWN2UP // 下到上
    }
}
