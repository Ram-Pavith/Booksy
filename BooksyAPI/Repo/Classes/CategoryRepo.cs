using BooksyAPI.Data;
using BooksyAPI.Models;
using BooksyAPI.Repo.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BooksyAPI.Repo.Classes
{
    public class CategoryRepo : ICategoryRepo<Category>
    {
        private ApplicationDbContext _context;
        public CategoryRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetById(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task Add(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Category category)
        {

             _context.Update(category);
            await _context.SaveChangesAsync();
        }
        public async Task Delete(int id)
        {
            Category category = _context.Categories.Find(id);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> CategoryExists(int id)
        {
            return await _context.Categories.FindAsync(id) != null;
        }
    }
}
