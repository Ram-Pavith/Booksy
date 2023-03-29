using BooksyAPI.Data;
using BooksyAPI.Models;
using BooksyAPI.Repo.Interfaces;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BooksyAPI.Repo.Classes
{
    public class CoverTypeRepo : ICoverTypeRepo<CoverType>
    {
        private readonly ApplicationDbContext _context;
        public CoverTypeRepo(ApplicationDbContext context) {
            _context = context;
        }
        public async Task Add(CoverType covertype)
        {
            await _context.CoverTypes.AddAsync(covertype);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CoverTypeExists(int id)
        {
            return await _context.CoverTypes.FindAsync(id) != null;
        }

        public async Task Delete(int id)
        {
            CoverType covertype = _context.CoverTypes.Find(id);
            _context.CoverTypes.Remove(covertype);
            await _context.SaveChangesAsync();
        }

        public async Task<CoverType> GetById(int id)
        {
            return await _context.CoverTypes.FindAsync(id);
        }

        public async Task<IEnumerable<CoverType>> GetCoverTypes()
        {
            return await _context.CoverTypes.ToListAsync();
        }

        public async Task Update(CoverType covertype)
        {
            _context.CoverTypes.Update(covertype);
            //covertype.Name; 
            await _context.SaveChangesAsync();
        }
    }
}
