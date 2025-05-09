using System.ComponentModel.DataAnnotations;

namespace ExpenseSystem.Database.Models;

public class User
{
    [Key]
    public string Id { get; set; }
        
    [Required]
    [MaxLength(50)]
    public string Username { get; set; }
        
    [Required]
    [MaxLength(100)]
    [EmailAddress]
    public string Email { get; set; }
        
    [Required]
    [MaxLength(100)]
    public string PasswordHash { get; set; }
        
    [MaxLength(50)]
    public string FirstName { get; set; }
        
    [MaxLength(50)]
    public string LastName { get; set; }
        
    [MaxLength(50)]
    public string Department { get; set; }
        
    [Required]
    public UserRole Role { get; set; }
        
    public virtual ICollection<Expense> SubmittedExpenses { get; set; }
    public virtual ICollection<Expense> ApprovedExpenses { get; set; }
        
    // Audit fields
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? LastLogin { get; set; }
}

public enum UserRole
{
    Employee,
    Manager,
    FinanceAdmin,
    SystemAdmin
}