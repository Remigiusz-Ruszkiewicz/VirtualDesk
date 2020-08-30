using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtualDesk.Contracts.Requests
{
    public class UpdateAccountListRequest
    {
        public string Pass { get; set; }
        public string EmailAdress { get; set; }
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string PopServer { get; set; }
        public int PopPort { get; set; }
        public bool IsPrivate { get; set; }
    }
}
