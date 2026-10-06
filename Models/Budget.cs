using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpensesApi.Models
{
    public class Budget
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        [StringLength(50, ErrorMessage = "Category can be at most 50 characters.")]
        public string Category { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, 100000000, ErrorMessage = "Monthly limit must be greater than 0.")]
        public decimal MonthlyLimit { get; set; }
    }
}