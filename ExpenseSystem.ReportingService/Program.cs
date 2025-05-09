using ExpenseSystem.Database;
using ExpenseSystem.ReportingService;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

// Tilføj DbContext
builder.Services.AddDbContext<ExpenseDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHostedService<ReportingWorker>();

var host = builder.Build();
host.Run();