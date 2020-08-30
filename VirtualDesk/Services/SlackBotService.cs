using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using VirtualDesk.Contracts.Requests;
using System.Net.Http;
using System.IO;
using Newtonsoft.Json;
using VirtualDesk.Contracts.Responses.Slack;
using VirtualDesk.Helpers;
using Slackbot;

namespace VirtualDesk.Services
{
    public class SlackBotService : ISlackBotService
    {
        private readonly string  token = "xoxp-914359964370-927168311733-936338397655-4436342264bccf221f3d3683e5497fdf";
        private static readonly HttpClient client = new HttpClient();
        private readonly WebClient webClient = new WebClient();

        public async Task<string> SendAsync(SlackMsgRequest msgRequest)
        {
            var data = new NameValueCollection();
            data["token"] = token;
            data["channel"] = msgRequest.channel;
            data["as_user"] = msgRequest.as_user;
            if (data["as_user"] == "false")
            {
                data["token"] = "xoxb - 914359964370 - 920226424034 - 5eUglTta5udR23ci4r6KlkDV";
            }
            // to send this message as the user who owns the token, false by default
            data["text"] = msgRequest.text;

            
            var response = webClient.UploadValues("https://slack.com/api/chat.postMessage", "POST", data);
            string responseInString = Encoding.UTF8.GetString(response);
            return responseInString;
        }
        //public Task<SlackBot> GetAsync(SlackBot sb);


        public async Task UploadFileAsync(string token, Stream stream, string fileName, string channels)
        {
            var multiForm = new MultipartFormDataContent();

            multiForm.Add(new StringContent(token), "token");
            multiForm.Add(new StringContent(channels), "channels");

            multiForm.Add(new StreamContent(stream), "file", fileName);

            var url = "https://slack.com/api/files.upload";
            var response = await client.PostAsync(url, multiForm);

            var responseJson = await response.Content.ReadAsStringAsync();

            SlackFileResponse fileResponse =
                JsonConvert.DeserializeObject<SlackFileResponse>(responseJson);

            if (fileResponse.ok == false)
            {
                throw new Exception(
                    "failed to upload message: " + fileResponse.error
                );
            }
            else
            {
                Console.WriteLine(
                        "Uploaded new file with id: " + fileResponse.file.id
                );
            }
        }
        public async Task<string> GetMessageAsync(string channel)
        {            
            
            var response = webClient.DownloadString($"https://slack.com/api/conversations.history?token={token}&channel={channel}&pretty=1");
            dynamic stuff = JsonConvert.DeserializeObject(response);
            SlackGetResponses newResponse = new SlackGetResponses();

            foreach (dynamic item in stuff.messages)
            {
                
                newResponse.text += item.text += Environment.NewLine;
                

            }
            newResponse.message += newResponse.text;
            return newResponse.message;
        }
        public async Task<string> GetChannelsListAsync()
        {
            var response = webClient.DownloadString($"https://slack.com/api/conversations.list?token={token}&pretty=1");
            dynamic stuff = JsonConvert.DeserializeObject(response);
            SlackGetResponses newResponse = new SlackGetResponses();

            foreach (dynamic item in stuff.channels)
            {
                newResponse.channel += item.id += Environment.NewLine;

            }
            return newResponse.channel;
        }


    }
}