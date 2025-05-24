using Microsoft.EntityFrameworkCore;
using Tutorial5.Data;
using Tutorial5.Services;

var builder = WebApplication.CreateBuilder(args);

// Dodaj obsługę kontrolerów
builder.Services.AddControllers();

// Konfiguracja DbContext – połączenie z SQL Server pobierane z appsettings.json
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Wstrzykiwanie zależności dla serwisu przepisów
builder.Services.AddScoped<IPrescriptionService, PrescriptionService>();

var app = builder.Build();

// Middleware
app.UseAuthorization();

// Mapowanie kontrolerów
app.MapControllers();

app.Run();