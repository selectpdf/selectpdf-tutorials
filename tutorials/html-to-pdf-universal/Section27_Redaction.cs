using System.IO;
using System.Text.RegularExpressions;
using SelectPdf.Universal;

namespace SelectPdfExamples.HtmlToPdfUniversalTutorial
{
    /// <summary>Removes a name and every e-mail address from a converted document.</summary>
    public static class Section27_Redaction
    {
        public static string Run(string outputDir)
        {
            string outputPdf = Path.Combine(outputDir, "redacted.pdf");

            #region snippet
            HtmlToPdf converter = new HtmlToPdf();
            using PdfDocument doc = converter.ConvertHtmlString(@"
<div style='font-family:Arial'>
  <h1>Support ticket #4471</h1>
  <p>Customer: Jane Doe (jane.doe@example.com)</p>
  <p>Jane Doe reported that invoices sent to billing@example.org were not delivered.</p>
  <p>Resolution: the mail filter was updated on 3 October.</p>
</div>");

            // Redaction deletes the content from the file - it is not just covered -
            // and paints each area over. The text around it keeps its position.
            using PdfRedactionManager redactor = new PdfRedactionManager();
            redactor.Load(doc);
            redactor.OverlayText = "[REDACTED]";

            // A known value, whole words only, any letter case...
            redactor.AddRedaction("Jane Doe", false, true);
            // ...and anything that follows a pattern, here e-mail addresses.
            redactor.AddRedaction(new Regex(@"[\w.+-]+@[\w-]+\.[\w.]+"));

            redactor.Save(outputPdf);
            #endregion

            return outputPdf;
        }
    }
}
