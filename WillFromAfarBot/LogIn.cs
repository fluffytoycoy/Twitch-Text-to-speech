using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WillFromAfarBot
{
    public partial class LogIn : UserControl
    {
        public event EventHandler<LoginEvent> Login_Event;

        public LogIn()
        {
            InitializeComponent();
        }

        public void button1_Click(object sender, EventArgs e)
        {
            Login_Event?.Invoke(this, new LoginEvent(BuildLoginModel()));
        }

        private LoginModel BuildLoginModel()
        {
            return new LoginModel
            {
                ChannelName = ChannelName.Text,
                BotName = BotName.Text,
                BotId = BotId.Text
            };
        }
    }
}
