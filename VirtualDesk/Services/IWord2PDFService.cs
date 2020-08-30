using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VirtualDesk.Models;

namespace VirtualDesk.Services
{
    public interface IWord2PDFService
    {
        Task<string> ConvertAsync(Stream stream, string fileName);
    }
}
