namespace ExpenseSystem.Shared.Models;

public class UserDto
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Department { get; set; }
    public UserRole Role { get; set; }
}

public enum UserRole
{
    Employee,
    Manager,
    FinanceAdmin,
    SystemAdmin
}