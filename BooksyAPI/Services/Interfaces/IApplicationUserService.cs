using BooksyAPI.Models;

namespace BooksyAPI.Services.Interfaces
{
    public interface IApplicationUserService<ApplicationUser>
    {
        public Task<IEnumerable<ApplicationUser>> GetApplicationUser();
        public Task<ApplicationUser> GetById(int id);
        public Task Add(ApplicationUser applicationuser);
        public Task Update(ApplicationUser applicationnuser);
        public Task Delete(int id);
        public Task<bool> ApplicationUserExists(int id);
    }
}
