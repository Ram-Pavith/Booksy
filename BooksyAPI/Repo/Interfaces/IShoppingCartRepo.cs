using BooksyAPI.Models;

namespace BooksyAPI.Repo.Interfaces
{
    public interface IShoppingCartRepo<ShoppingCart>
    {
        public Task<IEnumerable<ShoppingCart>> GetShoppingCarts();
        public Task<ShoppingCart> GetById(int id);
        public Task Add(ShoppingCart shoppingcart);
        public Task Update(ShoppingCart shoppingcart);
        public Task Delete(int id);
        public Task<bool> ShoppingCartExists(int id);
        public int IncrementCount(ShoppingCart shoppingCart, int count);
        public int DecrementCount(ShoppingCart shoppingCart, int count);
    }
}
