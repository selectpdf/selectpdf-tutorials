using System.IO;
using SelectPdf;

namespace SelectPdfExamples.HtmlToPdfTutorial
{
    /// <summary>Converts a live web page (by URL) to a PDF document.</summary>
    public static class Section02_UrlToPdf
    {
        public static string Run(string outputDir)
        {
            string outputPdf = Path.Combine(outputDir, "url.pdf");

            #region snippet
            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.RenderingEngine = RenderingEngine.Chromium;

            // Convert a live web page to a PDF document.
            PdfDocument doc = converter.ConvertUrl("https://www.selectpdf.com");

            doc.Save(outputPdf);
            doc.Close();
            #endregion

            return outputPdf;
        }
    }
}
