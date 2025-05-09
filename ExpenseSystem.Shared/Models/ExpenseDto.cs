namespace ExpenseSystem.Shared.Models;

public class ExpenseDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string Category { get; set; }
    public string ReceiptUrl { get; set; }
    public ExpenseStatus Status { get; set; }
    public string SubmittedById { get; set; }
    public string ApprovedById { get; set; }
}

public enum ExpenseStatus
{
    Draft,
    Submitted,
    UnderReview,
    Approved,
    Rejected,
    Paid
}