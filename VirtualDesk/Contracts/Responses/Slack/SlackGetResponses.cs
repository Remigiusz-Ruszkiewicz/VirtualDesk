using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtualDesk.Contracts.Responses.Slack
{
    public class SlackGetResponses
    {
        public string text { get; set; }
        public string message { get; set; }
        public string user { get; set; }
        public string channel { get; set; }
    }
}
