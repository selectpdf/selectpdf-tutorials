using System.IO;
using SelectPdf;

namespace SelectPdfExamples.HtmlToPdfTutorial
{
    /// <summary>Controls the virtual browser width and shrinks oversized content to fit.</summary>
    public static class Section14_ViewportWidth
    {
        public static string Run(string outputDir)
        {
            string outputPdf = Path.Combine(outputDir, "viewport-width.pdf");

            #region snippet
            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.RenderingEngine = RenderingEngine.Chromium;

            // Lay the HTML out at a fixed 1200px browser width...
            converter.Options.WebPageWidth = 1200;
            converter.Options.WebPageFixedSize = true;

            // ...then shrink it so the wide layout fits the PDF page width.
            converter.Options.AutoFitWidth = HtmlToPdfPageFitMode.ShrinkOnly;

            PdfDocument doc = converter.ConvertHtmlString(
                "<div style='width:1200px;background:#e0f2fe;padding:24px'>" +
                "<h1>Wide layout</h1>" +
                "<p>Rendered at a 1200px viewport, then shrunk to fit the page.</p></div>");

            doc.Save(outputPdf);
            doc.Close();
            #endregion

            return outputPdf;
        }
    }
}
