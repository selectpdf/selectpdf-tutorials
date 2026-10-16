using System.IO;
using System.Text;
using SelectPdf.Universal;

namespace SelectPdfExamples.HtmlToPdfUniversalTutorial
{
    /// <summary>Converts a Word-family document (here Markdown) to PDF, in-process, with no engine.</summary>
    public static class Section29_WordToPdf
    {
        public static string Run(string outputDir)
        {
            string outputPdf = Path.Combine(outputDir, "word-to-pdf.pdf");

            #region snippet
            // WordToPdf reads DOCX, DOC, RTF, WordML, TXT, HTML and Markdown.
            // Its Options, Header and Footer mirror the HTML converter's.
            WordToPdf converter = new WordToPdf();
            converter.Options.MarginTop = 40;
            converter.Options.MarginBottom = 40;

            // For a file, the format comes from the extension: converter.ConvertFile("report.docx").
            // Markdown, text and HTML have no signature, so in memory the format is named.
            string markdown =
                "# Release notes\n\n" +
                "Version **26.5** adds:\n\n" +
                "- tables of contents with page numbers\n" +
                "- fillable form fields from HTML forms\n" +
                "- signing with hardware tokens\n\n" +
                "| Platform | Status |\n|---|---|\n| Windows | Supported |\n| Linux | Supported |\n";

            PdfDocument doc = converter.ConvertBytes(Encoding.UTF8.GetBytes(markdown), WordFormat.Markdown);

            doc.Save(outputPdf);
            doc.Close();
            #endregion

            return outputPdf;
        }
    }
}
