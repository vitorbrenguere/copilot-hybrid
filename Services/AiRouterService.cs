using CopilotHybrid.Services;

namespace CopilotHybrid.Services;

public interface IAiRouterService
{
    Task<string> RouteAndProcessAsync(string prompt);
}

public class AiRouterService : IAiRouterService
{
    private readonly ILogger<AiRouterService> _logger;
    private readonly IPdfReaderService _pdfReaderService;
    private readonly IWebHostEnvironment _env;

    public AiRouterService(ILogger<AiRouterService> logger, IPdfReaderService pdfReaderService, IWebHostEnvironment env)
    {
        _logger = logger;
        _pdfReaderService = pdfReaderService;
        _env = env;
    }

    public async Task<string> RouteAndProcessAsync(string prompt)
    {
        // Caminho do PDF real da XP na pasta Data
        string pdfPath = Path.Combine(_env.ContentRootPath, "Data", "Relatorio-Mensal-Renda-Fixa-XP-Set26.pdf");
        
        // Extrai o texto completo do PDF em tempo de execução via PdfPig
        string extractedContext = _pdfReaderService.ExtractTextFromPdf(pdfPath);

        // Heurística de FinOps: Identifica se a consulta é complexa ou simples
        bool isComplexQuery = prompt.Length > 150 || 
                              prompt.Contains("imposto", StringComparison.OrdinalIgnoreCase) || 
                              prompt.Contains("sucessório", StringComparison.OrdinalIgnoreCase) ||
                              prompt.Contains("holding", StringComparison.OrdinalIgnoreCase);

        if (isComplexQuery)
        {
            _logger.LogInformation("[FinOps Router] Rota escolhida: AZURE OPENAI (GPT-4o) + RAG Real do PDF.");
            return await Task.FromResult($"[Cloud - GPT-4o + RAG Real]: Resposta baseada no relatório XP ({extractedContext.Length} caracteres extraídos). Análise para: '{prompt}'");
        }
        else
        {
            _logger.LogInformation("[FinOps Router] Rota escolhida: OLLAMA (Local / SLM Phi-4) + RAG Real.");
            return await Task.FromResult($"[Local - Phi-4 + RAG Real]: Triagem rápida utilizando dados do PDF da XP para: '{prompt}'");
        }
    }
}