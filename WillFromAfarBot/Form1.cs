using System;
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
            ShowBotMenu();

        }

        private void Logger_LogAdded(object sender, EventArgs e)
        {
            ThreadHelper.AddText(this, richTextBox1, Logger.GetLastLog());
            richTextBox1.ScrollToCaret();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() != "")
            {
                var textToSpeech = new TextToSpeech();
                var text = textBox1.Text;
                try
                {
                    textToSpeech.ConvertText(text);
                    textToSpeech.Speak();
                }
                catch (Exception a)
                {
                    MessageBox.Show(a.Message);
                }

            }
        }

        private void Correct_Login_Event(object sender, EventArgs e)
        {
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
            logIn1.Correct_Login_Event += new EventHandler(Correct_Login_Event);
        }
    }
}
