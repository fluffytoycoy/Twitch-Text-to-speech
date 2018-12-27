using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WillFromAfarBot
{
    public static class ThreadHelper
    {
        delegate void SetTextCallback(Form targetForm, Control control, string text);

        /// <summary>
        /// Set text property of various controls
        /// </summary>
        /// <param name="targetForm">The calling form</param>
        /// <param name="control">The control to be updated</param>
        /// <param name="text">Text to be loaded</param>
        public static void SetText(Form targetform, Control control, string text)
        {
           
            if (control.InvokeRequired)
            {
                SetTextCallback setText = new SetTextCallback(SetText);
                targetform.Invoke(setText, new object[] { targetform, control, text });
            }
            else
            {
                control.Text = text;
            }
        }
        
        public static void AddText(Form targetform, Control control, string text)
        {
            if (control.InvokeRequired)
            {
                SetTextCallback addText = new SetTextCallback(AddText);
                targetform.Invoke(addText, new object[] { targetform, control, text });
            }
            else
            {
                control.Text += text + "\n";
            }
        }
    }
}
