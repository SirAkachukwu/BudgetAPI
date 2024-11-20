using System;
using System.ComponentModel.DataAnnotations;

namespace BudgetAPI.DTOs
{
    public class BudgetCreateDto
    {
        public string Name { get; set; } = string.Empty;
        [Range(0, double.MaxValue, ErrorMessage = "Amount must be a positive value.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "StartDate is required.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "EndDate is required.")]
        public DateTime EndDate { get; set; }
    }
}
