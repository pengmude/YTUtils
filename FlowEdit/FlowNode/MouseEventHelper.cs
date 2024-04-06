using static YTUtils.FlowEdit.MouseEventHelper;
using System.Xml.Linq;

using YTUtils.FlowEdit;

/// <summary>
/// 类型名称：MouseEventHelper
/// 
/// 类的简要描述：该类在此项目中用于提供给流程节点类注册鼠标事件。
/// 
/// 更详细的类说明：
/// 鼠标从算法工具栏（AgorithmForm.cs）中拖拽到流程编辑区域（ProcessPanelForm.cs）的算子，会在ProcessPanelForm的
/// 
/// 创建人：pengmude
/// 创建日期：2024年3月31日10:11:05
/// </summary>
///
/// <remarks>
/// 特别注意事项或附加说明：
/// 在此部分可以添加任何与类使用相关的特别提示、依赖关系、
/// 或者特定使用场景的信息。
/// </remarks>
///
/// <example>
/// 使用示例：
/// 
/// ```csharp
/// // 这里展示如何实例化并使用此类的一个例子
///             FlowNode node = new FlowNode();
///int pX = (e.X - node.Width / 2);
///int pY = (e.Y - node.Height / 2);
///// 处理流程节点在流程图中的移动事件
///MouseEventHelper.RegistryMouseEvent(node, NodeMouseDown, MouseEventName.MouseDown);
///MouseEventHelper.RegistryMouseEvent(node, NodeMouseMove, MouseEventName.MouseMove);
///MouseEventHelper.RegistryMouseEvent(node, NodeMouseUp, MouseEventName.MouseUp);
///// 处理流程节点的连线事件
///MouseEventHelper.RegistryMouseEvent(node, NodeClick, MouseEventName.MouseDown);
/// ```
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YTUtils.FlowEdit
{
    /// <summary>
    /// 鼠标注册工具类
    /// </summary>
    public class MouseEventHelper
    {
        /// <summary>
        /// 给指定的控件注册鼠标事件
        /// </summary>
        public static void RegistryMouseEvent(Control control, MouseEventHandler mouseEventHandler, MouseEventName mouseEventName)
        {
            if(control.Controls.Count > 0)
            {
                foreach (Control con in control.Controls)
                {
                    switch (mouseEventName)
                    {
                        case MouseEventName.MouseDown:
                            con.MouseDown += new MouseEventHandler(mouseEventHandler);
                            break;
                        case MouseEventName.MouseMove:
                            con.MouseMove += new MouseEventHandler(mouseEventHandler);
                            break;
                        case MouseEventName.MouseUp:
                            con.MouseUp += new MouseEventHandler(mouseEventHandler);
                            break;
                    }
                    RegistryMouseEvent(con, mouseEventHandler, mouseEventName);
                }
            }
        }
        /// <summary>
        /// 鼠标事件名称
        /// </summary>
        public enum MouseEventName
        {
            MouseDown,
            MouseMove,
            MouseUp
        }
    }
}
