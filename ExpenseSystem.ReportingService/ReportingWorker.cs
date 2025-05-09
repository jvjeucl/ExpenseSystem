// ExpenseSystem.ReportingService/ReportingWorker.cs
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExpenseSystem.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ExpenseSystem.ReportingService
{
    public class ReportingWorker : BackgroundService
    {
        private readonly ILogger<ReportingWorker> _logger;
        private readonly IServiceProvider _serviceProvider;

        public ReportingWorker(ILogger<ReportingWorker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Reporting Service started at: {time}", DateTimeOffset.Now);

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Reporting Service running at: {time}", DateTimeOffset.Now);
                
                try
                {
                    await GenerateReports(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while generating reports");
                }

                // Run once per day
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }

        private async Task GenerateReports(CancellationToken stoppingToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ExpenseDbContext>();

            var now = DateTime.UtcNow;
            var firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
            var lastMonth = firstDayOfMonth.AddMonths(-1);

            // Get last month's expenses
            var lastMonthExpenses = await dbContext.Expenses
                .Where(e => e.Date >= lastMonth && e.Date < firstDayOfMonth)
                .OrderBy(e => e.Date)
                .ToListAsync(stoppingToken);

            _logger.LogInformation("Generating monthly report for {Month} with {Count} expenses",
                lastMonth.ToString("MMMM yyyy"), lastMonthExpenses.Count);

            // SECURITY ISSUE: Path traversal vulnerability
            var userInput = "monthly_report_" + lastMonth.ToString("yyyy_MM");
            var fileName = userInput + ".csv";
            
            // Vulnerable to path traversal
            var reportPath = Path.Combine("Reports", fileName);
            
            // Generate CSV report
            Directory.CreateDirectory(Path.GetDirectoryName(reportPath));
            
            using (var writer = new StreamWriter(reportPath))
            {
                // Write header
                writer.WriteLine("Id,Title,Amount,Date,Category,Status,SubmittedBy");
                
                // Write data
                foreach (var expense in lastMonthExpenses)
                {
                    // SECURITY ISSUE: CSV Injection vulnerability
                    writer.WriteLine($"{expense.Id},{expense.Title},{expense.Amount},{expense.Date},{expense.Category},{expense.Status},{expense.SubmittedById}");
                }
            }
            
            _logger.LogInformation("Monthly report generated at {Path}", reportPath);
        }
    }
}