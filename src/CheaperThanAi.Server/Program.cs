using CheaperThanAi.Server.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddMcpServer()
    .WithStdioServerTransport()
    .WithTools<ITTicketTools>();

builder.Services.UseOllamaClient();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseAuthorization();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
