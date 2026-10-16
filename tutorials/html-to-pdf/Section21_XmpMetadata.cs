using System.Collections.Generic;
using System.IO;
using SelectPdf;

namespace SelectPdfExamples.HtmlToPdfTutorial
{
    /// <summary>Writes custom XMP metadata into a PDF/A document without breaking conformance (v26.4+).</summary>
    public static class Section21_XmpMetadata
    {
        public static string Run(string outputDir)
        {
            string outputPdf = Path.Combine(outputDir, "xmp-metadata.pdf");

            #region snippet
            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.RenderingEngine = RenderingEngine.Chromium;
            converter.Options.PdfStandard = PdfStandard.PdfA3B;

            PdfDocument doc = converter.ConvertHtmlString(
                "<h1>Contract 2026-117</h1>" +
                "<p>Records-management fields are stored in the document's XMP metadata.</p>");
            doc.DocumentInformation.Title = "Contract 2026-117";

            // PDF/A only allows metadata from schemas it predefines, unless the schema is
            // described inside the XMP packet. AddXmpExtensionSchema writes both the schema
            // declaration and the values, so the file stays PDF/A-conformant.
            doc.AddXmpExtensionSchema(
                "acme",                                   // namespace prefix
                "http://ns.acme.example/records/1.0/",     // namespace URI
                "Acme records management",                // human-readable schema name
                new Dictionary<string, string>
                {
                    { "DocumentId", "CTR-2026-117" },
                    { "Department", "Legal" },
                    { "RetainUntil", "2036-12-31" },
                });

            doc.Save(outputPdf);
            doc.Close();
            #endregion

            return outputPdf;
        }
    }
}
