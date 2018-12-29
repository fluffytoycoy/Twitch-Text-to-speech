using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace WillFromAfarBot
{
    public class TwitchChatBot
    {
        private TwitchClient client;
        private LoginModel info;
        private ConnectionCredentials botCredentials;
        //public EventHandler Client_Disconnected;

        internal void Connect(LoginModel loginInput)
        {
            info = loginInput;
            ChangeBotCredentials();
            InitializeClient();
            client.Connect();
        }


        private void Client_OnDisconnect(object sender, OnDisconnectedArgs e)
        {
            //Client_Disconnected(this, new EventArgs());
        }

        private void Client_OnLog(object sender, OnLogArgs e)
        {
            Logger.Log(e.Data);
        }

        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            Logger.Log(e.ChatMessage.DisplayName + ":" + e.ChatMessage.Message);
            var WillFromAfar = new TextToSpeech();
            switch (e.ChatMessage.Message.ToLower())
            {
                case "!will":
                    WillFromAfar.ConvertText(e.ChatMessage.Message.Substring(5));
                    WillFromAfar.Speak();
                    break;
            }
        }

        /// <summary>
        /// Loads the bot name and BotId into an object for the twitch client to use.
        /// Can be used for changing bots out in future.
        /// </summary>
        public void ChangeBotCredentials()
        {
            botCredentials = new ConnectionCredentials(info.BotName, info.BotId);
        }


        /// <summary>
        /// Initialize the Twitch web Client
        /// 
        /// </summary>
        private void InitializeClient()
        {
            client = new TwitchClient();
            client.Initialize(botCredentials, info.ChannelName);
            InitializeEvents();
        }

        /// <summary>
        /// Initialize the Twitch web Client's Events
        /// Seperated out for easier extensions in the future
        /// </summary>
        public void InitializeEvents()
        {
            client.OnMessageReceived += Client_OnMessageReceived;
            client.OnDisconnected += Client_OnDisconnect;
        }
    }
}
