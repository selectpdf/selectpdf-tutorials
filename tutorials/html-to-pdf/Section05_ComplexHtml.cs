using System.IO;
using System.Text;
using SelectPdf;

namespace SelectPdfExamples.HtmlToPdfTutorial
{
    /// <summary>Renders modern CSS (grid, web font) and a long table that paginates with a repeating header.</summary>
    public static class Section05_ComplexHtml
    {
        public static string Run(string outputDir)
        {
            string outputPdf = Path.Combine(outputDir, "complex-html.pdf");

            #region snippet
            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.RenderingEngine = RenderingEngine.Chromium;

            // Give the web font a moment to download before the page is captured.
            converter.Options.MinPageLoadTime = 2;

            // The report uses CSS grid, a Google web font, and a 30-row table.
            // The table header repeats on every page (thead { display: table-header-group })
            // and rows are kept whole (page-break-inside: avoid) - all controlled from CSS.
            PdfDocument doc = converter.ConvertHtmlString(BuildReportHtml());

            doc.Save(outputPdf);
            doc.Close();
            #endregion

            return outputPdf;
        }

        private static string BuildReportHtml()
        {
            StringBuilder rows = new StringBuilder();
            for (int i = 1; i <= 30; i++)
            {
                rows.Append($"<tr style='page-break-inside:avoid'><td>#{i:000}</td>" +
                            $"<td>Line item {i}</td><td class='num'>{(i * 13) % 97}</td>" +
                            $"<td class='num'>${i * 42.5:0.00}</td></tr>");
            }

            return $@"<!DOCTYPE html><html><head><meta charset='utf-8'>
  <link href='https://fonts.googleapis.com/css2?family=Poppins:wght@400;700&display=swap' rel='stylesheet'>
  <style>
    body {{ font-family:'Poppins', Arial, sans-serif; color:#1f2933; margin:40px; }}
    h1 {{ color:#b45309; }}
    .cards {{ display:grid; grid-template-columns:repeat(3, 1fr); gap:14px; margin:20px 0; }}
    .card {{ background:#fef3c7; border-radius:8px; padding:16px; }}
    .card .n {{ font-size:24px; font-weight:700; color:#b45309; }}
    table {{ width:100%; border-collapse:collapse; margin-top:12px; }}
    th, td {{ padding:8px 10px; border-bottom:1px solid #e5e7eb; text-align:left; }}
    thead {{ display:table-header-group; }}
    th {{ background:#111827; color:#ffffff; }}
    .num {{ text-align:right; }}
  </style></head><body>
    <h1>Monthly report</h1>
    <div class='cards'>
      <div class='card'><div class='n'>30</div>line items</div>
      <div class='card'><div class='n'>$1,275</div>total value</div>
      <div class='card'><div class='n'>Poppins</div>web font, loaded live</div>
    </div>
    <table>
      <thead><tr><th>Ref</th><th>Name</th><th class='num'>Score</th><th class='num'>Amount</th></tr></thead>
      <tbody>{rows}</tbody>
    </table>
  </body></html>";
        }
    }
}
