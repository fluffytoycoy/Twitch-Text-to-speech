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
        TwitchClient client;

        internal void Connect(LoginModel info)
        {
            var credentials = new ConnectionCredentials(info.BotName, info.BotId);
            client = new TwitchClient();
            client.Initialize(credentials, info.ChannelName);
            
            client.OnMessageReceived += Client_OnMessageReceived;
            client.Connect();
        }

        private void Client_OnLog(object sender, OnLogArgs e)
        {
            Logger.Log(e.Data);
        }

        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            Logger.Log(e.ChatMessage.DisplayName + ":" + e.ChatMessage.Message);
            var WillFromAfar = new TextToSpeech();
            switch (e.ChatMessage.Message)
            {
                default:
                    WillFromAfar.ConvertText(e.ChatMessage.Message.Substring(5));
                    WillFromAfar.Speak();
                    break;
            }
        }
    }
}
