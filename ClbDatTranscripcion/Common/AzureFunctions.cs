using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;
using ClbModOCR.Common;
using iText.Html2pdf;
using iText.Kernel.Pdf;
using iText.Layout;
using Microsoft.AspNetCore.Http;

namespace ClbDatOCR.Common
{
    public class AzureFunctions : DatabaseConnections
    {
        public async Task<TempTranscript> GetTextFromImage(IFormFile file)
        {
            string key = base.GetAzureKey();
            string endpoint = base.GetAzureEndpoint();
            Stream stream = file.OpenReadStream();
            AzureKeyCredential credential = new AzureKeyCredential(key);
            DocumentAnalysisClient client = new DocumentAnalysisClient(new Uri(endpoint), credential);
            AnalyzeDocumentOperation operation = await client.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-read", stream);
            AnalyzeResult resultOperation = operation.Value;
            int words = 0;
            double confidence = 0;
            if (resultOperation.Content.Length > 0)
            {
                foreach (DocumentPage page in resultOperation.Pages)
                {
                    foreach (DocumentWord word in page.Words)
                    {
                        words++;
                        confidence += word.Confidence;
                    }
                }
                string percentage = "" + ((confidence / words) * 100);
                double porcentaje = double.Parse(percentage.Substring(0, 5));

                string transcription = "" + resultOperation.Content;
                var html = $"<html><body>{resultOperation.Content}</body></html>";
                return new TempTranscript
                {
                    transcript = transcription,
                    percentage = porcentaje
                };
            }
            else
            {
                return new TempTranscript
                {
                    transcript = "No se encontró texto en la imagen",
                    percentage = 0
                };

            }
        }

        public string[] CreatePdfFile(string text, string nombre)
        {
            using (MemoryStream outputStream = new MemoryStream())
            {
                using (var writer = new PdfWriter(outputStream))
                {
                    using (PdfDocument pdf = new PdfDocument(writer))
                    {
                        var document = new Document(pdf);
                        HtmlConverter.ConvertToPdf(text, pdf, new ConverterProperties());
                        document.Close();
                    }
                }
                var parts = nombre.Split('.');
                nombre = "Transcripts_1_" + parts[parts.Length - 1] + "_" + parts[0] + DateTime.Now.ToString("hhmmssffffff") + ".pdf";


                Stream stream = new MemoryStream(outputStream.ToArray());
                stream.CopyTo(File.Create("C:\\Proyectos\\Transcripts_1\\" + nombre));
                string[] respuesta = new string[2];
                respuesta[0] = ("C:\\Proyectos\\Transcripts_1\\" + nombre);
                respuesta[1] = nombre;
                return respuesta;
            }
        }
    }
}
