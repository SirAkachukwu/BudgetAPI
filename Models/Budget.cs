using System;
using System.ComponentModel.DataAnnotations;

namespace BudgetAPI.Models
{
    public class Budget
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;

        [Range(0, double.MaxValue)]
        public decimal Amount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public virtual User? User { get; set; }
        public List<Expense>? Expenses { get; set; }

    }
}
