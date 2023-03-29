using BooksyAPI.Models;

namespace BooksyAPI.Repo.Interfaces
{
    public interface ICompanyRepo<Company>
    {
        public Task<IEnumerable<Company>> GetCompanies();
        public Task<Company> GetById(int id);
        public Task Add(Company company);
        public Task Update(Company company);
        public Task Delete(int id);
        public Task<bool> CompanyExists(int id);
    }
}
