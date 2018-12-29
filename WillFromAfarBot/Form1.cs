﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WillFromAfarBot
{
  
    public partial class Form1 : Form
    {
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

        private async void button1_Click(object sender, EventArgs e)
        {
            
            if (textBox1.Text.Trim() != "")
            {
                var textToSpeech = new TextToSpeech();
                var text = textBox1.Text;
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
        }

        private void Correct_Login_Event(object sender, LoginEvent e)
        {
            var bot = new TwitchChatBot();

            try
            {
                bot.Connect(e.GetLoginInfo());


            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            logIn1.Disable();
           ShowBotMenu();
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

        private void LoadEvents()
        {
            Logger.LogAdded += new EventHandler(Logger_LogAdded);
            logIn1.Login_Event += new EventHandler<LoginEvent>(Correct_Login_Event);
            
        }
    }
}
