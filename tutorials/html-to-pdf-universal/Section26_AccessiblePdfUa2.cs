using System.IO;
using SelectPdf.Universal;

namespace SelectPdfExamples.HtmlToPdfUniversalTutorial
{
    /// <summary>Produces a tagged, accessible PDF/UA-2 document that is also archivable PDF/A-4.</summary>
    public static class Section26_AccessiblePdfUa2
    {
        public static string Run(string outputDir)
        {
            string outputPdf = Path.Combine(outputDir, "accessible-pdf-ua2.pdf");

            #region snippet
            HtmlToPdf converter = new HtmlToPdf();

            // PDF/UA-2 (accessibility) and PDF/A-4 (archiving) are both based on PDF 2.0,
            // so they combine. Selecting a UA standard turns on the tagged structure tree.
            converter.Options.AccessibilityStandard = PdfUAStandard.UA2;
            converter.Options.PdfStandard = PdfStandard.PdfA4;

            // Both standards require a document title and language.
            converter.Options.Title = "Annual accessibility statement";
            converter.Options.Language = "en-US";

            // Semantic HTML becomes the structure: headings, lists, tables and image alt text.
            PdfDocument doc = converter.ConvertHtmlString(@"
<html lang='en'><body style='font-family:Arial'>
  <h1>Annual accessibility statement</h1>
  <p>This document is tagged, so screen readers can follow its headings, lists and tables.</p>
  <h2>What we improved</h2>
  <ul><li>Keyboard navigation</li><li>Color contrast</li><li>Captions on all videos</li></ul>
  <h2>Audit results</h2>
  <table border='1' cellpadding='6' style='border-collapse:collapse'>
    <thead><tr><th scope='col'>Area</th><th scope='col'>Score</th></tr></thead>
    <tbody><tr><td>Web site</td><td>98</td></tr><tr><td>Documents</td><td>100</td></tr></tbody>
  </table>
</body></html>");

            doc.Save(outputPdf);
            doc.Close();
            #endregion

            return outputPdf;
        }
    }
}
