using System.IO;
using SelectPdf;

namespace SelectPdfExamples.HtmlToPdfTutorial
{
    /// <summary>Fits the whole document onto a single, taller PDF page (no page breaks).</summary>
    public static class Section10_SinglePagePdf
    {
        public static string Run(string outputDir)
        {
            string outputPdf = Path.Combine(outputDir, "single-page.pdf");

            #region snippet
            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.RenderingEngine = RenderingEngine.Chromium;

            // Produce one continuous page sized to the content, instead of paginating.
            converter.Options.GenerateSinglePagePdf = true;

            PdfDocument doc = converter.ConvertHtmlString(
                "<h1>Single-page output</h1>" +
                "<p>Section one.</p><p>Section two.</p><p>Section three.</p>" +
                "<p>All content stays on a single, taller page with no page breaks.</p>");

            doc.Save(outputPdf);
            doc.Close();
            #endregion

            return outputPdf;
        }
    }
}
