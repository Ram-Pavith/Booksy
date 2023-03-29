using BooksyAPI.Data;
using BooksyAPI.Models;
using BooksyAPI.Repo.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BooksyAPI.Repo.Classes
{
    public class ProductRepo: IProductRepo<Product>
    {
        private readonly ApplicationDbContext _context;
        public ProductRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Add(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            Product product = _context.Products.Find(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<Product> GetById(int id)
        {
            List<Product> product = await _context.Products.Include(p => p.Category).Include(p => p.CoverType).ToListAsync();
            return product.SingleOrDefault(p=>p.Id==id);
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context.Products.Include(p => p.CoverType).Include(p=>p.Category).ToListAsync();
        }

        public async Task<bool> ProductExists(int id)
        {
            return await _context.Products.FindAsync(id) != null;
        }

        public async Task Update(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
    }
}
