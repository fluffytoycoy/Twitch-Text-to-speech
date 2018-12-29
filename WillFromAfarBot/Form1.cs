using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TwitchLib.Client.Events;

namespace WillFromAfarBot
{
  
    public partial class Form1 : Form
    {
        private TwitchChatBot bot;
        private Timer reconnectionTimer = new Timer();

        public Form1()
        {
            InitializeComponent();
            LoadEvents();
            
        }

        private void Logger_LogAdded(object sender, EventArgs e)
        {
            ThreadHelper.AddText(this, richTextBox1, Logger.GetLastLog());
            ThreadHelper.AutoScroll(this, richTextBox1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var text = textBox1.Text.Trim();
            if (text != "")
            {
               Talk(text);
            }
        }

        private async void Talk(string text)
        {
            var textToSpeech = new TextToSpeech();

            try
            {
                await Task.Run(() => textToSpeech.ConvertText(text));
                await Task.Run(() => textToSpeech.Speak());
            }
            catch (Exception a)
            {
                MessageBox.Show(a.Message);
            }
        }

        private void Correct_Login_Event(object sender, LoginEvent e)
        {
            try
            {
                BuildTwitchBot().Connect(e.GetLoginInfo());
                logIn1.Disable();
                ShowBotMenu();
                Logger.Log(e.GetLoginInfo().BotName + "Is Connected to" + e.GetLoginInfo().ChannelName + "\'s");
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }
        
        private void HideBotMenu()
        {
            textBox1.Disable();
            button1.Disable();
            richTextBox1.Disable();
            this.Size = new Size(300, 300);
        }

        private void ShowBotMenu()
        {
            this.Size = new Size(816, 488);
            textBox1.Enable();
            button1.Enable();
            richTextBox1.Enable();  
        }
   
        private void Client_Disconnected_Error(object sender, EventArgs e)
        {
            Logger.Log( "Has been disconnected. \nAttempting to reconnect. \n");
            Invoke(new Action(() => button2.Disable()));
            Invoke(new Action (() => reconnectionTimer.Start()));
        }

        private void Client_Disconnected_Choice(object sender, EventArgs e)
        {
            Invoke(new Action (() =>HideBotMenu()));
            Invoke(new Action (() =>logIn1.Enable()));
        }

        private void Reconnect()
        {
            bot.Connect(bot.Info);
        }

        private void reconnection_tick(object sender, EventArgs e)
        {  
            if (bot.IsConnected)
            {
                reconnectionTimer.Stop();
                Logger.Log("- Bot has been reconnected \n");
                bot.Info.ReconnectionCount = 0;
                Invoke(new Action(() => button2.Enable()));
            }
            else if (bot.Info.ReconnectionCount > 10)
            {
                Logger.Log("\n Reconnection Failed \n");
                bot.Info.ReconnectionCount++;
            }
            else
            {
                bot.Reconnect();
                Logger.Log("-");
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bot.Info.ShouldReconnect = false;
            bot.Disconnect();
        }

        private TwitchChatBot BuildTwitchBot()
        {
            bot = new TwitchChatBot();
            bot.Client_Disconnected_Error += Client_Disconnected_Error;
            bot.Client_Disconnected_Choice += Client_Disconnected_Choice;
            return bot;
        }

        private void LoadEvents()
        {
            Logger.LogAdded += new EventHandler(Logger_LogAdded);
            logIn1.Login_Event += new EventHandler<LoginEvent>(Correct_Login_Event);
            reconnectionTimer.Interval = 1000;
            reconnectionTimer.Tick += reconnection_tick;
        }
    }
}
