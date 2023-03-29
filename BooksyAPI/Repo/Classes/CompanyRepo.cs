using BooksyAPI.Data;
using BooksyAPI.Models;
using BooksyAPI.Repo.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace BooksyAPI.Repo.Classes
{
    public class CompanyRepo : ICompanyRepo<Company>
    {
        private ApplicationDbContext _context;
        public CompanyRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Add(Company company)
        {
            await _context.Companies.AddAsync(company);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CompanyExists(int id)
        {
            return await _context.Companies.FindAsync(id)!=null;

        }

        public async Task Delete(int id)
        {
            Company company = _context.Companies.Find(id);
            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
        }

        public async Task<Company> GetById(int id)
        {
            return await _context.Companies.FindAsync(id);
        }

        public async  Task<IEnumerable<Company>> GetCompanies()
        {
            return await _context.Companies.ToListAsync();
        }

        public async Task Update(Company company)
        {
            _context.Companies.Update(company);
            await _context.SaveChangesAsync();
        }
    }
}
