using System.Drawing;
using System.IO;
using SelectPdf;

namespace SelectPdfExamples.HtmlToPdfTutorial
{
    /// <summary>Stamps a light text watermark behind the content of every page.</summary>
    public static class Section11_Watermark
    {
        public static string Run(string outputDir)
        {
            string outputPdf = Path.Combine(outputDir, "watermark.pdf");

            #region snippet
            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.RenderingEngine = RenderingEngine.Chromium;

            PdfDocument doc = converter.ConvertHtmlString(
                "<h1>Draft report</h1><p>Every page of this document carries a watermark.</p>");

            // Draw a large, light "CONFIDENTIAL" behind the content of every page,
            // using a background template that repeats on all pages.
            PdfFont font = doc.AddFont(PdfStandardFont.Helvetica);
            font.Size = 50;

            RectangleF pageRect = doc.Pages[0].ClientRectangle;
            PdfTemplate template = doc.AddTemplate(pageRect);
            template.Background = true;

            PdfTextElement watermark = new PdfTextElement(
                0, pageRect.Height / 2 - 30, pageRect.Width, "CONFIDENTIAL", font);
            watermark.ForeColor = Color.FromArgb(210, 210, 210);
            watermark.HorizontalAlign = PdfTextHorizontalAlign.Center;
            template.Add(watermark);

            doc.Save(outputPdf);
            doc.Close();
            #endregion

            return outputPdf;
        }
    }
}
