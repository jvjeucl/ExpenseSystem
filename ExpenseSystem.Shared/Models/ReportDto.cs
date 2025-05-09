namespace ExpenseSystem.Shared.Models;

public class ReportDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime GeneratedDate { get; set; }
    public ReportType Type { get; set; }
    public string GeneratedBy { get; set; }
    public string FileUrl { get; set; }
}

public enum ReportType
{
    MonthlyExpense,
    DepartmentSummary,
    UserExpense,
    AnnualReport
}