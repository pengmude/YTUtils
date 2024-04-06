namespace YTUtils.FlowEdit
{
    partial class FlowNode
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panel2 = new System.Windows.Forms.Panel();
            this.label_node_text = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label_node_id = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label_node_text);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(50, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(153, 56);
            this.panel2.TabIndex = 3;
            // 
            // label_node_text
            // 
            this.label_node_text.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_node_text.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_node_text.ForeColor = System.Drawing.SystemColors.Control;
            this.label_node_text.Location = new System.Drawing.Point(0, 0);
            this.label_node_text.Name = "label_node_text";
            this.label_node_text.Size = new System.Drawing.Size(153, 56);
            this.label_node_text.TabIndex = 0;
            this.label_node_text.Text = "自定义模块";
            this.label_node_text.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_node_text.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.OnNodeMouseDoubleClick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label_node_id);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(50, 56);
            this.panel1.TabIndex = 2;
            // 
            // label_node_id
            // 
            this.label_node_id.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.label_node_id.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_node_id.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold);
            this.label_node_id.ForeColor = System.Drawing.Color.White;
            this.label_node_id.Location = new System.Drawing.Point(0, 0);
            this.label_node_id.Name = "label_node_id";
            this.label_node_id.Size = new System.Drawing.Size(50, 56);
            this.label_node_id.TabIndex = 0;
            this.label_node_id.Text = "ID";
            this.label_node_id.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_node_id.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.OnNodeMouseDoubleClick);
            // 
            // FlowNode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "FlowNode";
            this.Size = new System.Drawing.Size(203, 56);
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label_node_id;
        private System.Windows.Forms.Label label_node_text;
    }
}
