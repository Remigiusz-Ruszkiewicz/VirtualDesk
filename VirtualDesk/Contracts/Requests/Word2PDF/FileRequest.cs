using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualDesk.Contracts.Requests.Word2PDF
{
    public class FileRequest
    {
        public IFormFile File { get; set; }
    }
}
