# HTML to PDF in C#

This is a complete, hands-on guide to converting HTML to PDF in C# with the [SelectPdf library for .NET](https://selectpdf.com/pdf-library-for-net/). It works up from a three-line conversion to a production-ready workflow: every one of the 22 code steps is a full, compiling example taken straight from this project and rendered with the Chromium engine, so the code you copy is code that actually runs. Along the way you will build a real invoice from a data model, render complex CSS and paginating tables, control layout, handle dynamic and authenticated pages, secure and archive your output, integrate with ASP.NET Core, and finish with notes on running in production and troubleshooting.

## Getting started

Add the SelectPdf packages, then build and run:

```bash
dotnet add package Select.Pdf.NetCore
dotnet add package Select.Pdf.NetCore.Chromium.Windows
dotnet run
```

Running the project writes each result PDF to `output/` and the preview images shown below into `images/`.

## Contents


**Part 1 - Getting started**

1. [Convert an HTML string to PDF](#1-convert-an-html-string-to-pdf)
2. [Convert a URL to PDF](#2-convert-a-url-to-pdf)
3. [Convert a local HTML file to PDF](#3-convert-a-local-html-file-to-pdf)

**Part 2 - Build a real document**

4. [Build an invoice from a template](#4-build-an-invoice-from-a-template)
5. [Render complex HTML that paginates](#5-render-complex-html-that-paginates)

**Part 3 - Control the page layout**

6. [Set page size, orientation and margins](#6-set-page-size-orientation-and-margins)
7. [Add headers and footers with page numbers](#7-add-headers-and-footers-with-page-numbers)
8. [Use HTML for headers and footers](#8-use-html-for-headers-and-footers)
9. [Generate bookmarks from HTML headings](#9-generate-bookmarks-from-html-headings)
10. [Produce a single-page PDF](#10-produce-a-singlepage-pdf)
11. [Add a watermark to every page](#11-add-a-watermark-to-every-page)

**Part 4 - Dynamic and remote content**

12. [Wait for JavaScript-rendered content](#12-wait-for-javascriptrendered-content)
13. [Inject custom CSS and run a script](#13-inject-custom-css-and-run-a-script)
14. [Control the browser viewport width](#14-control-the-browser-viewport-width)
15. [Send HTTP headers and cookies](#15-send-http-headers-and-cookies)
16. [Convert only part of a page](#16-convert-only-part-of-a-page)

**Part 5 - Secure, archive, integrate**

17. [Set the document properties](#17-set-the-document-properties)
18. [Password-protect and restrict permissions](#18-passwordprotect-and-restrict-permissions)
19. [Create an archivable PDF/A-3 document](#19-create-an-archivable-pdfa3-document)
20. [Create a ZUGFeRD / Factur-X e-invoice](#20-create-a-zugferd--facturx-einvoice)
21. [Add custom XMP metadata](#21-add-custom-xmp-metadata)
22. [Generate a PDF in ASP.NET Core](#22-generate-a-pdf-in-aspnet-core)

**Part 6 - Run it in production**

23. [Running in production](#23-running-in-production)
24. [Troubleshooting](#24-troubleshooting)

**FAQ**

25. [Frequently asked questions](#25-frequently-asked-questions)

## Part 1 - Getting started

### 1. Convert an HTML string to PDF

The simplest conversion: render a raw HTML string into a PDF document in a few lines of code.

```csharp
// Create the HTML to PDF converter and select the Chromium (CEF) engine.
HtmlToPdf converter = new HtmlToPdf();
converter.Options.RenderingEngine = RenderingEngine.Chromium;

// Convert a raw HTML string into a PDF document.
PdfDocument doc = converter.ConvertHtmlString(
    "<h1>Hello, SelectPdf!</h1>" +
    "<p>This PDF was generated from an HTML string in C#.</p>");

// Save the document to disk, then release the native resources.
doc.Save(outputPdf);
doc.Close();
```

**Output:**

<a href="images/html-to-pdf-section01.png"><img src="images/html-to-pdf-section01.png" alt="Convert an HTML string to PDF" width="360"></a>

> Full source: [Section01_HtmlStringToPdf.cs](Section01_HtmlStringToPdf.cs)

### 2. Convert a URL to PDF

Point the converter at any public web address and capture the rendered page as a PDF.

```csharp
HtmlToPdf converter = new HtmlToPdf();
converter.Options.RenderingEngine = RenderingEngine.Chromium;

// Convert a live web page to a PDF document.
PdfDocument doc = converter.ConvertUrl("https://www.selectpdf.com");

doc.Save(outputPdf);
doc.Close();
```

**Output:**

<a href="images/html-to-pdf-section02.png"><img src="images/html-to-pdf-section02.png" alt="Convert a URL to PDF" width="360"></a>

> Full source: [Section02_UrlToPdf.cs](Section02_UrlToPdf.cs)

### 3. Convert a local HTML file to PDF

Render an HTML file from disk - together with its local CSS and images - into a PDF.

```csharp
HtmlToPdf converter = new HtmlToPdf();
converter.Options.RenderingEngine = RenderingEngine.Chromium;

// ConvertUrl also accepts a local file path; relative CSS and images
// referenced by the file are resolved automatically.
PdfDocument doc = converter.ConvertUrl(htmlFile);

doc.Save(outputPdf);
doc.Close();
```

**Output:**

<a href="images/html-to-pdf-section03.png"><img src="images/html-to-pdf-section03.png" alt="Convert a local HTML file to PDF" width="360"></a>

> Full source: [Section03_HtmlFileToPdf.cs](Section03_HtmlFileToPdf.cs)

## Part 2 - Build a real document

### 4. Build an invoice from a template

Turn a C# data model into a styled, real-world invoice: branded header, a line-item table, VAT totals and a page-numbered footer.

```csharp
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
```

**Output:**

<a href="images/html-to-pdf-section04.png"><img src="images/html-to-pdf-section04.png" alt="Build an invoice from a template" width="360"></a>

> Full source: [Section04_InvoiceTemplate.cs](Section04_InvoiceTemplate.cs)

### 5. Render complex HTML that paginates

Prove the engine handles modern CSS - grid layout and a live web font - plus a long table whose header repeats on every page.

```csharp
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
```

**Output:**

<a href="images/html-to-pdf-section05.png"><img src="images/html-to-pdf-section05.png" alt="Render complex HTML that paginates" width="360"></a>

> Full source: [Section05_ComplexHtml.cs](Section05_ComplexHtml.cs)

## Part 3 - Control the page layout

### 6. Set page size, orientation and margins

Control the physical page: choose A4, switch to landscape, and set the page margins.

```csharp
HtmlToPdf converter = new HtmlToPdf();
converter.Options.RenderingEngine = RenderingEngine.Chromium;

// Page setup (margins are expressed in points; 72 points = 1 inch).
converter.Options.PdfPageSize = PdfPageSize.A4;
converter.Options.PdfPageOrientation = PdfPageOrientation.Landscape;
converter.Options.MarginLeft = 36;
converter.Options.MarginRight = 36;
converter.Options.MarginTop = 36;
converter.Options.MarginBottom = 36;

PdfDocument doc = converter.ConvertHtmlString(
    "<h1>A4 Landscape</h1>" +
    "<p>This page uses a custom size, orientation and half-inch margins.</p>");

doc.Save(outputPdf);
doc.Close();
```

**Output:**

<a href="images/html-to-pdf-section06.png"><img src="images/html-to-pdf-section06.png" alt="Set page size, orientation and margins" width="360"></a>

> Full source: [Section06_PageSettings.cs](Section06_PageSettings.cs)

### 7. Add headers and footers with page numbers

Draw a repeating header on every page and a footer that shows "Page X of Y".

```csharp
HtmlToPdf converter = new HtmlToPdf();
converter.Options.RenderingEngine = RenderingEngine.Chromium;

// Enable and populate the header.
converter.Options.DisplayHeader = true;
converter.Header.Height = 40;
PdfTextSection headerText = new PdfTextSection(
    0, 12, "SelectPdf Tutorial", new Font("Arial", 10, FontStyle.Bold));
headerText.HorizontalAlign = PdfTextHorizontalAlign.Left;
converter.Header.Add(headerText);

// Enable and populate the footer with page numbering tokens.
converter.Options.DisplayFooter = true;
converter.Footer.Height = 40;
PdfTextSection footerText = new PdfTextSection(
    0, 12, "Page {page_number} of {total_pages}", new Font("Arial", 8));
footerText.HorizontalAlign = PdfTextHorizontalAlign.Right;
converter.Footer.Add(footerText);

PdfDocument doc = converter.ConvertUrl("https://www.selectpdf.com");

doc.Save(outputPdf);
doc.Close();
```

**Output:**

<a href="images/html-to-pdf-section07.png"><img src="images/html-to-pdf-section07.png" alt="Add headers and footers with page numbers" width="360"></a>

> Full source: [Section07_HeadersAndFooters.cs](Section07_HeadersAndFooters.cs)

### 8. Use HTML for headers and footers

Build a fully styled header or footer from an HTML fragment instead of plain text.

```csharp
HtmlToPdf converter = new HtmlToPdf();
converter.Options.RenderingEngine = RenderingEngine.Chromium;

// Render the header from an HTML fragment so it can be fully styled with CSS.
converter.Options.DisplayHeader = true;
converter.Header.Height = 50;

PdfHtmlSection header = new PdfHtmlSection(
    "<div style='font-family:Arial;color:#b45309;font-size:18px;" +
    "border-bottom:2px solid #b45309;padding:6px 0;'>Acme Corporation</div>",
    string.Empty);
header.RenderingEngine = RenderingEngine.Chromium;
header.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
converter.Header.Add(header);

PdfDocument doc = converter.ConvertHtmlString(
    "<h1>Report body</h1><p>The header above is rendered from HTML.</p>");

doc.Save(outputPdf);
doc.Close();
```

**Output:**

<a href="images/html-to-pdf-section08.png"><img src="images/html-to-pdf-section08.png" alt="Use HTML for headers and footers" width="360"></a>

> Full source: [Section08_HtmlHeaderFooter.cs](Section08_HtmlHeaderFooter.cs)

### 9. Generate bookmarks from HTML headings

Turn the document's headings into a clickable PDF bookmarks (outline) tree.

```csharp
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
```

**Output:**

<a href="images/html-to-pdf-section09.png"><img src="images/html-to-pdf-section09.png" alt="Generate bookmarks from HTML headings" width="360"></a>

> Full source: [Section09_Bookmarks.cs](Section09_Bookmarks.cs)

### 10. Produce a single-page PDF

Fit the whole document onto one continuous page with no page breaks - handy for receipts and tickets.

```csharp
HtmlToPdf converter = new HtmlToPdf();
converter.Options.RenderingEngine = RenderingEngine.Chromium;

// Produce one continuous page sized to the content, instead of paginating.
converter.Options.GenerateSinglePagePdf = true;

PdfDocument doc = converter.ConvertHtmlString(
    "<h1>Single-page output</h1>" +
    "<p>Section one.</p><p>Section two.</p><p>Section three.</p>" +
    "<p>All content stays on a single, taller page with no page breaks.</p>");

doc.Save(outputPdf);
doc.Close();
```

**Output:**

<a href="images/html-to-pdf-section10.png"><img src="images/html-to-pdf-section10.png" alt="Produce a single-page PDF" width="360"></a>

> Full source: [Section10_SinglePagePdf.cs](Section10_SinglePagePdf.cs)

### 11. Add a watermark to every page

Stamp a semi-transparent text watermark - such as CONFIDENTIAL or DRAFT - behind the content of every page.

```csharp
HtmlToPdf converter = new HtmlToPdf();
converter.Options.RenderingEngine = RenderingEngine.Chromium;

PdfDocument doc = converter.ConvertHtmlString(
    "<h1>Draft report</h1><p>Every page of this document carries a watermark.</p>");

// Draw a large, light "CONFIDENTIAL" behind the content of every page,
// using a background template that repeats on all pages.
PdfFont font = doc.AddFont(PdfStandardFont.Helvetica);
font.Size = 50;

RectangleF pageRect = doc.Pages[0].ClientRectangle;
PdfTemplate template = doc.AddTemplate(pageRect);
template.Background = true;

PdfTextElement watermark = new PdfTextElement(
    0, pageRect.Height / 2 - 30, pageRect.Width, "CONFIDENTIAL", font);
watermark.ForeColor = Color.FromArgb(210, 210, 210);
watermark.HorizontalAlign = PdfTextHorizontalAlign.Center;
template.Add(watermark);

doc.Save(outputPdf);
doc.Close();
```

**Output:**

<a href="images/html-to-pdf-section11.png"><img src="images/html-to-pdf-section11.png" alt="Add a watermark to every page" width="360"></a>

> Full source: [Section11_Watermark.cs](Section11_Watermark.cs)

## Part 4 - Dynamic and remote content

### 12. Wait for JavaScript-rendered content

Give client-side scripts time to run so dynamic content is captured instead of a half-loaded page.

```csharp
HtmlToPdf converter = new HtmlToPdf();
converter.Options.RenderingEngine = RenderingEngine.Chromium;

// Give client-side scripts time to run before the page is captured (seconds).
converter.Options.MinPageLoadTime = 2;

PdfDocument doc = converter.ConvertHtmlString(
    "<h1>Dynamic content</h1>" +
    "<p id='status'>Loading...</p>" +
    "<script>setTimeout(function(){" +
    "document.getElementById('status').innerHTML = 'Loaded by JavaScript!';" +
    "}, 800);</script>");

doc.Save(outputPdf);
doc.Close();
```

**Output:**

<a href="images/html-to-pdf-section12.png"><img src="images/html-to-pdf-section12.png" alt="Wait for JavaScript-rendered content" width="360"></a>

> Full source: [Section12_WaitForJavaScript.cs](Section12_WaitForJavaScript.cs)

### 13. Inject custom CSS and run a script

Restyle the page with your own CSS and execute JavaScript after load, without touching the source HTML.

```csharp
HtmlToPdf converter = new HtmlToPdf();
converter.Options.RenderingEngine = RenderingEngine.Chromium;

// Inject additional CSS into the page before rendering.
converter.Options.CustomCSS =
    "body { font-family: Arial; background: #fef3c7; }" +
    "h1 { color: #b45309; }";

// Run JavaScript after the page has loaded (Chromium/Blink only).
converter.Options.PostLoadingScript =
    "document.body.insertAdjacentHTML('beforeend', '<p>Added by script.</p>');";

PdfDocument doc = converter.ConvertHtmlString(
    "<h1>Custom styled page</h1>" +
    "<p>The amber background and heading color come from injected CSS.</p>");

doc.Save(outputPdf);
doc.Close();
```

**Output:**

<a href="images/html-to-pdf-section13.png"><img src="images/html-to-pdf-section13.png" alt="Inject custom CSS and run a script" width="360"></a>

> Full source: [Section13_CustomCssAndScript.cs](Section13_CustomCssAndScript.cs)

### 14. Control the browser viewport width

Set the virtual browser width the HTML is laid out at, then shrink oversized layouts to fit the PDF page.

```csharp
HtmlToPdf converter = new HtmlToPdf();
converter.Options.RenderingEngine = RenderingEngine.Chromium;

// Lay the HTML out at a 1200px browser width...
converter.Options.WebPageWidth = 1200;

// ...then shrink it so the wide layout fits the PDF page width.
converter.Options.AutoFitWidth = HtmlToPdfPageFitMode.ShrinkOnly;

PdfDocument doc = converter.ConvertHtmlString(
    "<div style='width:1200px;background:#e0f2fe;padding:24px'>" +
    "<h1>Wide layout</h1>" +
    "<p>Rendered at a 1200px viewport, then shrunk to fit the page.</p></div>");

doc.Save(outputPdf);
doc.Close();
```

**Output:**

<a href="images/html-to-pdf-section14.png"><img src="images/html-to-pdf-section14.png" alt="Control the browser viewport width" width="360"></a>

To capture only the top of a long page, also set `WebPageHeight` and `WebPageFixedSize = true`. The content below that height is left out of the PDF. Since v26.4 the Chromium and Blink engines honor this too; earlier versions supported it only on WebKit.

> Full source: [Section14_ViewportWidth.cs](Section14_ViewportWidth.cs)

### 15. Send HTTP headers and cookies

Attach custom request headers and cookies to reach authenticated or localized pages.

```csharp
HtmlToPdf converter = new HtmlToPdf();
converter.Options.RenderingEngine = RenderingEngine.Chromium;

// Attach custom request headers (e.g. language, bearer token) and cookies.
converter.Options.HttpHeaders.Add("Accept-Language", "en-US");
converter.Options.HttpCookies.Add("session", "demo-cookie-value");

PdfDocument doc = converter.ConvertUrl("https://www.selectpdf.com");

doc.Save(outputPdf);
doc.Close();
```

**Output:**

<a href="images/html-to-pdf-section15.png"><img src="images/html-to-pdf-section15.png" alt="Send HTTP headers and cookies" width="360"></a>

> Full source: [Section15_HttpHeadersAndCookies.cs](Section15_HttpHeadersAndCookies.cs)

### 16. Convert only part of a page

Convert just one element of a page - a report card, a chart, a table - picked with a CSS selector, leaving the navigation, sidebar and footer out.

```csharp
HtmlToPdf converter = new HtmlToPdf();
converter.Options.RenderingEngine = RenderingEngine.Chromium;

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
```

Any selector the browser understands works - `#content`, `.infobox`, `table.wikitable tbody`. `VisibleWebElementSelector` is honored by the Chromium and Blink engines and produces a real, selectable PDF (since v26.4). The WebKit engines take an element id instead, through `VisibleWebElementId`, which every engine honors.

**Output:**

<a href="images/html-to-pdf-section16.png"><img src="images/html-to-pdf-section16.png" alt="Convert only part of a page" width="360"></a>

> Full source: [Section16_PartialPage.cs](Section16_PartialPage.cs)

## Part 5 - Secure, archive, integrate

### 17. Set the document properties

Fill in the PDF title, author, subject and keywords shown in the viewer and used by search indexes.

```csharp
HtmlToPdf converter = new HtmlToPdf();
converter.Options.RenderingEngine = RenderingEngine.Chromium;

PdfDocument doc = converter.ConvertHtmlString(
    "<h1>Quarterly Report</h1><p>Prepared for Acme Corporation.</p>");

// These appear in the viewer's "Document Properties" dialog and in search indexes.
doc.DocumentInformation.Title = "Quarterly Report";
doc.DocumentInformation.Author = "SelectPdf";
doc.DocumentInformation.Subject = "Q3 financial results";
doc.DocumentInformation.Keywords = "report, quarterly, finance";
doc.DocumentInformation.CreationDate = DateTime.Now;

doc.Save(outputPdf);
doc.Close();
```

**Output:**

<a href="images/html-to-pdf-section17.png"><img src="images/html-to-pdf-section17.png" alt="Set the document properties" width="360"></a>

> Full source: [Section17_DocumentProperties.cs](Section17_DocumentProperties.cs)

### 18. Password-protect and restrict permissions

Encrypt the PDF with an owner password and disable copying and editing.

```csharp
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
```

**Output:**

<a href="images/html-to-pdf-section18.png"><img src="images/html-to-pdf-section18.png" alt="Password-protect and restrict permissions" width="360"></a>

> Full source: [Section18_PasswordProtection.cs](Section18_PasswordProtection.cs)

### 19. Create an archivable PDF/A-3 document

Emit a PDF/A-3B file suitable for long-term archiving and compliance.

```csharp
HtmlToPdf converter = new HtmlToPdf();

// PDF/A (and tagged/accessible) output requires the Chromium engine.
converter.Options.RenderingEngine = RenderingEngine.Chromium;

// Emit a PDF/A-3B document suitable for long-term archiving.
converter.Options.PdfStandard = PdfStandard.PdfA3B;

PdfDocument doc = converter.ConvertHtmlString(
    "<h1>Archivable document</h1>" +
    "<p>Saved as PDF/A-3B for long-term archiving and compliance.</p>");

doc.Save(outputPdf);
doc.Close();
```

**Output:**

<a href="images/html-to-pdf-section19.png"><img src="images/html-to-pdf-section19.png" alt="Create an archivable PDF/A-3 document" width="360"></a>

PdfA3B is the basic conformance level. Use `PdfStandard.PdfA3U` when all text must map to Unicode, or `PdfStandard.PdfA3A` for the accessible level, which adds a tagged logical structure. As of v26.5, tagged PDF/A-3a and PDF/UA-1 conversions of real-world web pages pass veraPDF validation.

> Full source: [Section19_PdfA3.cs](Section19_PdfA3.cs)

### 20. Create a ZUGFeRD / Factur-X e-invoice

Turn the invoice from section 4 into a hybrid electronic invoice: a PDF/A-3 file that people read as a normal invoice and accounting software reads from the embedded XML.

```csharp
HtmlToPdf converter = new HtmlToPdf();
converter.Options.RenderingEngine = RenderingEngine.Chromium;

// A hybrid e-invoice is carried in a PDF/A-3 document (PdfA3B, PdfA3U or PdfA3A).
converter.Options.PdfStandard = PdfStandard.PdfA3B;

// The visible invoice - the same HTML template as section 4.
PdfDocument doc = converter.ConvertHtmlString(html);
doc.DocumentInformation.Title = "Invoice INV-1042";

// Embed the machine-readable invoice XML. The library names the file
// (factur-x.xml), links it as a PDF/A-3 associated file and writes the
// Factur-X metadata that invoice software looks for.
// MINIMUM carries less than the visible page, so the relationship is Data;
// use Alternative when the XML and the page carry exactly the same content.
doc.AddZugferdInvoice(invoiceXml, ZugferdProfile.Minimum, PdfAttachmentRelationship.Data);

doc.Save(outputPdf);
doc.Close();
```

The invoice XML is [assets/factur-x.xml](assets/factur-x.xml), a Cross Industry Invoice in the MINIMUM profile. Choose the profile that matches your XML: `Minimum`, `BasicWL`, `Basic`, `En16931`, `Extended` or `XRechnung`. For XRechnung the embedded file is named `xrechnung.xml`. In Germany, the BASIC, EN 16931, EXTENDED and XRECHNUNG profiles must use `PdfAttachmentRelationship.Alternative`. Factur-X 1.0 metadata is written by default. Pass `ZugferdSchema.Zugferd20` as a fourth argument only for recipients that still require the deprecated ZUGFeRD 2.0 schema.

**Output:**

<a href="images/html-to-pdf-section20.png"><img src="images/html-to-pdf-section20.png" alt="Create a ZUGFeRD / Factur-X e-invoice" width="360"></a>

> Full source: [Section20_ZugferdInvoice.cs](Section20_ZugferdInvoice.cs)

### 21. Add custom XMP metadata

Store your own fields - a document id, a department, a retention date - in the file's XMP metadata, without breaking PDF/A conformance.

```csharp
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
```

Document-management and records systems can read these values straight from the file. For e-invoices you don't need this method, because `AddZugferdInvoice` writes the Factur-X schema itself.

**Output:**

<a href="images/html-to-pdf-section21.png"><img src="images/html-to-pdf-section21.png" alt="Add custom XMP metadata" width="360"></a>

> Full source: [Section21_XmpMetadata.cs](Section21_XmpMetadata.cs)

### 22. Generate a PDF in ASP.NET Core

Build the PDF in a controller action and stream it straight to the browser as a downloadable file.

```csharp
// In an ASP.NET Core app, generate the PDF inside a controller action and stream it back.
public sealed class InvoiceController : ControllerBase
{
    [HttpGet("/invoice/{id:int}")]
    public IActionResult Get(int id)
    {
        HtmlToPdf converter = new HtmlToPdf();
        converter.Options.RenderingEngine = RenderingEngine.Chromium;

        PdfDocument doc = converter.ConvertHtmlString(
            $"<h1>Invoice #{id}</h1><p>Thank you for your order.</p>");

        // Save straight to a byte array - no temp file needed.
        byte[] pdf = doc.Save();
        doc.Close();

        // Return it as a downloadable file (omit the file name to show it inline).
        return File(pdf, "application/pdf", $"invoice-{id}.pdf");
    }
}
```

**Output:**

<a href="images/html-to-pdf-section22.png"><img src="images/html-to-pdf-section22.png" alt="Generate a PDF in ASP.NET Core" width="360"></a>

> Full source: [Section22_AspNetCore.cs](Section22_AspNetCore.cs)

## Part 6 - Run it in production

### 23. Running in production

This library (the classic SelectPdf for .NET) runs entirely in-process and is Windows-only (x86 and x64), and the host needs full trust. For Linux, macOS or Docker, use [SelectPdf.Universal](https://selectpdf.com/pdf-library-cross-platform/), the cross-platform edition - the [Universal tutorial](../html-to-pdf-universal/) covers the same steps. The same license covers both. Factor that into your hosting choice before you commit.

A converter instance is not thread-safe: create one HtmlToPdf per conversion rather than sharing one across threads, and cap parallelism with your own queue or semaphore - the library has no built-in concurrency limit.

- **Deployment** - ship Select.Pdf.dll together with Select.Html.dep and Select.Tools.dep; for the Chromium engine also deploy its runtime folder (the Chromium.Windows package copies it on build).
- **Process bitness** - match the package to the process or you get a BadImageFormatException. Under IIS, Enable 32-Bit Applications = True runs x86, False runs x64.
- **Memory** - on 64-bit servers use the x64 package - it can allocate far more memory for image-heavy pages.
- **Timeouts** - the default navigation timeout is 60 seconds; raise Options.MaxPageLoadTime for slow pages and use MinPageLoadTime to wait for late assets.
- **Licensing** - set GlobalProperties.LicenseKey once at startup, before any conversion; the trial watermarks every page.
- **Untrusted HTML** - disable scripts (Options.JavaScriptEnabled = false) and local file access (Options.DenyLocalFileAccess = true) when converting input you do not control.
- **Azure** - App Service needs the Chromium engine (v26.2+, Basic plan or above); Virtual Machines and Cloud Services have no restrictions.

### 24. Troubleshooting

A handful of problems come up again and again - here is the quick fix for each.

- **Blank page, or images and CSS missing from an HTML string** - pass a base URL: ConvertHtmlString(html, baseUrl) so relative URLs resolve.
- **Dynamic content missing** - the page needs more time or a signal - set Options.MinPageLoadTime, or use WindowStatus / PostLoadingScript.
- **BadImageFormatException** - the x86 or x64 package does not match the process bitness.
- **Could not find Select.Html.dep** - copy Select.Html.dep and Select.Tools.dep next to Select.Pdf.dll.
- **Navigation timeout** - the page is slower than 60s, the URL is not resolvable from the server, or a proxy is required - raise MaxPageLoadTime or set Options.ProxyOptions.
- **An image splits across two pages** - set Options.KeepImagesTogether = true, or style the img with page-break-inside: avoid.
- **OutOfMemoryException** - an image-heavy page - use the x64 package and optimize the images.
- **A WebGL map or 3D chart is blank** - WebGL is off by default on the Chromium and Blink engines (since v26.4) because that makes conversions faster. Set Options.WebGlEnabled = true for pages that need it.
- **The last page is cut in the wrong place** - with Chromium, the last page ends where the content ends. Set Options.ManagedContentTrimmingMode = ManagedContentTrimmingMode.Marker for the pre-26.4 cut, or Options.ManagedContentTrimming = false to turn this off.
- **Web fonts, fetch calls or file:// images behave differently in an HTML string (v26.5+)** - when you pass an http(s) base URL, the page now runs with that origin, as if the URL itself were converted. As a result, absolute file:// resources are refused. Use an empty or file:// base URL for local resources.

## FAQ

### 25. Frequently asked questions

- **Is SelectPdf free?** - The [Community Edition](https://selectpdf.com/community-edition/) (Select.HtmlToPdf) is free for HTML to PDF with a 5-page cap. This tutorial uses the commercial [Select.Pdf library](https://selectpdf.com/pdf-library-for-net/) - a paid license with the full feature set and no page limit.
- **Does it run on Linux, macOS or in Docker?** - Not this library: the classic SelectPdf is Windows-only (x86 and x64). [SelectPdf.Universal](https://selectpdf.com/pdf-library-cross-platform/) runs on Windows, Linux (x64 and ARM64), macOS on Apple Silicon and in Docker, keeps most of the same API, and is covered by the same license. If you're not on .NET, use the [SelectPdf online HTML-to-PDF REST API](https://selectpdf.com/html-to-pdf-api/).
- **Which rendering engine should I use?** - Chromium (used throughout this tutorial) for modern HTML5, CSS3 and web fonts. WebKit is the embedded default with nothing extra to deploy; Blink is an alternative Chromium-based engine. Chromium and Blink each need their companion NuGet package.
- **How do I remove the trial watermark?** - Set GlobalProperties.LicenseKey once at application startup, before any conversion. See [.NET library pricing](https://selectpdf.com/pricing/) for license options.
- **Can it convert a page that requires login?** - Yes - send authentication cookies or headers (see Send HTTP headers and cookies), or post form data with HttpPostParameters.
- **Is there a limit on the number of pages?** - Not in the commercial library. The 5-page cap applies only to the free [Community Edition](https://selectpdf.com/community-edition/).
- **Can I convert charts or content drawn by JavaScript?** - Yes, with the Chromium engine. Give scripts time to run with Options.MinPageLoadTime, or wait for a signal via WindowStatus (see Wait for JavaScript-rendered content).
- **Is the converter thread-safe?** - Use one HtmlToPdf instance per conversion and throttle concurrency yourself; see Running in production.

## Conclusion

That is the full arc of HTML to PDF with SelectPdf - from a three-line string conversion to a real invoice, complex paginating layouts, security, archival compliance, ASP.NET Core integration and production hardening. Because the same Chromium engine powers every example, what you preview in the browser is what lands in the PDF. Start with a [free trial](https://selectpdf.com/downloads/), browse the [full API reference](https://selectpdf.com/pdf-library/), or try the [live C# demo](https://selectpdf.com/demo/).

