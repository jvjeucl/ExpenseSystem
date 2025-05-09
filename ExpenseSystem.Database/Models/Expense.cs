using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ExpenseSystem.Shared.Models;

namespace ExpenseSystem.Database.Models;

public class Expense
{
    [Key]
    public int Id { get; set; }
        
    [Required]
    [MaxLength(100)]
    public string Title { get; set; }
        
    [MaxLength(500)]
    public string Description { get; set; }
        
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }
        
    [Required]
    public DateTime Date { get; set; }
        
    [Required]
    [MaxLength(50)]
    public string Category { get; set; }
        
    [MaxLength(255)]
    public string ReceiptUrl { get; set; }
        
    [Required]
    public ExpenseStatus Status { get; set; }
        
    [Required]
    public string SubmittedById { get; set; }
        
    public string ApprovedById { get; set; }
        
    [ForeignKey("SubmittedById")]
    public virtual User SubmittedBy { get; set; }
        
    [ForeignKey("ApprovedById")]
    public virtual User ApprovedBy { get; set; }
        
    // Audit fields
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}