using BooksyAPI.Models;
using BooksyAPI.Repo.Interfaces;
using BooksyAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Runtime.CompilerServices;

namespace BooksyAPI.Services.Classes
{
    public class CategoryService : ICategoryService<Category>
    {
        private readonly ICategoryRepo<Category> _repoobj;
        public CategoryService(ICategoryRepo<Category> robj) { _repoobj = robj; }
        public async Task Add(Category category)
        {
            try
            {
                await _repoobj.Add(category);
            }
            catch(DBConcurrencyException e)
            {
                throw new DBConcurrencyException(e.Message);
            }
            catch(Exception e)
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
            catch(DBConcurrencyException e)
            {
                throw new DBConcurrencyException(e.Message);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<Category> GetById(int id)
        {
            try
            {
                return await _repoobj.GetById(id);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
                try
                {
                    return await _repoobj.GetCategories();
                }
                catch(Exception e)
                {
                    throw new Exception(e.Message);
                }
            
        }

        public async Task Update(Category category)
        {
            try
            {
                await _repoobj.Update(category);
            }
            catch(DbUpdateException e)
            {
                throw new DbUpdateException(e.Message);
            }
            catch(DBConcurrencyException e)
            {
                throw new DBConcurrencyException(e.Message);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task<bool> CategoryExists(int id)
        {
            return await _repoobj.CategoryExists(id);
        }
    }
}
