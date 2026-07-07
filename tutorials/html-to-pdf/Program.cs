using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using SelectPdf;

namespace SelectPdfExamples.HtmlToPdfTutorial
{
    /// <summary>
    /// Runs every tutorial section, saving each result PDF into output/ and rendering
    /// a first-page preview PNG into images/. The generated images are what the README
    /// and the online documentation page display for each step.
    /// </summary>
    internal static class Program
    {
        private sealed class Section
        {
            public string Id;                       // sectionNN, used for the preview image file name
            public string Title;
            public Func<string, string> Run;        // (outputDir) -> produced pdf path
        }

        private static int Main(string[] args)
        {
            // Apply a license key if one is supplied via the environment, so no key is stored in source.
            // Without a key the output carries a trial watermark.
            string licenseKey = Environment.GetEnvironmentVariable("SELECTPDF_LICENSE_KEY");
            if (!string.IsNullOrEmpty(licenseKey))
            {
                GlobalProperties.LicenseKey = licenseKey;
            }

            // Output root defaults to this tutorial's folder; an explicit path can be passed as arg[0].
            string tutorialDir = (args != null && args.Length > 0 && !string.IsNullOrWhiteSpace(args[0]))
                ? args[0]
                : FindTutorialDir();
            string outputDir = Path.Combine(tutorialDir, "output");
            string imagesDir = Path.Combine(tutorialDir, "images");
            Directory.CreateDirectory(outputDir);
            Directory.CreateDirectory(imagesDir);

            // Preview images are prefixed with the tutorial slug (folder name) so they don't
            // collide with other tutorials' images in a shared media library.
            string slug = Path.GetFileName(tutorialDir.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));

            Section[] sections =
            {
                new Section { Id = "section01", Title = "Convert an HTML string to PDF",        Run = Section01_HtmlStringToPdf.Run },
                new Section { Id = "section02", Title = "Convert a URL to PDF",                  Run = Section02_UrlToPdf.Run },
                new Section { Id = "section03", Title = "Convert a local HTML file to PDF",      Run = Section03_HtmlFileToPdf.Run },
                new Section { Id = "section04", Title = "Build an invoice from a template",      Run = Section04_InvoiceTemplate.Run },
                new Section { Id = "section05", Title = "Render complex HTML that paginates",    Run = Section05_ComplexHtml.Run },
                new Section { Id = "section06", Title = "Set page size, orientation, margins",   Run = Section06_PageSettings.Run },
                new Section { Id = "section07", Title = "Add headers and footers",               Run = Section07_HeadersAndFooters.Run },
                new Section { Id = "section08", Title = "HTML headers and footers",              Run = Section08_HtmlHeaderFooter.Run },
                new Section { Id = "section09", Title = "Generate bookmarks from headings",      Run = Section09_Bookmarks.Run },
                new Section { Id = "section10", Title = "Single-page PDF",                       Run = Section10_SinglePagePdf.Run },
                new Section { Id = "section11", Title = "Add a watermark to every page",         Run = Section11_Watermark.Run },
                new Section { Id = "section12", Title = "Wait for JavaScript content",           Run = Section12_WaitForJavaScript.Run },
                new Section { Id = "section13", Title = "Inject custom CSS and a script",        Run = Section13_CustomCssAndScript.Run },
                new Section { Id = "section14", Title = "Control the viewport width",            Run = Section14_ViewportWidth.Run },
                new Section { Id = "section15", Title = "Send HTTP headers and cookies",         Run = Section15_HttpHeadersAndCookies.Run },
                new Section { Id = "section16", Title = "Set the document properties",           Run = Section16_DocumentProperties.Run },
                new Section { Id = "section17", Title = "Password-protect and set permissions",  Run = Section17_PasswordProtection.Run },
                new Section { Id = "section18", Title = "Create a PDF/A-3 archivable document",  Run = Section18_PdfA3.Run },
                new Section { Id = "section19", Title = "Generate a PDF in ASP.NET Core",        Run = Section19_AspNetCore.Run },
            };

            int failures = 0;
            foreach (Section s in sections)
            {
                Console.Write($"[{s.Id}] {s.Title} ... ");
                try
                {
                    string pdfPath = s.Run(outputDir);
                    string imagePath = Path.Combine(imagesDir, slug + "-" + s.Id + ".png");
                    RenderFirstPagePreview(pdfPath, imagePath);
                    Console.WriteLine("OK -> " + Path.GetFileName(pdfPath) + ", " + Path.GetFileName(imagePath));
                }
                catch (Exception ex)
                {
                    failures++;
                    Console.WriteLine("FAILED");
                    Console.Error.WriteLine("    " + ex.Message);
                }
            }

            Console.WriteLine();
            Console.WriteLine($"Done. {sections.Length - failures}/{sections.Length} sections succeeded.");
            return failures == 0 ? 0 : 1;
        }

        /// <summary>Renders the first page of a PDF to a PNG preview using SelectPdf's rasterizer.</summary>
        private static void RenderFirstPagePreview(string pdfPath, string imagePath)
        {
            PdfRasterizer rasterizer = new PdfRasterizer();
            rasterizer.Load(pdfPath);
            rasterizer.StartPageNumber = 1;
            rasterizer.EndPageNumber = 1;
            rasterizer.Resolution = 110;
            rasterizer.ColorSpace = PdfRasterizerColorSpace.RGB;

            System.Drawing.Image[] images = rasterizer.ConvertToImages();
            try
            {
                // Frame the preview with a light border so white pages don't blend into a white page.
                const int b = 3;
                using (Bitmap framed = new Bitmap(images[0].Width + 2 * b, images[0].Height + 2 * b))
                {
                    using (Graphics g = Graphics.FromImage(framed))
                    {
                        g.Clear(Color.FromArgb(0xCC, 0xCC, 0xCC));
                        // Draw into an explicit pixel rectangle so the source DPI is ignored.
                        g.DrawImage(images[0], new Rectangle(b, b, images[0].Width, images[0].Height));
                    }
                    framed.Save(imagePath, ImageFormat.Png);
                }
            }
            finally
            {
                foreach (System.Drawing.Image img in images) img.Dispose();
            }
        }

        /// <summary>Walks up from the assembly location to the project folder (the one with the .csproj).</summary>
        private static string FindTutorialDir()
        {
            string dir = AppContext.BaseDirectory;
            while (dir != null && Directory.GetFiles(dir, "*.csproj").Length == 0)
            {
                dir = Path.GetDirectoryName(dir);
            }
            return dir ?? Directory.GetCurrentDirectory();
        }
    }
}
