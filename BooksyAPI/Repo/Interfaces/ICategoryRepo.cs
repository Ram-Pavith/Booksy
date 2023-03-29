using BooksyAPI.Models;
namespace BooksyAPI.Repo.Interfaces
{
    public interface ICategoryRepo<Category>
    {
        public Task<IEnumerable<Category>> GetCategories();
        public Task<Category> GetById(int id);
        public Task Add(Category category);
        public Task Update(Category category);
        public Task Delete(int id);
        public  Task<bool> CategoryExists(int id);
    }
}
