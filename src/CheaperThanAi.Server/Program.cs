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
    // Ensure FTS5 virtual table and triggers exist for full text search
    try
    {
        var conn = db.Database.GetDbConnection();
        if (conn.State != System.Data.ConnectionState.Open)
        {
            conn.Open();
        }

        using (var cmd = conn.CreateCommand())
        {
            // Create FTS5 virtual table that references the tickets table
            cmd.CommandText = @"CREATE VIRTUAL TABLE IF NOT EXISTS TicketSearch USING fts5(
                IssueDescription, UserName, Category,
                content='tickets', content_rowid='rowid');";
            cmd.ExecuteNonQuery();

            // Rebuild the FTS index from existing tickets (safe to run)
            try
            {
                cmd.CommandText = "INSERT INTO TicketSearch(TicketSearch) VALUES('rebuild');";
                cmd.ExecuteNonQuery();
            }
            catch
            {
                // ignore rebuild errors
            }

            // Create triggers to keep TicketSearch in sync with tickets
            cmd.CommandText = @"DROP TRIGGER IF EXISTS tickets_ai;";
            cmd.ExecuteNonQuery();
            cmd.CommandText = @"CREATE TRIGGER tickets_ai AFTER INSERT ON tickets BEGIN
                INSERT INTO TicketSearch(rowid, IssueDescription, UserName, Category)
                VALUES (new.rowid, new.IssueDescription, new.UserName, new.Category);
            END;";
            cmd.ExecuteNonQuery();

            cmd.CommandText = @"DROP TRIGGER IF EXISTS tickets_ad;";
            cmd.ExecuteNonQuery();
            cmd.CommandText = @"CREATE TRIGGER tickets_ad AFTER DELETE ON tickets BEGIN
                DELETE FROM TicketSearch WHERE rowid = old.rowid;
            END;";
            cmd.ExecuteNonQuery();

            cmd.CommandText = @"DROP TRIGGER IF EXISTS tickets_au;";
            cmd.ExecuteNonQuery();
            cmd.CommandText = @"CREATE TRIGGER tickets_au AFTER UPDATE ON tickets BEGIN
                DELETE FROM TicketSearch WHERE rowid = old.rowid;
                INSERT INTO TicketSearch(rowid, IssueDescription, UserName, Category)
                VALUES (new.rowid, new.IssueDescription, new.UserName, new.Category);
            END;";
            cmd.ExecuteNonQuery();
        }
    }
    catch
    {
        // ignore errors here; FTS5 may not be available on all SQLite builds
    }
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseAuthorization();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
