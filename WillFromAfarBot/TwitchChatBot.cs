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

            if (e.ChatMessage.Message.StartsWith("!"))
            {
                var message = e.ChatMessage.Message.Trim();
      
                #region !Will Text to speech
                if (message.StartsWith("!will"))
                {
                    var WillFromAfar = new TextToSpeech();
                    WillFromAfar.ConvertText(e.ChatMessage.Message.Substring(5));
                    WillFromAfar.Speak();
                }
                #endregion

                #region !Test messages the current channel
                else if (message.StartsWith("!test"))
                {
                    client.SendMessage(e.ChatMessage.Channel, $"you are testing this in {e.ChatMessage.Channel}'s channel");
                }
                #endregion 

                #region !Coolest messages the current channel
                else if (message.StartsWith("!coolest", StringComparison.OrdinalIgnoreCase))
                {
                    client.SendMessage(e.ChatMessage.Channel, $"It is totally {e.ChatMessage.Channel}");
                }
                #endregion

                #region !Stats messages stats
                else if (message.StartsWith("!stats", StringComparison.OrdinalIgnoreCase))
                {
                    Stream streamInfo;

                    if (message.Length == 6)
                    {
                        streamInfo = GetCurrentInfoAsync(e.ChatMessage.Channel);
                        if (streamInfo == null)
                        {
                            client.SendMessage(e.ChatMessage.Channel, $"{e.ChatMessage.Channel} is currently offline");
                        }
                        else
                        {
                            PrintStreamInfo(e.ChatMessage.Channel, streamInfo);
                        }

                    }
                    else
                    {
                        if (message.Substring(6).Trim().Contains(" "))
                        {
                            client.SendMessage(e.ChatMessage.Channel, "What kind of channel name is that?");
                        }
                        else
                        {
                            streamInfo = GetCurrentInfoAsync(e.ChatMessage.Channel);
                            if (streamInfo == null)
                            {
                                client.SendMessage(e.ChatMessage.Channel, $"{e.ChatMessage.Channel} is currently offline");
                            }
                            else
                            {
                                PrintStreamInfo(e.ChatMessage.Channel, streamInfo);
                            }
                        }
                    }

                }
                #endregion
            }
        }

        private void PrintStreamInfo(string messagedChannel, Stream searchedChannel)
        {
            client.SendMessage(messagedChannel, $"{searchedChannel.Game}");
        }

        private Stream GetCurrentInfoAsync(string channel)
        {
            var stream = twitchAPI.Streams.v5.GetStreamByUserAsync(GetUserId(channel));
            return stream.Result.Stream;
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
