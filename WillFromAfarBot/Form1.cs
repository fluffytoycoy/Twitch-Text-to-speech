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
            var bot = new TwitchChatBot();
            bot.Connect();
        }

        private void Logger_LogAdded(object sender, EventArgs e)
        {
            ThreadHelper.AddText(this, richTextBox1, Logger.GetLastLog());
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

       
    }
}
