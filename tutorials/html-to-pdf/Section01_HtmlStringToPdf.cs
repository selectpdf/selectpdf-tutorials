using System.IO;
using SelectPdf;

namespace SelectPdfExamples.HtmlToPdfTutorial
{
    /// <summary>Converts an inline HTML string to a PDF document.</summary>
    public static class Section01_HtmlStringToPdf
    {
        public static string Run(string outputDir)
        {
            string outputPdf = Path.Combine(outputDir, "html-string.pdf");

            #region snippet
            // Create the HTML to PDF converter and select the Chromium (CEF) engine.
            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.RenderingEngine = RenderingEngine.Chromium;

            // Convert a raw HTML string into a PDF document.
            PdfDocument doc = converter.ConvertHtmlString(
                "<h1>Hello, SelectPdf!</h1>" +
                "<p>This PDF was generated from an HTML string in C#.</p>");

            // Save the document to disk, then release the native resources.
            doc.Save(outputPdf);
            doc.Close();
            #endregion

            return outputPdf;
        }
    }
}
