using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Api;
using TwitchLib.Api.Interfaces;
using TwitchLib.Api.Models.v5.Streams;
using TwitchLib.Api.Models.v5.Users;
using TwitchLib.Client;
using TwitchLib.Client.Models;

namespace WillFromAfarBot
{
    public class MessageHandler
    {
        private TwitchAPI twitchAPI = new TwitchAPI();

        public ChatMessage Message { get; set; }
        public TwitchClient Client { get; set; }

        public MessageHandler(ChatMessage message, TwitchClient client)
        {
            Message = message;
            Client = client;
        }

        public void HandleMessage()
        {
            Logger.Log(Message.DisplayName + ":" + Message.Message + "\n");

            if (Message.Message.StartsWith("!"))
            {

                var message = Message.Message.Trim();

                #region !Will Text to speech
                if (message.StartsWith("!will"))
                {
                    var WillFromAfar = new TextToSpeech();
                    WillFromAfar.ConvertText(Message.Message.Substring(5));
                    WillFromAfar.Speak();
                }
                #endregion

                #region !Test messages the current channel
                else if (message.StartsWith("!test"))
                {
                    Client.SendMessage(Message.Channel, $"you are testing this in {Message.Channel}'s channel");
                }
                #endregion 

                #region !Coolest messages the current channel
                else if (message.StartsWith("!coolest", StringComparison.OrdinalIgnoreCase))
                {
                    Client.SendMessage(Message.Channel, $"It is totally {Message.Channel}");
                }
                #endregion

                #region !Stats messages stats
                else if (message.StartsWith("!stats", StringComparison.OrdinalIgnoreCase))
                {
                    Stream streamInfo;

                    if (message.Length == 6)
                    {
                        streamInfo = GetCurrentInfoAsync(Message.Channel);
                        if (streamInfo == null)
                        {
                            Client.SendMessage(Message.Channel, $"{Message.Channel} is currently offline");
                        }
                        else
                        {
                            PrintStreamInfo(Message.Channel, streamInfo);
                        }

                    }
                    else
                    {
                        if (message.Substring(6).Trim().Contains(" "))
                        {
                            Client.SendMessage(Message.Channel, "What kind of channel name is that?");
                        }
                        else
                        {
                            streamInfo = GetCurrentInfoAsync(Message.Channel);
                            if (streamInfo == null)
                            {
                                Client.SendMessage(Message.Channel, $"{Message.Channel} is currently offline");
                            }
                            else
                            {
                                PrintStreamInfo(Message.Channel, streamInfo);
                            }
                        }
                    }

                }
                #endregion
            }
        }

        private Stream GetCurrentInfoAsync(string channel)
        {
            var stream = twitchAPI.Streams.v5.GetStreamByUserAsync(GetUserId(channel));
            return stream.Result.Stream;
        }

        private void PrintStreamInfo(string messagedChannel, Stream searchedChannel)
        {
            Client.SendMessage(messagedChannel, $"{searchedChannel.Game}");
        }

        private string GetUserId(string channelName)
        {
            var user = twitchAPI.Users.v5.GetUserByNameAsync(channelName).Result.Matches;
            return user[0].Id;
        }
    }
}
