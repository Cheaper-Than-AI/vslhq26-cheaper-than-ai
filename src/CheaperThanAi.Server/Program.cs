using CheaperThanAi.Server.Extensions;
using CheaperThanAi.Server.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// EF Core DbContext - configured via appsettings.json (ConnectionStrings:TicketsDb)
builder.Services.AddDbContext<TicketsDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("TicketsDb")));

builder.Services.AddScoped<ITTicketTools>();

builder.Services.UseOllamaClient();

var app = builder.Build();

// Ensure database exists on startup (prototype-friendly). For production use migrations instead.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TicketsDbContext>();
    db.Database.EnsureCreated();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseAuthorization();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
