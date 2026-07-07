using System.IO;
using SelectPdf;

namespace SelectPdfExamples.HtmlToPdfTutorial
{
    /// <summary>Injects extra CSS and runs a script after the page loads.</summary>
    public static class Section13_CustomCssAndScript
    {
        public static string Run(string outputDir)
        {
            string outputPdf = Path.Combine(outputDir, "custom-css.pdf");

            #region snippet
            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.RenderingEngine = RenderingEngine.Chromium;

            // Inject additional CSS into the page before rendering.
            converter.Options.CustomCSS =
                "body { font-family: Arial; background: #fef3c7; }" +
                "h1 { color: #b45309; }";

            // Run JavaScript after the page has loaded (Chromium/Blink only).
            converter.Options.PostLoadingScript =
                "document.body.insertAdjacentHTML('beforeend', '<p>Added by script.</p>');";

            PdfDocument doc = converter.ConvertHtmlString(
                "<h1>Custom styled page</h1>" +
                "<p>The amber background and heading color come from injected CSS.</p>");

            doc.Save(outputPdf);
            doc.Close();
            #endregion

            return outputPdf;
        }
    }
}
