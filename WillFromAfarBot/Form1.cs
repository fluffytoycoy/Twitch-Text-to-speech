﻿using System;
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
        private TwitchChatBot bot = new TwitchChatBot();
        private Timer reconnectionTimer;

        public Form1()
        {
            InitializeComponent();
            LoadEvents();
        }

        private void Logger_LogAdded(object sender, EventArgs e)
        {
            ThreadHelper.AddText(this, richTextBox1, Logger.GetLastLog());
            richTextBox1.ScrollToCaret();
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
                bot.Connect(e.GetLoginInfo());
                logIn1.Disable();
                ShowBotMenu();
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
        }

        private void ShowBotMenu()
        {
            this.Size = new Size(816, 488);
            textBox1.Enable();
            button1.Enable();
            richTextBox1.Enable();  
        }
   
        private void Client_Disconnected(object sender, EventArgs e)
        {
            Logger.Log(bot.Info.BotName + "Has been disconnected. \n Attempting to reconnect. \n");
            reconnectionTimer = new Timer();
            reconnectionTimer.Interval = 10000;
            reconnectionTimer.Start();            
        }

        private void reconnection_tick(object sender, EventArgs e)
        {
            bot.Connect(bot.Info);
            if (bot.IsConnected())
            {
                reconnectionTimer.Stop();
                Logger.Log(bot.Info.BotName + "Has reconnected to" + bot.Info.ChannelName + "'s channel");
            }
        }

        private void LoadEvents()
        {
            Logger.LogAdded += new EventHandler(Logger_LogAdded);
            logIn1.Login_Event += new EventHandler<LoginEvent>(Correct_Login_Event);
            bot.Client_Disconnected += Client_Disconnected;
            reconnectionTimer.Tick += new EventHandler(reconnection_tick);

        }

    }
}
