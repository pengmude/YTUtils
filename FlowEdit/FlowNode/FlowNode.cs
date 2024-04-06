using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YTUtils.FlowEdit;

namespace YTUtils.FlowEdit
{
    /// <summary>
    /// 流程节点类
    /// 流程节点采用双向链表实现，主要是方便动态组装节点的依赖关系
    /// </summary>
    public partial class FlowNode : UserControl
    {
        /// <summary>
        /// 算子模块描述文本
        /// </summary>
        [Category("流程控件属性")]
        public string NodeText { get => label_node_text.Text; set => label_node_text.Text = value; }
        /// <summary>
        /// 算子模块颜色
        /// </summary>
        [Category("流程控件属性")]
        public Color NodeColor { get => label_node_id.BackColor; set => label_node_id.BackColor = value; }

        /// <summary>
        /// 算子模块类别
        /// </summary>
        public NodeType NodeType { get; set; }
        /// <summary>
        /// 算子模块ID（算子模块唯一标识）
        /// </summary>
        public int NodeId { get => int.Parse(label_node_id.Text); set => label_node_id.Text = value.ToString(); }
        /// <summary>
        /// 保存上游的流程节点ID，使用HashSet保证相同节点ID只保存一个
        /// </summary>
        public HashSet<int> PreNodeIds = new HashSet<int>();
        /// <summary>
        /// 保存下游的流程节点ID，使用HashSet保证相同节点ID只保存一个
        /// </summary>
        public HashSet<int> NextNodeIds = new HashSet<int>();
        /// <summary>
        /// 当前节点连接上的所有节点（包括上游和下游节点），使用HashSet保证相同节点ID只保存一个
        /// </summary>
        private HashSet<int> AllConnectedNodes = new HashSet<int>();

        public Form ParamSettingsForm { get; set; }

        public FlowNode()
        {
            InitializeComponent();
            ProcessPanelForm.CreateNodeIdEvent += ProcessPanelForm_CreateNodeIdEvent;
        }

        /// <summary>
        /// 创建节点时，获得要创建节点的ID
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProcessPanelForm_CreateNodeIdEvent(object sender, int e)
        {
            NodeId = e;
            // 每个节点在创建后都要取消订阅，否则后面创建的节点都会覆盖前面创建的节点ID
            ProcessPanelForm.CreateNodeIdEvent -= ProcessPanelForm_CreateNodeIdEvent;
        }
        /// <summary>
        /// 绑定上下游节点的关系
        /// </summary>
        /// <param name="preControl"></param>
        /// <param name="nextControl"></param>
        /// <returns></returns>
        public static bool SetUpDownOfNode(Control preControl, Control nextControl)
        {
            if (preControl is FlowNode preNode && nextControl is FlowNode nextNode)
            {
                // 只要待连接的上游节点的AllConnectedNodes集合不包含待连接的下游节点的ID，
                // 就能说明当前两节点尚未关联，可以连接
                if (!preNode.AllConnectedNodes.Contains(nextNode.NodeId))
                {
                    preNode.NextNodeIds.Add(nextNode.NodeId);
                    nextNode.PreNodeIds.Add(preNode.NodeId);
                    preNode.AllConnectedNodes.Add(nextNode.NodeId);
                    nextNode.AllConnectedNodes.Add(preNode.NodeId);
                    return true;
                }
            }
            return false;
        }

        private void label_node_text_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (ProcessPanelForm.DrawStatus == DrawStatus.Normal)
            {
                string pre = "", next = "";
                foreach (var id in this.PreNodeIds)
                {
                    pre += id.ToString() + " ";
                }
                foreach (var id in this.NextNodeIds)
                {
                    next += id.ToString() + " ";
                }
                MessageBox.Show($"上游节点ID:({pre}),下游节点ID:({next})");
            }
        }
        /// <summary>
        /// 鼠标双击打开参数设置界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNodeMouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.ParamSettingsForm.Show();
        }
    }
    public enum NodeType
    {
        None,
        one,
        two,
        three,
        four,
        five,
        six,
        seven,
        eight
    }
}
