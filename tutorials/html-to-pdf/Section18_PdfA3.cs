using System.IO;
using SelectPdf;

namespace SelectPdfExamples.HtmlToPdfTutorial
{
    /// <summary>Produces an archivable PDF/A-3B document (requires the Chromium engine).</summary>
    public static class Section18_PdfA3
    {
        public static string Run(string outputDir)
        {
            string outputPdf = Path.Combine(outputDir, "pdf-a3.pdf");

            #region snippet
            HtmlToPdf converter = new HtmlToPdf();

            // PDF/A (and tagged/accessible) output requires the Chromium engine.
            converter.Options.RenderingEngine = RenderingEngine.Chromium;

            // Emit a PDF/A-3B document suitable for long-term archiving.
            converter.Options.PdfStandard = PdfStandard.PdfA3B;

            PdfDocument doc = converter.ConvertHtmlString(
                "<h1>Archivable document</h1>" +
                "<p>Saved as PDF/A-3B for long-term archiving and compliance.</p>");

            doc.Save(outputPdf);
            doc.Close();
            #endregion

            return outputPdf;
        }
    }
}
