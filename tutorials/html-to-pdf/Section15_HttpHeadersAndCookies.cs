using System.IO;
using SelectPdf;

namespace SelectPdfExamples.HtmlToPdfTutorial
{
    /// <summary>Sends custom HTTP headers and cookies with the request (e.g. for auth).</summary>
    public static class Section15_HttpHeadersAndCookies
    {
        public static string Run(string outputDir)
        {
            string outputPdf = Path.Combine(outputDir, "http-headers-cookies.pdf");

            #region snippet
            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.RenderingEngine = RenderingEngine.Chromium;

            // Attach custom request headers (e.g. language, bearer token) and cookies.
            converter.Options.HttpHeaders.Add("Accept-Language", "en-US");
            converter.Options.HttpCookies.Add("session", "demo-cookie-value");

            PdfDocument doc = converter.ConvertUrl("https://www.selectpdf.com");

            doc.Save(outputPdf);
            doc.Close();
            #endregion

            return outputPdf;
        }
    }
}
