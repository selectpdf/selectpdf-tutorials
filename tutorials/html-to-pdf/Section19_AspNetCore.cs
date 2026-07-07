using System.IO;
using Microsoft.AspNetCore.Mvc;
using SelectPdf;

namespace SelectPdfExamples.HtmlToPdfTutorial
{
    /// <summary>Shows how to return a generated PDF from an ASP.NET Core controller action.</summary>
    public static class Section19_AspNetCore
    {
        // Builds the preview image for this tutorial step (runs in the console runner).
        public static string Run(string outputDir)
        {
            string outputPdf = Path.Combine(outputDir, "aspnet-core.pdf");

            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.RenderingEngine = RenderingEngine.Chromium;
            PdfDocument doc = converter.ConvertHtmlString(
                "<h1>Order #1042 confirmed</h1>" +
                "<p>This PDF was generated on the server and streamed straight to the browser.</p>");
            doc.Save(outputPdf);
            doc.Close();

            return outputPdf;
        }
    }

    #region snippet
    // In an ASP.NET Core app, generate the PDF inside a controller action and stream it back.
    public sealed class InvoiceController : ControllerBase
    {
        [HttpGet("/invoice/{id:int}")]
        public IActionResult Get(int id)
        {
            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.RenderingEngine = RenderingEngine.Chromium;

            PdfDocument doc = converter.ConvertHtmlString(
                $"<h1>Invoice #{id}</h1><p>Thank you for your order.</p>");

            // Save straight to a byte array - no temp file needed.
            byte[] pdf = doc.Save();
            doc.Close();

            // Return it as a downloadable file (omit the file name to show it inline).
            return File(pdf, "application/pdf", $"invoice-{id}.pdf");
        }
    }
    #endregion
}
