using System.IO;
using SelectPdf.Universal;

namespace SelectPdfExamples.HtmlToPdfUniversalTutorial
{
    /// <summary>Converts only the part of a page matched by a CSS selector .</summary>
    public static class Section16_PartialPage
    {
        public static string Run(string outputDir)
        {
            string outputPdf = Path.Combine(outputDir, "partial-page.pdf");

            #region snippet
            HtmlToPdf converter = new HtmlToPdf();

            // Convert only the element matched by this CSS selector - the navigation,
            // sidebar and footer around it are left out. A selector that matches
            // nothing fails the conversion instead of converting the whole page.
            converter.Options.VisibleWebElementSelector = "main .summary";

            PdfDocument doc = converter.ConvertHtmlString(
                "<nav style='background:#1f2933;color:#fff;padding:12px'>Home | Reports | Settings</nav>" +
                "<aside style='float:right;width:200px;background:#f3f4f6;padding:12px'>Sidebar links</aside>" +
                "<main>" +
                "  <p>Intro text that should not be in the PDF.</p>" +
                "  <section class='summary' style='font-family:Arial;border:2px solid #b45309;padding:16px'>" +
                "    <h1 style='color:#b45309'>Q3 summary</h1>" +
                "    <p>Revenue grew 12% quarter on quarter.</p>" +
                "  </section>" +
                "</main>" +
                "<footer>Copyright footer</footer>");

            doc.Save(outputPdf);
            doc.Close();
            #endregion

            return outputPdf;
        }
    }
}
