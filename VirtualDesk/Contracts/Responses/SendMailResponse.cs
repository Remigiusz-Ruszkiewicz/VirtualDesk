using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualDesk.Models;

namespace VirtualDesk.Contracts.Responses
{
    public class SendMailResponse
    {
        public SendMailResponse()
        {
            ToAddresses = new List<MailSender>();
            FromAddresses = new List<MailSender>();
        }
        public List<MailSender> ToAddresses { get; set; }
        public List<MailSender> FromAddresses { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    }
}
