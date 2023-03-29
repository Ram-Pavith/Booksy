using BooksyAPI.Models;

namespace BooksyAPI.Services.Interfaces
{
    public interface IShoppingCartService<ShoppingCart>
    {
        public Task<IEnumerable<ShoppingCart>> GetShoppingCarts();
        public Task<ShoppingCart> GetById(int id);
        public Task Add(ShoppingCart shoppingcart);
        public Task Update(ShoppingCart shoppingcart);
        public Task Delete(int id);
        public Task<bool> ShoppingCartExists(int id);
    }
}
