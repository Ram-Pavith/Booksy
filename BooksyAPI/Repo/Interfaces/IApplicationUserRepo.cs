using BooksyAPI.Models;

namespace BooksyAPI.Repo.Interfaces
{
    public interface IApplicationUserRepo<ApplicationUser>
    {
        public Task<IEnumerable<ApplicationUser>> GetApplicationUsers();
        public Task<ApplicationUser> GetById(int id);
        public Task Add(ApplicationUser applicationuser);
        public Task Update(ApplicationUser applicationuser);
        public Task Delete(int id);
        public Task<bool> ApplicationUserExists(int id);
    }
}
