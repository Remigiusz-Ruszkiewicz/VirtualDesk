using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtualDesk.Contracts.Responses.Slack
{
    public class SlackFileResponse
    {
        public bool ok { get; set; }
        public String error { get; set; }
        public SlackFile file { get; set; }

    }
    public class SlackFile
    {
        public String id { get; set; }
        public String name { get; set; }
    }
}
