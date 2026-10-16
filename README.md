# SelectPdf Examples

Runnable C# tutorials for the [SelectPdf .NET library](https://selectpdf.com/pdf-library-for-net/) — the self-hosted PDF toolkit for .NET.

Each folder under [`tutorials/`](tutorials/) is a **complete, buildable project** with a step-by-step README, so you can clone it, run it, and see the result immediately.

## Tutorials

| Tutorial | What you'll build |
|----------|-------------------|
| [HTML to PDF](tutorials/html-to-pdf/) | A hands-on guide in six parts: convert HTML strings, URLs and files; build a styled invoice from a data model; render complex CSS that paginates; control page layout, headers/footers, bookmarks and watermarks; handle dynamic and authenticated pages and convert just part of a page; secure and archive (PDF/A), create ZUGFeRD / Factur-X e-invoices, add custom XMP metadata and integrate with ASP.NET Core; plus production and troubleshooting notes. Uses the classic Windows library, v26.5. |
| [HTML to PDF on Windows, Linux, macOS and Docker](tutorials/html-to-pdf-universal/) | The same 22 steps ported to [SelectPdf.Universal](https://selectpdf.com/pdf-library-cross-platform/), the cross-platform edition, plus what only it does: async conversion, tables of contents, fillable forms from HTML, PDF/UA-2, redaction, signature validation and Word to PDF. Runs on Windows, Linux (x64 / ARM64), macOS on Apple Silicon and in Docker, with a Dockerfile and notes on the remote engine for Alpine and Kubernetes. |

_More topics (create, edit, merge/split, security, forms &amp; signatures) are on the way._

## Requirements

- **.NET SDK 8.0 or later** — the tutorial projects target `net8.0`.
- **HTML to PDF (classic)** — Windows (x86 / x64). Uses [`Select.Pdf.NetCore`](https://www.nuget.org/packages/Select.Pdf.NetCore) and [`Select.Pdf.NetCore.Chromium.Windows`](https://www.nuget.org/packages/Select.Pdf.NetCore.Chromium.Windows).
- **HTML to PDF (Universal)** — Windows, Linux (x64 / ARM64, glibc 2.30+), macOS 13+ on Apple Silicon, or Docker. Uses [`SelectPdf.Universal.Windows`](https://www.nuget.org/packages/SelectPdf.Universal.Windows), [`.Linux`](https://www.nuget.org/packages/SelectPdf.Universal.Linux), [`.Linux.Arm64`](https://www.nuget.org/packages/SelectPdf.Universal.Linux.Arm64) or [`.MacOS`](https://www.nuget.org/packages/SelectPdf.Universal.MacOS), picked automatically for the OS you build on.

The NuGet packages are restored automatically on build.

## Running a tutorial

```bash
cd tutorials/html-to-pdf
dotnet run
```

Use `cd tutorials/html-to-pdf-universal` for the cross-platform tutorial, which also builds and runs in Docker with the included `Dockerfile`.

Each tutorial writes its result PDFs to an `output/` folder and preview images to `images/`.

Without a license key the output carries a trial watermark. To remove it, set your key in the `SELECTPDF_LICENSE_KEY` environment variable before running.

## Documentation &amp; links

- **.NET library:** https://selectpdf.com/pdf-library-for-net/
- **Documentation:** https://selectpdf.com/pdf-library/
- **Download &amp; free trial:** https://selectpdf.com/downloads/
- **Live C# demo:** https://selectpdf.com/demo/
- **Pricing:** https://selectpdf.com/pricing/
- **Contact:** https://selectpdf.com/contact/
