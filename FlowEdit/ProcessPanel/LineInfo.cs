using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YTUtils.FlowEdit.ProcessPanel
{
    class LineInfo
    {
        public Point[] Points { get; set; }
        public Pen Pen { get; set; }
        public LineForward LineForward { get; set; }
    }
}
