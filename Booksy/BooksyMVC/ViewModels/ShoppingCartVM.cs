using Booksy.Models;

namespace BooksyMVC.ViewModels
{
	public class ShoppingCartVM
	{
		public IEnumerable<ShoppingCart> ListCart { get; set; }

		public OrderHeader OrderHeader { get; set; }
	}
}
