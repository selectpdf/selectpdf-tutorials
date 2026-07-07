# SelectPdf Examples

Runnable C# tutorials for the [SelectPdf .NET library](https://selectpdf.com/pdf-library-for-net/) — the self-hosted PDF toolkit for .NET.

Each folder under [`tutorials/`](tutorials/) is a **complete, buildable project** with a step-by-step README, so you can clone it, run it, and see the result immediately.

## Tutorials

| Tutorial | What you'll build |
|----------|-------------------|
| [HTML to PDF](tutorials/html-to-pdf/) | A hands-on guide in six parts: convert HTML strings, URLs and files; build a styled invoice from a data model; render complex CSS that paginates; control page layout, headers/footers, bookmarks and watermarks; handle dynamic and authenticated pages; secure, archive (PDF/A) and integrate with ASP.NET Core; plus production and troubleshooting notes. |

_More topics (create, edit, merge/split, security, forms &amp; signatures) are on the way._

## Requirements

- **.NET SDK 8.0 or later** — the tutorial project targets `net8.0`.
- **Windows** — the Chromium rendering engine used in these examples is Windows-only (x86 / x64).
- The SelectPdf NuGet packages, restored automatically on build:
  - [`Select.Pdf.NetCore`](https://www.nuget.org/packages/Select.Pdf.NetCore)
  - [`Select.Pdf.NetCore.Chromium.Windows`](https://www.nuget.org/packages/Select.Pdf.NetCore.Chromium.Windows)

## Running a tutorial

```bash
cd tutorials/html-to-pdf
dotnet run
```

Each tutorial writes its result PDFs to an `output/` folder and preview images to `images/`.

Without a license key the output carries a trial watermark. To remove it, set your key in the `SELECTPDF_LICENSE_KEY` environment variable before running.

## Documentation &amp; links

- **.NET library:** https://selectpdf.com/pdf-library-for-net/
- **Documentation:** https://selectpdf.com/pdf-library/
- **Download &amp; free trial:** https://selectpdf.com/downloads/
- **Live C# demo:** https://selectpdf.com/demo/
- **Pricing:** https://selectpdf.com/pricing/
- **Contact:** https://selectpdf.com/contact/
