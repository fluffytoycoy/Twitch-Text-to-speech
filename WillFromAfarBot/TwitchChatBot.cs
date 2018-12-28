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
        public EventHandler Client_Disconnected;

        internal void Connect(LoginModel info)
        {
            
            var botCredentials = new ConnectionCredentials(info.BotName, info.BotId);
            client = new TwitchClient();
            client.Initialize(botCredentials, info.ChannelName);
            client.OnMessageReceived += Client_OnMessageReceived;
            client.OnDisconnected += Client_OnDisconnect;
            client.Connect();
        }

        private void Client_OnDisconnect(object sender, OnDisconnectedArgs e)
        {

            Client_Disconnected(this, new EventArgs());
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

        private void InitializeClient()
        {

        }
    }
}
