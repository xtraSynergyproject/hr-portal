using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HiQPdf;
using System.Web;
using System.Text.RegularExpressions;
using System.IO;
using System.Drawing;
using System.Net;
namespace ERP.Utility
{
    public class Pdf
    {
        HtmlToPdf htmlToPdfConverter;
        void Test(HtmlLoadedParams eventParams)
        {

            string alertMessage = "Your password is 1234";
            string javaScriptCode = "app.alert({cMsg:\"" + alertMessage + "\", cTitle: \"Password Hint\"});";

            // create the JavaScript action to display the alert
            PdfJavaScriptAction javaScriptAction =
                new PdfJavaScriptAction(javaScriptCode);

            // set the document JavaScript open action
            htmlToPdfConverter.Document.PdfDocumentObject.SetOpenAction(javaScriptAction);
        }
        void HtmlToPdfConverter_HtmlLoadedEvent(HtmlLoadedParams eventParams)
        {

            if (eventParams.HtmlWidthPoints > Constant.MaxPdfPageSize)
            {
                htmlToPdfConverter.Document.FitPageWidth = true;
                htmlToPdfConverter.Document.PdfDocumentObject.Pages.Remove(0);
                htmlToPdfConverter.Document.PdfDocumentObject.Pages.AddPage(new HiQPdf.PdfPageSize(Constant.MaxPdfPageSize, Constant.MaxPdfPageSize), new PdfDocumentMargins(5));
            }
            else if (eventParams.HtmlHeightPoints > Constant.MaxPdfPageSize)
            {
                htmlToPdfConverter.Document.FitPageHeight = true;
                htmlToPdfConverter.Document.PostCardMode = false;
                htmlToPdfConverter.Document.PdfDocumentObject.Pages.Remove(0);
                htmlToPdfConverter.Document.PdfDocumentObject.Pages.AddPage(new HiQPdf.PdfPageSize(eventParams.HtmlWidthPoints, Constant.MaxPdfPageSize), new PdfDocumentMargins(5));

            }
        }


        public void HtmlToPdf(string html, int width, int height, string fileName)
        {
            var pdfBuffer = GetPdfBuffer(html, width, height);
            //// inform the browser about the binary data format
            HttpContext.Current.Response.AddHeader("Content-Type", "application/pdf");

            //// let the browser know how to open the PDF document
            HttpContext.Current.Response.AddHeader("Content-Disposition", String.Concat("attachment; filename=", fileName, ".pdf;size=", pdfBuffer.Length.ToString()));
            ////// write the PDF buffer to HTTP response

            HttpContext.Current.Response.BinaryWrite(pdfBuffer);

            //// call End() method of HTTP response 
            //// to stop ASP.NET page processing
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }

       


