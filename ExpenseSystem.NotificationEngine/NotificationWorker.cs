// ExpenseSystem.NotificationEngine/NotificationWorker.cs
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExpenseSystem.Database;
using ExpenseSystem.Database.Models;
using ExpenseSystem.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ExpenseSystem.NotificationEngine
{
    public class NotificationWorker : BackgroundService
    {
        private readonly ILogger<NotificationWorker> _logger;
        private readonly IServiceProvider _serviceProvider;

        public NotificationWorker(ILogger<NotificationWorker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Notification Service started at: {time}", DateTimeOffset.Now);

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Notification Service running at: {time}", DateTimeOffset.Now);
                
                try
                {
                    await ProcessNotifications(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while processing notifications");
                }

                await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);
            }
        }

        private async Task ProcessNotifications(CancellationToken stoppingToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ExpenseDbContext>();

            // Find expenses that need notification
            var pendingExpenses = await dbContext.Expenses
                .Where(e => e.Status == ExpenseStatus.Submitted)
                .Include(e => e.SubmittedBy)
                .ToListAsync(stoppingToken);

            foreach (var expense in pendingExpenses)
            {
                // SECURITY ISSUE: Potential PII exposure in logging
                _logger.LogInformation("Processing notification for expense {ExpenseId} by {UserEmail} for ${Amount}",
                    expense.Id, expense.SubmittedBy.Email, expense.Amount);

                // Here would be code to send actual notifications
                // For simplicity, we're just updating the status
                expense.Status = ExpenseStatus.UnderReview;
                expense.UpdatedAt = DateTime.UtcNow;
            }

            if (pendingExpenses.Any())
            {
                await dbContext.SaveChangesAsync(stoppingToken);
            }
        }
    }
}