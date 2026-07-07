using System;
using System.IO;
using SelectPdf;

namespace SelectPdfExamples.HtmlToPdfTutorial
{
    /// <summary>Sets the PDF document properties (title, author, subject, keywords).</summary>
    public static class Section16_DocumentProperties
    {
        public static string Run(string outputDir)
        {
            string outputPdf = Path.Combine(outputDir, "document-properties.pdf");

            #region snippet
            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.RenderingEngine = RenderingEngine.Chromium;

            PdfDocument doc = converter.ConvertHtmlString(
                "<h1>Quarterly Report</h1><p>Prepared for Acme Corporation.</p>");

            // These appear in the viewer's "Document Properties" dialog and in search indexes.
            doc.DocumentInformation.Title = "Quarterly Report";
            doc.DocumentInformation.Author = "SelectPdf";
            doc.DocumentInformation.Subject = "Q3 financial results";
            doc.DocumentInformation.Keywords = "report, quarterly, finance";
            doc.DocumentInformation.CreationDate = DateTime.Now;

            doc.Save(outputPdf);
            doc.Close();
            #endregion

            return outputPdf;
        }
    }
}
