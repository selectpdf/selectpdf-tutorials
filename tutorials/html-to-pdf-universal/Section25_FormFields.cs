using System.IO;
using SelectPdf.Universal;

namespace SelectPdfExamples.HtmlToPdfUniversalTutorial
{
    /// <summary>Turns the controls of an HTML form into fillable PDF form fields.</summary>
    public static class Section25_FormFields
    {
        public static string Run(string outputDir)
        {
            string outputPdf = Path.Combine(outputDir, "form-fields.pdf");

            #region snippet
            HtmlToPdf converter = new HtmlToPdf();

            // Every visible input, select and textarea becomes a PDF form field of the
            // matching kind, placed over the control and keeping its current value.
            converter.Options.CreateFormFields = true;

            PdfDocument doc = converter.ConvertHtmlString(@"
<form style='font-family:Arial;max-width:480px'>
  <h1 style='color:#b45309'>Course registration</h1>
  <p><label>Full name<br><input name='name' value='Jane Doe' style='width:100%'></label></p>
  <p><label>Email<br><input type='email' name='email' style='width:100%'></label></p>
  <p><label>Course<br>
    <select name='course'><option>HTML to PDF basics</option><option selected>Advanced layouts</option></select>
  </label></p>
  <p>Format:
    <label><input type='radio' name='format' value='online' checked> Online</label>
    <label><input type='radio' name='format' value='onsite'> On site</label></p>
  <p><label><input type='checkbox' name='newsletter' checked> Send me the newsletter</label></p>
  <p><label>Comments<br><textarea name='comments' rows='3' style='width:100%'></textarea></label></p>
</form>");

            doc.Save(outputPdf);
            doc.Close();
            #endregion

            return outputPdf;
        }
    }
}
