using System.IO;
using SelectPdf;

namespace SelectPdfExamples.HtmlToPdfTutorial
{
    /// <summary>Converts HTML to PDF and builds a bookmark tree from H1/H2 headings.</summary>
    public static class Section09_Bookmarks
    {
        public static string Run(string outputDir)
        {
            string outputPdf = Path.Combine(outputDir, "bookmarks.pdf");

            #region snippet
            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.RenderingEngine = RenderingEngine.Chromium;

            // Build PDF bookmarks from the elements matching these CSS selectors.
            converter.Options.PdfBookmarkOptions.CssSelectors = new string[] { "h1", "h2" };

            // Open the document with the bookmarks (outline) panel visible.
            converter.Options.ViewerPreferences.PageMode = PdfViewerPageMode.UseOutlines;

            PdfDocument doc = converter.ConvertHtmlString(
                "<h1>Chapter 1</h1><p>Introduction.</p>" +
                "<h2>Section 1.1</h2><p>Details.</p>" +
                "<h1>Chapter 2</h1><p>More content.</p>");

            doc.Save(outputPdf);
            doc.Close();
            #endregion

            return outputPdf;
        }
    }
}
