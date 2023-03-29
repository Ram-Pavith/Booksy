using BooksyAPI.Models;

namespace BooksyAPI.Repo.Interfaces
{
    public interface ICoverTypeRepo<CoverType>
    {
        public Task<IEnumerable<CoverType>> GetCoverTypes();
        public Task<CoverType> GetById(int id);
        public Task Add(CoverType covertype);
        public Task Update(CoverType covertype);
        public Task Delete(int id);
        public Task<bool> CoverTypeExists(int id);
    }
}