        public void HtmlToPdf(string html, HiQPdf.PdfPageSize pageSize, HiQPdf.PdfPageOrientation orientation, string fileName)
        {
            // html = File.ReadAllText(@"D:\test.txt");

            // create the HTML to PDF converter
            HtmlToPdf htmlToPdfConverter = new HtmlToPdf();
            htmlToPdfConverter.SerialNumber = Utility.Constant.HiqPdfSerialNumber;
            // set browser width
            // htmlToPdfConverter.BrowserWidth = 1200;
            string baseUrl = HttpContext.Current.Request.Url.AbsoluteUri;
            var pdfBuffer = htmlToPdfConverter.ConvertHtmlToMemory(html, baseUrl);

            // inform the browser about the binary data format
            HttpContext.Current.Response.AddHeader("Content-Type", "application/pdf");

            // let the browser know how to open the PDF document
            HttpContext.Current.Response.AddHeader("Content-Disposition",
                            String.Format(string.Concat("attachment; filename=", fileName, ".pdf;  size={0}"), pdfBuffer.Length.ToString()));

            // write the PDF buffer to HTTP response
            HttpContext.Current.Response.BinaryWrite(pdfBuffer);

            // call End() method of HTTP response 
            // to stop ASP.NET page processing
            HttpContext.Current.Response.End();
            return;



            //htmlToPdfConverter = new HtmlToPdf();
            //htmlToPdfConverter.SerialNumber = Utility.Constant.HiqPdfSerialNumber;
            //htmlToPdfConverter.Document.Margins = new PdfMargins(0);
            //htmlToPdfConverter.Document.PageOrientation = orientation;
            //htmlToPdfConverter.Document.PageSize = pageSize;
            //htmlToPdfConverter.Document.FitPageHeight = true;
            //htmlToPdfConverter.Document.FitPageWidth = true;
            //htmlToPdfConverter.Document.ForceFitPageWidth = true;
            //string baseUrl = HttpContext.Current.Request.Url.AbsoluteUri;
            //var pdfBuffer = htmlToPdfConverter.ConvertHtmlToMemory(html, baseUrl);

            //HttpContext.Current.Response.AddHeader("Content-Type", "application/pdf");
            //HttpContext.Current.Response.AddHeader("Content-Disposition", String.Concat("attachment; filename=", fileName, ".pdf;size=", pdfBuffer.Length.ToString()));
            //HttpContext.Current.Response.BinaryWrite(pdfBuffer);
            //HttpContext.Current.Response.Flush();
            //HttpContext.Current.Response.End();
        }
        public byte[] HtmlToMemoryWithPassword(string html, string password)
        {

            htmlToPdfConverter = new HtmlToPdf();

            htmlToPdfConverter.Document.Security.OpenPassword = password;
            htmlToPdfConverter.SerialNumber = Utility.Constant.HiqPdfSerialNumber;
            return htmlToPdfConverter.ConvertHtmlToMemory(html, "");

        }
        public byte[] HtmlToMemory(string html)
        {
            htmlToPdfConverter = new HtmlToPdf();
            htmlToPdfConverter.SerialNumber = Utility.Constant.HiqPdfSerialNumber;
            return htmlToPdfConverter.ConvertHtmlToMemory(html, "");

        }
        public byte[] GetPdfBuffer(string html, int width, int height)
        {
            //html = Regex.Replace(html, @"(width: \d{5}px)", string.Concat("width: 16000px"), RegexOptions.Multiline);
            //var k = Regex.Matches(html, @"(/^width:/ \dpx)", RegexOptions.Multiline);
            // var k = Regex.Matches(html, @"(width: \d+px)", RegexOptions.Multiline).;
            //html = Regex.Replace(html, @"(width: \d+px)", string.Concat("width: ",  ,"px"), RegexOptions.Multiline);
            width = Helper.ConvertPixelToPoints(width);
            height = Helper.ConvertPixelToPoints(height);
            htmlToPdfConverter = new HtmlToPdf();
            htmlToPdfConverter.SerialNumber = Utility.Constant.HiqPdfSerialNumber;

            htmlToPdfConverter.HtmlLoadedEvent += HtmlToPdfConverter_HtmlLoadedEvent;
            htmlToPdfConverter.Document.Margins = new PdfMargins(5);
            htmlToPdfConverter.Document.PageOrientation = PdfPageOrientation.Landscape;
            htmlToPdfConverter.Document.FitPageWidth = true;
            htmlToPdfConverter.Document.FitPageHeight = true;
            if (width <= HiQPdf.PdfPageSize.A4.Height && height <= HiQPdf.PdfPageSize.A4.Width)
            {
                htmlToPdfConverter.Document.PageSize = HiQPdf.PdfPageSize.A4;
            }
            else if (width <= HiQPdf.PdfPageSize.A3.Height && height <= HiQPdf.PdfPageSize.A3.Width)
            {
                htmlToPdfConverter.Document.PageSize = HiQPdf.PdfPageSize.A3;
            }
            else
            {
                htmlToPdfConverter.Document.FitPageWidth = false;
                htmlToPdfConverter.Document.FitPageHeight = false;
                htmlToPdfConverter.Document.PostCardMode = true;
            }


            //width = width > HiQPdf.PdfPageSize.A4.Height ? width : Convert.ToInt32(HiQPdf.PdfPageSize.A4.Height);
            //height = height > HiQPdf.PdfPageSize.A4.Width ? height : Convert.ToInt32(HiQPdf.PdfPageSize.A4.Width);
            //width = width > Constant.MaxPdfPageSize ? Constant.MaxPdfPageSize : width;
            //height = height > Constant.MaxPdfPageSize ? Constant.MaxPdfPageSize : height;
            //htmlToPdfConverter.Document.FitPageWidth = false;
            //htmlToPdfConverter.Document.FitPageHeight = false;
            // htmlToPdfConverter.Document.PostCardMode = true;

            // the base URL used to resolve images, 
            // CSS and script files

            string baseUrl = HttpContext.Current.Request.Url.AbsoluteUri;
            // convert HTML code to a PDF memory buffer

            return htmlToPdfConverter.ConvertHtmlToMemory(html, baseUrl);


        }










