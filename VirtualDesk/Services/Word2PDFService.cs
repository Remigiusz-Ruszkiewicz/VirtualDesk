using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VirtualDesk.Models;
using WordToPDF;

namespace VirtualDesk.Services
{
    public class Word2PDFService : IWord2PDFService
    {
        public readonly string fileLocation = @"C:\Testy";
        public async Task<string> ConvertAsync(Stream stream, string filename)
        {         
            string destPath = fileLocation + "\\" + filename;
            using (var fileStream = new FileStream(destPath, FileMode.Create, FileAccess.Write))
            {
                stream.CopyTo(fileStream);
            }

            var word2Pdf = new Word2Pdf();

            string extension = Path.GetExtension(filename);
            object file = fileLocation + "\\" + filename;
            string changeExtension = filename.Replace(extension, ".pdf");

            if (File.Exists(fileLocation + "\\" + changeExtension))
            {
                File.Delete(fileLocation + "\\" + changeExtension);
            }

            if (extension == ".doc" || extension == ".docx")
            {
                object saveLocation = fileLocation + "\\" + changeExtension;
                word2Pdf.InputLocation = file;
                word2Pdf.OutputLocation = saveLocation;
                word2Pdf.Word2PdfCOnversion();
            }

            File.Delete(destPath);
            return fileLocation + "\\" + changeExtension;
        }
    }
}
