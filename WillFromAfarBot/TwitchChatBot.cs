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
        private TwitchAPI twitchAPI = new TwitchAPI();
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

            Logger.Log(e.ChatMessage.DisplayName + ":" + e.ChatMessage.Message + "\n");

            if (e.ChatMessage.Message.Contains("gamecube or nunchuck"))
            {
                client.SendMessage(e.ChatMessage.Channel, $"Well {e.ChatMessage.DisplayName} through mass computations I have found that the game cube is superior");
            }



            else if (e.ChatMessage.Message.StartsWith("test", StringComparison.OrdinalIgnoreCase))
            {
                
    
                //var stream = twitchAPI.Streams.v5.GetStreamByUserAsync();
                var streamer = twitchAPI.Users.v5.GetUserByNameAsync("fluffytoycoy");
                var test = streamer.Result.Matches[0].Id;
                var stream2 = twitchAPI.Streams.v5.GetStreamByUserAsync(test);
                var stream = twitchAPI.Streams.v5.GetStreamByUserAsync("47077109");
                var streaminfo = stream2.Result;
                //var streamR = stream.Result;
                client.SendMessage(e.ChatMessage.Channel, $"{stream.Result}It is totally {stream}");
                var streaminfo2 = stream.Result;
            }

            if (e.ChatMessage.Message.StartsWith("!"))
            {
                if (e.ChatMessage.Message.StartsWith("!will"))
                {
                    var WillFromAfar = new TextToSpeech();
                    WillFromAfar.ConvertText(e.ChatMessage.Message.Substring(5));
                    WillFromAfar.Speak();
                }
                if (e.ChatMessage.Message.StartsWith("!test"))
                {
                    client.SendMessage(e.ChatMessage.Channel, $"you are testing this in {e.ChatMessage.Channel}'s channel");
                }
                if (e.ChatMessage.Message.StartsWith("!Hey, spacebotcraig who is the coolest streamer", StringComparison.OrdinalIgnoreCase))
                {
                    client.SendMessage(e.ChatMessage.Channel, $"It is totally {e.ChatMessage.Channel}");
                }
            }
        }

        private async Task<StreamByUser> GetCurrentGameAsync()
        {
            var stream = await twitchAPI.Streams.v5.GetStreamByUserAsync(GetUserId(Info.ChannelName));
            return stream;
        }

        private string GetUserId(string channelName)
        {
            var user = twitchAPI.Users.v5.GetUserByNameAsync(channelName).Result.Matches;
            return user[0].Id;
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
