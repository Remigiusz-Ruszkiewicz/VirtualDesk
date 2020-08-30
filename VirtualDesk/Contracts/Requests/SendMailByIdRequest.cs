using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtualDesk.Contracts.Requests
{
    public class SendMailByIdRequest
    {
        public string NameFrom { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    }
}
