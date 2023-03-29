using BooksyAPI.Models;

namespace BooksyAPI.Services.Interfaces
{
    public interface ICoverTypeService<CoverType>
    {
        public Task<IEnumerable<CoverType>> GetCoverTypes();
        public Task<CoverType> GetById(int id);
        public Task Add(CoverType covertype);
        public Task Update(CoverType covertype);
        public Task Delete(int id);
        public Task<bool> CoverTypeExists(int id);
    }
}
