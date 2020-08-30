using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtualDesk.Contracts.Requests.Slack
{
    public class SLackAttachmentRequest
    {
        public string channel { get; set; }
        public IFormFile File { get; set; }
    }
}
