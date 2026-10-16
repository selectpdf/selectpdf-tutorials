using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using SelectPdf.Universal;

namespace SelectPdfExamples.HtmlToPdfUniversalTutorial
{
    /// <summary>Signs a converted document with a certificate, then validates the signature.</summary>
    public static class Section28_DigitalSignature
    {
        public static string Run(string outputDir)
        {
            string outputPdf = Path.Combine(outputDir, "signed.pdf");

            // A throwaway self-signed certificate so the example runs anywhere.
            // In production use your own .pfx, the certificate store or a hardware token.
            const string pfxPassword = "tutorial";
            string pfxFile = CreateSelfSignedPfx(outputDir, pfxPassword);

            #region snippet
            HtmlToPdf converter = new HtmlToPdf();
            using PdfDocument doc = converter.ConvertHtmlString(
                "<h1 style='font-family:Arial'>Service agreement</h1>" +
                "<p style='font-family:Arial'>Signed electronically by SelectPdf.</p>");

            // Load the signing certificate from a password-protected .pfx file.
            using PdfDigitalCertificatesCollection certificates =
                PdfDigitalCertificatesStore.GetCertificates(pfxFile, pfxPassword);

            // Add a visible signature to the first page. The document is signed when saved.
            PdfDigitalSignatureElement signature = new PdfDigitalSignatureElement(
                350, 120, 180, 60, certificates[0]);
            signature.Reason = "Agreement accepted";
            signature.Location = "Bucharest";
            signature.DigestAlgorithm = PdfSignatureDigest.SHA384;
            doc.Pages[0].Add(signature);

            doc.Save(outputPdf);

            // Check the signature. A self-signed certificate is only trusted when it is
            // passed in as a trusted root, the way you would pass your own CA.
            using PdfSecurityManager security = new PdfSecurityManager();
            security.Load(outputPdf);
            foreach (PdfSignatureValidationResult result in security.ValidateSignatures(certificates))
            {
                Console.Write($"signer={result.SignerName}, valid={result.IsValid}, trusted={result.IsTrusted}, " +
                              $"modified={result.IsDocumentModified} ... ");
            }
            #endregion

            File.Delete(pfxFile);
            return outputPdf;
        }

        private static string CreateSelfSignedPfx(string dir, string password)
        {
            using RSA rsa = RSA.Create(2048);
            CertificateRequest request = new CertificateRequest(
                "CN=SelectPdf Tutorial Signer, O=SelectPdf", rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            request.CertificateExtensions.Add(new X509KeyUsageExtension(
                X509KeyUsageFlags.DigitalSignature | X509KeyUsageFlags.NonRepudiation, true));
            using X509Certificate2 cert = request.CreateSelfSigned(DateTimeOffset.Now.AddDays(-1), DateTimeOffset.Now.AddYears(1));

            string path = Path.Combine(dir, "tutorial-signer.pfx");
            File.WriteAllBytes(path, cert.Export(X509ContentType.Pfx, password));
            return path;
        }
    }
}
