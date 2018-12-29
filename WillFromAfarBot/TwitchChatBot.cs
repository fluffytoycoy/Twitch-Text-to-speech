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
        private ConnectionCredentials botCredentials;
        public bool IsConnected => client.IsConnected;
        public EventHandler Client_Disconnected;

        public int ReconnectionCount {get; set;}

        public LoginModel Info { get; set; }

        internal void Connect(LoginModel loginInput)
        {
            Info = loginInput;
            ChangeBotCredentials();
            InitializeClient();
            client.Connect();
        }

        internal void Reconnect()
        {
            client.Connect();
        }

        internal void Disconnect()
        {
            client.Disconnect();
        }

        private void Client_OnDisconnect(object sender, EventArgs e)
        {
            Client_Disconnected?.Invoke(this, new EventArgs());
        }

        private void Client_OnLog(object sender, OnLogArgs e)
        {
            Logger.Log(e.Data);
        }

        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            var WillFromAfar = new TextToSpeech();
            Logger.Log(e.ChatMessage.DisplayName + ":" + e.ChatMessage.Message);
            if (e.ChatMessage.Message.StartsWith("!"))
            {
                if (e.ChatMessage.Message.StartsWith("!will"))
                {
                    WillFromAfar.ConvertText(e.ChatMessage.Message.Substring(5));
                    WillFromAfar.Speak();
                }

            }
        }

        /// <summary>
        /// Loads the bot name and BotId into an object for the twitch client to use.
        /// Can be used for changing bots out in future.
        /// </summary>
        public void ChangeBotCredentials()
        {
            botCredentials = new ConnectionCredentials(Info.BotName, Info.BotId);
        }


        /// <summary>
        /// Initialize the Twitch web Client
        /// 
        /// </summary>
        private void InitializeClient()
        {
            client = new TwitchClient();
            client.Initialize(botCredentials, Info.ChannelName);
            InitializeEvents();
        }

        /// <summary>
        /// Initialize the Twitch web Client's Events
        /// Seperated out for easier extensions in the future
        /// </summary>
        private void InitializeEvents()
        {
            client.OnMessageReceived += Client_OnMessageReceived;
            client.OnDisconnected += Client_OnDisconnect;
        }
    }
}
