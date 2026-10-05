using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpensesApi.Models
{
    public class Expense
    {
        public int Id { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, 1000000, ErrorMessage = "Value must be greater than 0.")]
        public decimal Value { get; set; }

        public DateTime Date { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(100, ErrorMessage = "Description can be at most 100 characters.")]
        public string Description { get; set; }

        public string? Category { get; set; }
    }
}