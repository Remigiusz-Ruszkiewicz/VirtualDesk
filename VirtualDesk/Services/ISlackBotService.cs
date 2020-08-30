using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualDesk.Contracts.Requests;
using VirtualDesk.Models;
using System.IO;
using VirtualDesk.Contracts.Responses.Slack;

namespace VirtualDesk.Services
{
    public interface ISlackBotService
    {
        Task<string> SendAsync(SlackMsgRequest msgRequest);
        Task UploadFileAsync(string token, Stream stream, string fileName, string channels);
        Task<string> GetMessageAsync(string channel);
        Task<string> GetChannelsListAsync();
    }
}
