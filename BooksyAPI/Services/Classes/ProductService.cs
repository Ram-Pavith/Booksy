using BooksyAPI.Models;
using BooksyAPI.Repo.Interfaces;
using BooksyAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BooksyAPI.Services.Classes
{
    public class ProductService:IProductService<Product>
    {
        private readonly IProductRepo<Product> _repoobj;
        public ProductService(IProductRepo<Product> repoobj)
        {
            _repoobj = repoobj;
        }

        public async Task Add(Product product)
        {
            try
            {
                await _repoobj.Add(product);
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

        public async Task<Product> GetById(int id)
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

        public async Task<IEnumerable<Product>> GetProducts()
        {
            try
            {
                return await _repoobj.GetProducts();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> ProductExists(int id)
        {
                try
                {
                    return await _repoobj.ProductExists(id);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
        }

        public async Task Update(Product product)
        {
            try
            {
                await _repoobj.Update(product);
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
