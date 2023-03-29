using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BooksyAPI.Models
{
    public class ApplicationUser 
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "User Name")]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; } = null!;
        [Required]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}")]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; } = null!;
        [Required]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        public bool isAdmin { get; set; } = false;
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public int? CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        [ValidateNever]
        public virtual Company Company { get; set; }
    }
}
