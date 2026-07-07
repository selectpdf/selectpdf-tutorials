using System.IO;
using SelectPdf;

namespace SelectPdfExamples.HtmlToPdfTutorial
{
    /// <summary>Encrypts the PDF with an owner password and restricts permissions.</summary>
    public static class Section17_PasswordProtection
    {
        public static string Run(string outputDir)
        {
            string outputPdf = Path.Combine(outputDir, "protected.pdf");

            #region snippet
            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.RenderingEngine = RenderingEngine.Chromium;

            // Encrypt with an owner password and restrict what readers may do.
            // (Set UserPassword as well to also require a password just to open the file.)
            converter.Options.SecurityOptions.OwnerPassword = "owner-secret";
            converter.Options.SecurityOptions.CanPrint = true;
            converter.Options.SecurityOptions.CanCopyContent = false;
            converter.Options.SecurityOptions.CanEditContent = false;

            PdfDocument doc = converter.ConvertHtmlString(
                "<h1>Confidential</h1><p>Copying and editing are disabled on this document.</p>");

            doc.Save(outputPdf);
            doc.Close();
            #endregion

            return outputPdf;
        }
    }
}
