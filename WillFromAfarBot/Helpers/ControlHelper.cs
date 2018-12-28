using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WillFromAfarBot
{
    public static class ControlHelper
    {
        public static void Disable(this Control control)
        {
            control.Enabled = false;
            control.Visible = false;
        }

        public static void Enable(this Control control)
        {
            control.Enabled = true;
            control.Visible = true;
        }
    }
}
