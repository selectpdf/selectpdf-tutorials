using System.IO;
using SelectPdf;

namespace SelectPdfExamples.HtmlToPdfTutorial
{
    /// <summary>Waits for JavaScript-rendered content before capturing the page.</summary>
    public static class Section12_WaitForJavaScript
    {
        public static string Run(string outputDir)
        {
            string outputPdf = Path.Combine(outputDir, "wait-for-javascript.pdf");

            #region snippet
            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.RenderingEngine = RenderingEngine.Chromium;

            // Give client-side scripts time to run before the page is captured (seconds).
            converter.Options.MinPageLoadTime = 2;

            PdfDocument doc = converter.ConvertHtmlString(
                "<h1>Dynamic content</h1>" +
                "<p id='status'>Loading...</p>" +
                "<script>setTimeout(function(){" +
                "document.getElementById('status').innerHTML = 'Loaded by JavaScript!';" +
                "}, 800);</script>");

            doc.Save(outputPdf);
            doc.Close();
            #endregion

            return outputPdf;
        }
    }
}
