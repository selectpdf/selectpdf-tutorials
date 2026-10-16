using System.IO;
using System.Text;
using SelectPdf.Universal;

namespace SelectPdfExamples.HtmlToPdfUniversalTutorial
{
    /// <summary>Generates table-of-contents pages with leaders and page numbers from the headings.</summary>
    public static class Section24_TableOfContents
    {
        public static string Run(string outputDir)
        {
            string outputPdf = Path.Combine(outputDir, "table-of-contents.pdf");
            string html = BuildHandbookHtml();

            #region snippet
            HtmlToPdf converter = new HtmlToPdf();

            // Draw the table of contents on pages of its own, inserted before the content.
            // Each heading becomes a clickable entry with a dotted leader and its page number.
            converter.Options.TableOfContents.Mode = PdfTableOfContentsMode.GeneratePages;
            converter.Options.TableOfContents.HeadingSelectors = new string[] { "h1", "h2" };
            converter.Options.TableOfContents.Title = "Employee handbook";

            PdfDocument doc = converter.ConvertHtmlString(html);

            doc.Save(outputPdf);
            doc.Close();
            #endregion

            return outputPdf;
        }

        /// <summary>A multi-page document: four chapters, each starting on a new page.</summary>
        private static string BuildHandbookHtml()
        {
            string[] chapters = { "Welcome", "Working hours", "Time off", "Equipment and security" };
            StringBuilder sb = new StringBuilder("<html><body style='font-family:Arial'>");
            for (int i = 0; i < chapters.Length; i++)
            {
                sb.Append($"<h1 style='{(i > 0 ? "break-before:page;" : "")}color:#b45309'>{i + 1}. {chapters[i]}</h1>");
                for (int j = 1; j <= 3; j++)
                {
                    sb.Append($"<h2>{i + 1}.{j} Topic {j}</h2>");
                    sb.Append("<p>Policy text for this topic. It explains what applies, who it applies to and where to ask.</p>");
                }
            }
            return sb.Append("</body></html>").ToString();
        }
    }
}
