using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtualDesk.Contracts.Responses
{
    public class SlackBotResponses
    {
        public string channel { get; set; }
        public string text { get; set; }
        public string user { get; set; }

    }
}
