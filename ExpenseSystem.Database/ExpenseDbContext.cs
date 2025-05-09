using ExpenseSystem.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseSystem.Database;

public class ExpenseDbContext : DbContext
{
    public ExpenseDbContext(DbContextOptions<ExpenseDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Expense> Expenses { get; set; }
        
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
            
        // Configure relationships
        modelBuilder.Entity<Expense>()
            .HasOne(e => e.SubmittedBy)
            .WithMany(u => u.SubmittedExpenses)
            .HasForeignKey(e => e.SubmittedById)
            .OnDelete(DeleteBehavior.Restrict);
                
        modelBuilder.Entity<Expense>()
            .HasOne(e => e.ApprovedBy)
            .WithMany(u => u.ApprovedExpenses)
            .HasForeignKey(e => e.ApprovedById)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);
    }
}