using System.IO;
using SelectPdf;

namespace SelectPdfExamples.HtmlToPdfTutorial
{
    /// <summary>Converts HTML to PDF with a custom page size, orientation and margins.</summary>
    public static class Section06_PageSettings
    {
        public static string Run(string outputDir)
        {
            string outputPdf = Path.Combine(outputDir, "page-settings.pdf");

            #region snippet
            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.RenderingEngine = RenderingEngine.Chromium;

            // Page setup (margins are expressed in points; 72 points = 1 inch).
            converter.Options.PdfPageSize = PdfPageSize.A4;
            converter.Options.PdfPageOrientation = PdfPageOrientation.Landscape;
            converter.Options.MarginLeft = 36;
            converter.Options.MarginRight = 36;
            converter.Options.MarginTop = 36;
            converter.Options.MarginBottom = 36;

            PdfDocument doc = converter.ConvertHtmlString(
                "<h1>A4 Landscape</h1>" +
                "<p>This page uses a custom size, orientation and half-inch margins.</p>");

            doc.Save(outputPdf);
            doc.Close();
            #endregion

            return outputPdf;
        }
    }
}
