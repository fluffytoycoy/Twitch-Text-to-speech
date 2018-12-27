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
            TwitchChatBot bot = new TwitchChatBot();
            bot.Connect();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() != "")
            {
                var textToSpeech = new TextToSpeech();
                //var selectedVoice = comboBox1.GetItemText(comboBox1.SelectedItem);
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
