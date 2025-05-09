using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ExpenseSystem.Shared.Models;
using Microsoft.Extensions.Configuration;

namespace ExpenseSystem.Web.Data
{
    public class ExpenseService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ExpenseService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<List<ExpenseDto>> GetExpensesAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<ExpenseDto>>("api/expenses");
                return response ?? new List<ExpenseDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching expenses: {ex.Message}");
                return new List<ExpenseDto>();
            }
        }

        public async Task<ExpenseDto> GetExpenseAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<ExpenseDto>($"api/expenses/{id}") ?? new ExpenseDto();
            }
            catch
            {
                return new ExpenseDto();
            }
        }

        public async Task<bool> CreateExpenseAsync(ExpenseDto expense)
        {
            try
            {
                Console.WriteLine(" called! id=" + expense.SubmittedById);
                var response = await _httpClient.PostAsJsonAsync("api/expenses", expense);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateExpenseAsync(ExpenseDto expense)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/expenses/{expense.Id}", expense);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<ExpenseDto>> SearchExpensesAsync(string searchTerm)
        {
            try
            {
                var content = new StringContent(
                    JsonSerializer.Serialize(searchTerm),
                    Encoding.UTF8,
                    "application/json");
                
                var response = await _httpClient.PostAsync("api/expenses/search", content);
                
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<ExpenseDto>>() ?? new List<ExpenseDto>();
                }
                
                return new List<ExpenseDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching expenses: {ex.Message}");
                return new List<ExpenseDto>();
            }
        }
    }
}