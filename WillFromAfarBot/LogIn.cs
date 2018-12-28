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
        public event EventHandler Correct_Login_Event;

        public LogIn()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            var login = BuildLoginModel();
            var bot = new TwitchChatBot();

            try
            {
                bot.Connect(login);
                if (Correct_Login_Event != null)
                {
                    Correct_Login_Event(this, new EventArgs());
                }

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
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
