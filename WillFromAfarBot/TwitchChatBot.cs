using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TwitchLib.Api;
using TwitchLib.Api.Interfaces;
using TwitchLib.Api.Models.v5.Streams;
using TwitchLib.Api.Models.v5.Users;
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
        public EventHandler Client_Disconnected_Error;
        public EventHandler Client_Disconnected_Choice;
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
            if (Info.ShouldReconnect)
            {
                Client_Disconnected_Error?.Invoke(this, new EventArgs());
            }
            else
            {
                Client_Disconnected_Choice?.Invoke(this, new EventArgs());
                Info.ShouldReconnect = true;
            }
        }

        private void Client_OnLog(object sender, OnLogArgs e)
        {
            Logger.Log(e.Data);
        }

        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            MessageHandler messenger = new MessageHandler(e.ChatMessage, client);
            messenger.HandleMessage();
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
