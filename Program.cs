using CopilotHybrid.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// OpenAPI nativo do .NET 10
builder.Services.AddOpenApi();

// Registando os nossos serviços de IA e RAG
builder.Services.AddScoped<IPdfReaderService, PdfReaderService>();
builder.Services.AddScoped<IAiRouterService, AiRouterService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Mapeia o endpoint do OpenAPI nativo (geralmente em /openapi/v1.json)
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();