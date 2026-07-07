using System.IO;
using SelectPdf;

namespace SelectPdfExamples.HtmlToPdfTutorial
{
    /// <summary>Converts a local .html file to a PDF document.</summary>
    public static class Section03_HtmlFileToPdf
    {
        public static string Run(string outputDir)
        {
            string tutorialDir = Directory.GetParent(outputDir).FullName;
            string htmlFile = Path.Combine(tutorialDir, "assets", "invoice.html");
            string outputPdf = Path.Combine(outputDir, "html-file.pdf");

            #region snippet
            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.RenderingEngine = RenderingEngine.Chromium;

            // ConvertUrl also accepts a local file path; relative CSS and images
            // referenced by the file are resolved automatically.
            PdfDocument doc = converter.ConvertUrl(htmlFile);

            doc.Save(outputPdf);
            doc.Close();
            #endregion

            return outputPdf;
        }
    }
}
