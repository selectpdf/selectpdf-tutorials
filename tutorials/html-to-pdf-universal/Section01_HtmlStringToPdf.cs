using System.IO;
using SelectPdf.Universal;

namespace SelectPdfExamples.HtmlToPdfUniversalTutorial
{
    /// <summary>Converts an inline HTML string to a PDF document.</summary>
    public static class Section01_HtmlStringToPdf
    {
        public static string Run(string outputDir)
        {
            string outputPdf = Path.Combine(outputDir, "html-string.pdf");

            #region snippet
            // Create the HTML to PDF converter. Chromium is the only engine, so there is nothing to select.
            HtmlToPdf converter = new HtmlToPdf();

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
