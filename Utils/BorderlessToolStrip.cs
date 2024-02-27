using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPirateZ_Genie2._0.Utils
{
    public class BorderlessToolstrip : ToolStripSystemRenderer
    {
        public BorderlessToolstrip() { }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            if (!(e.ToolStrip is ToolStrip))
                base.OnRenderToolStripBorder(e);
        }
    }
}
