using UglyToad.PdfPig;

namespace CopilotHybrid.Services;

public interface IPdfReaderService
{
    string ExtractTextFromPdf(string filePath);
}

public class PdfReaderService : IPdfReaderService
{
    public string ExtractTextFromPdf(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"O ficheiro PDF não foi encontrado no caminho: {filePath}");
        }

        var textContent = new System.Text.StringBuilder();

        // Lê o PDF real página por página usando o PdfPig
        using (var pdf = PdfDocument.Open(filePath))
        {
            foreach (var page in pdf.GetPages())
            {
                textContent.AppendLine(page.Text);
            }
        }

        return textContent.ToString();
    }
}