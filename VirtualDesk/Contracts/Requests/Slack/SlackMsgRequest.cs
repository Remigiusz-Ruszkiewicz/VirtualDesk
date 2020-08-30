using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtualDesk.Contracts.Requests
{
    public class SlackMsgRequest
    {
        public string text { get; set; }
        public string channel { get; set; }
        public string as_user { get; set; }
    }
}
