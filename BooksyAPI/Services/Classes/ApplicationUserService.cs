using BooksyAPI.Models;
using BooksyAPI.Repo.Interfaces;
using BooksyAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BooksyAPI.Services.Classes
{
    public class ApplicationUserService:IApplicationUserService<ApplicationUser>
    {
        private readonly IApplicationUserRepo<ApplicationUser> _repoobj;
        public ApplicationUserService(IApplicationUserRepo<ApplicationUser> repoobj)
        {
            _repoobj = repoobj;
        }

        public async Task Add(ApplicationUser applicationuser)
        {
            try
            {
                await _repoobj.Add(applicationuser);
            }
            catch (DBConcurrencyException e)
            {
                throw new DBConcurrencyException(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> ApplicationUserExists(int id)
        {
            try
            {
                return await _repoobj.ApplicationUserExists(id);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task Delete(int id)
        {
            try
            {
                await _repoobj.Delete(id);
            }
            catch (DBConcurrencyException e)
            {
                throw new DBConcurrencyException(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<IEnumerable<ApplicationUser>> GetApplicationUser()
        {
            try
            {
                return await _repoobj.GetApplicationUsers();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public async Task<ApplicationUser> GetById(int id)
        {
            try
            {
                return await _repoobj.GetById(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task Update(ApplicationUser applicationnuser)
        {
            try
            {
                await _repoobj.Update(applicationnuser);
            }
            catch (DbUpdateException e)
            {
                throw new DbUpdateException(e.Message);
            }
            catch (DBConcurrencyException e)
            {
                throw new DBConcurrencyException(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
