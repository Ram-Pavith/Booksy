using BooksyAPI.Data;
using BooksyAPI.Models;
using BooksyAPI.Repo.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BooksyAPI.Repo.Classes
{
    public class ApplicationUserRepo : IApplicationUserRepo<ApplicationUser>
    {
        private readonly ApplicationDbContext _context;
        public ApplicationUserRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Add(ApplicationUser applicationuser)
        {
            _context.ApplicationUsers.Add(applicationuser);
             _context.SaveChanges();
        }

        public async Task<bool> ApplicationUserExists(int id)
        {
            return await _context.ApplicationUsers.FindAsync(id) != null;
        }

        public async Task Delete(int id)
        {
            ApplicationUser applicationuser = _context.ApplicationUsers.Find(id);
            _context.ApplicationUsers.Remove(applicationuser);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ApplicationUser>> GetApplicationUsers()
        {
            return await _context.ApplicationUsers.Include(a=>a.Company).ToListAsync();
        }

        public async Task<ApplicationUser> GetById(int id)
        {
			List<ApplicationUser> applicationUsers= await _context.ApplicationUsers.Include(a => a.Company).ToListAsync();

			return applicationUsers.SingleOrDefault(a=>a.Id==id);
        }

        public async Task Update(ApplicationUser applicationuser)
        {
            _context.ApplicationUsers.Update(applicationuser);
            await _context.SaveChangesAsync();
        }
    }
}
