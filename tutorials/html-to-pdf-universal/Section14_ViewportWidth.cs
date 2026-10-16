using System.IO;
using SelectPdf.Universal;

namespace SelectPdfExamples.HtmlToPdfUniversalTutorial
{
    /// <summary>Sets the virtual browser width and scales content wider than it down to fit.</summary>
    public static class Section14_ViewportWidth
    {
        public static string Run(string outputDir)
        {
            string outputPdf = Path.Combine(outputDir, "viewport-width.pdf");

            #region snippet
            HtmlToPdf converter = new HtmlToPdf();

            // Lay the page out at a 1024px browser width.
            converter.Options.WebPageWidth = 1024;

            // Content wider than that - this 1600px banner - is normally cut at the right
            // edge, as a browser cuts it when printing. FitWideContent lays the page out at
            // the width the content needs and scales it down to fit the PDF page.
            converter.Options.FitWideContent = true;

            PdfDocument doc = converter.ConvertHtmlString(
                "<div style='width:1600px;background:linear-gradient(90deg,#e0f2fe,#b45309);padding:24px'>" +
                "<h1>Wide layout</h1>" +
                "<p style='text-align:right'>This text sits at the right edge of a 1600px banner.</p></div>");

            doc.Save(outputPdf);
            doc.Close();
            #endregion

            return outputPdf;
        }
    }
}
