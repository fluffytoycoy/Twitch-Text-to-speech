using NAudio.Wave;
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
        private WebRequest WebRequestUrl;
        private string VoiceUrlString;

        public TextToSpeech()
        {
            Client = new HttpClient();
            DummyFormUrl = new FormUrlEncodedContent(new Dictionary<string, string>());
            BuildWebRequest();
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
                using (Stream ms = new MemoryStream())
                {
                    using (Stream stream = WebRequest.Create(VoiceUrlString)
                        .GetResponse().GetResponseStream())
                    {
                        byte[] buffer = new byte[32768];
                        int read;
                        while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            ms.Write(buffer, 0, read);
                        }
                    }

                    ms.Position = 0;
                    using (WaveStream blockAlignedStream =
                        new BlockAlignReductionStream(
                            WaveFormatConversionStream.CreatePcmStream(
                                new Mp3FileReader(ms))))
                    {
                        using (WaveOut waveOut = new WaveOut(WaveCallbackInfo.FunctionCallback()))
                        {
                            waveOut.Init(blockAlignedStream);
                            waveOut.Play();
                            while (waveOut.PlaybackState == PlaybackState.Playing)
                            {
                                System.Threading.Thread.Sleep(100);
                            }
                        }
                    }
                }
            }
            else
            {
                throw new Exception("VoiceUrlString was empty");
            }
        }

        private void RequestVoiceUrlString(string text, string voiceid)
        {
            using (var stream = WebRequestUrl.GetRequestStream())
            {
                var voiceData = EncodeVoiceData(text, voiceid);
                stream.WriteAsync(voiceData, 0, voiceData.Length);
            }

            VoiceUrlString = ParseVoiceString(WebRequestUrl.GetResponse());
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

        private void BuildWebRequest()
        {
            WebRequestUrl = WebRequest.Create
            ("http://www.acapela-group.com:8080/webservices/1-34-01-Mobility/Synthesizer");
            WebRequestUrl.Method = "POST";
            WebRequestUrl.ContentType = "application/x-www-form-urlencoded";
        }

    }
}
