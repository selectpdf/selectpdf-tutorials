using System.IO;
using SelectPdf;

namespace SelectPdfExamples.HtmlToPdfTutorial
{
    /// <summary>Uses an HTML fragment (not just text) as the page header.</summary>
    public static class Section08_HtmlHeaderFooter
    {
        public static string Run(string outputDir)
        {
            string outputPdf = Path.Combine(outputDir, "html-header-footer.pdf");

            #region snippet
            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.RenderingEngine = RenderingEngine.Chromium;

            // Render the header from an HTML fragment so it can be fully styled with CSS.
            converter.Options.DisplayHeader = true;
            converter.Header.Height = 50;

            PdfHtmlSection header = new PdfHtmlSection(
                "<div style='font-family:Arial;color:#b45309;font-size:18px;" +
                "border-bottom:2px solid #b45309;padding:6px 0;'>Acme Corporation</div>",
                string.Empty);
            header.RenderingEngine = RenderingEngine.Chromium;
            header.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
            converter.Header.Add(header);

            PdfDocument doc = converter.ConvertHtmlString(
                "<h1>Report body</h1><p>The header above is rendered from HTML.</p>");

            doc.Save(outputPdf);
            doc.Close();
            #endregion

            return outputPdf;
        }
    }
}
