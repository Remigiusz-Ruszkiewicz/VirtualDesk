using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VirtualDesk.Contracts;
using VirtualDesk.Contracts.Requests.Word2PDF;
using VirtualDesk.Models;
using VirtualDesk.Services;

namespace VirtualDesk.Controllers
{
    [ApiController]
    public class Word2PDFController : ControllerBase
    {
        private readonly IWord2PDFService word2PDFService;
        public Word2PDFController(IWord2PDFService word2PDFService)
        {
            this.word2PDFService = word2PDFService;
        }
        /// <summary>
        /// Convert .doc/.docx to .pdf
        /// </summary>
        /// <param name="fileRequest"></param>
        /// <returns></returns>
        [HttpPost(ApiRoutes.Word2PDF.WordToPDF)]
        public async Task<IActionResult> ConvertFileAsync([FromForm]FileRequest fileRequest)
        {
            string link = await word2PDFService.ConvertAsync(fileRequest.File.OpenReadStream(), fileRequest.File.FileName);

            var net = new System.Net.WebClient();
            var data = net.DownloadData(link);
            var content = new System.IO.MemoryStream(data);
            var contentType = "APPLICATION/pdf";
            var fileName = Path.GetFileName(link);

            return File(content, contentType, fileName);
        }
    }
}