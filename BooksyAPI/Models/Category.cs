using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BooksyAPI.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Display(Name ="Display Order")]
        [Range(1,100,ErrorMessage ="Display Order must be between 1 and 100")]
        public int DisplayOrder { get; set; }

        [Display(Name ="Created Date")]
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }
}
