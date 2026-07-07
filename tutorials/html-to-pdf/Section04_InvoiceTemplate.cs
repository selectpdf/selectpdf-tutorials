using System.Drawing;
using System.IO;
using System.Text;
using SelectPdf;

namespace SelectPdfExamples.HtmlToPdfTutorial
{
    /// <summary>Builds a real, styled invoice from a data model and an HTML template.</summary>
    public static class Section04_InvoiceTemplate
    {
        private sealed class LineItem
        {
            public string Description;
            public int Qty;
            public decimal UnitPrice;
            public decimal Total => Qty * UnitPrice;
        }

        public static string Run(string outputDir)
        {
            string outputPdf = Path.Combine(outputDir, "invoice.pdf");

            #region snippet
            // 1) Your data - normally from a database or an order.
            var items = new[]
            {
                new LineItem { Description = "SelectPdf Developer License", Qty = 1, UnitPrice = 499m },
                new LineItem { Description = "Priority Support (1 year)",    Qty = 1, UnitPrice = 199m },
                new LineItem { Description = "On-site training (per day)",    Qty = 2, UnitPrice = 900m },
            };

            // 2) Render the data into an HTML template (see BuildInvoiceHtml in the source).
            string html = BuildInvoiceHtml("INV-1042", items);

            // 3) Convert to PDF, adding a footer with page numbers.
            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.RenderingEngine = RenderingEngine.Chromium;
            converter.Options.MarginTop = 20;
            converter.Options.MarginBottom = 30;

            converter.Options.DisplayFooter = true;
            converter.Footer.Height = 28;
            PdfTextSection pageNo = new PdfTextSection(0, 8,
                "Invoice INV-1042 - page {page_number} of {total_pages}",
                new Font("Arial", 8));
            pageNo.HorizontalAlign = PdfTextHorizontalAlign.Right;
            converter.Footer.Add(pageNo);

            PdfDocument doc = converter.ConvertHtmlString(html);
            doc.Save(outputPdf);
            doc.Close();
            #endregion

            return outputPdf;
        }

        /// <summary>Renders the invoice data into a styled HTML document.</summary>
        private static string BuildInvoiceHtml(string number, LineItem[] items)
        {
            decimal subtotal = 0m;
            StringBuilder rows = new StringBuilder();
            foreach (LineItem it in items)
            {
                subtotal += it.Total;
                rows.Append($"<tr><td>{it.Description}</td><td class='num'>{it.Qty}</td>" +
                            $"<td class='num'>${it.UnitPrice:0.00}</td><td class='num'>${it.Total:0.00}</td></tr>");
            }
            decimal tax = subtotal * 0.19m;
            decimal total = subtotal + tax;

            return $@"<!DOCTYPE html><html><head><meta charset='utf-8'><style>
  body {{ font-family: Arial, Helvetica, sans-serif; color:#1f2933; margin:40px; }}
  .head {{ display:flex; justify-content:space-between; align-items:flex-start; }}
  .brand {{ font-size:26px; font-weight:bold; color:#b45309; }}
  .muted {{ color:#6b7280; }}
  table.items {{ width:100%; border-collapse:collapse; margin-top:28px; }}
  table.items th, table.items td {{ padding:10px 12px; border-bottom:1px solid #e5e7eb; text-align:left; }}
  table.items th {{ background:#fef3c7; }}
  .num {{ text-align:right; }}
  table.totals {{ width:280px; margin-left:auto; margin-top:16px; border-collapse:collapse; }}
  table.totals td {{ padding:4px 12px; }}
  .grand td {{ font-weight:bold; border-top:2px solid #b45309; }}
</style></head><body>
  <div class='head'>
    <div><div class='brand'>SelectPdf</div><div class='muted'>123 PDF Street, Bucharest</div></div>
    <div style='text-align:right'><h2 style='margin:0'>INVOICE</h2>
      <div class='muted'>{number}<br>Issued 7 July 2026</div></div>
  </div>
  <table class='items'><thead style='display:table-header-group'><tr>
    <th>Description</th><th class='num'>Qty</th><th class='num'>Unit price</th><th class='num'>Amount</th>
  </tr></thead><tbody>{rows}</tbody></table>
  <table class='totals'>
    <tr><td>Subtotal</td><td class='num'>${subtotal:0.00}</td></tr>
    <tr><td>VAT (19%)</td><td class='num'>${tax:0.00}</td></tr>
    <tr class='grand'><td>Total due</td><td class='num'>${total:0.00}</td></tr>
  </table>
</body></html>";
        }
    }
}
