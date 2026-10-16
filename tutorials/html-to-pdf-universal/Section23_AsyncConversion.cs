using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SelectPdf.Universal;

namespace SelectPdfExamples.HtmlToPdfUniversalTutorial
{
    /// <summary>Converts HTML without blocking a thread, with a cancellation token and a using block.</summary>
    public static class Section23_AsyncConversion
    {
        public static string Run(string outputDir) => RunAsync(outputDir).GetAwaiter().GetResult();

        private static async Task<string> RunAsync(string outputDir)
        {
            string outputPdf = Path.Combine(outputDir, "async-conversion.pdf");

            #region snippet
            // Give up if the conversion takes longer than 30 seconds - or pass the
            // request's token (HttpContext.RequestAborted) in a web app.
            using CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));

            HtmlToPdf converter = new HtmlToPdf();

            // Await the conversion instead of blocking a thread while Chromium renders.
            // PdfDocument is IDisposable, so a using declaration replaces Close().
            using PdfDocument doc = await converter.ConvertHtmlStringAsync(
                "<h1>Converted asynchronously</h1>" +
                "<p>No thread was blocked while this page was rendered.</p>",
                cts.Token);

            doc.Save(outputPdf);
            #endregion

            return outputPdf;
        }
    }
}
