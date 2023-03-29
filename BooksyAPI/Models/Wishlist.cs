using System.ComponentModel.DataAnnotations.Schema;

namespace BooksyAPI.Models
{
    public class Wishlist
    {
        public int Id { get; set; }
        public string ProductId { get; set; }




        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}
