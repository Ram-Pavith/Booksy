using BooksyAPI.Models;
using BooksyAPI.Repo.Classes;
using BooksyAPI.Repo.Interfaces;
using BooksyAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Data;
using System.Security.Cryptography;

namespace BooksyAPI.Services.Classes
{
    public class CoverTypeService: ICoverTypeService<CoverType>
    {
        private readonly ICoverTypeRepo<CoverType> _repoobj;
        public CoverTypeService(ICoverTypeRepo<CoverType> repoobj)
        {
            _repoobj = repoobj;
        }

        public async Task Add(CoverType covertype)
        {
            try
            {
                await _repoobj.Add(covertype);
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

        public async Task<bool> CoverTypeExists(int id)
        {
            try
            {
                return await _repoobj.CoverTypeExists(id);
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

        public async Task<CoverType> GetById(int id)
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

        public async Task<IEnumerable<CoverType>> GetCoverTypes()
        {
            try
            {
                return await _repoobj.GetCoverTypes();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task Update(CoverType covertype)
        {
            try
            {
                await _repoobj.Update(covertype);
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
