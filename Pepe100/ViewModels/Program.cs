using PdfSharp.Pdf.Content;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Pepe100.ViewModels
{
    public class Program
    {
        ///// PDF MUOKKAUS TESTIÄ

        //static void Main(string[] args)
        //{
        //    string originalPdf = @"C:\Users\juhoh\source\repos\Pepe100\Pepe100\Sopimus\Sopimus2.pdf";



        //    //CreatePdf(originalPdf);

        //    using (var doc = PdfReader.Open(originalPdf, PdfDocumentOpenMode.Modify))
        //    {
        //        var page = doc.Pages[0];
        //        var contents = ContentReader.ReadContent(page);
                
        //        ReplaceText(contents, "Hello", "Hola");
        //        page.Contents.ReplaceContent(contents);

        //        doc.Pages.Remove(page);
        //        doc.AddPage().Contents.ReplaceContent(contents);

        //        doc.Save(originalPdf);
        //    }

        //    Process.Start(originalPdf);

        //}

    }
}