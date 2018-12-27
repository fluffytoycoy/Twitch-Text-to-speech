using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace WillFromAfarBot
{
    class TextToSpeech
    {
        private HttpClient Client;
        private readonly FormUrlEncodedContent DummyFormUrl;
        public WebRequest RequestUrl { get; set; }
        public string VoiceUrlString { get; set; }

        public TextToSpeech()
        {
            Client = new HttpClient();
            DummyFormUrl = new FormUrlEncodedContent(new Dictionary<string, string>());
            RequestUrl = WebRequest.Create
            ("http://www.acapela-group.com:8080/webservices/1-34-01-Mobility/Synthesizer");
            RequestUrl.Method = "POST";
            RequestUrl.ContentType = "application/x-www-form-urlencoded";
        }

        public void ConvertText(string text, string voice = "")
        {
            switch (voice)
            {
                case "WillLittleCreature":
                    RequestVoiceUrlString(text, "willlittlecreature_22k_ns.bvcu");
                    break;
                default:
                    RequestVoiceUrlString(text, "willfromafar_22k_ns.bvcu");
                    break;
            }
        }

        public void Speak()
        {
            if (VoiceUrlString != "")
            {
                WMPLib.WindowsMediaPlayer wplayer = new WMPLib.WindowsMediaPlayer
                {
                    URL = VoiceUrlString
                };

                wplayer.controls.play();
            }
            else
            {
                throw new Exception("VoiceUrlString was empty");
            }
        }

        private void RequestVoiceUrlString(string text, string voiceid)
        {
            using (var stream = RequestUrl.GetRequestStream())
            {
                var voiceData = EncodeVoiceData(text, voiceid);
                stream.WriteAsync(voiceData, 0, voiceData.Length);
            }

            VoiceUrlString = ParseVoiceString(RequestUrl.GetResponse());
        }

        private byte[] EncodeVoiceData(string text, string voiceid)
        {
            var urlString = $"req_voice=enu_{voiceid}&cl_pwd=&cl_vers=1-30&req_echo=ON&cl" +
                $"_login=AcapelaGroup&req_comment=%7B%22nonce%22%3A%22{GetEncriptionCode()}" +
                $"%22%2C%22user%22%3A%22null%22%7D&req_text={Uri.EscapeDataString(text)}&cl_" +
                $"env=ACAPELA_VOICES&prot_vers=2&cl_app=AcapelaGroup_WebDemo_Android";
            return Encoding.ASCII.GetBytes(urlString);
        }

        private string GetEncriptionCode()
        {
            var responseCode = Client.PostAsync("https://acapelavoices.acapela-group.com/index/getnonce/", DummyFormUrl).Result
            .Content.ReadAsStringAsync().Result;

            return ParseDummyCode(responseCode);
        }

        private string ParseVoiceString(WebResponse request)
        {
            var responseString = new StreamReader(request.GetResponseStream()).ReadToEnd();
            var regex = new Regex("snd_url=(.+)&snd_size");
            var response = regex.Match(responseString);
            if (response.Success)
            {
                return response.Groups[1].Value;
            }

            return "";
        }

        private string ParseDummyCode(string unparsedCode)
        {
            var regex = new Regex(@"^\{\""nonce\""\:\""(.+)\""\}$");
            var code = regex.Match(unparsedCode);

            return code.Groups.Count > 1 ? code.Groups[1].ToString() : "";
        }
    }
}
