using System.Security.Claims;
using ExpenseSystem.Database;
using ExpenseSystem.Database.Models;
using ExpenseSystem.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace ExpenseSystem.API.Controllers;

    [ApiController]
    [Route("api/[controller]")]
   // [Authorize]
    public class ExpensesController : ControllerBase
    {
        private readonly ExpenseDbContext _context;

        public ExpensesController(ExpenseDbContext context)
        {
            _context = context;
        }

        // GET: api/expenses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetExpenses()
        {
            var userId = User.Identity.Name;
            
            // SECURITY ISSUE: This lacks proper authorization filtering
            // A user can see all expenses, not just their own or ones they should approve
            var expenses = await _context.Expenses
                .Select(e => new ExpenseDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    Description = e.Description,
                    Amount = e.Amount,
                    Date = e.Date,
                    Category = e.Category,
                    ReceiptUrl = e.ReceiptUrl,
                    Status = e.Status,
                    SubmittedById = e.SubmittedById,
                    ApprovedById = e.ApprovedById
                })
                .ToListAsync();

            return expenses;
        }

        // GET: api/expenses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ExpenseDto>> GetExpense(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);

            if (expense == null)
            {
                return NotFound();
            }

            // SECURITY ISSUE: No authorization check if the user has rights to view this expense
            
            return new ExpenseDto
            {
                Id = expense.Id,
                Title = expense.Title,
                Description = expense.Description,
                Amount = expense.Amount,
                Date = expense.Date,
                Category = expense.Category,
                ReceiptUrl = expense.ReceiptUrl,
                Status = expense.Status,
                SubmittedById = expense.SubmittedById,
                ApprovedById = expense.ApprovedById
            };
        }

        // POST: api/expenses
        [HttpPost]
        public async Task<ActionResult<ExpenseDto>> CreateExpense(ExpenseDto expenseDto)
        {
            Console.WriteLine("expenseDto.SubmittedById: " + expenseDto.SubmittedById);
            // Sikkerhedskontrol: Sørg for at vi har en SubmittedById
            if (string.IsNullOrEmpty(expenseDto.SubmittedById))
            {
                // Prøv at hente fra token, hvis det er muligt
                var userId = User.FindFirst(ClaimTypes.Name)?.Value;
                if (!string.IsNullOrEmpty(userId))
                {
                    expenseDto.SubmittedById = userId;
                }
                else
                {
                    // Fejl hvis vi stadig ikke har en bruger-ID
                    return BadRequest("SubmittedById is required");
                }
            }

            // Debug output til console
            Console.WriteLine($"Creating expense with SubmittedById: {expenseDto.SubmittedById}");
    
            var expense = new Expense
            {
                Title = expenseDto.Title,
                Description = expenseDto.Description,
                Amount = expenseDto.Amount,
                Date = expenseDto.Date,
                Category = expenseDto.Category,
                ReceiptUrl = expenseDto.ReceiptUrl,
                Status = ExpenseStatus.Draft,
                SubmittedById = expenseDto.SubmittedById, // Brug værdien fra DTO'en
                ApprovedById = null, // Dette skal være explicit null for nye expenses
                CreatedAt = DateTime.UtcNow
            };

            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();

            expenseDto.Id = expense.Id;
            return CreatedAtAction(nameof(GetExpense), new { id = expense.Id }, expenseDto);
        }

        // PUT: api/expenses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExpense(int id, ExpenseDto expenseDto)
        {
            if (id != expenseDto.Id)
            {
                return BadRequest();
            }

            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null)
            {
                return NotFound();
            }

            // SECURITY ISSUE: No validation if user has rights to update this expense
            // and no handling of status transitions correctly
            
            expense.Title = expenseDto.Title;
            expense.Description = expenseDto.Description;
            expense.Amount = expenseDto.Amount;
            expense.Date = expenseDto.Date;
            expense.Category = expenseDto.Category;
            expense.ReceiptUrl = expenseDto.ReceiptUrl;
            expense.UpdatedAt = DateTime.UtcNow;

            // SECURITY ISSUE: Status can be set directly without proper workflow validation
            expense.Status = expenseDto.Status;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExpenseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // SECURITY ISSUE: SQL Injection vulnerability
        // POST: api/expenses/search
        [HttpPost("search")]
        public async Task<ActionResult<IEnumerable<ExpenseDto>>> SearchExpenses([FromBody] string searchTerm)
        {
            // VULNERABILITY: SQL Injection by directly incorporating the search term
            var expenses = await _context.Expenses
                .FromSqlRaw($"SELECT * FROM Expenses WHERE Title LIKE '%{searchTerm}%' OR Description LIKE '%{searchTerm}%'")
                .Select(e => new ExpenseDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    Description = e.Description,
                    Amount = e.Amount,
                    Date = e.Date,
                    Category = e.Category,
                    ReceiptUrl = e.ReceiptUrl,
                    Status = e.Status,
                    SubmittedById = e.SubmittedById,
                    ApprovedById = e.ApprovedById
                })
                .ToListAsync();

            return expenses;
        }

        private bool ExpenseExists(int id)
        {
            return _context.Expenses.Any(e => e.Id == id);
        }
    }