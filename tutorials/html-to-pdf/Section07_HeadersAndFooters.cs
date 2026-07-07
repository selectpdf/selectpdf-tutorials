using System.Drawing;
using System.IO;
using SelectPdf;

namespace SelectPdfExamples.HtmlToPdfTutorial
{
    /// <summary>Converts HTML to PDF with a page header and a page-numbering footer.</summary>
    public static class Section07_HeadersAndFooters
    {
        public static string Run(string outputDir)
        {
            string outputPdf = Path.Combine(outputDir, "headers-footers.pdf");

            #region snippet
            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.RenderingEngine = RenderingEngine.Chromium;

            // Enable and populate the header.
            converter.Options.DisplayHeader = true;
            converter.Header.Height = 40;
            PdfTextSection headerText = new PdfTextSection(
                0, 12, "SelectPdf Tutorial", new Font("Arial", 10, FontStyle.Bold));
            headerText.HorizontalAlign = PdfTextHorizontalAlign.Left;
            converter.Header.Add(headerText);

            // Enable and populate the footer with page numbering tokens.
            converter.Options.DisplayFooter = true;
            converter.Footer.Height = 40;
            PdfTextSection footerText = new PdfTextSection(
                0, 12, "Page {page_number} of {total_pages}", new Font("Arial", 8));
            footerText.HorizontalAlign = PdfTextHorizontalAlign.Right;
            converter.Footer.Add(footerText);

            PdfDocument doc = converter.ConvertUrl("https://www.selectpdf.com");

            doc.Save(outputPdf);
            doc.Close();
            #endregion

            return outputPdf;
        }
    }
}
