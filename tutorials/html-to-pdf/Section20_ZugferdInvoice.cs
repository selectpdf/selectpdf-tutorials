using System.IO;
using SelectPdf;

namespace SelectPdfExamples.HtmlToPdfTutorial
{
    /// <summary>Turns the section 4 invoice into a ZUGFeRD / Factur-X hybrid e-invoice (v26.4+).</summary>
    public static class Section20_ZugferdInvoice
    {
        public static string Run(string outputDir)
        {
            string tutorialDir = Directory.GetParent(outputDir).FullName;
            string invoiceXml = Path.Combine(tutorialDir, "assets", "factur-x.xml");
            string outputPdf = Path.Combine(outputDir, "zugferd-invoice.pdf");
            string html = Section04_InvoiceTemplate.BuildSampleInvoiceHtml();

            #region snippet
            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.RenderingEngine = RenderingEngine.Chromium;

            // A hybrid e-invoice is carried in a PDF/A-3 document (PdfA3B, PdfA3U or PdfA3A).
            converter.Options.PdfStandard = PdfStandard.PdfA3B;

            // The visible invoice - the same HTML template as section 4.
            PdfDocument doc = converter.ConvertHtmlString(html);
            doc.DocumentInformation.Title = "Invoice INV-1042";

            // Embed the machine-readable invoice XML. The library names the file
            // (factur-x.xml), links it as a PDF/A-3 associated file and writes the
            // Factur-X metadata that invoice software looks for.
            // MINIMUM carries less than the visible page, so the relationship is Data;
            // use Alternative when the XML and the page carry exactly the same content.
            doc.AddZugferdInvoice(invoiceXml, ZugferdProfile.Minimum, PdfAttachmentRelationship.Data);

            doc.Save(outputPdf);
            doc.Close();
            #endregion

            return outputPdf;
        }
    }
}
