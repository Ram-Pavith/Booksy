using BooksyAPI.Data;
using BooksyAPI.Models;
using BooksyAPI.Repo.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BooksyAPI.Repo.Classes
{
    public class ShoppingCartRepo:IShoppingCartRepo<ShoppingCart>
    {
        private readonly ApplicationDbContext _context;
        public ShoppingCartRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Add(ShoppingCart shoppingcart)
        {
            await _context.ShoppingCarts.AddAsync(shoppingcart);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            ShoppingCart shoppingcart = _context.ShoppingCarts.Find(id);
            _context.ShoppingCarts.Remove(shoppingcart);
            await _context.SaveChangesAsync();
        }

        public async Task<ShoppingCart> GetById(int id)
        {
            List<ShoppingCart> shoppingCarts = await _context.ShoppingCarts.Include(s => s.ApplicationUser).Include(s => s.Product).ToListAsync();
			return shoppingCarts.SingleOrDefault(s=>s.Id==id);
        }

        public async Task<IEnumerable<ShoppingCart>> GetShoppingCarts()
        {
            return await _context.ShoppingCarts.Include(s=>s.ApplicationUser).Include(s=>s.Product).ToListAsync();
        }

        public async Task<bool> ShoppingCartExists(int id)
        {
            return await _context.ShoppingCarts.FindAsync(id) != null;
        }

        public int DecrementCount(ShoppingCart shoppingCart, int count)
        {
            shoppingCart.Count -= count;
            _context.ShoppingCarts.Update(shoppingCart);
            _context.SaveChanges();
            return shoppingCart.Count;
        }

        public int IncrementCount(ShoppingCart shoppingCart, int count)
        {
            shoppingCart.Count += count;
            _context.ShoppingCarts.Update(shoppingCart);
            _context.SaveChanges();
            return shoppingCart.Count;
        }
        public async Task Update(ShoppingCart shoppingcart)
        {
            _context.ShoppingCarts.Update(shoppingcart);
            await _context.SaveChangesAsync();
        }
    }
}
