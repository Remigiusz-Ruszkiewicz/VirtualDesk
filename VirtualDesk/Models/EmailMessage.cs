using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtualDesk.Models
{
    public class EmailMessage
    {
        public EmailMessage()
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