        //public Image HtmlToImage(string html, int width, int height)
        //{

        //    // create the HTML to PDF converter
        //    HtmlToImage htmlToImage = new HtmlToImage();
        //    // set PDF page size and orientation
        //    //htmlToPdfConverter.Document.PageSize = GetSelectedPageSize();
        //    //htmlToPdfConverter.Document.PageOrientation = GetSelectedPageOrientation();
        //    // set PDF page margins
        //    width = width > HiQPdf.PdfPageSize.A3.Width ? width : Convert.ToInt32(HiQPdf.PdfPageSize.A3.Width);
        //    height = height > HiQPdf.PdfPageSize.A3.Height ? height : Convert.ToInt32(HiQPdf.PdfPageSize.A3.Height);
        //    //width = width > Constant.MaxPdfPageSize ? Constant.MaxPdfPageSize : width;
        //    //height = height > Constant.MaxPdfPageSize ? Constant.MaxPdfPageSize : height;
        //    htmlToImage.BrowserWidth = width;
        //    htmlToImage.BrowserHeight = height;
        //    string baseUrl = HttpContext.Current.Request.Url.AbsoluteUri;
        //    // convert HTML code to a PDF memory buffer

        //    byte[] pdfBuffer = htmlToImage.ConvertHtmlToMemory(html, baseUrl);





        //    //HttpContext.Current.Response.AddHeader("Content-Type", "image/jpeg");

        //    //// let the browser know how to open the PDF document
        //    //HttpContext.Current.Response.AddHeader("Content-Disposition",
        //    //    String.Format("attachment; filename=PdfText.jpeg;size={0}", pdfBuffer.Length.ToString()));

        //    //// write the PDF buffer to HTTP response
        //    //HttpContext.Current.Response.BinaryWrite(pdfBuffer);

        //    //// call End() method of HTTP response 
        //    //// to stop ASP.NET page processing
        //    //HttpContext.Current.Response.End();



        //    //return null;
        //    MemoryStream ms = new MemoryStream(pdfBuffer);
        //    Image returnImage = Image.FromStream(ms);
        //    return returnImage;

        //}
        //public void ImageToPdf(Image img, int width, int height)
        //{
        //    // create a PDF document
        //    PdfDocument document = new PdfDocument();
        //    // create a page in document
        //    PdfPage page1 = document.AddPage();
        //    // set a background color for the page
        //    PdfRectangle backgroundRectangle = new PdfRectangle(page1.DrawableRectangle);
        //    backgroundRectangle.Width = width;
        //    backgroundRectangle.Height = height;
        //    backgroundRectangle.BackColor = System.Drawing.Color.Transparent;
        //    page1.Layout(backgroundRectangle);

        //    // create the true type fonts that can be used in document
        //    //System.Drawing.Font sysFont = new System.Drawing.Font("Times New Roman",
        //    //        10, System.Drawing.GraphicsUnit.Point);
        //    //PdfFont pdfFont = document.CreateFont(sysFont);
        //    //PdfFont pdfFontEmbed = document.CreateFont(sysFont, true);

        //    float crtYPos = 0;
        //    float crtXPos = 0;

        //    // layout a clipped image
        //    PdfImage clippedPdfImage = new PdfImage(crtXPos, crtYPos, img);
        //    clippedPdfImage.CanGrow = true;
        //    clippedPdfImage.ClipRectangle =
        //            new System.Drawing.RectangleF(crtXPos, crtYPos, width, height);

        //    var imageLayoutInfo = page1.Layout(clippedPdfImage);

        //    // advance the Y position in the PDF page
        //    crtYPos += clippedPdfImage.ClipRectangle.Height + 5;

        //    try
        //    {
        //        // write the PDF document to a memory buffer
        //        byte[] pdfBuffer = document.WriteToMemory();

        //        // inform the browser about the binary data format
        //        HttpContext.Current.Response.AddHeader("Content-Type", "application/pdf");

        //        // let the browser know how to open the PDF document
        //        HttpContext.Current.Response.AddHeader("Content-Disposition",
        //            String.Format("attachment; filename=PdfText.pdf;size={0}", pdfBuffer.Length.ToString()));

        //        // write the PDF buffer to HTTP response
        //        HttpContext.Current.Response.BinaryWrite(pdfBuffer);

        //        // call End() method of HTTP response 
        //        // to stop ASP.NET page processing
        //        HttpContext.Current.Response.End();
        //    }
        //    finally
        //    {
        //        document.Close();
        //    }
        //}
    }
}
